using System;
using System.Collections.Generic;

using System.Text;

namespace DotNumerics_Samples.Harness
{
    public abstract class SampleSuite
    {

        private IObjectDumper objectDumper;

        protected internal IObjectDumper ObjectDumper
        {
            get { return objectDumper; }
            set { objectDumper = value; }
        }
 
        public virtual void InitSample(string connectionString)
        {
        }

        public virtual void TearDownSample()
        {
        }
    }
}
