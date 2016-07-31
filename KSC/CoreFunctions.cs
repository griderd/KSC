using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSC
{
    partial class Transpiler
    {
        void WriteLine(string line)
        {
            output.Add(line);
        }

        void AddBaseHeader()
        {
            WriteLine("@lazyglobal off.");
        }
    }
}
