using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MyStackAndQueueInfixPrefixExamples
{
    class PrefixAndPostfixEvaluation
    {
        //Kare parantez [] içerisindeki veriler tek bir sayı olarak düşünülür
        public double EvaluatePrefixExpression(StringBuilder evaluateString)
        {
           
            StringBuilder appendBuilder= new StringBuilder();
            MyDynamicStack<double> stack = new MyDynamicStack<double>();
            int length = evaluateString.Length;

            for (int i = length - 1; i >= 0; i--)
            {
                string c = evaluateString[i].ToString();
                
                if (char.IsDigit(c,0))
                {
                    double term = Convert.ToSByte(c);//Taranan ifadeler rakam olacağından SByte işimizi görecektir.
                    stack.Push(term);
                }
                else if (c == "]")//[] parantez ifadesi negatif ifade veya iki ve üstü haneli sayı olduğundan parantez sonuna kadar tarama yapılır.
                {
                    char join = evaluateString[--i];
                    while (join != '[')
                    {
                        appendBuilder.Insert(0,join);
                        join = evaluateString[--i];
                    }

                    stack.Push(Convert.ToDouble(appendBuilder.ToString()));
                    appendBuilder.Clear();
                }
                else
                {
                    switch (c)
                    {
                        case "^":
                            double first = stack.Pop();
                            double second = stack.Pop();
                            double result = Math.Pow(first, second);
                            stack.Push(result);
                            break;
                        case "*": 
                            first = stack.Pop();
                            second = stack.Pop();
                            result = first * second;
                            stack.Push(result);
                            break;
                        case "/":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = first / second;
                            stack.Push(result);
                            break;
                        case "+":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = first + second;
                            stack.Push(result);
                            break;
                        case "-":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = first-second;
                            result = second == 0 && evaluateString[0] == '-' ? -1 * result : result;//Son ifade - olursa ve Stack te tek bir değer varsa, sayı negatif olur.
                            stack.Push(result);
                            break;
                    }
                }
            }

            return stack.Pop();
        } // "-+2*34/[16]^23" iki ve ikiden büyük basamaklı sayılar [] parantezler arasına yazılmalıdır.
        public double EvaluatePostfixExpression(StringBuilder evaluateString)
        {
            StringBuilder appendBuilder = new StringBuilder();
            MyDynamicStack<double> stack = new MyDynamicStack<double>();
            int length = evaluateString.Length;

            for (int i = 0; i < length; i++)
            {
                string c = evaluateString[i].ToString();

                if (char.IsDigit(c, 0))
                {
                    double term = Convert.ToSByte(c);//Taranan ifadeler rakam olacağından SByte işimizi görecektir.
                    stack.Push(term);
                }
                else if (c == "[")//[] parantez ifadesi negatif ifade veya iki ve üstü haneli sayı olduğundan parantez sonuna kadar tarama yapılır.
                {
                    char join = evaluateString[++i];
                    while (join != ']')
                    {
                        appendBuilder.Append(join);
                        join = evaluateString[++i];
                    }

                    stack.Push(Convert.ToDouble(appendBuilder.ToString()));
                    appendBuilder.Clear();
                }
                else
                {
                    switch (c) //Postfix sırası Stack durumu second-first!
                    {
                        case "^":
                            double first = stack.Pop();
                            double second = stack.Pop();
                            double result = Math.Pow(second, first);
                            stack.Push(result);
                            break;
                        case "*":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = first * second;
                            stack.Push(result);
                            break;
                        case "/":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = second / first;
                            stack.Push(result);
                            break;
                        case "+":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = second + first;
                            stack.Push(result);
                            break;
                        case "-":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = second - first;
                            stack.Push(result);
                            break;
                    }
                }
            }

            return stack.Pop();
        }
        public double EvaluatePrefixExpression(string evaluateString)
        {

            StringBuilder appendBuilder = new StringBuilder();
            MyDynamicStack<double> stack = new MyDynamicStack<double>();
            int length = evaluateString.Length;

            for (int i = length - 1; i >= 0; i--)
            {
                string c = evaluateString[i].ToString();

                if (char.IsDigit(c, 0))
                {
                    double term = Convert.ToSByte(c);//Taranan ifadeler rakam olacağından SByte işimizi görecektir.
                    stack.Push(term);
                }
                else if (c == "]")//[] parantez ifadesi negatif ifade veya iki ve üstü haneli sayı olduğundan parantez sonuna kadar tarama yapılır.
                {
                    char join = evaluateString[--i];
                    while (join != '[')
                    {
                        appendBuilder.Insert(0, join);
                        join = evaluateString[--i];
                    }

                    stack.Push(Convert.ToDouble(appendBuilder.ToString()));
                    appendBuilder.Clear();
                }
                else
                {
                    switch (c)
                    {
                        case "^":
                            double first = stack.Pop();
                            double second = stack.Pop();
                            double result = Math.Pow(first, second);
                            stack.Push(result);
                            break;
                        case "*":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = first * second;
                            stack.Push(result);
                            break;
                        case "/":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = first / second;
                            stack.Push(result);
                            break;
                        case "+":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = first + second;
                            stack.Push(result);
                            break;
                        case "-":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = first - second;
                            result = second == 0 && evaluateString[0] == '-' ? -1 * result : result;//Son ifade - olursa ve Stack te tek bir değer varsa, sayı negatif
                            stack.Push(result);
                            break;
                    }
                }
            }

            return stack.Pop();
        } // "-+2*34/[16]^23" iki ve ikiden büyük basamaklı sayılar [] parantezler arasına yazılmalıdır.
        public double EvaluatePostfixExpression(string evaluateString)
        {
            StringBuilder appendBuilder = new StringBuilder();
            MyDynamicStack<double> stack = new MyDynamicStack<double>();
            int length = evaluateString.Length;

            for (int i = 0; i < length; i++)
            {
                string c = evaluateString[i].ToString();

                if (char.IsDigit(c, 0))
                {
                    double term = Convert.ToSByte(c);//Taranan ifadeler rakam olacağından SByte işimizi görecektir.
                    stack.Push(term);
                }
                else if (c == "[")//[] parantez ifadesi negatif ifade veya iki ve üstü haneli sayı olduğundan parantez sonuna kadar tarama yapılır.
                {
                    char join = evaluateString[++i];
                    while (join != ']')
                    {
                        appendBuilder.Append(join);
                        join = evaluateString[++i];
                    }

                    stack.Push(Convert.ToDouble(appendBuilder.ToString()));
                    appendBuilder.Clear();
                }
                else
                {
                    switch (c) //Postfix sırası Stack durumu second-first!
                    {
                        case "^":
                            double first = stack.Pop();
                            double second = stack.Pop();
                            double result = Math.Pow(second, first);
                            stack.Push(result);
                            break;
                        case "*":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = first * second;
                            stack.Push(result);
                            break;
                        case "/":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = second / first;
                            stack.Push(result);
                            break;
                        case "+":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = second + first;
                            stack.Push(result);
                            break;
                        case "-":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = second - first;
                            stack.Push(result);
                            break;
                    }
                }
            }

            return stack.Pop();
        }
        

    }
}
