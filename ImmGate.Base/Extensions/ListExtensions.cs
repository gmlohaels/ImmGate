using System.Collections.Generic;
using System.Linq;

namespace ImmGate.Base.Extensions
{
    public static class ListExtensions
    {

        public static T RemoveFirst<T>(this IList<T> list)
        {
            var element = list.FirstOrDefault();
            if (element != null)
                list.Remove(element);
            return element;


        }


    }
}
