using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MakeGenericAgain
{
    public static class NameReplacer
    {
        public static string ReplaceToGeneric(string str)
        {
            var toSplit = Regex.Replace(str, @"[^\w\.@-]", " ", RegexOptions.None, TimeSpan.FromSeconds(1.5)).Replace(".", " ");
            foreach (var word in toSplit.Split(" "))
            {
                if (word.Contains("Of") && !word.StartsWith("Of") && !word.EndsWith("Of") && !word.StartsWith("DateTime"))
                {
                    var genericWordFor = GetGenericWordFor(word);
                    if (word != genericWordFor)
                        str = str.Replace(word, genericWordFor);
                }
            }

            return str;
        }

        internal static string GetGenericWordFor(string word)
        {
            var parts = word.Split("Of");

            var closing = string.Join("",Enumerable.Repeat(">", parts.Length - 1));
            var opening = string.Join("", parts.Select((p,i) => i < parts.Length - 1 ? $"{TypeNameFor(p)}<" : TypeNameFor(p)));
            var result = opening + closing;

            return result;
        }

        private static string TypeNameFor(string type)
        {
            if (type.Contains("And"))
            {
                return string.Join(",", type.Split("And").Select(TypeNameFor));
            }

            if (type == "Integer")
                return "int";
            if (type == "String")
                return "string";
            if (type == "Boolean" || type == "Bool")
                return "bool";
            if (type == "Double")
                return "double";
            if (type == "Float")
                return "float";
            if (type == "Decimal")
                return "decimal";
            return type;
        }
    }
}