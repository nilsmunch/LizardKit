using System;
using System.Collections.Generic;
using System.Linq;

namespace LizardKit.Scaffolding.TypeCatalogs
{
    public class OneOfEach<T> : List<T> where T : class
    {
        public OneOfEach()
        {
            AddRange(
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(type =>
                        typeof(T).IsAssignableFrom(type) &&
                        !type.IsAbstract &&
                        !type.IsInterface &&
                        type.GetConstructor(Type.EmptyTypes) != null)
                    .Select(type => (T)Activator.CreateInstance(type))
            );
        }
    }
}