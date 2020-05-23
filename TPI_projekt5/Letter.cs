using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TPI_projekt5
{
    class Letter
    {
        public char name { get; set; }
        public string codeWord { get; set; }

        public List<int> codeNumbers = new List<int>() { };
        public Letter(char name, string codeWord)
        {
            this.name = name;
            this.codeWord = codeWord;
        }

        public void FillCodeNumbers()
        {
            foreach (char number in this.codeWord)
            {
                if (number == '0')
                {
                    this.codeNumbers.Add(0);
                }
                else
                {
                    this.codeNumbers.Add(1);
                }
            }
        }
    }
}
