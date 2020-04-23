using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIDemo
{
    public class LearnDI : ILearnDI
    {
        public LearnDI()
        { 
        
        }

        public string GetName(string argName)
        {
            return $"我的名字是:{argName}";
        }
    }
}
