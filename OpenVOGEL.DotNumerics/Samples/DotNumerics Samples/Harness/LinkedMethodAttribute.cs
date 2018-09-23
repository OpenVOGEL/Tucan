using System;
using System.Collections.Generic;

using System.Text;

namespace DotNumerics_Samples.Harness
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class LinkedMethodAttribute : Attribute
    {
        public LinkedMethodAttribute(string methodName)
        {
            this.MethodName = methodName;
        }

        private string methodName = "";

        public string MethodName
        {
            get { return methodName; }
            set { methodName = value; }
        }

    }
}