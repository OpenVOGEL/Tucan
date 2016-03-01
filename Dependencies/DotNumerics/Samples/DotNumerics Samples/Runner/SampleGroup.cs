using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using DotNumerics_Samples.Harness;

namespace DotNumerics_Samples.Runner
{
    public class SampleGroup : SampleBase
    {
        private IList<SampleBase> _children = new List<SampleBase>();

        public SampleGroup(SampleSuite sampleSuite,
            string title,
            string description)
            : base(sampleSuite, title, description)
        {
        }

        public IList<SampleBase> Children
        { 
            get { return _children; }
        }
    }
}
