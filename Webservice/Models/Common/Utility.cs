using System.Reflection;

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
