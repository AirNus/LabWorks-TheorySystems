using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabWork_2_TheorySystem
{
    
    class Program
    {   
        public static void OutputRezultInConsole(List<Symbol> symbols)
        {
            foreach (Symbol symbol in symbols)
            {
                Console.WriteLine(symbol.symbol + " - " + symbol.cipher);
            }
        }

        static void SortList(List<Symbol> symbols)
        {
            int KolElementsList = symbols.Count; 
            for(Boolean sorted = (KolElementsList == 0); sorted == false; KolElementsList --)
            {
                sorted = true;
                for(int i = 1; i < KolElementsList; i++)
                {
                    if(symbols[i-1].frequency < symbols[i].frequency)
                    {
                        sorted = false;

                        var buffer = symbols[i];
                        symbols[i] = symbols[i - 1];
                        symbols[i - 1] = buffer;
                    }
                }
            }           
        }

        static void Main(string[] args)
        {
            List<Symbol> symbols = new List<Symbol>();
            Console.WriteLine("Введите текст, который нужно зашифровать");
            string Text = Console.ReadLine();
            symbols = TextWorker.Fragmentation(Text);
            SortList(symbols);
            MethodShenonnaFano.Algoritm(symbols);
            OutputRezultInConsole(symbols);
            Console.ReadKey(true);
        }
    }

}
