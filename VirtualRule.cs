using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLexBase
{
    public class VirtualRule
    {
        public List<VirtualInstruction> vInstructions;
        public string name;

        public VirtualRule(List<VirtualInstruction> vs, string n = "")
        {
            vInstructions = vs;
            name = n;
        }

        public Rule real => new Rule(vInstructions.Parse(), name);

        public static implicit operator Rule(VirtualRule vr) => vr.real;
    }
}
