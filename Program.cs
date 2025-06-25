using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GSM_Stribog
{
    class Program
    {
        static void Main(string[] args)
        {
            int state = -1;
            while(state != 6)
            {
                Console.WriteLine("1. Проверка на контрольнах примерах");
                Console.WriteLine("2. Хэшировать строку");
                Console.WriteLine("3. Хэшировать файл");
                Console.WriteLine("4. Построить коллизию с помощью базового алгоритма");
                Console.WriteLine("5. Постоить коллизию для итеративного алгоритма");
                Console.WriteLine("6. Построить осознанную коллизию");
                state = int.Parse(Console.ReadLine());
                switch (state)
                {
                    case 1:
                        Stribog stribog1 = new Stribog(512,"s");
                        byte[] message = HexToBytes("323130393837363534333231303938373635343332313039383736353433323130393837363534333231303938373635343332313039383736353433323130");
                        string expected = "486f64c1917879417fef082b3381a4e211c324f074654c38823a7b76f830ad00fa1fbae42b1285c0352f227524bc9ab16254288dd6863dccd5b9f54a1ad0541b";
                        byte[] actual = stribog1.GetHash(message);
                        Console.WriteLine("Пример 1");
                        Console.WriteLine();
                        Console.WriteLine("Получилось: " + BitConverter.ToString(actual).Replace("-", "").ToLower());
                        Console.WriteLine("Ожидали: "+ expected);
                        Check(BitConverter.ToString(actual).Replace("-", "").ToLower(), expected);

                        Stribog stribog2 = new Stribog(256,"s");
                        byte[] message1 = HexToBytes("323130393837363534333231303938373635343332313039383736353433323130393837363534333231303938373635343332313039383736353433323130");
                        string expected1 = "00557be5e584fd52a449b16b0251d05d27f94ab76cbaa6da890b59d8ef1e159d";
                        byte[] actual1 = stribog2.GetHash(message1);
                        Console.WriteLine("Пример 2");
                        Console.WriteLine();
                        Console.WriteLine("Получилось: " + BitConverter.ToString(actual1).Replace("-", "").ToLower());
                        Console.WriteLine("Ожидали: " + expected1);
                        Check(BitConverter.ToString(actual1).Replace("-", "").ToLower(), expected1);

                        byte[] message2 = HexToBytes("fbe2e5f0eee3c820fbeafaebef20fffbf0e1e0f0f520e0ed20e8ece0ebe5f0f2f120fff0eeec20f120faf2fee5e2202ce8f6f3ede220e8e6eee1e8f0f2d1202ce8f0f2e5e220e5d1");
                        string expected2 = "28fbc9bada033b1460642bdcddb90c3fb3e56c497ccd0f62b8a2ad4935e85f037613966de4ee00531ae60f3b5a47f8dae06915d5f2f194996fcabf2622e6881e";
                        byte[] actual2 = stribog1.GetHash(message2);
                        Console.WriteLine("Пример 3");
                        Console.WriteLine();
                        Console.WriteLine("Получилось: " + BitConverter.ToString(actual2).Replace("-", "").ToLower());
                        Console.WriteLine("Ожидали: " + expected2);
                        Check(BitConverter.ToString(actual2).Replace("-", "").ToLower(), expected2);

                        byte[] message3 = HexToBytes("fbe2e5f0eee3c820fbeafaebef20fffbf0e1e0f0f520e0ed20e8ece0ebe5f0f2f120fff0eeec20f120faf2fee5e2202ce8f6f3ede220e8e6eee1e8f0f2d1202ce8f0f2e5e220e5d1");
                        string expected3 = "508f7e553c06501d749a66fc28c6cac0b005746d97537fa85d9e40904efed29d";
                        byte[] actual3 = stribog2.GetHash(message2);
                        Console.WriteLine("Пример 4");
                        Console.WriteLine();
                        Console.WriteLine("Получилось: " + BitConverter.ToString(actual3).Replace("-", "").ToLower());
                        Console.WriteLine("Ожидали: " + expected3);
                        Check(BitConverter.ToString(actual3).Replace("-", "").ToLower(), expected3);

                        Console.WriteLine();
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                }
            }
            Console.ReadKey();
        }

        static byte[] HexToBytes(string hex_string)
        {
            byte[] result = new byte[hex_string.Length / 2];
            string current_byte = "";
            for (int i = 0; i < hex_string.Length / 2; ++i)
            {
                current_byte = hex_string.Substring(i * 2, 2);
                result[i] = Convert.ToByte(current_byte, 16);
            }

            return result;
        }

        static void Check(string a, string b)
        {
            bool f = true;
            for(int i=0;i< a.Length;i++)
            {
                if (a[i] != b[i])
                {
                    f = false;
                    break;
                }
            }
            if(f )
            {
                Console.WriteLine("Ok!");
            }
            else
            {
                Console.WriteLine("Error");
            }
        }
    }
}
