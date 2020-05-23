using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace TPI_projekt5
{
    class CodeWord
    {
        public List<int> inputCodeWord = new List<int>() { };
        public List<int> secureCodeWord = new List<int>() { };
        public List<int> syndrome = new List<int>() { };
        public List<int> outputCodeWord = new List<int>() { };

        public void PrintCodeWord(List<int> Collection)
        {
            foreach (int number in Collection)
            {
                Console.Write($"{number} ");
            }
            Console.WriteLine();
        }

        public void SecureCodeword(Matrix m, int[,] G)
        {
            int countInColumn = 0;
            this.inputCodeWord.Reverse();

            for (int i = 0; i < m.columns; i++)
            {
                for (int j = 0; j < m.rows; j++)
                {
                    countInColumn += (this.inputCodeWord[j] * G[j, i]);
                    if (countInColumn == 2)
                    {
                        countInColumn = 0;
                    }
                }
                this.secureCodeWord.Add(countInColumn);
                countInColumn = 0;
            }
        }

        public void GeneratePositionsOfFaults(int count, List<int> Positions)
        {
            if (count >= 0 && count <= 3)
            {
                while (Positions.Count != count)
                {
                    Random fault = new Random();
                    int position = fault.Next(0, 6);

                    if (Positions.Count == 0)
                    {
                        Positions.Add(position);
                    }
                    else if (Positions.Count == 1 && position != Positions[0])
                    {
                        Positions.Add(position);
                    }
                    else if (Positions.Count == 2 && position != Positions[0] && position != Positions[1])
                    {
                        Positions.Add(position);
                    }
                }
            }
            else
            {
                Console.WriteLine("Incorrect number of faults");
            }
        }

        public void MakeFaults(List<int> Positions)
        {
            foreach (int position in Positions)
            {
                if (this.secureCodeWord[position] == 0)
                {
                    this.secureCodeWord[position] = 1;
                }
                else
                {
                    this.secureCodeWord[position] = 0;
                }
            }
        }

        public void Syndrome(Matrix m, int[,] H)
        {
            int[,] transposeControl = new int[m.columns, m.rows];
            for (int i = 0; i < m.columns; i++)
            {
                for (int j = 0; j < m.rows; j++)
                {
                    transposeControl[i, j] = H[j, i];
                }
            }
            
            int countInColumn = 0;

            for (int i = 0; i < m.rows; i++)
            {
                for (int j = 0; j < m.columns; j++)
                {
                    countInColumn += (secureCodeWord[j] * transposeControl[j, i]);
                    if (countInColumn == 2)
                    {
                        countInColumn = 0;
                    }
                }
                this.syndrome.Add(countInColumn);
                countInColumn = 0;
            }

            foreach (int number in this.syndrome)
            {
                Console.WriteLine(number);
            }
        }

        public void CorrectCodeword(int numberOfFaults)
        {
            if (numberOfFaults == 1)
            {
                double positionOfFault = 0;
                this.syndrome.Reverse();
                for (int i = 0; i < this.syndrome.Count; i++)
                {
                    if (this.syndrome[i] == 1)
                    {
                        positionOfFault += Math.Pow(2, (double)i);
                    }
                }
                Console.WriteLine($"Fault is on the {positionOfFault} st/nd/rd/th place");

                for (int i = 0; i < this.secureCodeWord.Count; i++)
                {
                    if (i == positionOfFault - 1)
                    {
                        if (this.secureCodeWord[i] == 0)
                        {
                            this.secureCodeWord[i] = 1;
                        }
                        else
                        {
                            this.secureCodeWord[i] = 0;
                        }
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Corrected codeword");
                this.PrintCodeWord(this.secureCodeWord);
            }
            else if (numberOfFaults == 0)
            {
                Console.WriteLine("Nothing to be corrected");
            }
            else
            {
                Console.WriteLine("Too much faults");
            }
        }

        public void FindInformationBits(int controlRows)
        {
            List<double> PowersOf2 = new List<double>() { };

            for (int i = 0; i < controlRows; i++)
            {
                PowersOf2.Add(Math.Pow(2, i));
            }

            for (int i = 1; i < this.secureCodeWord.Count + 1; i++)
            {
                int countOfNonPowers = 0;
                for (int j = 0; j < PowersOf2.Count; j++)
                {
                    if (i != PowersOf2[j])
                    {
                        ++countOfNonPowers;
                    }
                }

                if (countOfNonPowers == controlRows)
                {
                    this.outputCodeWord.Add(secureCodeWord[i - 1]);
                }
            }
        }

        public void FindRecievedLetter(List<Letter> Letters)
        {
            foreach (Letter letter in Letters)
            {
                letter.FillCodeNumbers();
                int sameElements = 0;

                for (int i = 0; i < letter.codeNumbers.Count; i++)
                {
                    if (letter.codeNumbers[i] == this.outputCodeWord[i])
                    {
                        ++sameElements;
                    }
                }

                if (sameElements == this.outputCodeWord.Count)
                {
                    Console.WriteLine($"Recieved codeword corresponds with letter {letter.name}");
                    break;
                }
            }
        }
    }
}

