# 🎨 LogoAIGenerate

![License](https://img.shields.io/github/license/dogukankosan/LogoAIGenerate)
![Stars](https://img.shields.io/github/stars/dogukankosan/LogoAIGenerate)
![Issues](https://img.shields.io/github/issues/dogukankosan/LogoAIGenerate)
![Last Commit](https://img.shields.io/github/last-commit/dogukankosan/LogoAIGenerate)

> Stability AI ile görsel üretimi, dosyadan toplu resim atama ve Logo ERP/JPlatform’a tek tık aktarım.  
> Ekstra: Dosya seçme (bulk import) + Malzeme kodu eşleştirme + Başarı/Fail renkli durum izleme.

───────────────────────────────────────────────────────────────────────────────
🚀 ÖZELLİKLER
───────────────────────────────────────────────────────────────────────────────
- 🖼 **<a href="https://platform.stability.ai" target="_blank">Stability AI Entegrasyonu</a>** → Prompt ile görsel üretir, otomatik malzeme kartına bağlar.  
- 📁 **Dosyadan Toplu Resim Atama** → <a href="https://learn.microsoft.com/dotnet" target="_blank">Windows Forms</a> ile dosya seçme ve malzeme kodu eşleştirme.  
- 🔀 **Hibrit Atama Modu** → Yerel dosya + AI çıktısını birleştirir.  
- 🌍 **<a href="https://ai.google.dev/gemini-api" target="_blank">Google Gemini API Çeviri</a>** → Açıklamaları otomatik İngilizce’ye çevirir.  
- 🗂 **Logo ERP / JPlatform Entegrasyonu** → Toplu aktarım.  
- 🔌 **Dinamik SQL Bağlantı Ayarları** → Çoklu veritabanı desteği.  
- 📝 **Dinamik Log Yönetimi** → Başarılı/uyarı/hata detaylarını kayıt altına alır.  
- 🎛 **Tema Desteği** → Karanlık/Açık mod.  
- ⚡ **Toplu İşlem** → Çoklu satır tek operasyonda.

───────────────────────────────────────────────────────────────────────────────
🗂 PROJE YAPISI
───────────────────────────────────────────────────────────────────────────────
LogoAIGenerate/
├─ StabilityAIHelper.cs     # Stability AI ile görsel üretim
├─ GeminiTranslation.cs     # Google Gemini API ile metin çeviri
├─ LogoApiService.cs        # Logo ERP / JPlatform entegrasyonu
├─ FileImportService.cs     # (Yeni) Dosyadan görsel okuma/eşleştirme
├─ MatchingRules.cs         # (Yeni) Malzeme kodu eşleştirme kuralları
├─ StatusStyler.cs          # (Yeni) Başarı/Hata renklendirme
├─ LogManager.cs            # Dinamik log yönetimi
├─ ThemeConfig.txt          # Tema ayarları
└─ MainForm.cs              # Ana uygulama ekranı
───────────────────────────────────────────────────────────────────────────────
🏃‍♂️ KULLANIM AKIŞI
───────────────────────────────────────────────────────────────────────────────
1. Uygulamayı başlat → SQL bağlantısını seç → Giriş yap.  
2. Malzeme kartlarını listele.  
3. (Opsiyonel) **Dosyadan Yükle** → “Klasör Seç” ile görselleri oku, eşleştir.  
4. (Opsiyonel) **AI ile Üret** → Stability AI prompt çalıştır.  
5. (Opsiyonel) **Gemini ile Çevir** → Açıklamaları İngilizce’ye çevir.  
6. **Aktar** → ERP ve/veya JPlatform seç → Toplu aktarım.  
7. **Log ekranı** → Sonuçları takip et.

───────────────────────────────────────────────────────────────────────────────
🎯 DURUM RENKLERİ
───────────────────────────────────────────────────────────────────────────────
🟢 **Başarılı** → Logo ERP/JPlatform güncellemesi tamam.  
🟡 **Uyarı** → Eşleşme bulundu ama dönüştürülerek yüklendi.  
🔴 **Hata** → Aktarım/format/bağlantı sorunu (Log’da detay).

───────────────────────────────────────────────────────────────────────────────
📄 LİNKLER
───────────────────────────────────────────────────────────────────────────────
- <a href="https://github.com/dogukankosan/LogoAIGenerate" target="_blank">📦 GitHub Repo</a>  
- <a href="https://platform.stability.ai" target="_blank">🖼 Stability AI</a>  
- <a href="https://ai.google.dev/gemini-api" target="_blank">🌍 Google Gemini API</a>  
- <a href="https://www.logo.com.tr" target="_blank">🏢 Logo ERP/JPlatform</a>  
- <a href="https://learn.microsoft.com/dotnet/desktop/winforms/" target="_blank">🖥 Windows Forms</a>  

───────────────────────────────────────────────────────────────────────────────
📄 LİSANS
───────────────────────────────────────────────────────────────────────────────
MIT License
