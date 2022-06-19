using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLexBase
{
    public class Rule
    {
        public List<Instruction> instructions;
        public string name;

        public Rule(List<Instruction> instructions, string name = "")
        {
            this.instructions = instructions;
            this.name = name;
        }

        public Rule(List<Instruction> instructions)
        {
            this.instructions = instructions;
            name = "";
        }

        public static Rule FromVirtual(params VirtualInstruction[] virts)
        {
            return new Rule(virts.ToList().Parse());
        }

        public override string ToString()
        {
            string toRet = name + ":\n";
            instructions.ForEach(i => toRet += i.GetType());
            return toRet;
        }
    }
}
