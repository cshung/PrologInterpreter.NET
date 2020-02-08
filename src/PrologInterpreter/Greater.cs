namespace Andrew.PrologInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Greater : Predicate
    {
        public Greater(string name, List<Term> arguments) : base(name, arguments)
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
            else 
            {
                if (string.Compare(firstTerm.ToString(), secondTerm.ToString()) > 0)
                {
                    return BuiltIns.t;
                }
                else
                {
                    return BuiltIns.f;
                }
            }
        }

        internal override Term Rename(List<Tuple<Variable, Variable>> replacements)
        {
            return new Greater(this.Name, this.Arguments.Select(t => t.Rename(replacements)).ToList());
        }        
    }
}