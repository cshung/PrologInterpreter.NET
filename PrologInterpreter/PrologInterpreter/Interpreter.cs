namespace Andrew.PrologInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class Interpreter
    {
        private List<Rule> rules;
        private bool tracing;

        internal Interpreter(List<Rule> rules, bool tracing)
        {
            this.rules = rules;
            this.tracing = tracing;
        }

        internal IEnumerable<List<Substitution>> Query(List<Term> queryTerms)
        {
            var queryVariables = new HashSet<Variable>();
            foreach (var queryTerm in queryTerms)
            {
                queryTerm.CollectVariables(queryVariables);
            }

            return this.Query(queryTerms, queryVariables.Select(t => new Substitution { Variable = t, By = t }).ToList());
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
                if (x is Variable)
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
                }
                else if (x == y)
                {
                    // no-op - identical atom/variable unifies
                }
                else if (x is Predicate && y is Predicate)
                {
                    Predicate xP = (Predicate)x;
                    Predicate yP = (Predicate)y;
                    if (xP.Name.Equals(yP.Name))
                    {
                        var argumentPairs = xP.Arguments.Zip(yP.Arguments, (p, q) => new Equation { Left = p, Right = q });
                        foreach (var argumentPair in argumentPairs)
                        {
                            stack.Push(argumentPair);
                        }
                    }
                }
                else
                {
                    return null;
                }
            }

            return substitutions;
        }

        private IEnumerable<List<Substitution>> Query(List<Term> queryTerms, List<Substitution> queryVariableSubstitions)
        {
            if (queryTerms.Count == 0)
            {
                yield return queryVariableSubstitions;
                yield break;
            }

            List<Term> resolvents = new List<Term>(queryTerms);
            if (this.tracing)
            {
                Console.WriteLine(string.Join(", ", resolvents));
            }

            Term currentQuery = resolvents.First();
            foreach (var rule in this.rules)
            {
                var rule2 = rule.Rename();
                var substitutions = Unify(currentQuery, rule2.Head);
                if (substitutions != null)
                {
                    var ruleSubstituted = rule2.Implies.Select(t =>
                    {
                        foreach (var sub in substitutions)
                        {
                            t = t.Substitute(sub.Variable, sub.By);
                        }

                        return t;
                    });
                    var queryVariableSubstitionsSubstituted = queryVariableSubstitions.Select(s =>
                    {
                        Term by = s.By;
                        foreach (var sub in substitutions)
                        {
                            by = by.Substitute(sub.Variable, sub.By);
                        }

                        return new Substitution { Variable = s.Variable, By = by };
                    }).ToList();
                    foreach (var result in this.Query(resolvents.Skip(1).Concat(ruleSubstituted).ToList(), queryVariableSubstitionsSubstituted))
                    {
                        yield return result;
                    }
                }
            }
        }
    }
}
