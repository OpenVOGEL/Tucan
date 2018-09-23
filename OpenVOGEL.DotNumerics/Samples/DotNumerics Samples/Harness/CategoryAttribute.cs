using System;
using System.Collections.Generic;

using System.Text;

namespace DotNumerics_Samples.Harness
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class CategoryAttribute : Attribute
    {
        public CategoryAttribute(string category)
        {
            this.Category = category;
        }

        private string category = "";

        public string Category
        {
            get { return category; }
            set { category = value; }
        }

    }
}
