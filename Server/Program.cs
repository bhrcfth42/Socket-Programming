using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Server";//Konsol başlığını belirliyoruz.
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 13000);//Ip adressi ve port numaramızı belirliyoruz
            Socket Serverlistener = new Socket(IPAddress.Parse("0.0.0.0").AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);//server ıp adresini belileyip,Socket bağlantısı oluşturuyoruz ve türünü belirliyoruz.
            Serverlistener.Bind(localEndPoint);//server bağlantı imkanını Açıyoruz
            Serverlistener.Listen(1);//server Dinlemeye başlatıyoruz
            Console.WriteLine("Server Dinlemeye Başladı");//ekrana Server dinlemeye başladı yazıyoruz
            Socket clientsocket = default(Socket);//Clientin Socket bağlantısının uygunluğu için bekliyoruz.
            Program p = new Program();//thread yapısı oluşturarak serverın çoklu client desteğini sağlıyoruz.
            while (true)//Sürekli olarak her isteği kullanıcıları bekliyor dinlemeye başlıyor
            {
                clientsocket = Serverlistener.Accept();//Client bağlanmasını bekliyor
                Console.WriteLine(clientsocket.RemoteEndPoint.ToString() + " Client Bağlandı");//Bağlanan Client'in Ip adresini Client bağlandı olarak yazıyoruz.
                Thread userThread = new Thread(new ThreadStart(() => p.User(clientsocket)));//Bağlanan clientın bilgilerini thread olarak çalıştırmak için User fonksiyonuna aktaruyoruz.
                userThread.Start();//Thread işlemini başlatıyoruz.
            }
        }
        public void User(Socket client)
        {
            var a = client.RemoteEndPoint.ToString().Split(':')[0];//Client Ip adresini algılatıyoruz. Port bağlantısı ile algıladığı için port kısmını string işleminde çıkarıyoruz. Bu sayede 192.168.1.1:13000 yerine sadece 192.168.1.1 şeklinde okuyor
            IPHostEntry hostEntry = Dns.GetHostEntry(a);//Ip adresinin Host name'ni buluyoruz
            string baglantikayit = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop).ToString() + "\\baglantikayit.txt";//Kayıt tutacağımız dosyanın masaüstünde baglantikayit.txt şeklinde kayıt edebilmesi için yol belirliyoruz                              
            while (true)//Bağlanan Clientin sürekli veri gönderip göndermediğini dinliyoruz.
            {
                while (client.Available > 0)//Clientten Gelen veri var ise işleme başlamasını istiyoruz. Veri gelip gelmediğinin kontrolünü yapıyoruz.
                {
                    byte[] islemclient = new byte[1024];//Gelen veri için bir byte değeri belirliyoruz.
                    int size=client.Receive(islemclient);//Clientten gelen veriyi okuyoruz ve boyutunu eşitliyoruz.
                    var deger = Encoding.ASCII.GetString(islemclient,0,size);//gelen veri acsii tablosuna göre byte değerinde olduğu için server az yorulması için gelen veri boyutu kadar stringe çeviri yapıyor.
                    if (deger.Substring(0,1) == "1")//clientten gelen ilk byte değeri bizim için yapaacağımız işlemi belirttiği için 1 ise eğer toplama işlemi olduğunu anlıyoruz.
                    {
                        var islem = deger.Substring(1);//burdada ilk karakterimizi çıkartıp clientin hesaplaması için gönderdiği değeri string şeklinde algılıyoruz.
                        while (islem.Length > 0)//string uzunluğu 0 dan büyük olduğu sürece çalış şeklinde komut veriyoruz
                        {
                            var sonuc = Toplama(Convert.ToInt64(islem));//işlem değerimizi int değere çevirip sonuca eşitliyoruz.
                            client.Send(Encoding.ASCII.GetBytes(sonuc.ToString()), 0, sonuc.ToString().Length, SocketFlags.None);//Bu işlem sonucunu cliente gönderiyoruz.,length yazan kısım uzunluğumuzu belirtiyor bu sayede 1024 değer döndermek yerine gerekli miktarda veri gönderiyor.Ağı doldurmaya çalışmıyor.
                            islem = sonuc.ToString();//hesaplanan sonucu islem değerine eşitliyoruz.
                            if (islem.Length == 1)//işlem sonucu eğer tek karakter olduysa ok sonucu için kontrol sorgulaması yapıyoruz.
                            {
                                client.Send(Encoding.ASCII.GetBytes("ok"), 0, 2, SocketFlags.None);//Tek değer kaldığı için toplama işlemi sonucunda sunucuya ok diye değer gönderiyoruz,0 index başlangıcımız,2 olarak belirttiğimiz mesajın ozunluğu ok olduğu için 2 kelime oyüzden 2
                                break;//while döngüsünden hızlıca çıkmasını sağlıyoruz.
                            }
                        }
                        FileStream fs = new FileStream(baglantikayit, FileMode.Append, FileAccess.Write);//Burada kayıt yapacağımız Dosyanın adı Dosya modu ve ne için açtığımızı belirtiyoruz.
                        StreamWriter sw = new StreamWriter(fs);//dosyayı açıyoruz.
                        sw.WriteLine("Host Adı=" + hostEntry.HostName + "\tIpv4 Adres=" + hostEntry.AddressList[1] + "\tIpv6 Adres=" + hostEntry.AddressList[0] + "\tSistem Saati=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\tKullanıcının Yapmak İstediği işlem türü= Toplama Hesaplama" + "\tKullanıcı Gönderdiği değer="+ deger.Substring(1));//dosyaya gerekli bilgileri yazdırıyoruz.
                        sw.Close();//dosyayı kapatıyoruz.
                    }
                    else if (deger.Substring(0, 1) == "2")//clientten gelen ilk byte değeri bizim için yapaacağımız işlemi belirttiği için 2 ise eğer faktöriyel işlemi olduğunu anlıyoruz.
                    {                       
                        var islem = deger.Substring(1);//Clientten gelen ilk değeri çıkartıp işleme eşitliyoruz.
                        var sonuc = Faktöriyel(Convert.ToInt64(islem));//string değerimizi int değere çevirip faktöriyel fonksiyonuna gönderiyoruz.
                        client.Send(Encoding.ASCII.GetBytes(sonuc.ToString()), 0, sonuc.ToString().Length, SocketFlags.None);//cliente hesaplanan faktöriyel sonucunu gönderiyoruz.
                        client.Send(Encoding.ASCII.GetBytes("ok"), 0, 2, SocketFlags.None);//Cliente işlemin bitti anlamında ok komutunu gönderiyoruz.
                        FileStream fs = new FileStream(baglantikayit, FileMode.Append, FileAccess.Write);//Burada kayıt yapacağımız Dosyanın adı Dosya modu ve ne için açtığımızı belirtiyoruz.
                        StreamWriter sw = new StreamWriter(fs);//dosyayı açıyoruz.
                        sw.WriteLine("Host Adı=" + hostEntry.HostName + "\tIpv4 Adres=" + hostEntry.AddressList[1] + "\tIpv6 Adres=" + hostEntry.AddressList[0] + "\tSistem Saati=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") +"\tKullanıcının Yapmak İstediği işlem türü= Faktöriyel Hesaplama" +"\tKullanıcı Gönderdiği değer=" + deger.Substring(1));//dosyaya gerekli bilgileri yazdırıyoruz.
                        sw.Close();//dosyayı kapatıyoruz.
                    }
                }
            }
        }
        public Int64 Toplama(Int64 sayi)//Toplama Fonksiyonumuz
        {
            Int64 sonuc = 0;//int veri tipinde sonuc tanımlıyoruz.
            while (sayi > 0)//sayı 0 dan büyük olduğu sürece toplamasını gerektiğini söylüyoruz. 
            {
                sonuc += (sayi % 10);//sayının basamak basamak değerlerini bulup toplamak için 10'a bölümünden kalanı alıyoruz.
                sayi = sayi / 10;//sayıyı 10'a bölerek okuduğumuz basamağı çıkartmış oluyoruz.
            }
            return sonuc;//Sonucumuzu fonsiyon sonucu olarak döndürüyoruz.
        }
        public Int64 Faktöriyel(Int64 sayi)//Faktöriyel Fonksiyonumuz
        {
            Int64 sonuc = 1;//int veri tipinde sonuc tanımlıyoruz. 1 e eşitliyouz.
            for (int i = 1; i <= sayi; i++)//döngü içine sokarak 1 den başlatıp sayı değerine eşit olasıya kadar döndürüyoruz.
            {
                sonuc = i * sonuc;//Sonucumuzu i değerimizle çarpıyoruz.
            }
            return sonuc;//Sonucumuzu fonsiyon sonucu olarak döndürüyoruz.
        }
    }
}
