# Socket-Programming

## Proje Konusu: Client-Server Projesi (Socket Programming)

Bu proje, Client-Server mimarisinin nasıl kullanıldığını ve soket programlamanın nasıl
yapılacağını öğrenmeyi amaçlamaktadır. Client – Server projesi kapsamında bir ağ
üzerindeki bilgisayarların birbirleriyle iletişim kurarak bilgi alışverişi yapmaları
sağlanacaktır.

Bu sistemde bir tane Server (sunucu) ve bir veya birden fazla Client (istemci) bilgisayar
çalışmaktadır.

```
Server:
```
```
 Sistemde yer alan Client bilgisayarların bağlantı isteklerini kabul eder.
 Bağlantı isteği kabul edilen Client’a hangi işlemin gerçekleştirilmesini istediğini
sorar.
 Server’da yapılabilecek işlemler şunlardır:
```
1. Toplama  Client tarafından gönderilen karakter dizisindeki tüm
    rakamları toplayarak sonucu Client’a gönderir. Toplama işlemi tek rakam
    kalıncaya kadar devam eder.
    Örneğin;
    Client> 234567
    Server> 27
    Server> 9
    Server> ok.


2. Faktoriyel n  Client tarafından gönderilen n sayısının faktoriyelini
    bularak Client’a gönderir.
    Örneğin;
    Client>
    Server>
    Server>ok.
 Server, client’ın gönderdiği host adını, IP adresini, sistem saatini ve kullanıcı
bilgilerini bir text dosyasına kaydeder. Böylelikle kendisiyle bağlantı kuran client
bilgilerini kayıtlı tutar.

```
Client:
```
```
 Server ile bağlantı kurarak yapılmasını istediği işlemle ilgili bilgileri gönderir.
 Server’a istek gönderme ve cevap alma süreleriyle ilgili bilgileri bir text
dosyasına kaydeder.
```
### İstenenler:

1. Proje C# veya JAVA programlama dili ile soket programlama (socket programming)
    uygulaması olarak geliştirilecektir.
2. Program kodu üzerinde anlaşılabilir şekilde **açıklamalar (yorum satırları)** olmalıdır.
3. Sistem çalıştıktan sonra testler gerçekleştirilecektir.
4. Projenin nasıl yapıldığını anlatan **detaylı rapor ve test sonuçları** (.pdf formatında)
    program kodlarıyla birlikte paket halinde teslim edilecektir.
5. Proje ile ilgili hazırlamış olduğunuz kodları anlatan bir video da teslim edilecektir.

### Testler:

###  Client’ların yaptığı her işlem (okuma ve yazma) için cevap alma süreleri hesaplanıp

```
tablolarda gösterilecektir.
```
###  Server’dan ortalama cevap alma süresi grafik olarak gösterilecektir.

### Puanlama:

```
%50 kod, %30 doküman ve testler, %20 video anlatımı...
```

# İçerik

![Socket Programming_Sayfa_1](https://user-images.githubusercontent.com/50529546/124367933-9041be00-dc64-11eb-83c1-8401a1589efe.jpg)
![Socket Programming_Sayfa_2](https://user-images.githubusercontent.com/50529546/124367934-9172eb00-dc64-11eb-9b8d-625009f8aee6.jpg)

# İçerik PDF Dosyası

[Socket Programming.pdf](https://github.com/bhrcfth42/Socket-Programming/files/6759096/Bilgisayar.Aglari.Proje.pdf)
