namespace Andrew.PrologInterpreter
{
    using System.Collections.Generic;

    internal class Greater : Predicate
    {
        internal Greater(string name, List<Term> arguments) : base(name, arguments)
        {
        }

        internal override Term Substitute(Variable x, Term y)
        {
            Predicate super = (Predicate)base.Substitute(x, y);
            Atom firstTerm =  super.Arguments[0] as Atom;
            Atom secondTerm =  super.Arguments[1] as Atom;
            if (firstTerm == null || secondTerm == null)
            {
                return new Greater(super.Name, super.Arguments);
            }
            else if (string.Compare(firstTerm.ToString(), secondTerm.ToString()) > 0)
            {
                return Program.t;
            }
            else
            {
                return Program.f;
            }
        }
    }
}