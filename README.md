# PrologInterpreter.NET
A simple Prolog interpreter written in C#

# Usage
A prolog interpreter answers query based on the rules that it knows. To use the interpreter, you set it up with a collection of rules, and then you query the interpreter for answers. Checkout the [sample](https://github.com/cshung/PrologInterpreter.NET/tree/master/src/Sample) that use the interpreter to perform a quick sort on a list of strings.

# How does it work?
It is assumed the reader understands basic Prolog terminologies.

As an overview, the prolog interpreter matches the first term in the query to the rules one by one in the given order. Once it find a match, the right hand side of the rule is prepended to the query with the first term removed and then the interpreter try to query for the new query again. This is done until the query is empty, at that point, the interpreter return a solution.

## Rule matching
Matching the rule's left hand side with a term is the heart of the interpreter, this is done through a routine called `Unify`. If the unification succeed, the matching rewrite the original query to a new query.

### Unification
The routine tried to equate two terms. If neither term is a variable, then it is simply an recursive unification. For variables, the unification algorithms try to make a variable becomes the other, but only if the other doesn't contains the same variable. When that happens, unless both side is the same variable, that couldn't work out, so the unification bails.

When the unification algorithm completes, it returns either `null` if it decides the unification is impossible, or a list of substitutions it has to do to unify the two terms.

### Replacement
Once the unification confirmed a term matches with a rule's head, it creates the new query. The new query contains all the terms on the rule's right hand side, and the remaining terms in the original query. All of them must perform all the substitutions required by the unification. 

## Computed Predicate
Some predicates, like comparison, perform some computation. The computation is performed during substitution. When the predicate's argument are all atoms, the computation begins and become 'subsituted' to either true or false, that way we can incorporate computation into the basic query matching paradigm.