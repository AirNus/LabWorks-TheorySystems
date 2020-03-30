using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabWork_2_TheorySystem
{
    class Symbol
    {
        public char symbol;
       public int frequency{ get; set; }
       public string cipher { get; set; }

    }
    class Program
    {
        static List<Symbol> Fragmentation(string Text)
        {
            // Коллекция уникальных символов текста (содержит в себе обьекты класса Symbol)
            List<Symbol> symbols = new List<Symbol>();
            for (int i = 0; i < Text.Length; i++)
            {              
                // Если в списке уже есть такой символ увеличить frequency на 1                
                var existsSymbolOnList = symbols.FirstOrDefault(x => x.symbol == Text[i]);
                if (existsSymbolOnList != null)
                {
                    existsSymbolOnList.frequency++;
                }
                else
                {
                    var newSymbol = new Symbol { symbol = Text[i], frequency = 1 };
                    symbols.Add(newSymbol);
                }
            }
            return symbols;
        }
     
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
            symbols = Fragmentation(Text);
            SortList(symbols);
            MethodShenonnaFano.Algoritm(symbols);
            OutputRezultInConsole(symbols);
            Console.ReadKey(true);
        }
    }

    class MethodShenonnaFano
    {
        public static int CountSymbolsInList(List<Symbol> symbols)
        {
            int count = 0;
            for (int i = 0; i < symbols.Count; i++)
            {
                count += symbols[i].frequency;
            }
            return count;
        }
        public static void Algoritm(List<Symbol> symbols)
        {
            List<Symbol> bufferOne = new List<Symbol>();
            List<Symbol> bufferSecond = new List<Symbol>();
            int delitel = 2;
            int count = CountSymbolsInList(symbols);
            int accum;
            accum = 0;            
            if(symbols.Count <= 1)
            {
                return;
            }
            for (int i = 0; i < symbols.Count; i++)
            {
                if (accum < count / delitel)
                {
                    symbols[i].cipher += "1";
                    accum += symbols[i].frequency;
                    bufferOne.Add(symbols[i]);
                }
                else
                {
                    bufferSecond.Add(symbols[i]);
                    symbols[i].cipher += "0";
                }
            }
            Algoritm(bufferOne);
            Algoritm(bufferSecond);
        }
    }
}
