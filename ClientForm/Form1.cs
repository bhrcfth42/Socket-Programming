using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientForm
{
    public partial class Form1 : Form
    {
        IPHostEntry ipHostInfo;
        IPAddress ipAddress;
        Socket Serverlistener;
        IPEndPoint localEndPoint;        
        DateTime gonderizamani, donuszamani;
        public Form1()
        {
            InitializeComponent();
            ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());//hostname, ıpv4 ve ıpv6 değerlerimizi algılatıyoruz.
            ipAddress = ipHostInfo.AddressList[1];//ipv4 adresimizi seçiyruz.
            localEndPoint = new IPEndPoint(ipAddress, 13000);//ipv4 adresimiz ve port numaramızı belirliyoruz.
            Serverlistener = new Socket(IPAddress.Parse("0.0.0.0").AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);//server ip adresini yazıp,Socket bağlantısı oluşturuyoruz ve türünü belirliyoruz.
            Serverlistener.Connect(localEndPoint);//Servera programı çalıştırdığı ağdaki ıp adresi atamasına göre bağlanıyoruz
            MessageBox.Show("Client Bağlandı.");//Servera bağlandığımıza dair ekrana ilk olarak client bağlandı diyoruz.
            CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;//Combobox değerlerimiz bulunuyor bunun içinde toplama ve faktöriyel yazılı oluyor ve default seçili olarak toplama yapıyoruz.
            backgroundWorker1.RunWorkerAsync();//Arka planda çalışacak işlemimizi (server dinlemesini) çalıştırıyoruz.
        }

        private void button1_Click(object sender, EventArgs e)//Servere değer gönderme butonumuz
        {
            var tur = comboBox1.SelectedIndex + 1;//işlem türünü serverda 1 toplama 2 faktöriyel şeklinde belirlediğimiz için comboboxta index değerleri 0 dan başladığı için 1 arttırarak servera uygun hale getiriyoruz.
            var gonderi = tur.ToString() + textBox1.Text;//servera göndereceğimiz değerileri birleştiriyoruz.
            Serverlistener.Send(Encoding.ASCII.GetBytes(gonderi), 0, gonderi.Length, SocketFlags.None);//gonderi değerini bytes değerine çevirip servera gönderiyouz,0 ise başlangıç indeximiz,gonderi.lengt ise göndereceğimiz mesajın boyutu oluyor.
            listBox1.Items.Add("Client>" + textBox1.Text);//clientin Gönderdiği değeri listboxa ekliyoruz.Server>12            
            gonderizamani = DateTime.Now;//gönderdiğimiz zamanı miliseconds dahil olmak üzere algılatıyoruz.
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)//arka plan işlemimiz server dinlemesi
        {
            while (true)//sürekli arka planda çalışması için sonsuz döngüye sokuyoruz.
            {
                byte[] MessageFromServer = new byte[1024];//serverdan gelecek veri byte türünde olduğu için byte tanımlıyoruz.                               
                int size = Serverlistener.Receive(MessageFromServer);//serverdan gelen mesaj olduğu anda okuyor ve uzunluğunu size değerine eşitliyoruz.
                listBox1.Items.Add("Server>" + Encoding.ASCII.GetString(MessageFromServer, 0, size));//Listboxa Serverdan gelen mesajı ekliyoruz,size olarak belirttiğimiz kısım ise gelen mesajın uzunluğunca okuması için 1024 değer döndermeyeceğini düşünerek bu şekilde yazıyoruz.
                if (Encoding.ASCII.GetString(MessageFromServer).IndexOf("ok") >= 0)//gelen mesajda ok yazısı olup olmadığının kontrolunu yapıyoruz;
                {
                    donuszamani = DateTime.Now;//bize gelen ok cevabının zamanını miliseconds dahil olmak üzere algılatıyoruz.
                    string surekayit = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop).ToString() + "\\sürebilgi.txt";//Kayıt tutacağımız dosyanın masaüstünde sürebilgi.txt şeklinde kayıt edebilmesi için yol belirliyoruz.
                    FileStream fs = new FileStream(surekayit, FileMode.Append, FileAccess.Write);//Burada kayıt yapacağımız Dosyanın adı Dosya modu ve ne için açtığımızı belirtiyoruz.
                    StreamWriter sw = new StreamWriter(fs);//dosyayı açıyoruz.
                    sw.WriteLine("İşlem Türü=" + comboBox1.SelectedItem.ToString() + "\tGönderilen Değer=" + textBox1.Text + "\tGönderilme zamanı=" + gonderizamani.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\tCevabın Verilme Süresi=" + donuszamani.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\tSüre Farkı=" + Convert.ToString(donuszamani - gonderizamani));//Dosyaya gönderilen değer zaman ve dönüş zamanı kayıt edilir.
                    sw.Close();//dosyayı kapatıyoruz.
                    textBox1.Text = "";//değeri yazdığı textbox kısmını boşaltıyoruz.
                }
            }           
        }
    }
}
