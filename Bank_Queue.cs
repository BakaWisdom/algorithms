using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_queue
{
    class Bank_Queue
    {
        static void Main(string[] args)
        {
            int closingTime;
            Person[] input = AcquireInput(out closingTime);

            long output = EvaluateInput(input, closingTime);

            Console.WriteLine(output);

            Console.ReadLine();
        }

        private static long EvaluateInput(Person[] input, int closingTime)
        {
            Array.Sort(input, (person1, person2) => person1.cash.CompareTo(person2.cash));
            // ascending order  first will be the lowest value last is the highest value

            int[] schedule = new int[closingTime];
            for (int i = input.Length - 1; i >= 0; i--)
            {
                int value = input[i].cash;
                for (int scheduleTime = input[i].patience ; scheduleTime >= 0 ; scheduleTime--)
                {
                    if (schedule[scheduleTime] < value)
                    {
                        int tempValue = schedule[scheduleTime];
                        schedule[scheduleTime] = value;
                        value = tempValue;

                        if(value == 0)
                        {
                            break;
                        }
                    }
                }
            }

            long output = 0;

            foreach (int patronCash in schedule)
            {
                output += patronCash;
            }

            return output;
        }

        private static Person[] AcquireInput(out int closingTime)
        {
            string thisLine = Console.ReadLine();
            string[] information = thisLine.Split(' ');

            int queueSize = int.Parse(information[0]);
            closingTime = int.Parse(information[1]);
            Person[] output = new Person[queueSize];

            for (int i = 0; i < queueSize; i++)
            {
                thisLine = Console.ReadLine();
                information = thisLine.Split(' ');

                output[i] = new Person( int.Parse(information[1]), int.Parse(information[0]) );
            }

            return output;
        }
    }

    internal class Person
    {
        public int patience;
        public int cash;

        public Person(int time, int worth)
        {
            patience = time;
            cash = worth;
        }
        

    }
}
