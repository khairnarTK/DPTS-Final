using System;
using System.IO;
using System.Net;

namespace DPTS.Common
{
    public class QrCodeGenerator
    {
        public static bool GenerateQrCode(string profileUrl, string savePath)
        {
            bool result = false;
            try
            {
                WebClient client = new WebClient();
                string url = "https://chart.googleapis.com/chart?chs=100x100&cht=qr&chl=" + profileUrl;
                var qrcodeImage = client.DownloadData(url);
                var memStream = new MemoryStream(qrcodeImage);
                /*MediaTypeNames.Image img = MediaTypeNames.Image.FromStream(memStream);
                img.Save(savePath);*/
                result = true;
            }
            catch (Exception exception)
            {
            }
            return result;
        }

        public static bool IsQrCodeExists(string path)
        {
            return File.Exists(path);
        }
    }
}