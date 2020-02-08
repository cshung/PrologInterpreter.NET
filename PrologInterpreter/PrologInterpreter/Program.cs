namespace Andrew.PrologInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class Program
    {
        internal static Term nil = new Atom("nil");
        internal static Atom t = new Atom("true");
        internal static Atom f = new Atom("false");

/*
append([],X,X).
append([P|Q],R,[P|S]) :- append(Q,R,S).
partition([],_,[],[],[]).
partition([B|T],B,P,[B|Q],R) :- partition(T, B, P, Q, R).
partition([H|T],B,[H|P],Q,R) :- H < B, partition(T, B, P, Q, R).
partition([H|T],B,P,Q,[H|R]) :- H > B, partition(T, B, P, Q, R).
qsort([],[]).
qsort([P|C]), B) :- partition(C,P,S,T,U), qsort(S,V), qsort(U,W), append(V, [P|T], X), append(X, W, B).
 */
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
            Variable u1 = new Variable("U1");
            Variable b1 = new Variable("B1");
            Variable t1 = new Variable("T1");
            Variable p1 = new Variable("P1");
            Variable q1 = new Variable("Q1");
            Variable r1 = new Variable("R1");

            var rules = new List<Rule>
            {
                new Rule { Head = Append(nil, x, x), Implies = new List<Term>() },
                new Rule { Head = Append(Cons(p, q), r, Cons(p, s)), Implies = new List<Term> { Append(q, r, s) } },
                new Rule { Head = Partition(nil, u1, nil, nil, nil), Implies = new List<Term>() },
            };

            foreach (var solution in new Interpreter(rules, /* tracing = */ false).Query(new List<Term> { Append(List(a, b), result, List(a, b, c, d)) }))
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

        private static Term Partition(Term list, Term pivot, Term left, Term center, Term right)
        {
            return new Predicate("Partition", new List<Term> {list, pivot, left, center, right});
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
