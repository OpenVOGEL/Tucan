using System;
using System.Collections.Generic;

using System.Text;

namespace DotNumerics_Samples.Harness
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public sealed class PrefixAttribute : Attribute
    {
        public PrefixAttribute(string prefix)
        {
            this.Prefix = prefix;
        }

        private string prefix = "";

        public string Prefix
        {
            get { return prefix; }
            set { prefix = value; }
        }

    }
}
