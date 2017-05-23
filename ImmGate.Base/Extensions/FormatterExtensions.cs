﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ImmGate.Base.Extensions
{
    public static class FormatterExtensions
    {

        public static string ToPrettyString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return ToPrettyString(dictionary, Environment.NewLine);

        }

        public static string ToPrettyString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, string separator)
        {
            return string.Join(separator, dictionary.Select(x => x.Key + "=" + x.Value).ToArray());
        }


        public static string ToPrettyString<T>(this IEnumerable<T> l)
        {

            return ToPrettyString(l, Environment.NewLine);

        }


        public static string ToPrettyString<T>(this IEnumerable<T> l, string separator)
        {
            return string.Join(separator, l.Select(t => t.ToString()).ToArray());
        }


    }
}
