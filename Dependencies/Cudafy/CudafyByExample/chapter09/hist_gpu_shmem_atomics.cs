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
using Cudafy.Atomics;
using Cudafy.Translator;

namespace CudafyByExample
{
    public class hist_gpu_shmem_atomics
    {
        public const int SIZE =  100 * 1024 * 1024;

        [Cudafy]
        public static void histo_kernel(GThread thread, byte[] buffer, int size, uint[] histo) 
        {
            // clear out the accumulation buffer called temp
            // since we are launched with 256 threads, it is easy
            // to clear that memory with one write per thread
            uint[] temp = thread.AllocateShared<uint>("temp", 256);
            temp[thread.threadIdx.x] = 0;
            thread.SyncThreads();

            // calculate the starting index and the offset to the next
            // block that each thread will be processing
            int i = thread.threadIdx.x + thread.blockIdx.x * thread.blockDim.x;
            int stride = thread.blockDim.x * thread.gridDim.x;
            while (i < size) 
            {
                thread.atomicAdd(ref temp[buffer[i]], 1 );
                i += stride;
            }
            // sync the data from the above writes to shared memory
            // then add the shared memory values to the values from
            // the other thread blocks using global memory
            // atomic adds
            // same as before, since we have 256 threads, updating the
            // global histogram is just one write per thread!
            thread.SyncThreads();

            thread.atomicAdd(ref (histo[thread.threadIdx.x]), temp[thread.threadIdx.x]);
        }

        static byte[] big_random_block(int size) 
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            byte[] data = new byte[size];
            for (int i=0; i<size; i++)
                data[i] = (byte)rand.Next(Byte.MaxValue);

            return data;
        }

        public static int Execute() 
        {
            CudafyModule km = CudafyTranslator.Cudafy();

            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);
            if (gpu is CudaGPU && gpu.GetDeviceProperties().Capability < new Version(1, 2))
            {
                Console.WriteLine("Compute capability 1.2 or higher required for atomics.");
                return -1;
            }
            gpu.LoadModule(km);

            byte[] buffer = big_random_block(SIZE);

            // cudart.dll must be accessible!
            GPGPUProperties prop = null;
            try
            {
                prop = gpu.GetDeviceProperties(true);
            }
            catch (DllNotFoundException)
            {
                prop = gpu.GetDeviceProperties(false);
            }
            
            // capture the start time
            // starting the timer here so that we include the cost of
            // all of the operations on the GPU.  if the data were
            // already on the GPU and we just timed the kernel
            // the timing would drop from 74 ms to 15 ms.  Very fast.
            gpu.StartTimer();

            // allocate memory on the GPU for the file's data
            byte[] dev_buffer = gpu.CopyToDevice(buffer);
            uint[] dev_histo = gpu.Allocate<uint>(256);
            gpu.Set(dev_histo);

            // kernel launch - 2x the number of mps gave best timing          
            int blocks = prop.MultiProcessorCount;
            if (blocks == 0)
                blocks = 16;
            Console.WriteLine("Processors: {0}", blocks);
            gpu.Launch(blocks * 2, 256).histo_kernel(dev_buffer, SIZE, dev_histo); 
    
            uint[] histo = new uint[256];
            gpu.CopyFromDevice(dev_histo, histo);

            // get stop time, and display the timing results
            float elapsedTime = gpu.StopTimer();
            Console.WriteLine( "Time to generate: {0} ms", elapsedTime );

            long histoCount = 0;
            for (int i = 0; i < 256; i++) 
            {
                histoCount += histo[i];
            }
            Console.WriteLine( "Histogram Sum:  {0}", histoCount );

            // verify that we have the same counts via CPU
            for (int i=0; i<SIZE; i++)
                histo[buffer[i]]--;
            for (int i=0; i<256; i++) 
            {
                if (histo[i] != 0)
                    Console.WriteLine("Failure at {0}!", i);
            }

            gpu.FreeAll();
        
            return 0;
        }
    }
}
