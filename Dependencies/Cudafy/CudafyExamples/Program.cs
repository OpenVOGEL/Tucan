/*
CUDAfy.NET - LGPL 2.1 License
Please consider purchasing a commerical license - it helps development, frees you from LGPL restrictions
and provides you with support.  Thank you!
Copyright (C) 2011 Hybrid DSP Systems
http://www.hybriddsp.com

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cudafy;
using CudafyExamples.Arrays;
using CudafyExamples.Dummy;
using CudafyExamples.Complex;
using CudafyExamples.Misc;
using Cudafy.Translator;
namespace CudafyExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                CudafyModes.Target = eGPUType.Cuda;
                CudafyModes.DeviceId = 0;
                CudafyTranslator.Language = CudafyModes.Target == eGPUType.OpenCL ? eLanguage.OpenCL : eLanguage.Cuda;

                Console.WriteLine("\r\nArrayBasicIndexing");
                ArrayBasicIndexing.Execute();
                Console.WriteLine("\r\nArrayMultidimensions");
                ArrayMultidimensions.Execute();

                if (CudafyModes.Target != eGPUType.OpenCL)
                {
                    Console.WriteLine("\r\nClass examples");
                    CudafyClassExamples.Execute();
                    Console.WriteLine("\r\nSIMDFunctions");
                    SIMDFunctions.Execute();
                    Console.WriteLine("\r\nGlobalArrays");
                    GlobalArrays.Execute();
                    Console.WriteLine("\r\nComplexNumbersD");
                    ComplexNumbersD.Execute();
                    Console.WriteLine("\r\nComplexNumbersF");
                    ComplexNumbersF.Execute();               
                    Console.WriteLine("\r\nDummyFunctions");
                    DummyFunctions.Execute();
                
                    Console.WriteLine("\r\nTextInsertion");
                    TextInsertion.Execute();
                

                    Console.WriteLine("\r\nVoting.Ballot");
                    Voting.Ballot.Execute();
                    Console.WriteLine("\r\nVoting.SyncThreadCount");
                    Voting.SyncThreadCount.Execute();
                
                    Console.WriteLine("\r\nFinanceTest");
                    FinanceTest.Execute();
                    Console.WriteLine("\r\nTiming");
                    Timing.Execute();
                    Console.WriteLine("\r\nPinnedAsyncIO");
                    PinnedAsyncIO.Execute();
                }
                Console.WriteLine("Done!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadKey();
        }
    }
}
