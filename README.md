# 🎨 LogoAIGenerate

<img width="1520" height="841" alt="11" src="https://github.com/user-attachments/assets/f08fa7ea-735f-43ca-8859-b752574c58a0" />

[![License](https://img.shields.io/github/license/dogukankosan/LogoAIGenerate)](LICENSE) [![Stars](https://img.shields.io/github/stars/dogukankosan/LogoAIGenerate)](https://github.com/dogukankosan/LogoAIGenerate/stargazers) [![Issues](https://img.shields.io/github/issues/dogukankosan/LogoAIGenerate)](https://github.com/dogukankosan/LogoAIGenerate/issues) [![Last Commit](https://img.shields.io/github/last-commit/dogukankosan/LogoAIGenerate)](https://github.com/dogukankosan/LogoAIGenerate/commits/main) [![.NET Framework](https://img.shields.io/badge/.NET-Framework-blue?logo=dotnet)](https://learn.microsoft.com/dotnet/) [![Windows Forms](https://img.shields.io/badge/Windows%20Forms-UI-lightgrey)](https://learn.microsoft.com/dotnet/desktop/winforms/) [![Stability AI](https://img.shields.io/badge/AI-Stability-yellow)](https://platform.stability.ai) [![Gemini API](https://img.shields.io/badge/Translate-Gemini-orange)](https://ai.google.dev/gemini-api)

> **LogoAIGenerate**, Stability AI ile görsel üretme, Google Gemini API ile açıklamaları İngilizce’ye çevirme, dosyadan toplu görsel atama ve Logo ERP/JPlatform’a tek tıkla aktarım özelliklerini bir arada sunar. ✅ Dosya seçme (bulk import) ✅ Malzeme kodu ile otomatik eşleştirme ✅ Durum bazlı renkli geri bildirim (🟢 / 🟡 / 🔴)

## 🚀 Özellikler
- 🖼 **[Stability AI Entegrasyonu](https://platform.stability.ai)** → Prompt ile yüksek kaliteli ürün görselleri üretir.  
- 📁 **Dosyadan Toplu Resim Atama** → [Windows Forms](https://learn.microsoft.com/dotnet/desktop/winforms/) arayüzü ile kolay dosya seçme, otomatik kod eşleştirme.  
- 🔀 **Hibrit Atama Modu** → AI + Yerel dosya desteği (öncelik yerel dosyada).  
- 🌍 **[Google Gemini API Çeviri](https://ai.google.dev/gemini-api)** → Malzeme açıklamalarını İngilizce’ye çevirir.  
- 🗂 **Logo ERP / JPlatform Entegrasyonu** → Toplu aktarım desteği.  
- 🔌 **Dinamik SQL Bağlantı Ayarları** → Çoklu veritabanı desteği.  
- 📝 **Dinamik Log Yönetimi** → İşlem bazlı log kaydı, hata detayları.  
- 🎛 **Tema Desteği** → Karanlık / Açık tema.  
- ⚡ **Toplu İşlem** → Tek tıklamayla yüzlerce kayıt güncelleme.

## 📂 Proje Yapısı
```yaml
LogoAIGenerate:
  - StabilityAIHelper.cs: "Stability AI ile görsel üretim"
  - GeminiTranslation.cs: "Google Gemini API ile metin çeviri"
  - LogoApiService.cs: "Logo ERP / JPlatform entegrasyonu"
  - FileImportService.cs: "(Yeni) Dosyadan görsel okuma/eşleştirme"
  - MatchingRules.cs: "(Yeni) Malzeme kodu eşleştirme kuralları"
  - StatusStyler.cs: "(Yeni) Başarı/Hata renklendirme"
  - LogManager.cs: "Dinamik log yönetimi"
  - ThemeConfig.txt: "Tema ayarları"
  - MainForm.cs: "Ana uygulama ekranı"
```

## 🏃‍♂️ Kullanım Akışı
1️⃣ Uygulamayı başlat ve SQL bağlantısını seç. 2️⃣ Malzeme kartlarını listele. 3️⃣ (Opsiyonel) **Dosyadan Yükle** → Klasör seç, otomatik eşleştirme yap. 4️⃣ (Opsiyonel) **AI ile Üret** → Stability AI prompt çalıştır. 5️⃣ (Opsiyonel) **Gemini ile Çevir** → Açıklamaları İngilizce’ye çevir. 6️⃣ **Aktar** → ERP ve/veya JPlatform seç, toplu aktarım yap. 7️⃣ **Log Ekranı** → Başarılı / Uyarı / Hata durumlarını takip et.

## 🎯 Durum Renkleri
- 🟢 **Başarılı** → Aktarım tamamlandı.  
- 🟡 **Uyarı** → Eşleşme bulundu ancak dönüştürülerek yüklendi.  
- 🔴 **Hata** → Aktarım/format/bağlantı sorunu (Log ekranında detaylı bilgi).

## 🔗 Faydalı Linkler
📦 [GitHub Repo](https://github.com/dogukankosan/LogoAIGenerate) | 🖼 [Stability AI](https://platform.stability.ai) | 🌍 [Google Gemini API](https://ai.google.dev/gemini-api) | 🏢 [Logo ERP/JPlatform](https://www.logo.com.tr) | 🖥 [Windows Forms](https://learn.microsoft.com/dotnet/desktop/winforms/)

## 📜 Lisans
Bu proje **MIT License** ile lisanslanmıştır. [📄 Lisans Dosyasını Görüntüle](LICENSE)
