🎨 LogoAIGenerate

LogoAIGenerate, Stability AI ile görsel oluşturma, Logo ERP/JPlatform malzeme kartlarına toplu olarak resim ekleme ve Google Gemini API ile malzeme açıklamalarını İngilizce’ye çevirme özelliklerini bir arada sunan C#/.NET masaüstü uygulamasıdır. SQL bağlantı ayarları, dinamik log yönetimi ve yönetilebilir tema desteği ile esnek bir yapı sağlar.

🚀 Özellikler
🖼️ Stability AI Entegrasyonu → Malzeme kartları için yapay zeka destekli görsel üretimi

🌍 Gemini API Çeviri → Malzeme açıklamalarını otomatik İngilizce’ye çevirme

🗂️ Logo ERP / JPlatform Entegrasyonu → Oluşturulan görselleri toplu olarak malzeme kartlarına ekleme

🔌 Dinamik SQL Bağlantı Ayarları → Farklı şirket veritabanlarına kolayca bağlanma

📝 Log Yönetimi → Tüm işlemleri kayıt altına alan dinamik log sistemi

🎨 Tema Desteği → Yönetilebilir, modern ve kişiselleştirilebilir arayüz

⚡ Toplu İşlem Desteği → Birden fazla malzeme kartını tek seferde güncelleme

🗂 Proje Yapısı
LogoAIGenerate/
├── StabilityAIHelper.cs     # Stability AI ile görsel üretim
├── GeminiTranslation.cs     # Google Gemini API ile metin çeviri
├── LogoApiService.cs        # Logo ERP / JPlatform entegrasyonu
├── DatabaseConfig.txt       # SQL bağlantı bilgileri
├── ThemeConfig.txt          # Tema ayarları
├── LogManager.cs            # Dinamik log yönetimi
└── MainForm.cs              # Ana uygulama ekranı
🛠️ Kurulum & Çalıştırma
Projeyi Klonla:

bash
git clone https://github.com/dogukankosan/LogoAIGenerate.git
cd LogoAIGenerate
Bağlantı Ayarlarını Yap:

DatabaseConfig.txt dosyasına SQL bağlantı cümleni yaz

Stability AI ve Gemini API anahtarlarını ilgili ayar dosyalarına ekle

Visual Studio ile Aç ve Çalıştır (F5):

SQL bağlantısını seç

Tema ayarlarını isteğe göre değiştir

⚡ Kullanım Senaryosu
Uygulamayı başlat

SQL bağlantısını seç ve giriş yap

Malzeme kartlarını listele

Görsel oluşturmak istediğin malzemeleri seç → Stability AI ile otomatik üret

Açıklamaları Gemini API ile İngilizce’ye çevir

Tek tıkla Logo ERP/JPlatform’a toplu olarak aktar

Tüm işlem detaylarını log ekranından takip et

📸 Ekran Görüntüleri
Buraya proje ekran görüntüleri eklenecek

🤝 Katkı
Katkı sağlamak için projeyi forklayabilir ve pull request gönderebilirsiniz. Hatalar ve öneriler için → Issues sekmesi

📄 Lisans
MIT License

📬 İletişim
👨‍💻 Geliştirici: @dogukankosan

🐞 Hata / Öneri: Issues sekmesi

<p align="center"> <img src="https://img.shields.io/badge/.NET-Framework-blue?logo=dotnet" alt=".NET Framework" /> <img src="https://img.shields.io/badge/Windows%20Forms-UI-lightgrey" alt="Windows Forms" /> <img src="https://img.shields.io/badge/AI-StabilityAI-yellow" alt="Stability AI" /> <img src="https://img.shields.io/badge/Translate-Gemini-orange" alt="Gemini API" /> </p>
