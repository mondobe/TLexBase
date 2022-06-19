using System.Collections.Generic;

namespace TLexBase.Testing
{
    public class Token
    {
        public string content;
        public List<string> tags = new List<string>();

        public override string ToString()
        {
            string toRet = "(" + content + ": ";
            tags.ForEach(t => toRet += "\"" + t + "\", ");
            return toRet + ")";
        }

        public Token(string c)
        {
            content = c;
        }
    }
}
