using System;
using System.Collections.Generic;
using System.Text;
using DotNumerics_Samples.Harness;
using DotNumerics_Samples.Runner;
using DotNumerics_Samples.Dumper;
using System.IO;

namespace DotNumerics_Samples
{
    public class TextWriterSampleRunner : SampleRunner
    {
        private int _indent = 0;
        private TextWriter _output;

        public TextWriterSampleRunner(TextWriter output) : base(NullObjectDumper.Instance)
        {
            _output = output;
        }

        private int failureCount = 0;

        public int FailureCount
        {
            get { return failureCount; }
            set { failureCount = value; }
        }
        private int successCount = 0;

        public int SuccessCount
        {
            get { return successCount; }
            set { successCount = value; }
        }

        public override void OnStarting(Sample sample)
        {
            _output.Write("{0}Running {1}... ", new string(' ', _indent), sample.Title);
        }

        public override void OnFailure(Sample sample, Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;

            _output.WriteLine("FAILED: {0}", ex.Message);
            FailureCount++;
        }

        public override void OnSuccess(Sample sample)
        {
            _output.WriteLine("SUCCESS");
            SuccessCount++;
        }

        public override void OnStartingGroup(SampleGroup group)
        {
            _output.WriteLine("{0}Running {1}... ", new string(' ', _indent), group.Title);
            _indent++;
        }

        public override void OnFinishedGroup(SampleGroup group)
        {
            _indent--;
            _output.WriteLine("{0}Finished {1} ", new string(' ', _indent), group.Title);
        }
    }
}
