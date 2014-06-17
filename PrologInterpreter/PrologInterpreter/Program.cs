namespace Andrew.PrologInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class Program
    {
        private static Term nil = new Atom("nil");

        private static void Main(string[] args)
        {
            Term a = new Atom("a");
            Term b = new Atom("b");
            Term c = new Atom("c");
            Term d = new Atom("d");
            Term result = new Variable("Result");

            Variable x = new Variable("X");

            Variable p = new Variable("P");
            Variable q = new Variable("Q");
            Variable r = new Variable("R");
            Variable s = new Variable("S");

            var rules = new List<Rule>
            {
                new Rule { Head = Append(nil, x, x), Implies = new List<Term>() },
                new Rule { Head = Append(Cons(p, q), r, Cons(p, s)), Implies = new List<Term> { Append(q, r, s) } },
            };

            foreach (var solution in new Interpreter(rules, /* tracing = */ true).Query(new List<Term> { Append(List(a, b), result, List(a, b, c, d)) }))
            {
                Console.WriteLine("Yes.");
                foreach (var substitution in solution)
                {
                    Console.WriteLine("{0} is substituted by {1}", substitution.Variable, substitution.By);
                }
            }
        }

        private static Term Append(Term one, Term two, Term result)
        {
            return new Predicate("Append", new List<Term> { one, two, result });
        }

        private static Term Cons(Term head, Term tail)
        {
            return new Predicate("Cons", new List<Term> { head, tail });
        }

        private static Term List(params Term[] terms)
        {
            if (terms.Length == 0)
            {
                return nil;
            }
            else
            {
                return Cons(terms[0], List(terms.Skip(1).ToArray()));
            }
        }
    }
}
