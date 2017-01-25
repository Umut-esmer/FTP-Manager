using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FTP_Yöneticisi
{
    public class SiteBilgileri
    {
        private string serverName = null;
        private string serverPass = null;

        public string KullanıcıAdı
        {
            get { return serverName; }
            set
            {
                if (value != "")
                {
                    serverName = value;
                }
            }
        }

        public string Şifre
        {
            get { return serverPass; }
            set
            {
                if (value != "")
                {
                    serverPass = value;
                }
            }
        }

        public SiteBilgileri(string kullanıcıAdı, string şifre)
        {
            KullanıcıAdı = kullanıcıAdı;
            Şifre = şifre;
        }
    }

    public class Yükleyici
    {
        private string name = null;
        private string pass = null;

        public Yükleyici(SiteBilgileri bilgi)
        {
            name = bilgi.KullanıcıAdı;
            pass = bilgi.Şifre;
        }

        public delegate void DosyaYüklediğinde(string dosyaAdı, string yüklenenFtpAdresi);
        public event DosyaYüklediğinde DosyaYüklendi;
        public void Yükle(Link adres, YerelDosyaYolu yol)
        {
            if (name != null && pass != null)
            {
                WebClient client = new WebClient();
                client.Credentials = new NetworkCredential(name, pass);
                client.UploadFileCompleted += (object sender, UploadFileCompletedEventArgs e) =>
                {
                    DosyaYüklendi(yol.Adres, adres.Adres.ToString());
                };
                client.UploadFileAsync(adres.Adres, yol.Adres);
            }
        }
    }

    public class İndirici
    {
        private string name = null;
        private string pass = null;

        public İndirici(SiteBilgileri bilgi)
        {
            name = bilgi.KullanıcıAdı;
            pass = bilgi.Şifre;
        }

        public delegate void Dosyaİndirildiğinde(string dosyaAdı);
        public event Dosyaİndirildiğinde Dosyaİndirildi;

        public delegate void DosyaİndirilmeDurumuDeğiştiğinde(string dosyaAdı, long indirilenByte, long toplamByte);
        public event DosyaİndirilmeDurumuDeğiştiğinde DosyaİndirilmeDurumuDeğişti;
        public void İndir(Link adres, YerelDosyaYolu yol)
        {
            if (name != null && pass != null)
            {
                WebClient client = new WebClient();
                client.Credentials = new NetworkCredential(name, pass);
                client.DownloadFileCompleted += (object sender, System.ComponentModel.AsyncCompletedEventArgs e) =>
                {
                    Dosyaİndirildi(yol.Adres);
                };
                client.DownloadProgressChanged += (object sender, DownloadProgressChangedEventArgs e) =>
                {
                    DosyaİndirilmeDurumuDeğişti(yol.Adres, e.BytesReceived, e.TotalBytesToReceive);
                };
                client.DownloadFileAsync(adres.Adres, yol.Adres);
            }
        }
    }

    public class Link
    {
        private Uri link = null;

        public Uri Adres
        {
            get { return link; }
            set { link = new Uri(value.ToString()); }
        }

        public Link(string link)
        {
            Adres = new Uri(link);
        }
    }

    public class YerelDosyaYolu
    {
        private string link = null;

        public string Adres
        {
            get { return link; }
            set
            {
                if (value != "") link = value;
            }
        }

        public YerelDosyaYolu(string link)
        {
            Adres = link;
        }
    }
}

