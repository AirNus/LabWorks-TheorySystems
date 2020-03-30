using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ConsoleApplication2
{
    class Cipher
    {
        public string word { get; set; }
        public string code { get; set; }
        public double probability { get; set; }
    }
    class Program
    {
        static void EnterWord(List<Cipher> cipher)
        {
            Console.WriteLine("Введите текст, который нужно преобразовать в код");
            string Text = Console.ReadLine();
            int numerator; // Используется для расчета вхождений буквы в тексте 
            char buffer; // Хранит в себе символы текста
            for (int i = 0; i < Text.Length; i++)
            {
                numerator = 0;
                buffer = Text[i];
                for (int counter = 0; counter < cipher.Count; counter++)
                {
                    // Если данная буква уже была отработана
                    if (buffer.ToString() == cipher[counter].word)
                        buffer = '~';
                }
                for (int j = 0; j < Text.Length; j++)
                {
                    if (buffer == Text[j])
                        numerator++;
                }
                if (buffer != '~')
                {
                    // Создаем новый объект который хранит данные текущей буквы
                    Cipher row = new Cipher()
                    {
                        word = buffer.ToString(),
                        probability = (double) numerator / Text.Length // вычисляем вероятность ее вхождения
                    };
                    // Добавляем отработанную букву и ее данные в список
                    cipher.Add(row);
                }
            }
        }
        static void sort(List<Cipher> cipher)
        {
            var buffer = new Cipher(); // Создаем временную коллекцию для хранения элементов списка cipher
            for (int i = 0; i < cipher.Count; i++)
            {
                for (int j = 0; j < cipher.Count; j++)
                {
                    // Если вероятность текущего элемента больше остальных
                    if (cipher[i].probability > cipher[j].probability)
                    {
                        // Меняем элементы местами
                        buffer = cipher[i];
                        cipher[i] = cipher[j];
                        cipher[j] = buffer;
                    }
                }
            }

        }
        static void Union(ref List<Cipher> cipher)
        {
            List<Cipher> first_buffer = new List<Cipher>(cipher);           
            int index_penult = 0, 
                index_min = 0;
            double min_probablty,
                   penult_probablty;
            // Начинаем обьединять буквы в группы по их частота
            // Пока частота группы не равна 1
            while (first_buffer.Count > 0 && first_buffer[0].probability < 1)
            {
                sort(first_buffer);
                min_probablty = 1; // присваем переменной максимальную частоту для поиска буквы с минимальной частотой
                for (int i = 0; i < first_buffer.Count; i++)
                {
                    // Ищем букву с  минимальной частотой
                    if (first_buffer[i].probability <= min_probablty)
                    {
                        index_min = i;
                        min_probablty = first_buffer[i].probability;
                    }
                }
                penult_probablty = 1; // тоже самое только вместо минимальной ищем  ту что идет за ней либо равна минимальной
                for (int i = 0; i < first_buffer.Count; i++)
                {
                    if (first_buffer[i].probability <= penult_probablty && i != index_min)
                    {
                        index_penult = i;
                        penult_probablty = first_buffer[i].probability;
                    }
                }
                for (int i = 0; i < cipher.Count; i++)
                {
                    // Если в сформированной группе букв есть текущая буква ей присвается код
                    // по такому правилу
                    // Минимальной присваивается 0
                    // Следующей за минимальной присваивается 1
                    if (first_buffer[index_min].word.Contains(cipher[i].word))
                    {
                        cipher[i].code += "0";
                    }
                    if (first_buffer[index_penult].word.Contains(cipher[i].word))
                    {
                        cipher[i].code += "1";
                    }
                }
                first_buffer.Add(new Cipher
                {
                    probability = min_probablty + penult_probablty,
                    word = first_buffer[index_min].word + first_buffer[index_penult].word
                });
                first_buffer.Remove(first_buffer[index_min]);
                first_buffer.Remove(first_buffer[index_penult]);
            }
        }
        public static void ReverseCode(List<Cipher> cipher)
        {
            char[] buff;
            for (int i = 0; i < cipher.Count; i++)
            {
                buff = cipher[i].code.ToCharArray();
                Array.Reverse(buff);
                cipher[i].code = "";
                for (int j = 0; j < buff.Length; j++)
                {
                    cipher[i].code += buff[j];
                }
            }
        }
        static void Main(string[] args)
        {           
            List<Cipher> cipher = new List<Cipher>();            
            EnterWord(cipher);
            sort(cipher);
            Union(ref cipher);
            ReverseCode(cipher);
            foreach (var item in cipher)
            {
                Console.WriteLine("Символ: " + item.word + " преобразован в код: " + item.code);
            }
            Console.ReadKey();
        }
    }
}