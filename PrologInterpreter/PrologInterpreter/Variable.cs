namespace Andrew.PrologInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class Variable : Term
    {
        private static int id = 1;

        private string name;

        private int version;

        internal Variable(string name)
        {
            this.name = name;
            this.version = id++;
        }

        internal string Name
        {
            get { return this.name; }
        }

        public override string ToString()
        {
            return this.name + this.version;
        }

        internal override bool HasVariable(Variable variable)
        {
            return this == variable;
        }

        internal override Term Substitute(Variable x, Term y)
        {
            if (x == this)
            {
                return y;
            }
            else
            {
                return this;
            }
        }

        internal override void CollectVariables(HashSet<Variable> variables)
        {
            variables.Add(this);
        }

        internal override Term Rename(List<Tuple<Variable, Variable>> replacements)
        {
            return replacements.Single(r => r.Item1 == this).Item2;
        }
    }
}
