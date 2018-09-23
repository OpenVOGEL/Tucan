using System;
using System.Collections.Generic;

using System.Text;

namespace DotNumerics_Samples.Harness
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class LinkedClassAttribute : Attribute
    {
        public LinkedClassAttribute(string className)
        {
            this.ClassName = className;
        }

        private string className = "";

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

    }
}
