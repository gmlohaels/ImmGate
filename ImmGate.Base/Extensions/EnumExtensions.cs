using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ImmGate.Base.Extensions
{
    public static class EnumExtensions
    {
        public static T RandomValue<T>(this T enumeratedType) where T : struct
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(new Random().Next(v.Length));
        }


        public static bool ContainExactFlagSet(this Enum obj, Enum flags)
        {
            var objFlags = obj.EnumerateSettedUpFlags().ToList();
            var setFlags = flags.EnumerateSettedUpFlags().ToList();

            if (objFlags.Count != setFlags.Count)
                return false;

            foreach (var tmpFlag in objFlags)
            {
                if (!setFlags.Contains(tmpFlag))
                    return false;

            }
            return true;

        }


        public static IEnumerable<Enum> EnumerateSettedUpFlags(this Enum flags)
        {
            foreach (var value in Enum.GetValues(flags.GetType()).Cast<Enum>())
            {

                ulong num = Convert.ToUInt64(value);

                var hasFlag = ((Convert.ToUInt64(flags) & num) == num);

                if (hasFlag)
                    yield return value;
            }
        }

        public static IEnumerable<Enum> EnumerateAllFlags(this Enum flags)
        {
            foreach (var value in Enum.GetValues(flags.GetType()).Cast<Enum>())
            {
                yield return value;
            }
        }

        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");
            var arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(arr, src) + 1;
            return arr.Length == j ? arr[0] : arr[j];
        }


        public static bool HasFlag<TEnum>(this TEnum enumeratedType, TEnum value)
            where TEnum : struct, IComparable, IFormattable, IConvertible

        {
            if (!(enumeratedType is Enum))
            {
                throw new InvalidOperationException("Struct is not an Enum.");
            }

            if (typeof(TEnum).GetCustomAttributes(
                typeof(FlagsAttribute), false).Length == 0)
            {
                throw new InvalidOperationException("Enum must use [Flags].");
            }

            long enumValue = enumeratedType.ToInt64(CultureInfo.InvariantCulture);
            long flagValue = value.ToInt64(CultureInfo.InvariantCulture);

            if ((enumValue & flagValue) == flagValue)
            {
                return true;
            }

            return false;
        }
    }
}