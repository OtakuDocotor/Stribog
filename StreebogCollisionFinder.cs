using GSM_Stribog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GSM_Stribog
{
    public class StreebogCollisionFinder
    {
        public static (byte[], byte[]) FindCollisionBasic(int nBits, int maxIterations = 1000000)
        {
            int nBytes = (nBits + 7) / 8;
            var seen = new Dictionary<string, byte[]>(); // коллекция существующих хэш/сообщение

            var rng = RandomNumberGenerator.Create();

            for (int i = 0; i < maxIterations; i++)
            {
                byte[] message = new byte[64];
                rng.GetBytes(message);

                var hasher = new Streebog(512, "s");
                byte[] fullHash = hasher.GetHash(message);
                byte[] truncatedHash = fullHash.Take(nBytes).ToArray();

                string hashKey = Convert.ToBase64String(truncatedHash);

                if (seen.TryGetValue(hashKey, out byte[] existing))
                {
                    if (!existing.SequenceEqual(message))
                    {
                        return (existing, message);
                    }
                }
                else
                {
                    seen[hashKey] = message;
                }
            }

            throw new Exception($"Collision not found in {maxIterations} iterations");
        }

        public static (byte[], byte[]) FindCollisionIterative(int nBits, int maxIterations = 100000)
        {
            int nBytes = (nBits + 7) / 8;
            var chain = new Dictionary<string, byte[]>();

            var rng = RandomNumberGenerator.Create();
            byte[] current = new byte[64];
            rng.GetBytes(current);

            for (int i = 0; i < maxIterations; i++)
            {
                var hasher = new Streebog(512, "s");
                byte[] fullHash = hasher.GetHash(current);
                byte[] truncated = fullHash.Take(nBytes).ToArray();

                string hashKey = Convert.ToBase64String(truncated);

                if (chain.TryGetValue(hashKey, out byte[] existing))
                {
                    if (!existing.SequenceEqual(current))
                    {
                        return (existing, current);
                    }
                }
                else
                {
                    chain[hashKey] = current;
                }

                byte[] next = new byte[64];
                Buffer.BlockCopy(fullHash, 0, next, 0, Math.Min(fullHash.Length, next.Length));

                if (fullHash.Length < next.Length)
                {
                    byte[] remaining = new byte[next.Length - fullHash.Length];
                    rng.GetBytes(remaining);
                    Buffer.BlockCopy(remaining, 0, next, fullHash.Length, remaining.Length);
                }

                current = next;

                //current = truncated;
            }

            throw new Exception($"Collision not found in {maxIterations} iterations");
        }
    }
}