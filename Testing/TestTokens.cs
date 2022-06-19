using System;
using System.Collections.Generic;
namespace TLexBase.Testing
{
    public static class TestTokens
    {
        public static string TokensToString(this List<Token> tokens)
        {
            string toRet = "";

            foreach (Token t in tokens)
                toRet += t + "\n";

            return toRet.Trim();
        }
    }
}
