using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using DotNumerics_Samples.Harness;

namespace DotNumerics_Samples.Runner
{
    public class Sample : SampleBase
    {
        private readonly MethodInfo _method;
        private readonly string _sourceCode;

        public Sample(
            SampleSuite sampleSuite,
            MethodInfo method,
            string title,
            string description,
            string sourceCode) : base(sampleSuite, title, description)
        {
            this._method = method;
            this._sourceCode = sourceCode;
        }

        public MethodInfo Method
        {
            get { return _method; }
        }

        public string SourceCode
        {
            get { return _sourceCode; }
        }
    }
}
