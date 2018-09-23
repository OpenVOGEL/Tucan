using System;
using System.Collections;
using System.Collections.Generic;

using System.Data.Common;
using System.IO;
using System.Reflection;

using System.Windows.Forms;
using DotNumerics_Samples.Harness;

namespace DotNumerics_Samples.Dumper
{
    public class ObjectDumper : IObjectDumper
    {

        private TextBox outputTextBox;

        public TextBox OutputTextBox
        {
            get { return outputTextBox; }
            set { outputTextBox = value; }
        }


        public static bool CatchExceptions = true;


        public void Write(string s)
        {
            try
            {
                if (OutputTextBox != null)
                {
                    StringWriter sw = new StringWriter();
                    sw.WriteLine(s);
                    OutputTextBox.Text += sw.ToString();
                }
            }
            catch (Exception ex)
            {
                if (!CatchExceptions)
                    throw;
                MessageBox.Show("EXCEPTION: " + ex.ToString());
            }
        }
    }
}
