using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace MakeGenericAgain
{
    public static class CommandLineArgs
    {
        public static T Parse<T>(string[] args) where T : new()
        {
            return Parse<T>(string.Join(" ", args));
        }

        public static T Parse<T>(string args) where T : new()
        {
            var res = new T();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                var namesforProp = (prop.GetCustomAttribute<FromCommandLineAttribute>()?.ParamNames.Select(s => s.EnsureStartsWith("-").ToLower()) ?? Enumerable.Empty<string>()).Concat(new [] {"-"+prop.Name.ToLower()}).Distinct();
                foreach (var name in namesforProp)
                {
                    var indexOf = args.IndexOf(name, StringComparison.Ordinal);
                    if (indexOf >= 0)
                    {
                        var rest = args.Substring(indexOf + name.Length);
                        var end = rest.IndexOf(" ");
                        end = end > 0 ? end : rest.Length;
                        var value = rest.Substring(0, end).Trim().Replace("\"", "");
                        if (IsEnumerable(prop))
                        {
                            var values = value.Replace(" ", string.Empty).Split(',');
                            prop.SetValue(res, values);
                        }
                        else
                        {
                            prop.SetValue(res, value);
                        }
                    }
                }
            }
            return res;
        }

        private static string EnsureStartsWith(this string str, string toStartWith)
        {
            if (!str.StartsWith(toStartWith))
                str = toStartWith + str;
            return str;
        }

        private static bool IsEnumerable(PropertyInfo prop)
        {
            return prop.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(prop.PropertyType);
        }
    }
}