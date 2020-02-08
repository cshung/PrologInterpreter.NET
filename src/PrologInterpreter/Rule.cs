namespace Andrew.PrologInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Rule
    {
        // TODO: Restrict public access to read only by providing constructor
        public Term Head { get; set; }

        public List<Term> Implies { get; set; }

        internal Rule Rename()
        {
            var variables = new HashSet<Variable>();
            this.Head.CollectVariables(variables);
            foreach (var imply in this.Implies)
            {
                imply.CollectVariables(variables);
            }

            List<Tuple<Variable, Variable>> replacements = variables.Select(t => Tuple.Create(t, new Variable(t.Name))).ToList();
            return new Rule { Head = this.Head.Rename(replacements), Implies = this.Implies.Select(t => t.Rename(replacements)).ToList() };
        }
    }
}
