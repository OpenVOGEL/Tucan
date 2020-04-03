using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;
using System;
using System.Collections.Generic;

namespace OpenVOGEL.GpuTools
{
    public static class GpuCore
    {
        /// <summary>
        /// Launches a small program to check if the GPU is up and running.
        /// </summary>
        /// <param name="DeviceId"></param>
        /// <returns></returns>
        public static bool TestGpuDoublePrecision(int DeviceId)
        {
            if (DeviceId > CudafyHost.GetDeviceCount (eGPUType.OpenCL))
            {
                return false;
            }

            try
            {
                CudafyModes.Target = eGPUType.OpenCL;
                CudafyTranslator.Language = eLanguage.OpenCL;
                CudafyModule Module = CudafyTranslator.Cudafy();
                GPGPU gpu = CudafyHost.GetDevice(eGPUType.OpenCL, DeviceId);
                gpu.LoadModule(Module);

                double c;
                double[] dev_c = gpu.Allocate<double>(); 
                gpu.Launch().add_double(2.5d, 7.5d, dev_c);
                gpu.CopyFromDevice(dev_c, out c);                
                gpu.Free(dev_c);
                return c == 10.0d;
            }
            catch
            { return false; }
        }

        /// <summary>
        /// Small cudafy test program to check functionality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        [Cudafy]
        public static void add_double(double a, double b, double[] c)
        {
            c[0] = a + b;
        }

    }

    /// <summary>
    /// Object able to comunicate with the GPU in order to calculate induced velocities.
    /// This object contains a small GPU program (translated later by Codafy) that calculates the
    /// velocity induced by a series of vortices on a series of points.
    /// </summary>
    public class VortexSolver
    {
        private bool Initialized = false;

        private GPGPU Gpu;

        public void Initialize(int DeviceId, String Directory)
        {
            CudafyModes.Target = eGPUType.OpenCL;
            CudafyTranslator.Language = eLanguage.OpenCL;
            CudafyTranslator.WorkingDirectory = Directory;
            CudafyTranslator.DeleteTempFiles = false;
            CudafyModule Module = CudafyTranslator.Cudafy();
            Gpu = CudafyHost.GetDevice(eGPUType.OpenCL, DeviceId);
            Gpu.LoadModule(Module);
            Initialized = true;
        }
        
