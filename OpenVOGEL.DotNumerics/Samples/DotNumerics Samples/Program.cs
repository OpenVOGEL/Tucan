using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using DotNumerics_Samples.Runner;
using DotNumerics_Samples.Utils;
using DotNumerics_Samples.Samples;

namespace DotNumerics_Samples
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // set up a loader and source code search paths
            SampleLoader loader = new SampleLoader();
            loader.AddSourceDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "."));
            loader.AddSourceDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Samples"));
            loader.AddSourceDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Samples"));

            // load samples
            SampleGroup allSamples = new SampleGroup(null, "DotNumerics Samples", "");
            allSamples.Children.Add(loader.Load(new LinearAlgebra()));
            allSamples.Children.Add(loader.Load(new DifferentialEquations()));
            allSamples.Children.Add(loader.Load(new Optimization()));


            Application.EnableVisualStyles();
            Application.Run(new FormSampleRunner(allSamples));


        }
    }
}
