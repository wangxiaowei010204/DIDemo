using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIDemo
{
    public class LearnDI : ILearnDI, IDisposable
    {

        public LearnDI()
        {
        }

        public void Dispose()
        {
            Console.WriteLine("LearnDI is disposed");
        }

        public string GetName(string argName)
        {
            return $"我的名字是:{argName}";
        }
    }
}
