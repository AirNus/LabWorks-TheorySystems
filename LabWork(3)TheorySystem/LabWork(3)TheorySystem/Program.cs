using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabWork_2_TheorySystem;

namespace LabWork_3_TheorySystem
{  
    class OutputRezultInConsole 
    {
        static public void OutputRezult(List<Symbol> symbols)
        {
            foreach (Symbol symbol in symbols)
            {
                Console.WriteLine(symbol.symbol + " - " + symbol.cipher);
            }
        }
    }

    class Sort
    {
        static public void SortListDesc(List<Symbol> symbols)
        {
            int last = symbols.Count;
            for (bool sorted = (last == 0); !sorted; --last)
            {
                sorted = true;
                for (int i = 1; i < last; i++)
                {
                    if (symbols[i - 1].frequency < symbols[i].frequency)
                    {
                        sorted = false;
                        var tmp = symbols[i - 1];
                        symbols[i - 1] = symbols[i];
                        symbols[i] = tmp;
                    }
                }
            }
        }
    }

    class Program
    {


        static void Main(string[] args)
        {            
            List<Symbol> symbols = new List<Symbol>();
            Console.WriteLine("Введите текст, который нужно зашифровать");
            string Text ="Это текст который я придумал для себя лично и дополнил для проверки моего алгоритма";// Console.ReadLine();
            symbols = TextWorker.Fragmentation(Text);
            Sort.SortListDesc(symbols);
            MethodShenonnaFano.Algoritm(symbols);
            OutputRezultInConsole.OutputRezult(symbols);

            CodingHemming.PreparingMessage(symbols,Text);

            Console.ReadKey(true);
        }
    }

    class CodingHemming
    {  
        const int lengthMessage = 11;

        static public void PreparingMessage(List<Symbol> symbols, string Text)
        {
            List<string> messages = new List<string>();
            string tmpMessage = "";
            string message = "";
            for(int i = 0; i < Text.Length; i++)
            {
                var tmp = symbols.First(x => x.symbol == Text[i]);
                message += tmp.cipher;
            }

          
            int lengthResidual = message.Length % lengthMessage;
            int lengthMain = message.Length - lengthResidual;
            for(int i = 0; i < lengthMain; i ++)
            {
                tmpMessage += message[i];
                if(tmpMessage.Length == lengthMessage)  
                {
                    messages.Add(tmpMessage);
                    tmpMessage = "";
                }
            }
            for(int i = 0; i < lengthResidual; i ++)
            {
                tmpMessage += message[lengthMain + i];
            }
            messages.Add(tmpMessage);
            AddControlByte(messages);
        }

        static public void AddControlByte(List<string> messages)
        {
            for(int i = 0; i < messages.Count; i++)
            {
                for(int j = 0; j <= (int) Math.Log(messages[i].Length,2); j++)
                {
                    int currNumber = (int) Math.Pow(2, j);
                    messages[i] = messages[i].Insert(currNumber - 1, "5");
                }

            }


            for (int i = 0; i < messages.Count; i++)
            {
                for (int j = 0; j <= (int)Math.Log(messages[i].Length, 2); j++)
                {
                    int currNumber = (int)Math.Pow(2, j);
                    char[] tmp = messages[i].ToCharArray();
                    int countUnits = 0;
                    for(int f = currNumber + 1 ; f < tmp.Length; f++)
                    {         
                        1   3   5   7   9    11    13    15
                          2 3     6 7     10 11       14 15
                              4 5 6 7           12 13 14 15
                                      8 9 10 11 12 13 14 15
                        if(tmp[f] == '1')
                        {
                            countUnits++;
                        }
                    }
                    if (countUnits % 2 == 0)
                    {

                        tmp[currNumber - 1] = '7';
                    }
                    else
                        tmp[currNumber - 1] = '8';
                    messages[i] = "";
                    foreach(char code in tmp)
                    {
                        messages[i] += code;
                    }
                }

            }
        }
    }
}
