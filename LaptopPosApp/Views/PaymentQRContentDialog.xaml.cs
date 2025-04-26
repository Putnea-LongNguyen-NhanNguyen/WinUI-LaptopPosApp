using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaptopPosApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PaymentQRContentDialog : Page
    {
        public PaymentQRContentDialog(string qrContent)
        {
            this.InitializeComponent();
            CreateQRImageSource(qrContent);
        }

        private async void CreateQRImageSource(string qrContent)
        {

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new(qrCodeData);
            byte[] qrCodeBytes = qrCode.GetGraphic(20);

            // Convert to image source for WinUI
            var imageSource = new BitmapImage();
            using (var stream = new InMemoryRandomAccessStream())
            {
                // Write bytes to stream without disposing it immediately
                DataWriter writer = new DataWriter(stream);
                writer.WriteBytes(qrCodeBytes);
                await writer.StoreAsync();
                writer.DetachStream(); // Detach the stream so it's not closed when writer is disposed
                writer.Dispose();

                // Reset stream position to beginning
                stream.Seek(0);

                // Set the source
                await imageSource.SetSourceAsync(stream);
            }

            ImageQR.Source = imageSource;
        }
    }
}
