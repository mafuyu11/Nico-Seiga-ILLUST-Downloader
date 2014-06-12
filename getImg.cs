using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace nocoSaiga
{
    public class getImg
    {
        #region prop
        public static string newXMLFile
        {
            get
            {
                return Application.StartupPath + @"\new.xml";
            }
        }
        
        #endregion

        public void new_illust()
        {
            File.Delete(newXMLFile);
            {
                var sx = new XDocument();
                var root = new XElement("root", "");
                sx.Add(root);
                sx.Save(newXMLFile);
                sx = null;
            }
            using (var wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                var ng = wb.DownloadString(@"http://seiga.nicovideo.jp/rss/illust/new");
                var doc = XDocument.Parse(ng);

                foreach (var vc in doc.Descendants("item"))
                {
                    doc = XDocument.Load(newXMLFile);
                    doc.Root.Add(new XElement("item",
                        new XElement("author", "게시자 : " + vc.Element("author").Value),
                        new XElement("img", vc.Element("link").Value.Replace(@"http://seiga.nicovideo.jp/seiga/im", @"http://lohas.nicoseiga.jp/img/") + "m"),
                        new XElement("link", "출처 : " + vc.Element("link").Value)
                        ));
                    doc.Save(newXMLFile);
                    doc = null;
                }
            }
        }

        public void sel_illust(string query)
        {
            File.Delete(newXMLFile);
            {
                var sx = new XDocument();
                var root = new XElement("root", "");
                sx.Add(root);
                sx.Save(newXMLFile);
                sx = null;
            }
            var ss = loginToNicoVide("http://seiga.nicovideo.jp/api/tagslide/data?query=" + query);
            var docv = XDocument.Parse(ss);
            
            foreach (var sr in docv.Descendants("image"))
            {
                var doc = XDocument.Load(newXMLFile);
                doc.Root.Add(new XElement("item",
                    new XElement("author", "게시자 : " + sr.Element("nickname").Value),
                    new XElement("img", @"http://lohas.nicoseiga.jp/img/" + sr.Element("id").Value + "m"),
                    new XElement("link", "출처 : " + @"http://seiga.nicovideo.jp/seiga/im" + sr.Element("id").Value)
                    ));
                doc.Save(newXMLFile);
                doc = null;
            }

        }

        private string loginToNicoVide(string _url)
        {
            string id = "mafuyu12@daum.net";//メアド

            string password = "park4394";//パスワード

            Hashtable vals = new Hashtable();

            vals["next_url"] = "";

            vals["mail"] = id;

            vals["password"] = password;

            string url = "https://secure.nicovideo.jp/secure/login?site=niconico";

            string param = "";

            foreach (string k in vals.Keys)
            {
                param += String.Format("{0}={1}&", k, vals[k]);
            }

            byte[] data = Encoding.ASCII.GetBytes(param);

            CookieContainer cc = new CookieContainer(); 

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            req.Method = "POST";

            req.ContentType = "application/x-www-form-urlencoded";

            req.ContentLength = data.Length;

            req.CookieContainer = cc;


            Stream reqStream = req.GetRequestStream();

            reqStream.Write(data, 0, data.Length);

            reqStream.Close();

            WebResponse res = req.GetResponse();

            Stream resStream = res.GetResponseStream();

            Encoding encoder = Encoding.GetEncoding("UTF-8");

            StreamReader sr = new StreamReader(resStream, encoder);

            string result = sr.ReadToEnd();

            sr.Close();

            resStream.Close();

            Console.WriteLine(result);

            req = (HttpWebRequest)WebRequest.Create(_url);

            req.CookieContainer = cc;

            res = req.GetResponse();

            resStream = res.GetResponseStream();

            sr = new StreamReader(resStream, encoder);

            result = sr.ReadToEnd();

            sr.Close();

            resStream.Close();

            return result;

        }
    }
}
