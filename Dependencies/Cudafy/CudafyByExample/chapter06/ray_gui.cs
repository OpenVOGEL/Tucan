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
    public partial class ray_gui : Form
    {
        private bool bDONE = false;

        public const int DIM = 1024;

        public enum eRayVersion { CUDA, CUDA_const, OpenCL, OpenCL_const };

        public ray_gui(eRayVersion rayVersion)
        {
            InitializeComponent();

            Text = rayVersion.ToString();

            int side = DIM;
            Bitmap bmp = new Bitmap(side, side, PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            if (rayVersion == eRayVersion.CUDA_const)
                ray.Execute(rgbValues);
            else if (rayVersion == eRayVersion.CUDA)
                ray_noconst.Execute(rgbValues);
            else if (rayVersion == eRayVersion.OpenCL_const)
                ray_opencl_const.Execute(rgbValues);
            else
                ray_opencl.Execute(rgbValues);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

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

    
}
