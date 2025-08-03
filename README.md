<!DOCTYPE html>
<html lang="tr">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>LogoAIGenerate - README</title>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; max-width: 900px; margin: auto; padding: 20px; background: #f5f5f5; }
        h1, h2, h3 { color: #333; }
        pre, code { background: #272822; color: #f8f8f2; padding: 10px; display: block; border-radius: 5px; overflow-x: auto; }
        .badges img { margin-right: 5px; vertical-align: middle; }
        .section { background: #fff; padding: 20px; border-radius: 10px; margin-bottom: 20px; box-shadow: 0 2px 5px rgba(0,0,0,0.1); }
        ul { padding-left: 20px; }
        footer { text-align: center; margin-top: 40px; color: #777; }
    </style>
</head>
<body>

    <div class="section">
        <h1>🎨 LogoAIGenerate</h1>
        <div class="badges">
            <img src="https://img.shields.io/github/license/dogukankosan/LogoAIGenerate" alt="License">
            <img src="https://img.shields.io/github/stars/dogukankosan/LogoAIGenerate" alt="Stars">
            <img src="https://img.shields.io/github/issues/dogukankosan/LogoAIGenerate" alt="Issues">
            <img src="https://img.shields.io/github/last-commit/dogukankosan/LogoAIGenerate" alt="Last Commit">
        </div>
        <p><strong>LogoAIGenerate</strong>, Stability AI ile görsel oluşturma, Logo ERP/JPlatform malzeme kartlarına toplu olarak resim ekleme ve Google Gemini API ile malzeme açıklamalarını İngilizce’ye çevirme özelliklerini bir arada sunan C#/.NET masaüstü uygulamasıdır. SQL bağlantı ayarları, dinamik log yönetimi ve yönetilebilir tema desteği ile esnek bir yapı sağlar.</p>
    </div>

    <div class="section">
        <h2>🚀 Özellikler</h2>
        <ul>
            <li>🖼 <strong>Stability AI Entegrasyonu</strong> → Malzeme kartları için yapay zeka destekli görsel üretimi</li>
            <li>🌍 <strong>Gemini API Çeviri</strong> → Malzeme açıklamalarını otomatik İngilizce’ye çevirme</li>
            <li>🗂 <strong>Logo ERP / JPlatform Entegrasyonu</strong> → Oluşturulan görselleri toplu olarak malzeme kartlarına ekleme</li>
            <li>🔌 <strong>Dinamik SQL Bağlantı Ayarları</strong> → Farklı şirket veritabanlarına kolayca bağlanma</li>
            <li>📝 <strong>Log Yönetimi</strong> → Tüm işlemleri kayıt altına alan dinamik log sistemi</li>
            <li>🎨 <strong>Tema Desteği</strong> → Yönetilebilir, modern ve kişiselleştirilebilir arayüz</li>
            <li>⚡ <strong>Toplu İşlem Desteği</strong> → Birden fazla malzeme kartını tek seferde güncelleme</li>
        </ul>
    </div>

    <div class="section">
        <h2>🗂 Proje Yapısı</h2>
        <pre><code>LogoAIGenerate/
├── StabilityAIHelper.cs     # Stability AI ile görsel üretim
├── GeminiTranslation.cs     # Google Gemini API ile metin çeviri
├── LogoApiService.cs        # Logo ERP / JPlatform entegrasyonu
├── DatabaseConfig.txt       # SQL bağlantı bilgileri
├── ThemeConfig.txt          # Tema ayarları
├── LogManager.cs            # Dinamik log yönetimi
└── MainForm.cs              # Ana uygulama ekranı
</code></pre>
    </div>

    <div class="section">
        <h2>🛠️ Kurulum & Çalıştırma</h2>
        <ol>
            <li><strong>Projeyi Klonla:</strong>
                <pre><code>git clone https://github.com/dogukankosan/LogoAIGenerate.git
cd LogoAIGenerate</code></pre>
            </li>
            <li><strong>Bağlantı Ayarlarını Yap:</strong>
                <ul>
                    <li><code>DatabaseConfig.txt</code> dosyasına SQL bağlantı cümleni yaz.</li>
                    <li>Stability AI ve Gemini API anahtarlarını ilgili ayar dosyalarına ekle.</li>
                </ul>
            </li>
            <li><strong>Projeyi Visual Studio ile Aç ve Çalıştır (<code>F5</code>):</strong>
                <ul>
                    <li>İlk açılışta SQL bağlantısını seç.</li>
                    <li>Tema ayarlarını isteğe göre değiştir.</li>
                </ul>
            </li>
        </ol>
    </div>

    <div class="section">
        <h2>⚡ Kullanım Senaryosu</h2>
        <ol>
            <li>Uygulamayı başlat.</li>
            <li>SQL bağlantısını seç ve giriş yap.</li>
            <li>Malzeme kartlarını listele.</li>
            <li>Görsel oluşturmak istediğin malzemeleri seç → Stability AI ile otomatik üret.</li>
            <li>İstersen açıklamaları İngilizce’ye çevir (Gemini API).</li>
            <li>Tek tıkla Logo ERP/JPlatform’a toplu olarak aktar.</li>
            <li>Tüm işlem detaylarını log ekranından takip et.</li>
        </ol>
    </div>

    <div class="section">
        <h2>📸 Ekran Görüntüleri</h2>
        <p><em>Buraya proje ekran görüntüleri eklenecek</em></p>
    </div>

    <div class="section">
        <h2>🤝 Katkı</h2>
        <p>Katkı sağlamak için projeyi forklayabilir ve pull request gönderebilirsiniz.</p>
    </div>

    <div class="section">
        <h2>📄 Lisans</h2>
        <p>MIT License</p>
    </div>

    <div class="section">
        <h2>📬 İletişim</h2>
        <ul>
            <li>👨‍💻 Geliştirici: <a href="https://github.com/dogukankosan">@dogukankosan</a></li>
            <li>🐞 Hata / Öneri: <a href="https://github.com/dogukankosan/LogoAIGenerate/issues">Issues sekmesi</a></li>
        </ul>
    </div>

    <footer>
        <p>
            <img src="https://img.shields.io/badge/.NET-Framework-blue?logo=dotnet" alt=".NET Framework">
            <img src="https://img.shields.io/badge/Windows%20Forms-UI-lightgrey" alt="Windows Forms">
            <img src="https://img.shields.io/badge/AI-StabilityAI-yellow" alt="Stability AI">
            <img src="https://img.shields.io/badge/Translate-Gemini-orange" alt="Gemini API">
        </p>
    </footer>

</body>
</html>
