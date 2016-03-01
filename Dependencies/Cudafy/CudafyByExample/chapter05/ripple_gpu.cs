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
using Cudafy.Translator;

namespace CudafyByExample
{
    public class ripple_gpu
    {
        public ripple_gpu()
        {
        }
        
        public const int DIM = 1024;

        private byte[] _dev_bitmap;

        private GPGPU _gpu;

        private dim3 _blocks;

        private dim3 _threads;

        public void Initialize(int bytes)
        {
            CudafyModule km = CudafyTranslator.Cudafy();

            _gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);
            _gpu.LoadModule(km);

            _dev_bitmap = _gpu.Allocate<byte>(bytes);

            _blocks = new dim3(DIM / 16, DIM / 16);
            _threads = new dim3(16, 16);
        }

        public void Execute(byte[] resultBuffer, int ticks)
        {
            _gpu.Launch(_blocks, _threads).thekernel(_dev_bitmap, ticks);
            _gpu.CopyFromDevice(_dev_bitmap, resultBuffer);
        }

        [Cudafy]
        public static void thekernel(GThread thread, byte[] ptr, int ticks)
        {
            // map from threadIdx/BlockIdx to pixel position
            int x = thread.threadIdx.x + thread.blockIdx.x * thread.blockDim.x;
            int y = thread.threadIdx.y + thread.blockIdx.y * thread.blockDim.y;
            int offset = x + y * thread.blockDim.x * thread.gridDim.x;

            // now calculate the value at that position
            float fx = x - DIM/2;
            float fy = y - DIM/2;
            float d = GMath.Sqrt(fx * fx + fy * fy );
            //float d = thread.sqrtf(fx * fx + fy * fy);
            byte grey = (byte)(128.0f + 127.0f * GMath.Cos(d / 10.0f - ticks / 7.0f) /
                                                 (d/10.0f + 1.0f));
            ptr[offset*4 + 0] = grey;
            ptr[offset*4 + 1] = grey;
            ptr[offset*4 + 2] = grey;
            ptr[offset*4 + 3] = 255;        
        }

        public void ShutDown()
        {
            _gpu.FreeAll();
        }
    }
}
