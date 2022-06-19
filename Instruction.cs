using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLexBase
{
    public abstract class Instruction 
    {
        public static implicit operator VirtualInstruction(Instruction i) => new IndirectInstruction(i);
    }
    public abstract class BlockInstruction : Instruction 
    { 
        public List<Instruction> block
        {
            set
            {
                rule.instructions = value;
            }
            get
            {
                return rule.instructions;
            }
        }
        public Rule rule = new(new List<Instruction>()); 
    }

    public class NextInstruction : Instruction { public override string ToString() { return "NEXT"; } }

    public class IfInstruction : BlockInstruction
    {
        public string tag;

        public override string ToString()
        {
            string toRet = "IF " + tag + " \n{";

            foreach (Instruction i in block)
                toRet += "\n\t" + i;

            return toRet + "\n}";
        }

        public IfInstruction(string t, List<Instruction> b)
        {
            tag = t;
            block = b;
        }

        public IfInstruction(string t, params Instruction[] b)
        {
            tag = t;
            block = b.ToList();
        }
    }

    public class ElseInstruction : BlockInstruction
    {
        public override string ToString() 
        {
            string toRet = "ELSE\n{";

            foreach (Instruction i in block)
                toRet += "\n\t" + i;

            return toRet + "\n}";
        }

        public ElseInstruction(List<Instruction> b)
        {
            block = b;
        }

        public ElseInstruction(params Instruction[] b)
        {
            block = b.ToList();
        }
    }

    public class CancelInstruction : Instruction { public override string ToString() { return "CANCEL"; } }

    public class SkipInstruction : Instruction { public override string ToString() { return "SKIP"; } }

    public class RepeatInstruction : Instruction { public override string ToString() { return "REPEAT"; } }

    public class BackInstruction : Instruction { public override string ToString() { return "BACK"; } }

    public class WrapInstruction : Instruction { public override string ToString() { return "WRAP"; } }

    public class DeleteInstruction : Instruction { public override string ToString() { return "DELETE"; } }

    public class AddInstruction : Instruction
    {
        public string tag;

        public override string ToString() { return "ADD " + tag; }

        public AddInstruction(string t)
        {
            tag = t;
        }
    }
}