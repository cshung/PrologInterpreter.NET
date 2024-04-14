namespace Andrew.PrologInterpreter
{
    using System;
    using System.Collections.Generic;

    public class Cut : Term
    {
        private Cutter cutter;

        internal override bool HasVariable(Variable variable)
        {
            return false;
        }

        internal override Term Substitute(Variable x, Term y)
        {
            return new Cut();
        }

        internal override void CollectVariables(HashSet<Variable> variables)
        {
            // no-op
        }

        internal override Term Rename(List<Tuple<Variable, Variable>> replacements)
        {
            return new Cut();
        }

        internal void Prepare(Cutter cutter)
        {
            if (this.cutter == null)
            {
                this.cutter = cutter;
            }
        }

        internal void Apply()
        {
            cutter.Cut = true;
        }

        public override string ToString()
        {
            return "!";
        }
    }
}
