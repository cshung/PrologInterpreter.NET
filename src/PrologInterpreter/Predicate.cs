namespace Andrew.PrologInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Predicate : Term
    {
        private string name;

        private List<Term> arguments;

        public Predicate(string name, List<Term> arguments)
        {
            this.name = name;
            this.arguments = arguments;
        }

        internal string Name
        {
            get { return this.name; }
        }

        internal List<Term> Arguments
        {
            get { return this.arguments; }
        }

        public override string ToString()
        {
            return this.name + "(" + string.Join(", ", this.arguments) + ")";
        }

        internal override bool HasVariable(Variable variable)
        {
            return this.arguments.Any(t => t.HasVariable(variable));
        }

        internal override Term Substitute(Variable x, Term y)
        {
            return new Predicate(this.name, this.arguments.Select(t => t.Substitute(x, y)).ToList());
        }

        internal override void CollectVariables(HashSet<Variable> variables)
        {
            foreach (var argument in this.arguments)
            {
                argument.CollectVariables(variables);
            }
        }

        internal override Term Rename(List<Tuple<Variable, Variable>> replacements)
        {
            return new Predicate(this.name, this.arguments.Select(t => t.Rename(replacements)).ToList());
        }
    }
}
