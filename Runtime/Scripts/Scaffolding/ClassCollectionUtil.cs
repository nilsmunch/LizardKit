using System;
using System.Collections.Generic;
using System.Linq;

namespace LizardKit.Scaffolding
{
    public class ClassCollectionUtil
    {
        private static List<Type> GetAllConcreteSubclassesOf<TBase>()
        {
            var baseType = typeof(TBase);

            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => 
                    baseType.IsAssignableFrom(t) &&
                    t != baseType &&
                    !t.IsAbstract)
                .ToList();
        }
    }
}