using System;
using System.Collections.Generic;

using System.Text;
using DotNumerics_Samples.Harness;
using System.Collections;

namespace DotNumerics_Samples.Dumper
{
    public class NullObjectDumper : IObjectDumper
    {
        public static readonly IObjectDumper Instance = new NullObjectDumper();

        private NullObjectDumper()
        {
        }

        #region IObjectDumper Members


        public void Write(string s)
        {
            Write(s);
        }


        #endregion
    }
}
