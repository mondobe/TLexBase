using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLexBase.Testing
{
    public class TestingEnv
    {
        const int SANITY_CHECK = 1000;

        public string chars;

        public List<Token> tokens;
        private int index = 0;
        private int startIndex = 0;
        public Token token => tokens[index];

        public Rule rule => ruleStack[ruleStack.Count - 1].rule;
        private int ruleIndex => ruleStack[ruleStack.Count - 1].index;
        public Instruction instruction => rule.instructions[ruleIndex];
        public List<(Rule rule, int index)> ruleStack { get; private set; } = new();

        private IfInstruction lastIf;
        private List<Token> buffer;

        public bool done = false;

        public TestingEnv(string chars, Rule rule)
        {
            ruleStack = new();
            ruleStack.Add((rule, 0));
            this.chars = chars + "\u0000";

            GenerateTokens();
        }

        public TestingEnv(List<Token> tokens, Rule rule)
        {
            ruleStack = new();
            ruleStack.Add((rule, 0));
            this.tokens = tokens;
            buffer = new List<Token>();
        }

        public void GenerateTokens()
        {
            tokens = new List<Token>();

            foreach(char c in chars)
            {
                Token t = new(c.ToString());
                tokens.Add(t);
                t.tags.Add(c.ToString());
            }

            buffer = new List<Token>();
        }

        public void Step()
        {
            switch (instruction)
            {
                case NextInstruction:
                    NextToken();
                    BreakScope();
                    NextOpcode();
                    break;

                case IfInstruction i:
                    if (token.tags.Contains(i.tag))
                        ruleStack.Add((new Rule(i.block), 0));
                    else
                        NextOpcode();
                    lastIf = i;
                    break;

                case ElseInstruction i:
                    if (lastIf is IfInstruction e && !token.tags.Contains(e.tag))
                        ruleStack.Add((new Rule(i.block), 0));
                    else
                        NextOpcode();
                    lastIf = null;
                    break;

                case CancelInstruction:
                    RestartNextToken();
                    break;

                case SkipInstruction:
                    BreakScope();
                    NextOpcode();
                    break;

                case RepeatInstruction:
                    BreakScope();
                    if(instruction is ElseInstruction)
                        ruleStack[ruleStack.Count - 1] = (rule, ruleIndex - 1);
                    NextToken();
                    break;

                case BackInstruction:
                    buffer.Remove(token);
                    index--;
                    if (index < 0)
                        index = 0;
                    if (startIndex >= index)
                        startIndex--;
                    NextOpcode();
                    break;

                case WrapInstruction:
                    Token wrap = new Token("");
                    foreach (Token t in buffer)
                        wrap.content += t.content;
                    tokens.RemoveRange(startIndex, buffer.Count);
                    tokens.Insert(startIndex, wrap);
                    index = startIndex + 1;
                    NextOpcode();
                    break;

                case DeleteInstruction:
                    tokens.Remove(token);
                    NextOpcode();
                    break;

                case AddInstruction i:
                    token.tags.Add(i.tag);
                    NextOpcode();
                    break;

                default:
                    throw new FormatException("The instruction given was invalid. " + instruction);
            }

            while (ruleIndex == rule.instructions.Count)
            {
                if (ruleStack.Count == 1)
                {
                    RestartNextToken();
                }
                else
                {
                    BreakScope();
                }
            }
            if (index == tokens.Count)
            {
                done = true;
                return;
            }
        }

        private void NextToken()
        {
            buffer.Add(token);
            index++;
        }

        private void RestartNextToken()
        {
            startIndex++;
            index = startIndex;
            ruleStack = new List<(Rule rule, int index)> { (ruleStack[0].rule, 0) };
            buffer.Clear();
        }

        private void NextOpcode()
        {
            ruleStack[ruleStack.Count - 1] = (rule, ruleIndex + 1);
        }

        private void BreakScope()
        {
            if (ruleStack.Count > 1)
                ruleStack.RemoveAt(ruleStack.Count - 1);
            lastIf = null;
        }

        public void Run(bool verbose = false, int sanity = SANITY_CHECK)
        {
            int i = 0;

            while (!done && i < sanity)
            {
                Step();
                i++;
                if (verbose)
                    try
                    {
                        Console.WriteLine("Step " + i + ":\n" + token + "\n" + instruction);
                    }
                    catch { }
            }
        }
    }
}
