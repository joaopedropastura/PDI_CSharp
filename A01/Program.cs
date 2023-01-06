using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

void drawHist(Image img)
{

}

byte[] bytes(Image img)
{
    var bmp = img as Bitmap;
    var data = bmp.LockBits(
        new Rectangle(0, 0, img.Width, img.Height),
        ImageLockMode.ReadOnly,
        PixelFormat.Format24bppRgb);
    
    byte[] byteArray = new byte[data.Stride * data.Height];
    Marshal.Copy(data.Scan0, byteArray, 0, byteArray.Length);

    bmp.UnlockBits(data);

    return byteArray;
}

Image img(Image img, byte[] bytes)
{
    var bmp = img as Bitmap;
    var data = bmp.LockBits(
        new Rectangle(0, 0, img.Width, img.Height),
        ImageLockMode.ReadOnly,
        PixelFormat.Format24bppRgb);
    
    Marshal.Copy(bytes, 0, data.Scan0, bytes.Length);

    bmp.UnlockBits(data);
    return img;
}

Image grayScale(Image img)
{
    var bmp = img as Bitmap;
    var result = (Bitmap)bmp.Clone();

    for (int j = 0; j < bmp.Height; j++)
    {
        for (int i = 0; i < bmp.Width; i++)
        {
            var color = bmp.GetPixel(i, j);
            var grayValue = (30 * color.R + 59 * color.G + 11 * color.B) / 100;
            var newColor = Color.FromArgb(grayValue, grayValue, grayValue);
            result.SetPixel(i, j, newColor);
        }
    }

    return result;
}

void show(Image img)
{
    ApplicationConfiguration.Initialize();

    Form form = new Form();

    PictureBox pb = new PictureBox();
    pb.Dock = DockStyle.Fill;
    pb.SizeMode = PictureBoxSizeMode.Zoom;
    form.Controls.Add(pb);

    form.WindowState = FormWindowState.Maximized;
    form.FormBorderStyle = FormBorderStyle.None;

    form.Load += delegate
    {
        pb.Image = img;
    };

    form.KeyDown += (o, e) =>
    {
        if (e.KeyCode == Keys.Escape)
        {
            Application.Exit();
        }
    };

    Application.Run(form);
}

var planta = Bitmap.FromFile("planta.jpg");
// var bytesPlanta = bytes(planta);

// for (int i = 0; i < 2500; i++)
//     bytesPlanta[i] = 3;

// img(planta, bytesPlanta);

show(planta);