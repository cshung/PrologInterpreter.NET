namespace Andrew.PrologInterpreter
{
    using System;
    using System.Collections.Generic;

    public abstract class Term
    {
        internal abstract bool HasVariable(Variable variable);

        internal abstract Term Substitute(Variable x, Term y);

        internal abstract void CollectVariables(HashSet<Variable> variables);

        internal abstract Term Rename(List<Tuple<Variable, Variable>> replacements);
    }
}
