using System;
using System.Collections.Generic;

using System.Text;

namespace DotNumerics_Samples.Harness
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public sealed class TitleAttribute : Attribute
    {
        public TitleAttribute(string title)
        {
            this.Title = title;
        }

        private string title = "";

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

    }
}
