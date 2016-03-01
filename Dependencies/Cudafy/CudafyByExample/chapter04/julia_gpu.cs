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
    public class julia_gpu
    {
        // Emulation will be very slow if we do a 1000x1000 image especially if debugger attached, 
        // for emulation reduce to say 100
        public const int DIM = 1000;

        public static void Execute(byte[] ptr)
        {
            CudafyModule km = CudafyTranslator.Cudafy();

            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);
            gpu.LoadModule(km);

            byte[] dev_bitmap = gpu.Allocate<byte>(ptr.Length);

            gpu.Launch(new dim3(DIM, DIM), 1).thekernel(dev_bitmap);

            gpu.CopyFromDevice(dev_bitmap, ptr);

            gpu.FreeAll();
            
        }
        
        [Cudafy]
        public static int julia(int x, int y)
        {
            const float scale = 1.5F;
            float jR = scale * (float)(DIM / 2 - x) / (DIM / 2);
            float jI = scale * (float)(DIM / 2 - y) / (DIM / 2);

            float cR = -0.8F;
            float cI = 0.156F;

            int i = 0;
            float tR = jR;
            float tI = jI;
            for (i = 0; i < 200; i++)
            {
                // jx + jy / cx + cy
                //         r * a.r - i
                tR = jR * jR - jI * jI;
                tI = jI * jR + jR * jI;
                jR = tR + cR;
                jI = tI + cI;

                if (jR * jR + jI * jI > 1000)
                    return 0;
            }

            return 1;
        }

        [Cudafy]
        public static void thekernel(GThread thread, byte[] ptr)
        {
            int x = thread.blockIdx.x;
            int y = thread.blockIdx.y;
            int offset = x + y * thread.gridDim.x;

            int juliaValue = julia(x, y);
            ptr[offset * 4 + 0] = (byte)(255.0F * juliaValue);
            ptr[offset * 4 + 1] = 0;
            ptr[offset * 4 + 2] = 0;
            ptr[offset * 4 + 3] = 255;                         
        }

    }
}
