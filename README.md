# Otomasyon
 
Projeyi Çalıştırma
Package Manager Console (PMC) açın ve aşağıdaki komutları çalıştırın:
 cd .\Infrastructure
 dotnet ef migrations add Yeni --startup-project ../Otomasyon
 dotnet ef database update --startup-project ../Otomasyon

 
SQL Server Management Studio'yu açın.
OtomasyonDb -> Kisiler yolunu izleyin ve yeni bir kullanıcı ekleyin. Kullanıcının Unvan alanına Admin (büyük 'A' ile) yazdığınızdan emin olun.

Projeyi çalıştırın ve Admin olarak giriş yapın.

Normal bir kullanıcı ekleyin.

Çıkış yapın ve şimdi normal bir kullanıcı olarak giriş yapabilirsiniz.
