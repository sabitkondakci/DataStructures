using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStackAndQueueInfixPrefixExamples
{
    class BracketControlAlgorithm
    {
        public bool CheckBrackets(string sampleBrackets)
        {
            MyDynamicStack<char> myDynamicStack=new MyDynamicStack<char>();
            char[] bracketChars = sampleBrackets.ToCharArray();

            foreach (var bracketChar in bracketChars)
            {
                if(bracketChar=='('||bracketChar=='{'||bracketChar=='[')
                    myDynamicStack.Push(bracketChar);
                else
                {
                    if (bracketChar == ')' && myDynamicStack.Peek()=='(')
                        myDynamicStack.Pop();
                    else if (bracketChar == '}' && myDynamicStack.Peek() == '{')
                        myDynamicStack.Pop();
                    else if (bracketChar == ']' && myDynamicStack.Peek() == '[')
                        myDynamicStack.Pop();
                    else
                        return false;
                }
            }

            if (myDynamicStack.IsEmpty())
                return true;

            return false;
        }
    }
}
