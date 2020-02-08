namespace Andrew.PrologInterpreter
{
    using System;
    using System.Collections.Generic;

    public class Atom : Term
    {
        private string name;

        public Atom(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return this.name;
        }

        internal override bool HasVariable(Variable variable)
        {
            return false;
        }

        internal override Term Substitute(Variable x, Term y)
        {
            return this;
        }

        internal override void CollectVariables(HashSet<Variable> variables)
        {
            // no-op
        }

        internal override Term Rename(List<Tuple<Variable, Variable>> replacements)
        {
            return this;
        }
    }
}
