using GSM_Stribog;
using System.Linq;
using System.Text;
using System;

public class MeaningfulCollisionFinder
{
    public static (string, string, byte[]) FindMeaningfulCollision(
    string msgText1,
    string msgText2,
    int bytesToMatch,  // байты
    int maxIterations = 1000000)
    {
        Console.WriteLine($"Base messages: \"{msgText1}\" and \"{msgText2}\"");

        var hasher = new Streebog(512, "s");

        for (int counter = 0; counter < maxIterations; counter++)
        {
            if (counter % 1000 == 0)
                Console.WriteLine($"Trying iteration {counter}");

            string suffix1 = counter.ToString();
            string suffix2 = (counter + 1).ToString();

            string fullMsg1 = msgText1 + suffix1;
            string fullMsg2 = msgText2 + suffix2;

            byte[] hash1 = hasher.GetHash(Encoding.UTF8.GetBytes(fullMsg1));
            byte[] hash2 = hasher.GetHash(Encoding.UTF8.GetBytes(fullMsg2));

            bool match = true;
            for (int i = 0; i < bytesToMatch; i++)  // сравниваем байты
            {
                if (hash1[i] != hash2[i])
                {
                    match = false;
                    break;
                }
            }

            if (match)
            {
                Console.WriteLine("Collision found!");
                byte[] commonPrefix = hash1.Take(bytesToMatch).ToArray();

                Console.WriteLine($"Message 1: {fullMsg1}");
                Console.WriteLine($"Message 2: {fullMsg2}");
                Console.WriteLine($"Common hash prefix: {BitConverter.ToString(commonPrefix)}");

                return (fullMsg1, fullMsg2, commonPrefix);
            }
        }

        throw new Exception($"Collision not found in {maxIterations} iterations.");
    }

}
