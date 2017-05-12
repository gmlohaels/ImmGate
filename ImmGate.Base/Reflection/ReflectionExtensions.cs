using System.Collections.Generic;
using System.Linq;

namespace ImmGate.Base.Reflection
{
    public static class ReflectionExtensions
    {




        public static IEnumerable<T> GetPropertiesThatImplements<T>(this object propertyHolder) where T : class
        {
            var properties = propertyHolder.GetType().GetProperties();
            return properties.Select(property =>
                property.GetValue(propertyHolder, null)
            ).OfType<T>();
        }


    }
}
