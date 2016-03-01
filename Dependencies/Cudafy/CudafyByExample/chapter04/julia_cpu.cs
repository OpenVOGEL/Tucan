/* 
 * This software is based upon the book CUDA By Example by Sanders and Kandrot
 * and source code provided by NVIDIA Corporation.
 * It is a good idea to read the book while studying the examples!
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Cudafy;
namespace CudafyByExample
{

    
    public partial class julia_gui : Form
    {
        private bool bDONE = false;
        
        public julia_gui(bool gpu)
        {
            
            InitializeComponent();

            Text = gpu ? "julia_gpu" : "julia_cpu";
            
            int side = gpu ? julia_gpu.DIM : julia_cpu.DIM;
            Bitmap bmp = new Bitmap(side, side, PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes  = bmpData.Stride * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            if (!gpu)
                julia_cpu.Execute(rgbValues);
            else
                julia_gpu.Execute(rgbValues);

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            pictureBox.Image = bmp;

            bDONE = true;

            if (CudafyModes.Target == eGPUType.Emulator)
                timer1.Interval = 120000;
            
            timer1.Start();
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            if(bDONE)
                Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (bDONE)
                Close();
        }
    }

    public class julia_cpu
    {
        public const int DIM = 1000;

        public static void Execute(byte[] ptr)
        {
            julia_cpu julia = new julia_cpu();
            julia.kernel(ptr);
        }

        int julia(int x, int y)
        {
            const float scale = 1.5F;
            float jx = scale * (float)(DIM / 2 - x) / (DIM / 2);
            float jy = scale * (float)(DIM / 2 - y) / (DIM / 2);

            cuComplex c = new cuComplex(-0.8F, 0.156F);
            cuComplex a = new cuComplex(jx, jy);

            int i = 0;
            for (i = 0; i < 200; i++)
            {
                a = a * a + c;
                if (a.magnitude2() > 1000)
                    return 0;
            }

            return 1;
        }

        public void kernel(byte[] ptr)
        {
            for (int y = 0; y < DIM; y++)
            {
                for (int x = 0; x < DIM; x++)
                {
                    int offset = x + y * DIM;

                    int juliaValue = julia(x, y);
                    ptr[offset * 4 + 0] = (byte)(255.0F * juliaValue);
                    ptr[offset * 4 + 1] = 0;
                    ptr[offset * 4 + 2] = 0;
                    ptr[offset * 4 + 3] = 255;
                }
            }
        }
    }
}
