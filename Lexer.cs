using System;
using System.Collections.Generic;
using TLexBase.Testing;

namespace TLexBase
{
    public class Lexer
    {
        public List<Rule> rules;

        public Lexer()
        {
            rules = new();
        }

        public List<Token> ApplyAll(string test, bool verbose = false, int sanity = 1000)
        {
            List<Token> last = null;
            foreach(Rule r in rules)
            {
                TestingEnv te = last is null ? new(test, r) : new(last, r);

                te.Run(verbose, sanity);
                last = te.tokens;
            }
            if (verbose)
                Console.WriteLine(last.TokensToString());
            return last;
        }
    }
}