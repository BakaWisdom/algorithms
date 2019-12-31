using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS5_6
{
    class RumorMill
    {
        static void Main(string[] args)
        {
            Dictionary<string, SortedSet<string>> friendships;
            SortedSet<string> students;
            string[] rumorStarters = AcquireInput(out students, out friendships);
            string[][] result = EvaluateInput(rumorStarters, students, friendships);

            WriteRumorOutput(result[0]);
            foreach (string[] involvedStudents in result.Skip(1))
            {
                Console.WriteLine();
                WriteRumorOutput(involvedStudents);
            }

            Console.ReadLine();
        }

        private static void WriteRumorOutput(string[] involvedStudents)
        {
            Console.Write(involvedStudents[0]);
            foreach (string student in involvedStudents.Skip(1))
            {
                Console.Write(" " + student);
                
            }
        }

        private static string[][] EvaluateInput(string[] rumorStarters, SortedSet<string> students, Dictionary<string, SortedSet<string>> friendships)
        {
            string[][] output = new string[rumorStarters.Length][];

            for (int i = 0; i < rumorStarters.Length; i++)
            {
                string starter = rumorStarters[i];
                string[] thisRumor = new string[students.Count];

                HashSet<string> studentCheckOff = new HashSet<string>();

                thisRumor[0] = starter;
                studentCheckOff.Add(starter);

                List<SortedSet<string>> days = new List<SortedSet<string>>();

                {
                    SortedSet<string> today = new SortedSet<string>();

                    foreach (string friend in friendships[starter])
                    {
                        today.Add(friend);
                        studentCheckOff.Add(friend);
                    }

                    days.Add(today);
                }

                
                for (int currentDay = 0; currentDay < days.Count; currentDay++)
                {
                    SortedSet<string> today = new SortedSet<string>();

                    foreach (string studentInTheKnow in days[currentDay])
                    {
                        foreach (string friend in friendships[studentInTheKnow])
                        {
                            if(!studentCheckOff.Contains(friend))
                            {
                                today.Add(friend);
                                studentCheckOff.Add(friend);
                            }
                        }
                    }
                    if (today.Count > 0)
                    {
                        days.Add(today);
                    }
                }

                int currentStudentNum = 1;
                foreach (SortedSet<string> day in days)
                {
                    foreach (string student in day)
                    {
                        thisRumor[currentStudentNum++] = student;
                    }
                }

                foreach (string otherStudents in students)
                {
                    if (!studentCheckOff.Contains(otherStudents))
                    {
                        thisRumor[currentStudentNum++] = otherStudents;
                    }
                }

                output[i] = thisRumor;
            }

            return output;
        }

        public static string[] AcquireInput(out SortedSet<string> students, out Dictionary<string, SortedSet<string>> friendships)
        {
            string thisLine;
            students = new SortedSet<string>();
            friendships = new Dictionary<string, SortedSet<string>>();
            string[] rumorStarters;

            thisLine = Console.ReadLine();
            int setSize = int.Parse(thisLine);

            for (int i = 0; i < setSize; i++)
            {
                string studentName = Console.ReadLine();

                students.Add(studentName);

                friendships.Add(studentName, new SortedSet<string>());
            }

            thisLine = Console.ReadLine();
            setSize = int.Parse(thisLine);

            for (int i = 0; i < setSize; i++)
            {
                thisLine = Console.ReadLine();
                string[] friendship = thisLine.Split(' ');

                friendships[friendship[0]].Add(friendship[1]);
                friendships[friendship[1]].Add(friendship[0]);

            }

            thisLine = Console.ReadLine();
            setSize = int.Parse(thisLine);
            rumorStarters = new string[setSize];

            for (int i = 0; i < setSize; i++)
            {
                string studentName = Console.ReadLine();

                rumorStarters[i] = studentName;
            }

            return rumorStarters;

        }
    }
}
