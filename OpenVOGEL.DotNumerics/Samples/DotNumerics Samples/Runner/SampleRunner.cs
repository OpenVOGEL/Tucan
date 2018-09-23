using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using DotNumerics_Samples.Harness;

namespace DotNumerics_Samples.Runner
{
    public abstract class SampleRunner
    {
        private IObjectDumper _objectDumper;

        public SampleRunner(IObjectDumper dumper)
        {
            _objectDumper = dumper;
        }


        public IObjectDumper ObjectDumper 
        {
            get { return _objectDumper; }
        }

        public void Run(Sample sample)
        {
            sample.Suite.ObjectDumper = _objectDumper;
            try
            {
                OnStarting(sample);
                sample.Method.Invoke(sample.Suite, null);
                OnSuccess(sample);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sample.Suite.TearDownSample();
            }
        }

        public void Run(SampleGroup group)
        {
            OnStartingGroup(group);
            foreach (SampleBase s in group.Children)
            {
                if (s is SampleGroup)
                    Run((SampleGroup)s);
                else
                    Run((Sample)s);
            }
            OnFinishedGroup(group);
        }

        public abstract void OnStarting(Sample sample);
        public abstract void OnSuccess(Sample sample);
        public abstract void OnFailure(Sample sample, Exception ex);
        public abstract void OnStartingGroup(SampleGroup group);
        public abstract void OnFinishedGroup(SampleGroup group);

    }
}
