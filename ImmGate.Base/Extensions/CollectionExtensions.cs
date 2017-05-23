using System.Collections.Generic;
using System.Linq;

namespace ImmGate.Base.Extensions
{
    public static class CollectionExtensions
    {

        public static T RemoveFirst<T>(this ICollection<T> collection)
        {
            var element = collection.FirstOrDefault();
            if (element != null)
                collection.Remove(element);
            return element;


        }


    }
}
