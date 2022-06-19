using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLexBase
{
    public abstract class VirtualInstruction
    {
        public abstract List<Instruction> parsed { get; }
    }

    public static class VirtualInstructionListParser
    {
        public static List<Instruction> Parse(this List<VirtualInstruction> virtuals)
        {
            List<Instruction> toRet = new();
            virtuals.ForEach(v => toRet.AddRange(v.parsed));
            return toRet;
        }
    }

    public class IndirectInstruction : VirtualInstruction
    {
        Instruction direct;

        public override List<Instruction> parsed => new List<Instruction> { direct };

        public IndirectInstruction(Instruction i)
        {
            direct = i;
        }

        public static implicit operator Instruction(IndirectInstruction i) => i.direct;
    }

    public class TagCharsInstruction : VirtualInstruction
    {
        public string chars, tag;

        public override List<Instruction> parsed
        {
            get
            {
                List<Instruction> toRet = new();
                foreach (char c in chars)
                    toRet.Add(new IfInstruction(c.ToString(), 
                        new AddInstruction(tag), new CancelInstruction()));
                return toRet;
            }
        }

        public TagCharsInstruction(string c, string t)
        {
            chars = c;
            tag = t;
        }
    }

    public class RequireInstruction : VirtualInstruction
    {
        public string tag;

        public override List<Instruction> parsed =>
            new List<VirtualInstruction>
            {
                VInstr.IF(tag, VInstr.NEXT),
                VInstr.ELSE(VInstr.CANCEL)
            }.Parse();

        public RequireInstruction(string t)
        {
            tag = t;
        }
    }

    public class ZeroOrMoreInstruction : VirtualInstruction
    {
        public string tag;

        public override List<Instruction> parsed =>
            new List<VirtualInstruction>
            {
                VInstr.IF(tag, VInstr.REPEAT),
                VInstr.ELSE(VInstr.SKIP)
            }.Parse();
        public ZeroOrMoreInstruction(string t)
        {
            tag = t;
        }
    }
}
