using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLexBase
{
    public static class VInstr
    {

        public static IndirectInstruction IF(string t, params Instruction[] b) => new IndirectInstruction(new IfInstruction(t, b));

        public static IndirectInstruction ELSE(params Instruction[] b) => new IndirectInstruction(new ElseInstruction(b));

        public static IndirectInstruction ADD(string t) => new IndirectInstruction(new AddInstruction(t));

        public static IndirectInstruction NEXT => new IndirectInstruction(new NextInstruction());
        public static IndirectInstruction CANCEL => new IndirectInstruction(new CancelInstruction());
        public static IndirectInstruction SKIP => new IndirectInstruction(new SkipInstruction());
        public static IndirectInstruction REPEAT => new IndirectInstruction(new RepeatInstruction());
        public static IndirectInstruction BACK => new IndirectInstruction(new BackInstruction());
        public static IndirectInstruction WRAP => new IndirectInstruction(new WrapInstruction());
        public static IndirectInstruction DELETE => new IndirectInstruction(new DeleteInstruction());
    }
}
