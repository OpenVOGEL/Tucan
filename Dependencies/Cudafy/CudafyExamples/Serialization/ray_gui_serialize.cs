using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace CudafyExamples.Serialization
{
    public partial class ray_gui_serialize : Form
    {
        public ray_gui_serialize(bool cleanOnStartUp)
        {
            InitializeComponent();

            Text = "ray";

            if (cleanOnStartUp)
                ray_serialize.Clean();

            int side = ray_serialize.DIM;
            Bitmap bmp = new Bitmap(side, side, PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            ray_serialize.Execute(rgbValues);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            pictureBox.Image = bmp;
        }
    }

    
}
