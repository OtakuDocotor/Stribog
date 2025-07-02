using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace GSM_Stribog
{
    class Program
    {
        static void Main(string[] args)
        {
            int state = -1;
            while(state < 7)
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
                        Streebog stribog1 = new Streebog(512,"s");
                        byte[] message = HexToBytes("323130393837363534333231303938373635343332313039383736353433323130393837363534333231303938373635343332313039383736353433323130");
                        string expected = "486f64c1917879417fef082b3381a4e211c324f074654c38823a7b76f830ad00fa1fbae42b1285c0352f227524bc9ab16254288dd6863dccd5b9f54a1ad0541b";
                        byte[] actual = stribog1.GetHash(message);
                        Console.WriteLine();

                        Console.WriteLine("---Control value 1---");
                        Console.WriteLine("Real    : " + BitConverter.ToString(actual).Replace("-", "").ToLower());
                        Console.WriteLine("Expected: "+ expected);
                        Check(BitConverter.ToString(actual).Replace("-", "").ToLower(), expected);

                        Streebog stribog2 = new Streebog(256,"s");
                        byte[] message1 = HexToBytes("323130393837363534333231303938373635343332313039383736353433323130393837363534333231303938373635343332313039383736353433323130");
                        string expected1 = "00557be5e584fd52a449b16b0251d05d27f94ab76cbaa6da890b59d8ef1e159d";
                        byte[] actual1 = stribog2.GetHash(message1);
                        Console.WriteLine();

                        Console.WriteLine("---Control value 2---");
                        Console.WriteLine("Real    : " + BitConverter.ToString(actual1).Replace("-", "").ToLower());
                        Console.WriteLine("Expected: " + expected1);
                        Check(BitConverter.ToString(actual1).Replace("-", "").ToLower(), expected1);

                        byte[] message2 = HexToBytes("fbe2e5f0eee3c820fbeafaebef20fffbf0e1e0f0f520e0ed20e8ece0ebe5f0f2f120fff0eeec20f120faf2fee5e2202ce8f6f3ede220e8e6eee1e8f0f2d1202ce8f0f2e5e220e5d1");
                        string expected2 = "28fbc9bada033b1460642bdcddb90c3fb3e56c497ccd0f62b8a2ad4935e85f037613966de4ee00531ae60f3b5a47f8dae06915d5f2f194996fcabf2622e6881e";
                        byte[] actual2 = stribog1.GetHash(message2);
                        Console.WriteLine();

                        Console.WriteLine("---Control value 3---");
                        Console.WriteLine("Real    : " + BitConverter.ToString(actual2).Replace("-", "").ToLower());
                        Console.WriteLine("Expected: " + expected2);
                        Check(BitConverter.ToString(actual2).Replace("-", "").ToLower(), expected2);

                        byte[] message3 = HexToBytes("fbe2e5f0eee3c820fbeafaebef20fffbf0e1e0f0f520e0ed20e8ece0ebe5f0f2f120fff0eeec20f120faf2fee5e2202ce8f6f3ede220e8e6eee1e8f0f2d1202ce8f0f2e5e220e5d1");
                        string expected3 = "508f7e553c06501d749a66fc28c6cac0b005746d97537fa85d9e40904efed29d";
                        byte[] actual3 = stribog2.GetHash(message2);
                        Console.WriteLine();

                        Console.WriteLine("---Control value 4---");
                        Console.WriteLine("Real    : " + BitConverter.ToString(actual3).Replace("-", "").ToLower());
                        Console.WriteLine("Expected: " + expected3);
                        Check(BitConverter.ToString(actual3).Replace("-", "").ToLower(), expected3);

                        Console.WriteLine();
                        break;
                    case 2:
                        break;
                    case 3:
                        Streebog str1 = new Streebog(512, "s");
                        Streebog str2 = new Streebog(512, "f");
                        byte[] input = File.ReadAllBytes("WarAndWorld.txt");
                        Stopwatch sw1 = new Stopwatch();
                        Console.WriteLine("---Slow version---");
                        sw1.Start();
                        Console.WriteLine($"Total hash: {BitConverter.ToString(str1.GetHash(input)).Replace("-", "").ToLower()}");
                        sw1.Stop();
                        Console.WriteLine($"Total time: {sw1.ElapsedMilliseconds} ms");
                        Console.WriteLine("---Fast version---");
                        sw1.Restart();
                        Console.WriteLine($"Total hash: {BitConverter.ToString(str2.GetHash(input)).Replace("-", "").ToLower()}");
                        sw1.Stop();
                        Console.WriteLine($"Total time: {sw1.ElapsedMilliseconds} ms");
                        Console.WriteLine();
                        break;
                    case 4:
                        int n = 24;
                        var (msg1, msg2) = StreebogCollisionFinder.FindCollisionBasic(n);

                        Console.WriteLine("---Collision found!---");
                        Console.WriteLine($"Message 1: {BitConverter.ToString(msg1)}");
                        Console.WriteLine($"Message 2: {BitConverter.ToString(msg2)}");

                        // Проверка
                        var hasher_basic = new Streebog(512, "s");
                        byte[] hash1 = hasher_basic.GetHash(msg1);
                        byte[] hash2 = hasher_basic.GetHash(msg2);

                        Console.WriteLine($"Truncated hash 1: {BitConverter.ToString(hash1.Take(n / 8 + 1).ToArray())}");
                        Console.WriteLine($"Truncated hash 2: {BitConverter.ToString(hash2.Take(n / 8 + 1).ToArray())}");
                        Console.WriteLine();
                        break;
                    case 5:
                        n = 24;
                        var (msg_1, msg_2) = StreebogCollisionFinder.FindCollisionIterative(n);

                        Console.WriteLine("---Collision found!---");
                        Console.WriteLine($"Message 1: {BitConverter.ToString(msg_1)}");
                        Console.WriteLine($"Message 2: {BitConverter.ToString(msg_2)}");

                        // Проверка
                        var hasher_iter = new Streebog(512, "s");
                        byte[] hash_1 = hasher_iter.GetHash(msg_1);
                        byte[] hash_2 = hasher_iter.GetHash(msg_2);

                        Console.WriteLine($"Truncated hash 1: {BitConverter.ToString(hash_1.Take(n / 8 + 1).ToArray())}");
                        Console.WriteLine($"Truncated hash 2: {BitConverter.ToString(hash_2.Take(n / 8 + 1).ToArray())}");
                        Console.WriteLine();
                        break;
                    case 6:
                        MeaningfulCollisionFinder.FindMeaningfulCollision("Streebog", "Sdvaabog", 2);
                        break;
                    default:
                        Titles();
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
                Console.WriteLine("Result: OK");
            }
            else
            {
                Console.WriteLine("Result: Error");
            }
        }
        #region }
        public static void Titles()
        {
            Console.Clear();
            Console.WriteLine("Thank you for your attention! We need a salary from the University, please postav`te 4! Mi vas lyubim! <3");
            Console.WriteLine();

            Console.WriteLine("Над проектoм работали:");
            List<string> developers = new List<string>() { "Ilya Sokolov aka Agent JRU", "Ilya Gusev aka ZhateckyGus",
                            "Dmitriy Milkov aka Small code", "Deepseek aka human", "ChatGPT aka woman", "GitHub Repositories", "And Others",
                            "with supporting from Eseniya", "with supporting from Gleb",
                            "with supporting from Konstantin", "under the leadership A. Belov"};

            foreach (var developer in developers)
            {
                Console.WriteLine(developer);
                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine("");
                    Thread.Sleep(1000);
                }
            }
            Console.WriteLine("To be continued...");
        }
        #endregion
    }
}
