/* 
 * This software is based upon the book CUDA By Example by Sanders and Kandrot
 * and source code provided by NVIDIA Corporation.
 * It is a good idea to read the book while studying the examples!
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;

namespace CudafyByExample
{

    public class ray_opencl_const
    {
        public const int RAND_MAX = Int32.MaxValue;
        public const float INF = 2e10f;

        public static float rnd(float x)
        {
            float f = x * (float)rand.NextDouble();
            return f;
        }

        public static Random rand = new Random((int)DateTime.Now.Ticks);

        public const int SPHERES = 20;

        [Cudafy]
        public static float hit(SphereOpenCL s, float ox1, float oy1, ref float n1)
        {
            float dx = ox1 - s.x;
            float dy = oy1 - s.y;
            if (dx * dx + dy * dy < s.radius * s.radius)
            {
                float dz = GMath.Sqrt(s.radius * s.radius - dx * dx - dy * dy);
                n1 = dz / GMath.Sqrt(s.radius * s.radius);
                return dz + s.z;
            }
            return -2e10f;
        }

        [Cudafy]
        public static SphereOpenCL[] spheres = new SphereOpenCL[SPHERES];

        [Cudafy]
        public static void thekernel(GThread thread, byte[] ptr)
        {
            // map from threadIdx/BlockIdx to pixel position
            int x = thread.threadIdx.x + thread.blockIdx.x * thread.blockDim.x;
            int y = thread.threadIdx.y + thread.blockIdx.y * thread.blockDim.y;
            int offset = x + y * thread.blockDim.x * thread.gridDim.x;
            float ox = (x - ray_gui.DIM / 2);
            float oy = (y - ray_gui.DIM / 2);

            float r = 0, g = 0, b = 0;
            float maxz = -INF;
            for (int i = 0; i < SPHERES; i++)
            {
                float n = 0;

                float t = hit(spheres[i], ox, oy, ref n);
                if (t > maxz)
                {
                    float fscale = n;
                    r = spheres[i].r * fscale;
                    g = spheres[i].g * fscale;
                    b = spheres[i].b * fscale;
                    maxz = t;
                }
            }

            ptr[offset * 4 + 0] = (byte)(r * 255);
            ptr[offset * 4 + 1] = (byte)(g * 255);
            ptr[offset * 4 + 2] = (byte)(b * 255);
            ptr[offset * 4 + 3] = 255;
        }

        public static void Execute(byte[] bitmap)
        {
            CudafyModule km = CudafyModule.TryDeserialize();
            if (km == null || !km.TryVerifyChecksums())
            {
                km = CudafyTranslator.Cudafy(typeof(SphereOpenCL), typeof(ray_opencl_const));
                km.TrySerialize();
            }

            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);
            gpu.LoadModule(km);

            // capture the start time
            gpu.StartTimer();

            // allocate memory on the GPU for the bitmap (same size as ptr)
            byte[] dev_bitmap = gpu.Allocate(bitmap);

            // allocate memory for the Sphere dataset
            //SphereOpenCL[] s = gpu.Allocate<SphereOpenCL>(SPHERES);

            // allocate temp memory, initialize it, copy to constant memory on the GPU
            SphereOpenCL[] temp_s = new SphereOpenCL[SPHERES];
            for (int i = 0; i < SPHERES; i++)
            {
                temp_s[i].r = rnd(1.0f);
                temp_s[i].g = rnd(1.0f);
                temp_s[i].b = rnd(1.0f);

                temp_s[i].x = rnd(1000.0f) - 500;
                temp_s[i].y = rnd(1000.0f) - 500;
                temp_s[i].z = rnd(1000.0f) - 500;
                temp_s[i].radius = rnd(100.0f) + 20;

            }
            //gpu.CopyToDevice(temp_s, s);
            gpu.CopyToConstantMemory(temp_s, spheres);

            // generate a bitmap from our sphere data
            dim3 grids = new dim3(ray_gui.DIM / 16, ray_gui.DIM / 16);
            dim3 threads = new dim3(16, 16);
            //gpu.Launch(grids, threads).kernel(s, dev_bitmap); // Dynamic
            gpu.Launch(grids, threads, ((Action<GThread, byte[]>)thekernel), dev_bitmap); // Strongly typed

            // copy our bitmap back from the GPU for display
            gpu.CopyFromDevice(dev_bitmap, bitmap);

            // get stop time, and display the timing results
            float elapsedTime = gpu.StopTimer();
            Console.WriteLine("Time to generate: {0} ms", elapsedTime);

            gpu.FreeAll();
        }
    }
}
