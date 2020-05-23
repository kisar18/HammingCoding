using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security;

namespace TPI_projekt5
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create a new list of letters and add all possible choices and their codeword
            List<Letter> Letters = new List<Letter>() { };
            Letters.Add(new Letter('A', "0000"));
            Letters.Add(new Letter('B', "0001"));
            Letters.Add(new Letter('C', "0010"));
            Letters.Add(new Letter('D', "0011"));
            Letters.Add(new Letter('E', "0100"));
            Letters.Add(new Letter('F', "0101"));
            Letters.Add(new Letter('G', "0110"));
            Letters.Add(new Letter('H', "0111"));
            Letters.Add(new Letter('I', "1000"));
            Letters.Add(new Letter('J', "1001"));
            Letters.Add(new Letter('K', "1010"));
            Letters.Add(new Letter('L', "1011"));
            Letters.Add(new Letter('M', "1100"));
            Letters.Add(new Letter('N', "1101"));
            Letters.Add(new Letter('O', "1110"));
            Letters.Add(new Letter('P', "1111"));

            //User chooses his letter
            Console.WriteLine("Enter the letter to be encoded");
            char input = Console.ReadKey().KeyChar;
            Console.WriteLine();

            //Searches chosen letter and assigns him to CodeWord type
            CodeWord c1 = new CodeWord();
            foreach (Letter letter in Letters)
            {
                if (letter.name == input)
                {
                    foreach (char ch in letter.codeWord)
                    {
                        //Put the codeword string of letter into CodeWords list inputCodeWord
                        if (ch == '0')
                        {
                            c1.inputCodeWord.Add(0);
                        }
                        else
                        {
                            c1.inputCodeWord.Add(1);
                        }
                    }
                    break;
                }
            }

            c1.PrintCodeWord(c1.inputCodeWord);
            Console.WriteLine();

            //Creating control (H) matrix with values of numbers one to seven by binary code
            Matrix control = new Matrix(3, 7);
            control.mat = new int[,] { { 0, 0, 0, 1, 1, 1, 1 }, { 0, 1, 1, 0, 0, 1, 1 }, { 1, 0, 1, 0, 1, 0, 1} };

            //Swapping columns for creating generating matrix
            control.SwapColumns(control.mat, 0, 6);
            control.SwapColumns(control.mat, 1, 5);
            control.SwapColumns(control.mat, 3, 4);

            //Creating generating (G) matrix using a unit matrix and a control matrix
            Matrix generating = new Matrix(c1.inputCodeWord.Count, control.columns);
            generating.mat = new int[generating.rows, generating.columns];
            generating.FillGeneratingMatrix(generating.mat, control.mat, c1, control);

            //Swapping columns back to original
            generating.SwapColumns(generating.mat, 0, 6);
            generating.SwapColumns(generating.mat, 1, 5);
            generating.SwapColumns(generating.mat, 3, 4);

            control.SwapColumns(control.mat, 0, 6);
            control.SwapColumns(control.mat, 1, 5);
            control.SwapColumns(control.mat, 3, 4);

            Console.WriteLine("Generating matrix");
            generating.Printmatrix(generating.mat);
            Console.WriteLine();

            //Securing input letter by Hamming 7/4 coding
            c1.SecureCodeword(generating, generating.mat);
            Console.WriteLine("Secure codeword");
            c1.PrintCodeWord(c1.secureCodeWord);
            Console.WriteLine();
            
            //Choosing number of faults to be generated in secured codeword
            Console.WriteLine("Enter number (0 - 3) of faults");
            int numberOfFaults = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            //Making faults on random places of secured codeword
            List<int> PositionsOfFaults = new List<int>() { };
            c1.GeneratePositionsOfFaults(numberOfFaults, PositionsOfFaults);
            c1.MakeFaults(PositionsOfFaults);

            Console.WriteLine($"Secure codeword with {numberOfFaults} faults");
            c1.PrintCodeWord(c1.secureCodeWord);
            Console.WriteLine();

            Console.WriteLine("Control matrix");
            control.Printmatrix(control.mat);
            Console.WriteLine();

            //Counting syndrome of secured codeword with some faults
            Console.WriteLine("Syndrome of the codeword");
            c1.Syndrome(control, control.mat);
            Console.WriteLine();

            //If user choose only one fault, program will correct it
            c1.CorrectCodeword(numberOfFaults);
            c1.FindInformationBits(control.rows);
            Console.WriteLine();

            Console.WriteLine("Recieved codeword without security");
            c1.PrintCodeWord(c1.outputCodeWord);
            Console.WriteLine();

            c1.FindRecievedLetter(Letters);
        }
    }
}
