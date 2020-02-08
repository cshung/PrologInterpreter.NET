namespace Andrew.PrologInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class Rule
    {
        internal Term Head { get; set; }

        internal List<Term> Implies { get; set; }

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
