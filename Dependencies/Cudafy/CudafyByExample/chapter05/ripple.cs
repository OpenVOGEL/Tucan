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
    public partial class ripple : Form
    {
        private bool bDONE = false;
        
        public ripple()
        {
            InitializeComponent();
        }

        public void Execute()
        {
            Show();
            int loops = CudafyModes.Target == eGPUType.Emulator ? 2 : 200;
            int side = ripple_gpu.DIM;
            Bitmap bmp = new Bitmap(side, side, PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            int bytes = side * side * 4;
            byte[] rgbValues = new byte[bytes];
            ripple_gpu ripple = new ripple_gpu();
            ripple.Initialize(bytes);
            for (int x = 0; x < loops && !bDONE; x++)
            {
                ripple.Execute(rgbValues, Environment.TickCount);
                BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
                IntPtr ptr = bmpData.Scan0;

                System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
                bmp.UnlockBits(bmpData); 
                Text = x.ToString();
                pictureBox.Image = bmp;
                Refresh();
            }
            ripple.ShutDown();
            if(CudafyModes.Target == eGPUType.Emulator)
                MessageBox.Show("Click to continue.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}
