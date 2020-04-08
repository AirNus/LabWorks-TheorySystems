using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using LabWork_2_TheorySystem;

namespace LabWork_3_TheorySystem
{  

    // Вывод результатов кодирования методом Шеннона-Фано на консоль
    class OutputRezultInConsole 
    {
        static public void OutputRezult(List<Symbol> symbols)
        {
            Console.WriteLine("Символы в сообщении закодировались следующим кодом");
            foreach (Symbol symbol in symbols)
            {
                Console.WriteLine(symbol.symbol + " - " + symbol.cipher);
            }
            Console.WriteLine("Нажмите клавишу для продолжения...");
            Console.ReadKey(true);
            Console.Clear();
        }
    }

    class Sort
    {
        //Сортировка коллекции символов по частоте пузырьковым методом
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

    class Decoder
    {
        // Обработка полученного двоичного сообщения и преобразование в исходный текст
        static public void DecoderMethShenonnaFano(List<Symbol> symbols, string Message)
        {
            string OriginalText = "";
            string tmp = "";
            for(int i = 0; i < Message.Length; i++)
            {
                // В переменную tmp посимвольно добавляется код из сообщения
                tmp += Message[i];
                // Проверяет есть ли среди закодированных символов, символ с текущим кодом
                var tmpSymbol = symbols.FirstOrDefault(x => x.cipher == tmp);
                if (tmpSymbol != null)
                {
                    tmp = "";
                    OriginalText += tmpSymbol.symbol;
                }
            }
            Console.WriteLine("Сообщение, которое высветилось у получателя:");
            Console.WriteLine(OriginalText);
        }
    }
    class Program
    {


        static void Main(string[] args)
        {            
            List<Symbol> symbols = new List<Symbol>();
            Console.WriteLine("Введите текст, который нужно зашифровать");
            string Text = Console.ReadLine();//"Это текст который я придумал для себя лично"; 
            // Обработка исключения связанного с неккоректным вводом 
            if (Text.Length < 2)
            {
                Console.WriteLine("Вы ввели слишком короткий текст");
                Console.WriteLine("Вместо него будет введено слово 'Тортик'\nПотому что тортик это клево :)");
                Text = "Тортик";
            }
            // Кодируем текст методом Шеннона Фано (код подключен из проекта прошлой лабораторной)
            symbols = TextWorker.Fragmentation(Text);
            Sort.SortListDesc(symbols);
            MethodShenonnaFano.Algoritm(symbols);
            OutputRezultInConsole.OutputRezult(symbols);

            // Кодируем исходный текст и фасуем его по сообщениям определенной длины
            List<string> messages = new List<string>();
            messages = CodingHemming.PreparingMessage(symbols, Text);

            Console.WriteLine("Нажмите клавишу для продолжения");
            Console.ReadKey(true);
            Console.Clear();

            //Эмуляция отправки сообщения с одной ошибкой
            CodingHemming.SendingMessageEmulation(messages);

            // Повторный расчет контрольных бит
            CodingHemming.CalculationControlBits(messages);
            // Превращаем полученное сообщение обратно в текст
            string Message = CodingHemming.DeleteControlBits(messages);
            Decoder.DecoderMethShenonnaFano(symbols, Message);

            Console.ReadKey(true);

        }
    }

    class CodingHemming
    {
        // Константа отвечающая за длину сообщений, можно менять
        const int lengthMessage = 11;


        // Превращение текста в сообщения для отправки
        static public List<string> PreparingMessage(List<Symbol> symbols, string Text)
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

            // Добавляем контрольные биты
            AddControlBits(messages);

            // Расчет контрольных бит (первичный)
            CalculationControlBits(messages);
            //Вывод результатов в консоль
            OutputMessagesInConsole(messages);

            return messages;
        }

        // Функция добавления исходгых бит (без расчета) на их места
        static public void AddControlBits(List<string> messages)
        {
            for(int i = 0; i < messages.Count; i++)
            {
                for(int j = 0; j <= (int) Math.Log(messages[i].Length,2); j++)
                {
                    int currNumber = (int) Math.Pow(2, j);
                    messages[i] = messages[i].Insert(currNumber - 1, "5");
                }

            }          
        }

        //Расчет контрольных бит
        static public void CalculationControlBits(List<string> messages)
        {
            List<int> incorrectBits = new List<int>();
            int numberIncorrectMessage = 0;
            for (int i = 0; i < messages.Count; i++)
            {
                char[] tmp = messages[i].ToCharArray();              
                for (int j = 0; j <= (int)Math.Log(messages[i].Length, 2); j++)
                {
                    int currNumber = (int)Math.Pow(2, j);
                    
                    int countUnits = 0;
                    for (int counter = currNumber - 1; counter < tmp.Length; counter += currNumber)
                    {
                        // Пример прохода цикла (для 4 контрольных бит)
                        //1   3   5   7   9    11    13    15   
                        //  2 3     6 7     10 11       14 15 
                        //      4 5 6 7           12 13 14 15
                        //              8 9 10 11 12 13 14 15
                        int step = 0;
                        while (step < currNumber)
                        {
                            // Считаем количество единиц для текущего контрольного бита
                            try
                            {
                                if (tmp[counter] == '1')
                                {
                                    countUnits++;
                                }
                            }
                            catch(IndexOutOfRangeException)
                            {
                                break;
                            }
                            step++;
                            counter++;
                        }
                    }
                    // Если количество единиц четно
                    if (countUnits % 2 == 0)
                    {
                        // Если фактический контрольный бит отличается от только что высчитанного (для повторного расчета)
                        if (tmp[currNumber - 1] == '7')
                        {
                            numberIncorrectMessage = i;
                            incorrectBits.Add(currNumber);
                            continue;
                        }
                        tmp[currNumber - 1] = '8';
                    }
                    // Если количество единиц нечетно
                    else if (countUnits % 2 == 1)
                    {
                        // Если фактический контрольный бит отличается от только что высчитанного (для повторного расчета)
                        if (tmp[currNumber - 1] == '8')
                        {
                            numberIncorrectMessage = i;
                            incorrectBits.Add(currNumber);
                            continue;
                        }
                        tmp[currNumber - 1] = '7';
                    }                 
                    // Меняем изначальное сообщение на сообщение с высчитанными контрольными битами
                        messages[i] = "";
                    foreach (char byteCode in tmp)
                    {
                        messages[i] += byteCode;
                    }
                }
            }
            // Функция по замене поврежденного при отправке бита
            ReplacingDamagedBit(incorrectBits, messages, numberIncorrectMessage);
        }

