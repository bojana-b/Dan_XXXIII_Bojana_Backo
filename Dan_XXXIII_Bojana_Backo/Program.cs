using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dan_XXXIII_Bojana_Backo
{
    class Program
    {
        public static string fileMatrix = @"..\..\FileByThread_1.txt";
        public static string fileOddNumber = @"..\..\FileByThread_22.txt";
        static void IdentityMatrix()
        {
            try
            {
                File.Delete(fileMatrix);
                using (StreamWriter sw = File.CreateText(fileMatrix))
                {
                    int[,] mat = new int[100, 100];
                    for (int i = 0; i < mat.GetLength(0); i++)
                    {
                        for (int j = 0; j < mat.GetLength(1); j++)
                        {
                            if (i == j)
                            {
                                mat[i, j] = 1;
                            }
                            sw.Write(mat[i, j] + " ");
                        }
                        sw.WriteLine();
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"The file was not found: '{e}'");
            }
            catch (IOException e)
            {
                Console.WriteLine($"The file could not be opened: '{e}'");
            }
        }

        static void GenerateOddNumbers()
        {
            try
            {
                using (StreamWriter sw = File.CreateText(fileOddNumber))
                {
                    Random random = new Random();
                    for (int i = 0; i < 1001; i++)
                    {
                        int x = (int)(random.Next(0, 10001));
                        x += (x % 2 == 0 ? 1 : 0);
                        sw.WriteLine(x);
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"The file was not found: '{e}'");
            }
            catch (IOException e)
            {
                Console.WriteLine($"The file could not be opened: '{e}'");
            }
        }

        static void ReadMatrixFromFile()
        {
            try
            {
                using (StreamReader sr = File.OpenText(fileMatrix))
                {
                    string line;
                    string[] arr;
                    int[,] matrix = new int[100, 100];
                    int m = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        arr = line.Split();
                        int i = 0;
                        for (int n = 0; n < matrix.GetLongLength(1); n++)
                        {
                            matrix[m, n] = Convert.ToInt32(arr[i]);
                            i++;
                        }
                        m++;
                    }
                    for (int i = 0; i < matrix.GetLength(0); i++)
                    {
                        for (int j = 0; j < matrix.GetLength(1); j++)
                        {
                            Console.Write(matrix[i, j]);
                        }
                        Console.WriteLine();
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"The file was not found: '{e}'");
            }
            catch (IOException e)
            {
                Console.WriteLine($"The file could not be opened: '{e}'");
            }
        }

        static void SumOfOddNumbers()
        {
            try
            {
                using (StreamReader sr = File.OpenText(fileOddNumber))
                {
                    string line;
                    string[] arr;
                    long sum = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        arr = line.Split();
                        for (int i = 0; i < arr.Length; i++)
                        {
                            sum += Convert.ToInt64(arr[i]);
                        }
                    }
                    Console.WriteLine("The sum of odd numbers is {0}", sum);
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"The file was not found: '{e}'");
            }
            catch (IOException e)
            {
                Console.WriteLine($"The file could not be opened: '{e}'");
            }
        }

        static void Main(string[] args)
        {
            Thread[] threads = new Thread[4];
            threads[0] = new Thread(() => IdentityMatrix());
            threads[1] = new Thread(() => GenerateOddNumbers());
            threads[2] = new Thread(() => ReadMatrixFromFile());
            threads[3] = new Thread(() => SumOfOddNumbers());
            for (int i = 0; i < 4; i++)
            {
                if(i % 2 != 0)
                {
                    threads[i].Name = string.Format("Thread_{0}{0}", i + 1);
                }
                else
                {
                    threads[i].Name = string.Format("Thread_{0}", i + 1);
                }
                Console.WriteLine("{0} is created!", threads[i].Name);
            }
            var watch = Stopwatch.StartNew();
            threads[0].Start();
            threads[1].Start();
            threads[0].Join();
            threads[1].Join();
            Console.WriteLine("DONE: {0} Milliseconds", watch.ElapsedMilliseconds);
            threads[2].Start();
            threads[3].Start();

            Console.ReadKey();
        }
    }
}
