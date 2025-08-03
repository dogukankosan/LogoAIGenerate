🧠 LogoAIGenerate

LogoAIGenerate, Logo J-Platform ve ERP sistemleri için malzeme kartlarına yapay zeka ile otomatik görsel üretimi ve İngilizce çeviri sağlayan, SQL tabanlı, dinamik temalı ve log yönetimli bir masaüstü C#/.NET uygulamasıdır.

🚀 Özellikler
🧠 Stability AI ile ürün görseli üretimi (prompt tabanlı)

🌍 Gemini API ile ürün adlarını İngilizce’ye çevirme

🗃 SQL bağlantı ayarları ve çoklu veritabanı desteği

🖼 Malzeme kartlarına toplu görsel basma

🎨 Yönetilebilir tema ve dinamik log ekranı

🧾 Prompt birleştirme ve negative prompt yönetimi

📦 ERP sistemleriyle tam uyumlu görsel entegrasyonu

🔐 API anahtarı güvenliği ve erişim kısıtlamaları

🗂 Proje Yapısı
LogoAIGenerate/
├── StabilityAI.cs         # Görsel üretim işlemleri
├── GeminiTranslate.cs     # İngilizce çeviri işlemleri
├── SqlManager.cs          # SQL bağlantı ve veri işlemleri
├── PromptHelper.cs        # Prompt birleştirme ve yönetimi
├── ThemeManager.cs        # Tema ayarları ve dinamik log ekranı
├── App.config             # API anahtarları ve bağlantı ayarları
└── ...                    # UI bileşenleri ve diğer yardımcı dosyalar
⚙️ Kurulum & Çalıştırma
Projeyi Klonla:

bash
git clone https://github.com/dogukankosan/LogoAIGenerate.git
cd LogoAIGenerate
Bağlantı Ayarlarını Yap:

App.config dosyasına SQL bağlantı cümleni ve API anahtarlarını gir.

Stability AI ve Gemini API için erişim bilgilerini tanımla.

Visual Studio ile çalıştır (F5):

Malzeme kartlarını listele.

Prompt oluştur → görsel üret → İngilizce çevir → SQL’e kaydet.

Dinamik log ekranından işlem geçmişini takip et.

📌 Kullanım Senaryosu
Uygulamayı başlat.

Malzeme kartlarını SQL’den çek.

Ürün adlarını Gemini API ile İngilizce’ye çevir.

Stability AI ile prompt oluştur ve görsel üret.

Üretilen görselleri ERP sistemine aktar.

Tüm işlemleri log ekranından takip et, temayı özelleştir.

🧩 Entegrasyonlar
Sistem	Amaç	Durum
Stability AI	Görsel üretimi	✅ Aktif
Gemini API	İngilizce çeviri	✅ Aktif
SQL Server	Veri yönetimi	✅ Aktif
Logo ERP	Malzeme kartı entegrasyonu	✅ Aktif
🤝 Katkı
Katkı sağlamak için projeyi forklayabilir ve pull request gönderebilirsiniz.

📄 Lisans
MIT License

📬 İletişim
👨‍💻 Geliştirici: @dogukankosan

🐞 Sorunlar veya öneriler: Issues sekmesi

<p align="center"> <img src="https://img.shields.io/badge/.NET-Framework-blue?logo=dotnet" alt="dotnet" /> <img src="https://img.shields.io/badge/Windows%20Forms-UI-lightgrey" alt="winforms" /> <img src="https://img.shields.io/badge/Stability%20AI-Image%20Generation-purple" alt="stability" /> <img src="https://img.shields.io/badge/Gemini%20API-Translation-green" alt="gemini" /> </p>
