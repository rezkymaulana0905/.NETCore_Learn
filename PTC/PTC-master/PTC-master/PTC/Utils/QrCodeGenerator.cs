using QRCoder;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
namespace PTC.Utils;

public class QrCodeGenerator
{
    public static string GenerateQrCode(string code)
    {
        string path = "wwwroot/qr/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
        QRCode qrCode = new QRCode(qrCodeData);    
        Bitmap qrCodeImage = qrCode.GetGraphic(20);
        Bitmap qrCodeWithText = AddTextToQRCode(qrCodeImage, code);
        string fileName = $"{code}.png";
        string filePath = Path.Combine(path, fileName);


        qrCodeWithText.Save(filePath, ImageFormat.Png);

        return filePath;
    }

static Bitmap AddTextToQRCode(Bitmap bitMap, string code)
{
    // Define the rectangle for the text above the QR code
    // Adjust the rectangle size and position as necessary to fit your QR code
    RectangleF rectf1 = new RectangleF(0, 8, bitMap.Width, 100);

    // Create graphics from the bitmap
    Graphics g = Graphics.FromImage(bitMap);

    // Set quality properties for the graphics object
    g.SmoothingMode = SmoothingMode.AntiAlias;
    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

    // Define the string format to center the text
    StringFormat format = new StringFormat();
    format.Alignment = StringAlignment.Center;
    format.LineAlignment = StringAlignment.Center;

    // Draw the text above the QR code, centered horizontally
    g.DrawString(code, new Font("Arial Black", 18   , FontStyle.Bold), Brushes.Black, rectf1, format);

    // Flush and return the modified bitmap
    g.Flush();
    return bitMap;
}

}
