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
            doGetImg();
        }
        private void GetCookie()
        {
            try
            {
                //要post提交的地址。先用HttpWebRequest进行请求以得到cookie，并保存起来在后面获取验证码的时候使用
                string Url="http://220.160.52.164:9085/super/Framework_checkLogin.jspx";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
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

                var st = request.CookieContainer.GetCookieHeader(request.RequestUri);
                //将当前cookie存储到CookieContainer中
                cookies.Add(response.Cookies);
                response.Close();
            }
            catch { 
            
            }
        }
        private void doGetImg()
        {
            string url = "http://220.160.52.164:9085/super/pages/login/image.jsp?_time=" + DateTime.Now.ToString();
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
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
                request.CookieContainer = cookies; //暂存到新实例
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
                var cookiesstr = request.CookieContainer.GetCookieHeader(request.RequestUri); //把cookies转换成字符串
                var img = Bitmap.FromStream(ms);

                this.pictureBox1.Image = img;
            }
            catch(Exception ex)
            {

            }
        } 

        private void doLogin()
        {
            //string pwd = "5937ffad5ecd5f6dd9a83b83895e480f";//xc123456 md5加密后的字符串
            string postdata = "loginname=%E6%AD%A6%E5%A4%B7%E5%B1%B1%E5%B8%82%E6%98%9F%E6%9D%91%E9%95%87&loginpwd=5937ffad5ecd5f6dd9a83b83895e480f&checkCode="+this.textBox1.Text;
            string loginUrl = "http://220.160.52.164:9085/super/super/Framework_doLogin.jspx";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(loginUrl);
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.9");
            request.Accept = "text/html, */*; q=0.01";
            request.KeepAlive = true;

            request.UserAgent = "User-Agent Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.AllowAutoRedirect = true;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Host = "220.160.52.164:9085";
            request.CookieContainer = cookies;

            byte[] postdatabytes = Encoding.UTF8.GetBytes(postdata);
            request.ContentLength = postdatabytes.Length;
            Stream stream = request.GetRequestStream();
            //设置POST 数据
            stream.Write(postdatabytes, 0, postdatabytes.Length);
            stream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            cookies = request.CookieContainer; //保存cookies
            response.Close();
            var cookiesstr = request.CookieContainer.GetCookieHeader(request.RequestUri); //把cookies转换成字符串
        }

        private void GetASingleInfo()
        {
            string url = "http://220.160.52.164:9085/fw/FwAction_toInfo.jspx?fw.id=BFC58877-B7CB-009A-E043-0A822906009A&mode=4&page=1&city1=350700&county1=350782&street1=350782003000&community1=350782003007&zgqk1=&pcjl1=&ydxz1=&lrfs1=&sfpkh1=undefined&sfygatb1=&jclx1=&sjqk1=&ywsgct1=&jglx1=&syyt1=&gzqk1=&gznr1=&fwlb1=&yy1=&timeq1=&timez1=&dqzt1=undefined&keywords1=";
            string srl = "http://220.160.52.164:9085/fw/FwAction_toInfo.jspx?fw.id=BFC3D75C-F13A-01DA-E043-0A82290601DA&mode=4&page=1&city1=350700&county1=350782&street1=350782003000&community1=350782003007&zgqk1=&pcjl1=&ydxz1=&lrfs1=&sfpkh1=undefined&sfygatb1=&jclx1=&sjqk1=&ywsgct1=&jglx1=&syyt1=&gzqk1=&gznr1=&fwlb1=&yy1=&timeq1=&timez1=&dqzt1=undefined&keywords1=";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN");
            request.Accept = "text/html, application/xhtml+xml,*/*";
            request.KeepAlive = true;

            request.UserAgent = "User-Agent Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.AllowAutoRedirect = true;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Host = "220.160.52.164:9085";
            request.CookieContainer = cookies; 
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = null;
            if (response.ContentEncoding.ToLower() == "gzip")
            {
                responseStream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
            }
            string result = "";
            using (var sr = new StreamReader(responseStream, Encoding.UTF8))
            {
                 result=sr.ReadToEnd();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetCookie();
            doGetImg();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            doLogin();
            GetASingleInfo();
        }
    }
}
