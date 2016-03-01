using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using Cudafy;
using Cudafy.Host;


namespace CudafyExamples.Serialization
{
    [Cudafy]
    public struct Sphere
    {
        public float r;
        public float b;
        public float g;
        public float radius;
        public float x;
        public float y;
        public float z;

        public float hit(float ox1, float oy1, ref float n1)
        {
            float dx = ox1 - x;
            float dy = oy1 - y;
            if (dx * dx + dy * dy < radius * radius)
            {
                float dz = GPUMath.Sqrt(radius * radius - dx * dx - dy * dy);
                n1 = dz / GPUMath.Sqrt(radius * radius);
                return dz + z;
            }
            return -2e10f;
        }
    }
    
    public class ray_serialize
    {                
        public const int DIM = 1024;
        public const int RAND_MAX = Int32.MaxValue;
        public const float INF = 2e10f;



        public static float rnd( float x ) 
        {
            float f = x * (float)rand.NextDouble();
            return f;
        }

        public static Random rand = new Random((int)DateTime.Now.Ticks);

        public const int SPHERES = 20;

        [Cudafy]
        public static Sphere[] s = new Sphere[SPHERES];

        [Cudafy]
        public void kernel(HThread thread, byte[] ptr ) 
        {
            // map from threadIdx/BlockIdx to pixel position
            int x = thread.threadIdx.x + thread.blockIdx.x * thread.blockDim.x;
            int y = thread.threadIdx.y + thread.blockIdx.y * thread.blockDim.y;
            int offset = x + y * thread.blockDim.x * thread.gridDim.x;
            float   ox = (x - DIM/2);
            float   oy = (y - DIM/2);

            float   r=0, g=0, b=0;
            float   maxz = -INF;
            
            for(int i=0; i<SPHERES; i++) 
            {
                float n = 0;
                
                float   t = s[i].hit( ox, oy, ref n );
                if (t > maxz) 
                {
                    float fscale = n;
                    r = s[i].r * fscale;
                    g = s[i].g * fscale;
                    b = s[i].b * fscale;
                    maxz = t;
                }
            }
           
            ptr[offset * 4 + 0] = (byte)(r * 255);
            ptr[offset * 4 + 1] = (byte)(g * 255);
            ptr[offset * 4 + 2] = (byte)(b * 255);
            ptr[offset * 4 + 3] = 255;
        }

        private const string csFILENAME = "ray_serialize";

        public static void Clean()
        {
            CudafyModule.Clean(csFILENAME);
        }

        public static void Execute(byte[] bitmap)
        {
            DateTime dt = DateTime.Now;
            CudafyModule km = CudafyModule.TryDeserialize(csFILENAME);
            // Check the module exists and matches the .NET modules, else make new
            if (km == null || !km.TryVerifyChecksums())
            {
                Console.WriteLine("There was no cached module available so we make a new one.");
                km = CudafyModule.Deserialize(typeof(ray_serialize).Name);
                km.Serialize(csFILENAME);
            }

            GPGPU gpu = CudafyHost.GetGPGPU(CudafyModes.Target, 1);
            gpu.LoadModule(km);

            Console.WriteLine("Time taken to load module: {0}ms", DateTime.Now.Subtract(dt).Milliseconds);

            // capture the start time
            gpu.StartTimer();

            // allocate memory on the GPU for the bitmap (same size as ptr)
            byte[] dev_bitmap = gpu.Allocate(bitmap);

            // allocate temp memory, initialize it, copy to constant memory on the GPU
            Sphere[] temp_s = new Sphere[SPHERES]; 
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

            gpu.CopyToConstantMemory(temp_s, s);

            // generate a bitmap from our sphere data
            dim3 grids = new dim3(DIM/16, DIM/16);
            dim3 threads = new dim3(16, 16);
            gpu.Launch(grids, threads, "kernel", dev_bitmap);

            // copy our bitmap back from the GPU for display
            gpu.CopyFromDevice(dev_bitmap, bitmap);

            // get stop time, and display the timing results
            float elapsedTime = gpu.StopTimer();
            Console.WriteLine("Time to generate: {0} ms", elapsedTime);

            gpu.DeviceFreeAll();
        }
    }
}
