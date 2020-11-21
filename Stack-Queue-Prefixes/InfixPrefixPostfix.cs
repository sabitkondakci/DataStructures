using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStackAndQueueInfixPrefixExamples
{
    class InfixPrefixPostfix 
    {
        //Format: ([-12]+1)*5/6 şeklinde olmalıdır.
        //Görüldüğü üzere negatif sayılar , iki ve üstü haneli sayılar [] kare parantezler arasına yazılmalıdır.

        //Dönüşüm işlemlerinde StringBuilder sınıfı kullanılmasının nedeni performanstır.
        //String char[] dizisi olduğundan RAM'den sıralı bölge talep eder, bu da diğer işlemlerin yavaşlamasına neden olur.
        //String kuçük dizilerde kullanılabilir, dönüşümlerde bu durumdan faydalanılmıştır.
        private int CheckRank(string x) //Stack giriş ve çıkış sırası için belirlenen öncelikler.
        {
            switch (x)
            {
                case "^":
                    return 3;
                case "*":
                case "/":
                    return 2;
                case "+":
                case "-":
                    return 1;
                default:
                    return 0;
            }
        }
        public StringBuilder InfixToPostfixConverter(StringBuilder stringBuilder)
        {
            MyDynamicStack<string> stack= new MyDynamicStack<string>();
            StringBuilder appendBuilder= new StringBuilder();
            StringBuilder tempBuilder = new StringBuilder();
            int length = stringBuilder.Length;

            for (int i = 0; i < length; i++) //String üzerinde soldan-sağa dolaşılır.
            {
                string c = stringBuilder[i].ToString();//Stack string seçildiğinden gelen char ifadesi string'e dönüştürülür.

                if (char.IsLetterOrDigit(c,0)) 
                {
                    appendBuilder.Append(c);
                }
                else if (c =="^" || c=="(" ||c=="{") //Parantez ve ^ önceliği için koşullar, [ ] parantezi iki ve üstü haneli sayıları temsilen kullanılmıştır. 
                {
                    stack.Push(c);
                }
                else if (c==")" || c=="}") 
                {
                    while (stack.Peek()!=null && stack.Peek()!="("&& stack.Peek()!="{")
                    {
                        appendBuilder.Append(stack.Pop());
                    }

                    stack.Pop();
                }
                else if (c=="[") //Sağdan sola tarama yapılır, [ ifadesine rastlanması halinde döngüye girilir, ta ki ] ifadesine gelene değin.
                {
                    char join = stringBuilder[++i];
                    while (join != ']')
                    {
                        tempBuilder.Append(join);//[ifade] ifade geçici olarak tempBuilder içinde tutulur.
                        join = stringBuilder[++i];
                    }

                    appendBuilder.Append("["+tempBuilder.ToString()+"]");//İki ve üstü haneli sayılar [] parantezleri arasına yazılır.
                    tempBuilder.Clear();
                }
                else 
                {
                    if (CheckRank(c)>CheckRank(stack.Peek()))
                    {
                        stack.Push(c);
                    }
                    else
                    {
                        while (stack.Peek()!=null && CheckRank(c)<=CheckRank(stack.Peek()))
                        {
                            appendBuilder.Append(stack.Pop());
                        }
                        stack.Push(c);
                    }
                }
               
            }

            while (stack.Peek()!=null)
            {
                appendBuilder.Append(stack.Pop());
            }

            return appendBuilder;
        }
        public StringBuilder InfixToPrefixConverter(StringBuilder stringBuilder)
        {
            MyDynamicStack<string> stack = new MyDynamicStack<string>();
            StringBuilder appendBuilder = new StringBuilder();
            StringBuilder tempBuilder=new StringBuilder();
            int length = stringBuilder.Length;

            for (int i = length - 1; i >= 0; i--)//Sondan başa doğru tarama yapılır.
            {
                string c = stringBuilder[i].ToString();

                if (char.IsLetterOrDigit(c,0))
                {
                    appendBuilder.Append(c);
                }
                else if (c == "^" || c == ")" || c == "}")
                {
                    stack.Push(c);
                }
                else if (c == "(" || c == "{")
                {
                    while (stack.Peek() != null && stack.Peek() != ")" && stack.Peek() != "}")
                    {
                        appendBuilder.Append(stack.Pop());
                    }

                    stack.Pop();
                }
                else if (c == "]") //Tarama tersten yapıldığı için karşılaşılan ilk parantez ] olur.
                {
                    char join = stringBuilder[--i];
                    while (join != '[')
                    {
                        tempBuilder.Append(join); // Reverse işlemi gerçekleşeceğinden Insert yerine Append metodunu kullandım.
                        join = stringBuilder[--i];
                    }

                    appendBuilder.Append("[" + tempBuilder.ToString() + "]");
                    tempBuilder.Clear();
                }
                else
                {
                    if (CheckRank(c) > CheckRank(stack.Peek()))
                    {
                        stack.Push(c);
                    }
                    else
                    {
                        while (stack.Peek() != null && CheckRank(c) < CheckRank(stack.Peek()))
                        {
                            appendBuilder.Append(stack.Pop());
                        }
                        stack.Push(c);
                    }
                }

            }

            while (stack.Peek() != null)
            {
                appendBuilder.Append(stack.Pop());
            }

            return Reverse(appendBuilder);//İfade ters çevrilerek işlem sonlandırılır.
        }
        private StringBuilder Reverse(StringBuilder reverseBuilder)
        {
            StringBuilder reverseAppend= new StringBuilder();
            int length = reverseBuilder.Length;
            for (int i = length-1 ; i >=0; i--)
            {
                if (reverseBuilder[i]=='[') // Parantez karmaşasının çözümü için gereken koşul.
                {
                    reverseAppend.Append(']');
                }
                else if (reverseBuilder[i] == ']')
                {
                    reverseAppend.Append('[');
                }
                else
                {
                    reverseAppend.Append(reverseBuilder[i]);
                }
               
            }

            return reverseAppend;
        }
        public string PostfixToInfix(string stringBuilder)
        {
            StringBuilder appendBuilder = new StringBuilder();
            MyDynamicStack<string> stack = new MyDynamicStack<string>();
            int length = stringBuilder.Length;

            for (int i = 0; i < length; i++)
            {
                string c = stringBuilder[i].ToString();

                if (char.IsLetterOrDigit(c, 0))
                {
                    stack.Push(c);
                }
                else if (c == "[")
                {
                    char join = stringBuilder[++i];
                    while (join != ']')
                    {
                        appendBuilder.Append(join);
                        join = stringBuilder[++i];
                    }

                    string result = appendBuilder.ToString();
                    result = "[" + result + "]";
                    stack.Push(result);
                    appendBuilder.Clear();
                }
                else
                {
                    switch (c) // Stack sırasına göre işlemler düzenlenir, second^first , ab için stack sırasına dikkat edilir. 
                    {
                        case "^":
                            string first = stack.Pop();
                            string second = stack.Pop();
                            string result = second + "^" + first;
                            stack.Push(result);
                            break;
                        case "*":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = second + "*" + first;
                            stack.Push(result);
                            break;
                        case "/":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = second + "/" + first;
                            stack.Push(result);
                            break;
                        case "+":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = second!=null?"{" + second + "+" + first + "}":first; // Stack tek eleman içermesi durumunda koşullar.
                            stack.Push(result);
                            break;
                        case "-":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = second!=null?"{" + second + "-" + first + "}":first;
                            stack.Push(result);
                            break;
                    }
                }
            }

            return stack.Pop();
        }
        public StringBuilder PostfixToInfix(StringBuilder stringBuilder)
        {
            StringBuilder appendBuilder = new StringBuilder();
            MyDynamicStack<string> stack = new MyDynamicStack<string>();
            int length = stringBuilder.Length;

            for (int i = 0; i < length; i++)
            {
                string c = stringBuilder[i].ToString();

                if (char.IsLetterOrDigit(c, 0))
                {
                    stack.Push(c);
                }
                else if (c == "[")
                {
                    char join = stringBuilder[++i];
                    while (join != ']')
                    {
                        appendBuilder.Append(join);
                        join = stringBuilder[++i];
                    }

                    string result = appendBuilder.ToString();
                    result = "[" + result + "]";
                    stack.Push(result);
                    appendBuilder.Clear();
                }
                else
                {
                    switch (c)
                    {
                        case "^":
                            string first = stack.Pop();
                            string second = stack.Pop();
                            string result = second + "^" + first;
                            stack.Push(result);
                            break;
                        case "*":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = second + "*" + first;
                            stack.Push(result);
                            break;
                        case "/":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = second + "/" + first;
                            stack.Push(result);
                            break;
                        case "+":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = second != null ? "{" + second + "+" + first + "}" : first;
                            stack.Push(result);
                            break;
                        case "-":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = second != null ? "{" + second + "-" + first + "}" : first;
                            stack.Push(result);
                            break;
                    }
                }
            }

            return appendBuilder.Append(stack.Pop());
        }
        public string PrefixToInfix(string stringBuilder)
        {
            StringBuilder appendBuilder = new StringBuilder();
            MyDynamicStack<string> stack = new MyDynamicStack<string>();
            int length = stringBuilder.Length;

            for (int i = length-1; i >=0; i--)
            {
                string c = stringBuilder[i].ToString();

                if (char.IsLetterOrDigit(c, 0))
                {
                    stack.Push(c);
                }
                else if (c == "]") //Tersten tarama yapıldığı için karşılaşılan parantez "]" olur.
                {
                    char join = stringBuilder[--i];
                    while (join != '[')
                    {
                        appendBuilder.Insert(0,join); //Tarama sırası dikkate alınır, Insert işlemi terimi baştan ekler.
                        join = stringBuilder[--i];
                    }
                    string result = appendBuilder.ToString();
                    result = "[" + result + "]";
                    stack.Push(result);
                    appendBuilder.Clear();
                }
                else
                {
                    switch (c)
                    {
                        case "^":
                            string first = stack.Pop();
                            string second = stack.Pop();
                            string result = first + "^" + second;
                            stack.Push(result);
                            break;
                        case "*":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = first + "*" + second;
                            stack.Push(result);
                            break;
                        case "/":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = first + "/" + second;
                            stack.Push(result);
                            break;
                        case "+":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = second != null ? "{" + first + "+" + second + "}" : first;
                            stack.Push(result);
                            break;
                        case "-":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = second != null ? "{" + first + "-" + second + "}" : first;
                            stack.Push(result);
                            break;
                    }
                }
            }

            return stack.Pop();
        }
        public StringBuilder PrefixToInfix(StringBuilder stringBuilder)
        {
            StringBuilder appendBuilder = new StringBuilder();
            MyDynamicStack<string> stack = new MyDynamicStack<string>();
            int length = stringBuilder.Length;

            for (int i = length - 1; i >= 0; i--)
            {
                string c = stringBuilder[i].ToString();

                if (char.IsLetterOrDigit(c, 0))
                {
                    stack.Push(c);
                }
                else if (c == "]")
                {
                    char join = stringBuilder[--i];
                    while (join != '[')
                    {
                        appendBuilder.Insert(0, join);
                        join = stringBuilder[--i];
                    }

                    string result = appendBuilder.ToString();
                    result = "[" + result + "]";
                    stack.Push(result);
                    appendBuilder.Clear();
                }
                else
                {
                    switch (c)
                    {
                        case "^":
                            string first = stack.Pop();
                            string second = stack.Pop();
                            string result = first + "^" + second;
                            stack.Push(result);
                            break;
                        case "*":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = first + "*" + second;
                            stack.Push(result);
                            break;
                        case "/":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = first + "/" + second;
                            stack.Push(result);
                            break;
                        case "+":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = second != null ? "{" + first + "+" + second + "}" : first;
                            stack.Push(result);
                            break;
                        case "-":
                            first = stack.Pop();
                            second = stack.Pop();
                            result = second != null ? "{" + first + "-" + second + "}" : first;
                            stack.Push(result);
                            break;
                    }
                }
            }

            return appendBuilder.Append(stack.Pop());
        }


    }
}


   
        
