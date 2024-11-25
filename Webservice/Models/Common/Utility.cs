using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Models.Common
{
    public class Utility
    {
        public static Assembly[] GetModelsAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return assemblies;
        }
    }
}
