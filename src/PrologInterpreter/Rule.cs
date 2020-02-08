namespace Andrew.PrologInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Rule
    {
        private Term head;
        private List<Term> implies;

        public Rule(Term head, List<Term> implies)
        {
            this.head = head;
            this.implies = implies;
        }

        internal Term Head { get { return this.head; } }

        internal List<Term> Implies { get { return this.implies; } }

        internal Rule Rename()
        {
            var variables = new HashSet<Variable>();
            this.Head.CollectVariables(variables);
            foreach (var imply in this.Implies)
            {
                imply.CollectVariables(variables);
            }

            List<Tuple<Variable, Variable>> replacements = variables.Select(t => Tuple.Create(t, new Variable(t.Name))).ToList();
            return new Rule(this.Head.Rename(replacements), this.Implies.Select(t => t.Rename(replacements)).ToList());
        }
    }
}