        /// <summary>
        /// Calculates the velocity at the given control points induced by the given vortex segments.
        /// </summary>
        /// <param name="DeviceId">The GPU device to use</param>
        /// <param name="G">The circulation of the vortices</param>
        /// <param name="Ax">Vortex node A</param>
        /// <param name="Ay">Vortex node A</param>
        /// <param name="Az">Vortex node A</param>
        /// <param name="Bx">Vortex node B</param>
        /// <param name="By">Vortex node B</param>
        /// <param name="Bz">Vortex node B</param>
        /// <param name="Px">Control point</param>
        /// <param name="Py">Control point</param>
        /// <param name="Pz">Control point</param>
        /// <param name="Vx">Velocity</param>
        /// <param name="Vy">Velocity</param>
        /// <param name="Vz">Velocity</param>
        /// <param name="Cutoff">Cutoff parameter</param>
        public void CalculateVelocity (
            double[] G,
            double[] Ax, double[] Ay, double[] Az,
            double[] Bx, double[] By, double[] Bz,
            double[] Px, double[] Py, double[] Pz,
            double[] Vx, double[] Vy, double[] Vz,
            double Cutoff)
        {

            if (!Initialized) { return; }

            // Cuttoff

            double[] C = new double[1];
            C[0] = Cutoff;
            double[] C_D = Gpu.Allocate<double>(C);
            Gpu.CopyToDevice(C, C_D);
            
            // Circulation

            double[] G_D = Gpu.Allocate<double>(G);
            Gpu.CopyToDevice(G, G_D);

            // Vortex node A

            double[] Ax_D = Gpu.Allocate<double>(Ax);
            Gpu.CopyToDevice(Ax, Ax_D);

            double[] Ay_D = Gpu.Allocate<double>(Ay);
            Gpu.CopyToDevice(Ay, Ay_D);

            double[] Az_D = Gpu.Allocate<double>(Az);
            Gpu.CopyToDevice(Az, Az_D);

            // Vortex node B

            double[] Bx_D = Gpu.Allocate<double>(Bx);
            Gpu.CopyToDevice(Bx, Bx_D);

            double[] By_D = Gpu.Allocate<double>(By);
            Gpu.CopyToDevice(By, By_D);

            double[] Bz_D = Gpu.Allocate<double>(Bz);
            Gpu.CopyToDevice(Bz, Bz_D);

            // Control point

            double[] Px_D = Gpu.Allocate<double>(Px);
            Gpu.CopyToDevice(Px, Px_D);

            double[] Py_D = Gpu.Allocate<double>(Py);
            Gpu.CopyToDevice(Py, Py_D);

            double[] Pz_D = Gpu.Allocate<double>(Pz);
            Gpu.CopyToDevice(Pz, Pz_D);

            // Velocity

            double[] Vx_D = Gpu.Allocate<double>(Vx);
            Gpu.CopyToDevice(Vx, Vx_D);

            double[] Vy_D = Gpu.Allocate<double>(Vy);
            Gpu.CopyToDevice(Vy, Vy_D);

            double[] Vz_D = Gpu.Allocate<double>(Vz);
            Gpu.CopyToDevice(Vz, Vz_D);

            int nThreads = Vx.GetLength(0);
            int nBlocks = 1;

            Gpu.Launch(nBlocks, nThreads).CalculateVelocities(
                G_D, C_D,
                Ax_D, Ay_D, Az_D,
                Bx_D, By_D, Bz_D,
                Px_D, Py_D, Pz_D,
                Vx_D, Vy_D, Vz_D);

            Gpu.CopyFromDevice(Vx_D, Vx);
            Gpu.CopyFromDevice(Vy_D, Vy);
            Gpu.CopyFromDevice(Vz_D, Vz);

            // Free GPU memory

            Gpu.Free(G_D);

            Gpu.Free(Ax_D);
            Gpu.Free(Ay_D);
            Gpu.Free(Az_D);

            Gpu.Free(Bx_D);
            Gpu.Free(By_D);
            Gpu.Free(Bz_D);

            Gpu.Free(Px_D);
            Gpu.Free(Py_D);
            Gpu.Free(Pz_D);

            Gpu.Free(Vx_D);
            Gpu.Free(Vy_D);
            Gpu.Free(Vz_D);
        }

        [Cudafy]
        public static void CalculateVelocities(GThread thread, 
            double[] G, double[] C,
            double[] Ax, double[] Ay, double[] Az,
            double[] Bx, double[] By, double[] Bz,
            double[] Px, double[] Py, double[] Pz,
            double[] Vx, double[] Vy, double[] Vz)
        {
            int i = thread.threadIdx.x;
            
            Vx[i] = 0d;
            Vy[i] = 0d;
            Vz[i] = 0d;

            for (int j = 0; j < Ax.GetLength(0); j++)
            {
                // NOTE: this can be performed only once

                double Lx = Bx[j] - Ax[j];
                double Ly = By[j] - Ay[j];
                double Lz = Bz[j] - Az[j];

                double R1x = Px[i] - Ax[j];
                double R1y = Py[i] - Ay[j];
                double R1z = Pz[i] - Az[j];

                double vx = Ly * R1z - Lz * R1y;
                double vy = Lz * R1x - Lx * R1z;
                double vz = Lx * R1y - Ly * R1x;

                double D = 12.566370614359d * (vx * vx + vy * vy + vz * vz);
                
                if (D > C[0])
                {
                    double R2x = Px[i] - Bx[j];
                    double R2y = Py[i] - By[j];
                    double R2z = Pz[i] - Bz[j];

                    double NR1 = 1d / GMath.Sqrt((float) (R1x * R1x + R1y * R1y + R1z * R1z));
                    double NR2 = 1d / GMath.Sqrt((float) (R2x * R2x + R2y * R2y + R2z * R2z));

                    double dx = NR1 * R1x - NR2 * R2x;
                    double dy = NR1 * R1y - NR2 * R2y;
                    double dz = NR1 * R1z - NR2 * R2z;

                    double F = G[j] * (Lx * dx + Ly * dy + Lz * dz) / D;

                    Vx[i] = Vx[i] + F * vx;
                    Vy[i] = Vy[i] + F * vy;
                    Vz[i] = Vz[i] + F * vz;
                }
            }
        }

    }
}
