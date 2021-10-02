using System;

namespace MakeGenericAgain
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class FromCommandLineAttribute: Attribute
    {
        public FromCommandLineAttribute(params string[] paramNames)
        {
            ParamNames = paramNames;
        }

        public string[] ParamNames { get; set; }
    }
}