        // Функция по замене поврежденного при отправке бита
        static void ReplacingDamagedBit(List<int> incorrectBits,List<string> messages,int numberIncorrectMessage)
        {
            // Если были неккоректные биты (коллекция с номерами неправильных контрольных битов не пуста)
            if (incorrectBits.Count > 0)
            {
                char[] tmp = messages[numberIncorrectMessage].ToCharArray();
                int IncorrectBitPosition = 0;
                // Получаем позицию поврежденного бита
                foreach (int number in incorrectBits)
                {
                    IncorrectBitPosition += number;
                }
                Console.WriteLine("Поврежден бит под номером " + IncorrectBitPosition + " в сообщении под номером " + (numberIncorrectMessage + 1));
                IncorrectBitPosition--;
                // Меняем его на противоположный 
                if (tmp[IncorrectBitPosition] == '0')
                {
                    tmp[IncorrectBitPosition] = '1';
                }
                else if (tmp[IncorrectBitPosition] == '1')
                {
                    tmp[IncorrectBitPosition] = '0';
                }
                else if (tmp[IncorrectBitPosition] == '7')
                {
                    tmp[IncorrectBitPosition] = '8';
                }
                else if (tmp[IncorrectBitPosition] == '8')
                {
                    tmp[IncorrectBitPosition] = '7';
                }
                messages[numberIncorrectMessage] = "";
                foreach (char byteCode in tmp)
                {
                    messages[numberIncorrectMessage] += byteCode;
                }
            }
        }

        // Вывод результатов на консоль
        static void OutputMessagesInConsole(List<string> messages)
        {
            Console.WriteLine("Полученные сообщения:");
            foreach(string message in messages)
            {
                Console.WriteLine("\t"+message);
            }
        }

        // Эмуляция отправки сообщения и повреждения одного из бит
        static public void SendingMessageEmulation(List<string> messages)
        {
            Console.WriteLine("Выберите 1 из " + messages.Count + " сообщений и введите его номер");
            int numberMessage = Convert.ToInt32(Console.ReadLine()) - 1;
            // Обработка неправильного ввода номера сообщения
            try
            {
                Console.WriteLine("Выбранное сообщение: " + messages[numberMessage]);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Вы ввели неккоректный номер. Значению будет присвоено значение '1'");
                numberMessage = 0;
                Console.WriteLine("Полученное сообщение: " + messages[numberMessage]);
            }
            
            Console.WriteLine("Какой бит вы бы хотели изменить?\nВведите номер от 1 до " + messages[numberMessage].Length);
            int numberByteInMessage = Convert.ToInt32(Console.ReadLine()) - 1;
            char selectedBit = '0';
            // Обработка неправильного номера бита
            try
            {
               selectedBit = messages[numberMessage].ElementAt(numberByteInMessage);
            }
            catch(ArgumentOutOfRangeException)
            {
                Console.WriteLine("Вы ввели неккоректный номер. Значению будет присвоено значение '1'");
                numberByteInMessage = 0;
            }            
            messages[numberMessage] = messages[numberMessage].Remove(numberByteInMessage, 1);
            if (selectedBit == '0')
                selectedBit = '1';
            else if (selectedBit == '8')
                selectedBit = '7';
            else if (selectedBit == '1')
                selectedBit = '0';
            else if (selectedBit == '7')
                selectedBit = '8';
            else
            {
                Console.WriteLine("Произошла ошибка!Выбранный бит содержал неверный формат");
                return;
            }
            messages[numberMessage] = messages[numberMessage].Insert(numberByteInMessage, selectedBit.ToString());
            Console.WriteLine("Измененное сообщение: " + messages[numberMessage]);
            Console.WriteLine("Нажмите клавишу для продолжения");           
            Console.ReadKey(true);
            Console.Clear();
        }

        // Функция по удалению контрольных бит перед декодированием
        static public string DeleteControlBits(List<string> messages)
        {
            for (int i = 0; i < messages.Count; i++)
            {
                messages[i] = messages[i].Replace("7", "");
                messages[i] = messages[i].Replace("8", "");
            }
            string Message = "";
            foreach (string message in messages)
            {
                Message += message;
            }
            return Message;
        }
    }
}
