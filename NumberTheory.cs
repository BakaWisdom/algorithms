using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace PS7_6
{
    class NumberTheory
    {
        static void Main(string[] args)
        {
            List<int[]> instructions = AcquireInstructions();

            EvaluateInput(instructions);

            Console.Read();
        }

        private static void EvaluateInput(List<int[]> input)
        {
            foreach (int[] instructions in input)
            {
                switch(instructions[0])
                {
                    case 0:
                        Console.WriteLine( gcd(instructions[1], instructions[2]) );
                        break;

                    case 1:
                        Console.WriteLine( exp(instructions[1] % instructions[3], instructions[2], instructions[3]) );
                        break;

                    case 2:

                        int result = inverse(instructions[1], instructions[2]);

                        if(result == -1)
                        {
                            Console.WriteLine("none");
                        }
                        else
                        {
                            Console.WriteLine(result);
                        }
                        break;

                    case 3:

                        if(isprime(instructions[1]))
                        {
                            Console.WriteLine("yes");
                        }
                        else
                        {
                            Console.WriteLine("no");
                        }
                        break;

                    case 4:
                        key(instructions[1], instructions[2]);
                        break;
                }
            }
        }

        private static int gcd(int num1, int num2)
        {
            if(num2 == 0)
            {
                return num1;
            }

            return gcd(num2, num1 % num2);
        }

        private static int exp(int num, int exponent, int mod)
        {
            //note I took the mod before entering

            if(num == 0)
            {
                return num;
            }
            
            if( exponent == 1)
            {
                return num;
            }
            else
            {
                BigInteger subResult = (BigInteger) exp(num, exponent / 2, mod);
                BigInteger subSquared = (subResult * subResult);
                if ( exponent % 2 == 0)
                {
                    return (int) (subSquared % mod);
                }
                else
                {
                    int output = (int) ((subSquared * num) % mod);
                    return output;
                }
            }
        }

        private static int inverse(int num, int mod)
        {

            int[] result = ExtendedEuclidean(num, mod);

            if (result[2] != 1)
            {
                return -1;
            }

            int output = result[0];
            if (output < 0)
            {
                output += mod;
            }

            return output;

        }

        private static int[] ExtendedEuclidean(int num, int oldMod)
        {
            if (oldMod == 0)
            {
                return new int[3] { 1, 0, num };
            }

            int newMod = num % oldMod;
            while (newMod < 0)
            {
                newMod += oldMod;
            }

            int[] previousResult = ExtendedEuclidean(oldMod, newMod);

            int remainder = previousResult[1];
            int calculation = (previousResult[0] - previousResult[1] * (num / oldMod));

            return new int[3] { remainder, calculation, previousResult[2] };
        }

        private static bool isprime(int testNum)
        {

            int test5 = exp(5, testNum-1, testNum);
            int test3 = exp(3, testNum-1, testNum);
            int test2 = exp(2, testNum-1, testNum);


            if (test2 != 1 || test3 != 1 || test5 !=1)
            {
                return false;
            }

            return true;
        }

        private static void key(int num1, int num2)
        {
            BigInteger publicMod = num1 * (BigInteger) num2;
            Console.Write(publicMod + " ");

            BigInteger encryptionMod = (num1 - 1) * ( (BigInteger) num2 - 1);

            int publicKey = 2;
            while(gcdBig(++publicKey, encryptionMod) != 1)
            {
                //Point of loop is in the check
            }

            Console.Write(publicKey + " ");

            //overflow in encryptionMod figure out how to deal
            BigInteger privateKey = inverseBig(publicKey, encryptionMod);
            Console.WriteLine(privateKey);
        }

        /// <summary>
        /// Simple does the gcd algorithm until numbers are small enough to use int instead
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        private static int gcdBig(BigInteger num1, BigInteger num2)
        {
            if (num1 <= int.MaxValue && num2 <= int.MaxValue)
            {
                return gcd( (int) num1, (int) num2);
            }

            return gcdBig(num2, num1 % num2);
        }

        /// <summary>
        /// Basically inverse method for modular arithmetic but also able to accept numbers > int.max
        /// </summary>
        /// <param name="num"></param>
        /// <param name="mod"></param>
        /// <returns></returns>
        private static BigInteger inverseBig(BigInteger num, BigInteger mod)
        {
            BigInteger[] result = ExtendedEuclideanBig(num, mod);
            

            if (result[2] != 1)
            {
                return -1;
            }

            BigInteger output = result[0];
            if (output < 0)
            {
                output += mod;
            }

            return output;

        }

        /// <summary>
        /// ExtendedEuclideanBig takes in BigIntegers and does Extended Euclidean until the BigInts can be converted
        /// to ints.  At this point it switches to the regular Extended Euclidean method.
        /// </summary>
        /// <param name="num"></param>
        /// <param name="oldMod"></param>
        /// <returns></returns>
        private static BigInteger[] ExtendedEuclideanBig(BigInteger num, BigInteger oldMod)
        {

            BigInteger[] previousResult;
            BigInteger newMod;

            if (num <= int.MaxValue && oldMod <= int.MaxValue)
            {
                int[] smallResult;
                smallResult = ExtendedEuclidean((int)num, (int)oldMod);

                previousResult = new BigInteger[3];

                for (int i = 0; i < smallResult.Length; i++)
                {
                    previousResult[i] = (BigInteger)smallResult[i];
                }

                //returning now to prevent double jeopardy on calculations
                return previousResult;
            }
            else
            {
                newMod = num % oldMod;

                while (newMod < 0)
                {
                    newMod += oldMod;
                }

                previousResult = ExtendedEuclideanBig(oldMod, newMod);
            }

            BigInteger remainder = previousResult[1];
            BigInteger calculation = (previousResult[0] - previousResult[1] * (num / oldMod));

            return new BigInteger[3] { remainder, calculation, previousResult[2] };
        }

        private static List<int[]> AcquireInstructions()
        {
            List<int[]> output = new List<int[]>();

            string thisLine = Console.ReadLine();

            while (!string.IsNullOrEmpty(thisLine))
            {
                string[] description = thisLine.Split(' ');

                int mod;
                int num;
                int num2;
                int exponent;

                switch (description[0])
                {
                    case "gcd":
                        //code 0
                        num = int.Parse(description[1]);
                        num2 = int.Parse(description[2]);
                        output.Add(new int[3] { 0, num, num2 });
                        break;

                    case "exp":
                        //code 1
                        num = int.Parse(description[1]);
                        exponent = int.Parse(description[2]);
                        mod = int.Parse(description[3]);
                        output.Add(new int[4] { 1, num, exponent, mod });
                        break;

                    case "inverse":
                        //code 2
                        num = int.Parse(description[1]);
                        mod = int.Parse(description[2]);
                        output.Add(new int[3] { 2, num, mod });
                        break;

                    case "isprime":
                        //code 3
                        num = int.Parse(description[1]);
                        output.Add(new int[2] { 3, num });
                        break;

                    case "key":
                        //code 4
                        num = int.Parse(description[1]);
                        num2 = int.Parse(description[2]);
                        output.Add(new int[3] { 4, num, num2 });
                        break;

                }

                thisLine = Console.ReadLine();
            }

            return output;
        }
    }
}
