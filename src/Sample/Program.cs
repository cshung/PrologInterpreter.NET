namespace Andrew.PrologInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class Program
    {
        internal static bool tracing = true;


        private static void Main(string[] args)
        {
            Variable x1 = new Variable("X1");

            Variable p2 = new Variable("P2");
            Variable q2 = new Variable("Q2");
            Variable r2 = new Variable("R2");
            Variable s2 = new Variable("S2");

            Variable u3 = new Variable("U3");

            Variable b4 = new Variable("B4");
            Variable t4 = new Variable("T4");
            Variable p4 = new Variable("P4");
            Variable q4 = new Variable("Q4");
            Variable r4 = new Variable("R4");

            Variable h5 = new Variable("B5");
            Variable t5 = new Variable("T5");
            Variable b5 = new Variable("B5");
            Variable p5 = new Variable("P5");
            Variable q5 = new Variable("Q5");
            Variable r5 = new Variable("R5");

            Variable h6 = new Variable("B6");
            Variable t6 = new Variable("T6");
            Variable b6 = new Variable("B6");
            Variable p6 = new Variable("P6");
            Variable q6 = new Variable("Q6");
            Variable r6 = new Variable("R6");

            Variable p8 = new Variable("P7");
            Variable c8 = new Variable("C7");
            Variable b8 = new Variable("B7");
            Variable s8 = new Variable("S7");
            Variable t8 = new Variable("T7");
            Variable u8 = new Variable("U7");
            Variable v8 = new Variable("V7");
            Variable w8 = new Variable("W7");
            Variable x8 = new Variable("X7");

            var rules = new List<Rule>
            {
                new Rule { Head = BuiltIns.t, Implies = new List<Term>() },
                // append([],X,X).
                new Rule { Head = Append(BuiltIns.nil, x1, x1), Implies = new List<Term>() },
                // append([P|Q],R,[P|S]) :- append(Q,R,S).
                new Rule { Head = Append(Cons(p2, q2), r2, Cons(p2, s2)), Implies = new List<Term> { Append(q2, r2, s2) } },
                // partition([],_,[],[],[]).
                new Rule { Head = Partition(BuiltIns.nil, u3, BuiltIns.nil, BuiltIns.nil, BuiltIns.nil), Implies = new List<Term>() },
                // partition([B|T],B,P,[B|Q],R) :- partition(T, B, P, Q, R).
                new Rule { Head = Partition(Cons(b4, t4), b4, p4, Cons(b4,q4), r4), Implies = new List<Term> { Partition(t4, b4, p4, q4, r4) } },
                // partition([H|T],B,[H|P],Q,R) :- H < B, partition(T, B, P, Q, R).
                new Rule { Head = Partition(Cons(h5, t5), b5, Cons(h5, p5), q5, r5), Implies = new List<Term> { Less(h5, b5), Partition(t5, b5, p5, q5, r5) } },
                // partition([H|T],B,P,Q,[H|R]) :- H > B, partition(T, B, P, Q, R).
                new Rule { Head = Partition(Cons(h6, t6), b6, p6, q6, Cons(h6, r6)), Implies = new List<Term> { Greater(h6, b6), Partition(t6, b6, p6, q6, r6) } },
                // qsort([],[]).
                new Rule { Head = Qsort(BuiltIns.nil, BuiltIns.nil), Implies = new List<Term>() },
                // qsort([P|C]), B) :- partition(C,P,S,T,U), qsort(S,V), qsort(U,W), append(V, [P|T], X), append(X, W, B).
                new Rule { Head = Qsort(Cons(p8, c8), b8), Implies = new List<Term> { Partition(c8,p8,s8,t8,u8), Qsort(s8,v8), Qsort(u8,w8), Append(v8, Cons(p8,t8), x8), Append(x8, w8, b8) } },
            };

            Term a = new Atom("prolog");
            Term b = new Atom("interpreter");
            Term c = new Atom("dot");
            Term d = new Atom("net");
            Term result = new Variable("Result");
            foreach (var solution in new Interpreter(rules, tracing).Query(new List<Term> { Qsort(List(a, b, c, d), result) }))
            {
                Console.WriteLine("Yes.");
                foreach (var substitution in solution)
                {
                    Console.WriteLine("{0} is substituted by {1}", substitution.Variable.Name, substitution.By);
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

        private static Term Less(Term left, Term right)
        {
            return new Predicate("Less", new List<Term>{left, right}, (terms => string.Compare(((Atom)terms[0]).ToString(), ((Atom)terms[1]).ToString()) < 0 ));
        }

        private static Term Greater(Term left, Term right)
        {
            return new Predicate("Greater", new List<Term>{left, right}, (terms => string.Compare(((Atom)terms[0]).ToString(), ((Atom)terms[1]).ToString()) > 0 ));
        }

        private static Term Qsort(Term input, Term output)
        {
            return new Predicate("Qsort", new List<Term>{input, output});
        }

        private static Term List(params Term[] terms)
        {
            if (terms.Length == 0)
            {
                return BuiltIns.nil;
            }
            else
            {
                return Cons(terms[0], List(terms.Skip(1).ToArray()));
            }
        }
    }
}
