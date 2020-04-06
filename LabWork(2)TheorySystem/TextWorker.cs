using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabWork_2_TheorySystem
{
    class TextWorker
    {
        public static List<Symbol> Fragmentation(string Text)
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
    }
}
