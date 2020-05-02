using Newtonsoft.Json;
using NSoup.Nodes;
using NSoup.Parse;
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
        private static CookieContainer cookies = new CookieContainer();
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
                string Url = "http://220.160.52.164:9085/super/Framework_checkLogin.jspx";
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
            catch
            {

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
            catch
            {

            }
        }

        private void doLogin()
        {
            //string pwd = "5937ffad5ecd5f6dd9a83b83895e480f";//xc123456 md5加密后的字符串
            string postdata = "loginname=%E6%AD%A6%E5%A4%B7%E5%B1%B1%E5%B8%82%E6%98%9F%E6%9D%91%E9%95%87&loginpwd=5937ffad5ecd5f6dd9a83b83895e480f&checkCode=" + this.textBox1.Text;
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

        private string GetASingleInfo(string id)
        {
            string url = $"http://220.160.52.164:9085/fw/FwAction_toInfo.jspx?fw.id={id}&mode=4&page=1&city1=350700&county1=350782&street1=350782003000&community1=350782003007&zgqk1=&pcjl1=&ydxz1=&lrfs1=&sfpkh1=undefined&sfygatb1=&jclx1=&sjqk1=&ywsgct1=&jglx1=&syyt1=&gzqk1=&gznr1=&fwlb1=&yy1=&timeq1=&timez1=&dqzt1=undefined&keywords1=";
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
                result = sr.ReadToEnd();
            }

            Document document = Parser.Parse(result, "");
            Element element = document.GetElementById("hzdh");
            var phoneNumber = element.Attr("value");

            return phoneNumber;
        }

        private List<Record> GetPagedList()
        {
            var outPutList = new List<Record>();
            for (var pageindex = 1; pageindex <= 41; pageindex++)
            {
                string postdata = $"page={pageindex}&rp=20&sortname=undefined&sortorder=desc&query=&qtype=&timeq=&timez=&pcjl=&yy=&fwlb=&ydxz=&lrfs=&sfpkh=&sfygatb=&jclx=&sjqk=&ywsgct=&jglx=&syyt=&gzqk=&gznr=&dqzt=&keywords=&city=350700&county=350782&street=350782003000&community=350782003007&jzmj_min=&jzmj_max=&zgqk=&pcnf=&gdsj_q=&gdsj_z=";
                string loginUrl = "http://220.160.52.164:9085/fw/FwAction_getList.jspx?mode=4";
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
                Stream responseStream = null;
                if (response.ContentEncoding.ToLower() == "gzip")
                {
                    responseStream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                }
                string result = "";
                using (var sr = new StreamReader(responseStream, Encoding.UTF8))
                {
                    result = sr.ReadToEnd();
                }

                var obj = JsonConvert.DeserializeObject<ResultObject>(result);
                foreach (var row in obj.rows)
                {
                    outPutList.Add(new Record()
                    {
                        CurrentState = Parser.ParseBodyFragment(row.cell[2], "").Body.GetElementsByTag("font").Text,
                        Concluse = Parser.ParseBodyFragment(row.cell[3], "").Body.GetElementsByTag("font").Text,
                        RecorderUser = row.cell[4],
                        PaiChaUser = row.cell[5],
                        Year = row.cell[6],
                        Area = row.cell[7],
                        Viliage = row.cell[8],
                        Adress = row.cell[9],
                        HzUserName = row.cell[10],
                        HzPhone = GetASingleInfo(row.cell[27]),
                        IsProll = row.cell[11] == null ? string.Empty : row.cell[11],
                        Catelog = row.cell[12],
                        EarthNature = row.cell[13],
                        BuildYear = row.cell[14],
                        BuildArea = row.cell[15],
                        BasicNature = row.cell[17],
                        OverFlow = row.cell[18],
                        Desgin = row.cell[19],
                        IsHaveScript = row.cell[20] == null ? string.Empty : row.cell[20],
                        ReDesgin = row.cell[21],
                        ReDesignCotent = row.cell[22] == null ? string.Empty : row.cell[22],
                        NowUsage = row.cell[23] == null ? string.Empty : row.cell[23],
                        Structure = row.cell[24] == null ? string.Empty : row.cell[24],
                        DataSource = Parser.ParseBodyFragment(row.cell[25], "").Body.GetElementsByTag("font").Text,
                        AuditTime = row.cell[26],
                        TechUserName = row.cell[29],
                        TechUserPhone = row.cell[30],
                        TechUserUnit = row.cell[31],
                    });
                }
            }

            return outPutList;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //var r=Parser.ParseBodyFragment("<font color =\"green\">暂无安全隐患</font>","");
            //var result = r.Body.GetElementsByTag("font").Text;

            GetCookie();
            doGetImg();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            doLogin();
            var outputList = GetPagedList();
            using (var db = new testEntities())
            {
                foreach (var item in outputList)
                {
                    db.RecorderInfo.Add(new RecorderInfo
                    {
                        CurrentState = item.CurrentState,
                        Concluse = item.Concluse,
                        RecorderUser = item.RecorderUser,
                        PaiChaUser = item.PaiChaUser,
                        Year = item.Year,
                        Area = item.Area,
                        Viliage = item.Viliage,
                        Adress = item.Adress,
                        HzUserName = item.HzUserName,
                        HzPhone = item.HzPhone,
                        IsProll = item.IsProll,
                        Catelog = item.Catelog,
                        EarthNature = item.EarthNature,
                        BuildYear = item.BuildYear,
                        BuildArea = item.BuildArea,
                        BasicNature = item.BasicNature,
                        OverFlow = item.OverFlow,
                        Desgin = item.Desgin,
                        IsHaveScript = item.IsHaveScript,
                        ReDesgin = item.ReDesgin,
                        ReDesignCotent = item.ReDesignCotent,
                        NowUsage = item.NowUsage,
                        Structure = item.Structure,
                        DataSource = item.DataSource,
                        AuditTime = item.AuditTime,
                        TechUserName = item.TechUserName,
                        TechUserPhone = item.TechUserPhone,
                        TechUserUnit = item.TechUserUnit
                    });
                }

                db.SaveChanges();
            }
        }
    }
}
