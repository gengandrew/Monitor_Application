using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker
{
    class Conversions
    {
        public static TimeSpan Limit1 = new TimeSpan(0000, 0, 3);
        public static TimeSpan Limit2 = new TimeSpan(0000, 0, 6);
        public static TimeSpan Limit3 = new TimeSpan(0000, 0, 10);
        public static DateTime nullity = new DateTime(0001, 1, 1);

        public static void testPrint2DArray(DateTime[,] input)
        {
            for (int i = 0; i < InitBaseGrid.rowCount; i++)
            {
                for (int j = 0; j < InitBaseGrid.columnCount; j++)
                {
                    Console.Write(input[j, i].ToString());
                    if (input[j, i].Equals(nullity))
                    {
                        Console.Write("T\t");
                    }
                    else
                    {
                        Console.Write("F\t");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static char intToChar(int input)
        {
            char output = 'A';
            output = (char)((int)output + input);
            return output;
        }

        public static int charToInt(char input)
        {
            int output = ((int)input) - ((int)('A'));
            return output;
        }

        public static int boolToInt(bool input)
        {
            if (input)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static bool intToBool(int input)
        {
            if (input == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int[] progressionIDToArray(string ProgressionID)
        {
            List<int> placeHolder = new List<int>();
            string temp = "";
            foreach (char i in ProgressionID)
            {
                if (i == ',')
                {
                    placeHolder.Add(Int32.Parse(temp));
                    temp = "";
                }
                else
                {
                    temp = temp + i;
                }
            }
            placeHolder.Add(Int32.Parse(temp));
            int[] output = placeHolder.ToArray();
            return output;
        }

        public static string progressionIDToString(int[] ProgressionID)
        {
            string output = "";
            for (int i = 0; i < ProgressionID.Length; i++)
            {
                if (i != ProgressionID.Length - 1)
                {
                    output = output + ProgressionID[i] + ",";
                }
                else
                {
                    output = output + ProgressionID[i];
                }
            }
            return output;
        }

        public static int[] UpdateProgressionID(int[] ProgressionID)
        {
            int index = 0;
            while (ProgressionID[index] == 0)
            {
                if (index == ProgressionID.Length - 1)
                {
                    break;
                }
                else
                {
                    index++;
                }
            }
            ProgressionID[index] = 0;
            return ProgressionID;
        }

        public static int getLastProgressionID(int[] ProgressionID)
        {
            int index = 0;
            while (ProgressionID[index] == 0)
            {
                if (index == ProgressionID.Length - 1)
                {
                    break;
                }
                else
                {
                    index++;
                }
            }
            return index;
        }

        public static TimeSpan DateTimeToTimeSpan(DateTime input)
        {
            return input.Subtract(nullity);
        }

        public static TimeSpan[] DateTimeToTimeSpan(DateTime[] input)
        {
            TimeSpan[] output = new TimeSpan[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = input[i].Subtract(nullity);
            }
            return output;
        }

        public static TimeSpan StripMiliSec(TimeSpan inTime)
        {
            return new TimeSpan(inTime.Days, inTime.Hours, inTime.Minutes, inTime.Seconds);
        }

        public static string formatProgressionIDString(string progressionID)
        {
            bool isValid = false;
            string output = "";
            foreach (char c in progressionID)
            {
                if (c != ',' && c != '0')
                {
                    isValid = true;
                }
                if (isValid)
                {
                    output = output + c;
                }
            }
            return output;
        }
    }
}
