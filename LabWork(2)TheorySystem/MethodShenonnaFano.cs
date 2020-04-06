using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabWork_2_TheorySystem
{
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
            if (symbols.Count <= 1)
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
