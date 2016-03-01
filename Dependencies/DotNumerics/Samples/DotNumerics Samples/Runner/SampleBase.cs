using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using DotNumerics_Samples.Harness;

namespace DotNumerics_Samples.Runner
{
    public abstract class SampleBase
    {
        private readonly string _title;
        private readonly string _description;
        private readonly SampleSuite _sampleSuite;

        protected SampleBase(
            SampleSuite sampleSuite,
            string title,
            string description)
        {
            this._title = title;
            this._description = description;
            this._sampleSuite = sampleSuite;
        }

        public SampleSuite Suite
        {
            get { return _sampleSuite; }
        }

        public string Title
        {
            get { return _title; }
        }

        public string Description
        {
            get { return _description; }
        }
    }
}
