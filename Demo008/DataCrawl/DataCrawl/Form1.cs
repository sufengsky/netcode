using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace DataCrawl
{
    public partial class Form1 : Form
    {
        private static  CookieContainer cookies = new CookieContainer();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGetImage_Click(object sender, EventArgs e)
        {
            string url = "http://220.160.52.164:9085/super/pages/login/image.jsp?_time=" + DateTime.Now.ToString();
            doGetImg(url);
        }

        private void doGetImg(string Url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url.ToString());
                request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
                request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN");
                request.Accept = "text/html, application/xhtml+xml,*/*";
                request.KeepAlive = true;

                request.UserAgent = "User-Agent Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                request.AllowAutoRedirect = true;
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Host = "220.160.52.164:9085";
                request.CookieContainer = new CookieContainer(); //暂存到新实例
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream responseStream = null;
                MemoryStream ms = null;
                if (response.ContentEncoding.ToLower() == "gzip")
                {
                    responseStream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    using (var stream = responseStream)
                    {
                        Byte[] buffer = new Byte[4096];
                        int offset = 0, actuallyRead = 0;
                        do
                        {
                            actuallyRead = stream.Read(buffer, offset, buffer.Length - offset);
                            offset += actuallyRead;
                        }
                        while (actuallyRead > 0);
                        ms = new MemoryStream(buffer);
                    }

                }
                cookies = request.CookieContainer; //保存cookies
                response.Close();
                //var cookiesstr = request.CookieContainer.GetCookieHeader(request.RequestUri); //把cookies转换成字符串
                var img = Bitmap.FromStream(ms);
                if (File.Exists("code.jpeg"))
                {
                    File.Delete("code.jpeg");
                }
                img.Save("code.jpg", ImageFormat.Jpeg);
            }
            catch
            {
            }
        }
    }
}
