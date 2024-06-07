using System;
using System.Collections;
using System.Collections.Generic;
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
                var namesForProp = (prop.GetCustomAttribute<FromCommandLineAttribute>()?.ParamNames.Select(s => s.EnsureStartsWith("-").ToLower()) ?? Enumerable.Empty<string>())
                    .Concat(new[] { "-" + prop.Name.ToLower() })
                    .Distinct();

                foreach (var name in namesForProp)
                {
                    var indexOf = args.IndexOf(name + "=", StringComparison.Ordinal);
                    if (indexOf < 0) indexOf = args.IndexOf(name + " ", StringComparison.Ordinal);
                    if (indexOf < 0) continue;

                    var startOfValue = indexOf + name.Length;
                    if (args[startOfValue] == '=') startOfValue++;

                    var endOfValue = args.IndexOf(" -", startOfValue, StringComparison.Ordinal);
                    if (endOfValue == -1) endOfValue = args.Length;

                    var value = args.Substring(startOfValue, endOfValue - startOfValue).Trim();
                    
                    if (value.StartsWith("\"") && value.EndsWith("\""))
                    {
                        value = value[1..^1];
                    }

                    if (IsCollection(prop))
                    {
                        var values = value.Split(',')
                            .Select(v => v.Trim())
                            .Where(v => !string.IsNullOrEmpty(v))
                            .ToArray();
                        prop.SetValue(res, values);
                    }
                    else
                    {
                        prop.SetValue(res, value);
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

        private static bool IsCollection(PropertyInfo prop) 
            => prop.PropertyType != typeof(string) && prop.PropertyType.IsCollection();

        public static bool IsCollection(this Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>) || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>) || typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string) || type.IsArray || type.GetInterfaces().Any(IsCollection);
    }
}