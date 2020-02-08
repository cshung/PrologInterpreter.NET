namespace Andrew.PrologInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Interpreter
    {
        private List<Rule> rules;
        private bool tracing;

        public Interpreter(List<Rule> rules, bool tracing)
        {
            this.rules = rules;
            this.tracing = tracing;
        }

        public IEnumerable<List<Substitution>> Query(List<Term> queryTerms)
        {
            var queryVariables = new HashSet<Variable>();
            foreach (var queryTerm in queryTerms)
            {
                queryTerm.CollectVariables(queryVariables);
            }

            return this.Query(queryTerms, queryVariables.Select(t => new Substitution { Variable = t, By = t }).ToList(), 0);
        }

        private static List<Substitution> Unify(Term left, Term right)
        {
            List<Substitution> substitutions = new List<Substitution>();
            Stack<Equation> stack = new Stack<Equation>();
            stack.Push(new Equation { Left = left, Right = right });
            while (stack.Count > 0)
            {
                Equation equation = stack.Pop();
                Term x = equation.Left;
                Term y = equation.Right;
                if (x == y)
                {
                    // no-op - identical atom/variable unifies
                }
                else if (x is Variable)
                {
                    Variable vx = (Variable)x;
                    if (!y.HasVariable(vx))
                    {
                        foreach (var stackEquation in stack)
                        {
                            stackEquation.Left = stackEquation.Left.Substitute(vx, y);
                            stackEquation.Right = stackEquation.Right.Substitute(vx, y);
                        }

                        substitutions.Add(new Substitution { Variable = vx, By = y });
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (y is Variable)
                {
                    Variable vy = (Variable)y;
                    if (!x.HasVariable(vy))
                    {
                        foreach (var stackEquation in stack)
                        {
                            stackEquation.Left = stackEquation.Left.Substitute(vy, x);
                            stackEquation.Right = stackEquation.Right.Substitute(vy, x);
                        }

                        substitutions.Add(new Substitution { Variable = vy, By = x });
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (x is Predicate && y is Predicate)
                {
                    Predicate xP = (Predicate)x;
                    Predicate yP = (Predicate)y;
                    if (xP.Name.Equals(yP.Name))
                    {
                        // Assumption - arity equals if name equals
                        var argumentPairs = xP.Arguments.Zip(yP.Arguments, (p, q) => new Equation { Left = p, Right = q });
                        foreach (var argumentPair in argumentPairs)
                        {
                            stack.Push(argumentPair);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }

            return substitutions;
        }

        private IEnumerable<List<Substitution>> Query(List<Term> queryTerms, List<Substitution> queryVariableSubstitions, int indent)
        {
            if (queryTerms.Count == 0)
            {
                yield return queryVariableSubstitions;
                yield break;
            }

            if (this.tracing)
            {
                Console.Write(new String(' ', indent));
                Console.WriteLine(string.Join(", ", queryTerms));
            }

            Term currentQuery = queryTerms.First();
            foreach (var rule in this.rules)
            {
                var rule2 = rule.Rename();
                var substitutions = Unify(currentQuery, rule2.Head);
                if (substitutions != null)
                {
                    List<Term> resolvents = new List<Term>();
                    foreach (Term u in rule2.Implies.Concat(queryTerms.Skip(1)))
                    {
                        Term t = u;
                        foreach (var sub in substitutions)
                        {
                            t = t.Substitute(sub.Variable, sub.By);
                        }
                        resolvents.Add(t);
                    }
                    var queryVariableSubstitionsSubstituted = queryVariableSubstitions.Select(s =>
                    {
                        Term by = s.By;
                        foreach (var sub in substitutions)
                        {
                            by = by.Substitute(sub.Variable, sub.By);
                        }

                        return new Substitution { Variable = s.Variable, By = by };
                    }).ToList();
                    foreach (var result in this.Query(resolvents, queryVariableSubstitionsSubstituted, indent + 2))
                    {
                        yield return result;
                    }
                }
            }
        }
    }
}
