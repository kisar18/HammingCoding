using System;
using System.Collections.Generic;
using System.Text;

namespace TPI_projekt5
{
    class Matrix
    {
        public int rows { get; set; }
        public int columns { get; set; }
        public int[,] mat = new int[,] { };

        public Matrix(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
        }

        public void Printmatrix(int[,] matrix)
        {
            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                {
                    Console.Write($"{matrix[i, j]} ");
                }
                Console.WriteLine();
            }
        }
        public void SwapColumns(int[,] matrix, int first, int second)
        {
            for (int i = 0; i < this.rows; i++)
            {
                int t = matrix[i, first];
                matrix[i, first] = matrix[i, second];
                matrix[i, second] = t;
            }
        }

        public void FillGeneratingMatrix(int[,] G, int[,] H, CodeWord c, Matrix control)
        {
            int[,] tmp = new int[c.inputCodeWord.Count, control.rows];
            for (int i = 0; i < c.inputCodeWord.Count; i++)
            {
                for (int j = 0; j < control.rows; j++)
                {
                    tmp[i, j] = H[j, i];
                }
            }
            
            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                {
                    if (j < this.rows)
                    {
                        if (i == j)
                        {
                            G[i, j] = 1;
                        }
                        else
                        {
                            G[i, j] = 0;
                        }
                    }
                    else
                    {
                        G[i, j] = tmp[i, j - this.rows];
                    }
                }
            }
            
        }
    }
}
