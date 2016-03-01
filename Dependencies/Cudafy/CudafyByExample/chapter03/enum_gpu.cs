/* 
 * This software is based upon the book CUDA By Example by Sanders and Kandrot
 * and source code provided by NVIDIA Corporation.
 * It is a good idea to read the book while studying the examples!
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cudafy;
using Cudafy.Host;

namespace CudafyByExample
{

    public class enum_gpu
    {
        public static void Execute()
        {          
            int i = 0;

            foreach (GPGPUProperties prop in CudafyHost.GetDeviceProperties(CudafyModes.Target, false))
            {
                Console.WriteLine("   --- General Information for device {0} ---", i);
                Console.WriteLine("Name:  {0}", prop.Name);
                Console.WriteLine("Platform Name:  {0}", prop.PlatformName);
                Console.WriteLine("Device Id:  {0}", prop.DeviceId);
                Console.WriteLine("Compute capability:  {0}.{1}", prop.Capability.Major, prop.Capability.Minor);
                Console.WriteLine("Clock rate: {0}", prop.ClockRate);
                Console.WriteLine("Simulated: {0}", prop.IsSimulated);
                Console.WriteLine();

                Console.WriteLine("   --- Memory Information for device {0} ---", i);
                Console.WriteLine("Total global mem:  {0}", prop.TotalMemory);
                Console.WriteLine("Total constant Mem:  {0}", prop.TotalConstantMemory);
                Console.WriteLine("Max mem pitch:  {0}", prop.MemoryPitch);
                Console.WriteLine("Texture Alignment:  {0}", prop.TextureAlignment);
                Console.WriteLine();

                Console.WriteLine("   --- MP Information for device {0} ---", i);
                Console.WriteLine("Shared mem per mp: {0}", prop.SharedMemoryPerBlock);
                Console.WriteLine("Registers per mp:  {0}", prop.RegistersPerBlock);
                Console.WriteLine("Threads in warp:  {0}", prop.WarpSize);
                Console.WriteLine("Max threads per block:  {0}", prop.MaxThreadsPerBlock);
                Console.WriteLine("Max thread dimensions:  ({0}, {1}, {2})", prop.MaxThreadsSize.x, prop.MaxThreadsSize.y, prop.MaxThreadsSize.z);
                Console.WriteLine("Max grid dimensions:  ({0}, {1}, {2})", prop.MaxGridSize.x, prop.MaxGridSize.y, prop.MaxGridSize.z);

                Console.WriteLine();

                i++;
            }

        }
    }
}
