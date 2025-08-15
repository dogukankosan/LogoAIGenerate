# 🎨 LogoAIGenerate — README (Console Stili)

> Stability AI ile görsel üretimi, dosyadan toplu resim atama ve Logo ERP/JPlatform’a tek tık aktarım.
> Ekstra: Dosya seçme (bulk import) + Malzeme kodu eşleştirme + Başarı/Fail renkli durum izleme.

───────────────────────────────────────────────────────────────────────────────
🚀 ÖZELLİKLER
───────────────────────────────────────────────────────────────────────────────
• 🖼 Stability AI Entegrasyonu
  - Prompta göre görsel üretir, sonuçları otomatik malzeme kartlarına bağlar.

• 📁 Dosyadan Toplu Resim Atama (Yeni)
  - "Klasör Seç" / "Dosya Seç" ile yerel resimleri içeri al.
  - İsimlendirme: <MALZEME_KODU>.<jpg|png|jpeg|webp>
  - Örn: ABC-1001.jpg → malzeme kodu: ABC-1001 olarak eşleştirilir.

• 🔀 Hibrit Atama Modu (Yeni)
  - AI ile otomatik üret + Dosyadan seçilen görselleri bir arada kullan.
  - Eşleşen yerel dosya varsa onu; yoksa Stability AI çıktısını bağlar.

• 🌍 Google Gemini API Çeviri
  - Malzeme açıklamalarını otomatik İngilizce’ye çevirir (opsiyonel).

• 🗂 Logo ERP / JPlatform Entegrasyonu
  - Toplu görsel ve açıklama güncellemesi.
  - Hem Logo ERP hem de JPlatform için entegre aktarım akışı.

• 🔌 Dinamik SQL Bağlantı Ayarları
  - Birden fazla şirket veritabanına hızlı geçiş.

• 📝 Dinamik Log Yönetimi
  - İşlem bazlı loglar; tarih/saat, işlem tipi, malzeme kodu, sonuç.

• 🎛 Tema Desteği
  - Modern, karanlık/açık tema; ThemeConfig.txt ile yönetilebilir.

• ⚡ Toplu İşlem Desteği
  - Çoklu malzeme satırını tek operasyonda işler.

───────────────────────────────────────────────────────────────────────────────
🗂 PROJE YAPISI
───────────────────────────────────────────────────────────────────────────────
LogoAIGenerate/
├─ StabilityAIHelper.cs     # Stability AI ile görsel üretim
├─ GeminiTranslation.cs     # Google Gemini API ile metin çeviri
├─ LogoApiService.cs        # Logo ERP / JPlatform entegrasyonu
├─ FileImportService.cs     # (Yeni) Dosyadan görsel okuma/eşleştirme
├─ MatchingRules.cs         # (Yeni) Malzeme kodu eşleştirme kuralları
├─ StatusStyler.cs          # (Yeni) Başarı/Hata renklendirme (yeşil/kırmızı)
├─ LogManager.cs            # Dinamik log yönetimi
├─ ThemeConfig.txt          # Tema ayarları
└─ MainForm.cs              # Ana uygulama ekranı

───────────────────────────────────────────────────────────────────────────────
🔧 KURULUM
───────────────────────────────────────────────────────────────────────────────
1) Klonla:
   git clone https://github.com/dogukankosan/LogoAIGenerate.git
   cd LogoAIGenerate

2) Visual Studio ile aç → F5

3) İlk Açılış:
   - SQL bağlantısı seç (Sunucu, DB, kullanıcı).
   - ThemeConfig.txt ile tema seç (dark/light).

4) Gerekli Anahtarlar (User Secrets / App.config):
   - STABILITY_API_KEY = <key>
   - GEMINI_API_KEY    = <key>
   - LOGO_API_BASE     = <url>
   - LOGO_COMPANY_CODE = <firma_kodu>

───────────────────────────────────────────────────────────────────────────────
⚙️ KONFİGÜRASYON NOTLARI
───────────────────────────────────────────────────────────────────────────────
• SQL:
  - Performans için yalnızca gerekli kolonları çek.
  - Büyük listelerde sayfalama açık olmalı (MainForm → grid paging).

• Logo ERP/JPlatform:
  - Bağlantı bilgileri ve firma kodu zorunlu.
  - Aktarım öncesi bağlantı testi yapılır.

• AI Parametreleri:
  - StabilityAIHelper: model, çözünürlük, guidance scale, seed.
  - GeminiTranslation: hedef dil(en-US), alan (product).

───────────────────────────────────────────────────────────────────────────────
🧠 EŞLEŞTİRME (DOSYA → MALZEME) — (Yeni)
───────────────────────────────────────────────────────────────────────────────
• Varsayılan Kural:
  DosyaAdı = <MALZEME_KODU>.<uzantı>
  Örn: 100200-AX.png → malzeme kodu: 100200-AX

• Alternatif Kurallar (MatchingRules.cs):
  - Trim: Boşluk/özel karakter temizleme.
  - Normalize: Türkçe karakter normalizasyonu (Ç→C, Ğ→G, İ→I vb.).
  - Prefix/Suffix: “IMG_”, “PIC-”, “-V1” gibi ekleri kırpma.

• Çakışma Yönetimi:
  - Bir malzeme koduna birden çok dosya: son değiştirilen (mtime) öncelikli.
  - Hiç eşleşme yoksa: AI üretimi devreye girer (hibrit mod aktifse).

───────────────────────────────────────────────────────────────────────────────
🏃‍♂️ KULLANIM AKIŞI
───────────────────────────────────────────────────────────────────────────────
1) Uygulamayı başlat → SQL bağlantısını seç → Giriş yap.
2) Malzeme kartlarını listele.
3) (Opsiyonel) “Dosyadan Yükle”:
   - “Klasör Seç” ile toplu görsel oku ve ön eşleştirme yap.
   - Grid’de “Kaynak” kolonu: Local / AI / Mix.
4) (Opsiyonel) “AI ile Üret”:
   - Seçilen malzemeler için Stability AI prompt çalıştır.
5) (Opsiyonel) “Gemini ile Çevir”:
   - Açıklamaları İngilizce’ye çevir ve önizle.
6) “Aktar”:
   - ERP ve/veya JPlatform seç → toplu aktarım.
7) “Log” ekranından tüm işlem detaylarını izle.

───────────────────────────────────────────────────────────────────────────────
🎯 DURUM RENKLERİ (UI) — (Yeni)
───────────────────────────────────────────────────────────────────────────────
🟢 Başarılı: Logo ERP/JPlatform güncellemesi tamam.
🟡 Uyarı: Eşleşme bulundu ancak dosya/AI çıktısı dönüştürülerek yüklendi.
🔴 Hatalı: Aktarım/bağlantı/format sorunları (detay için Log).

───────────────────────────────────────────────────────────────────────────────
🧾 LOG KAPSAMI
───────────────────────────────────────────────────────────────────────────────
• Zaman damgası, Kullanıcı, İşlem Tipi (Import/AI/Translate/Push)
• Malzeme Kodu, Kaynak (Local/AI), Hedef (ERP/JPlatform)
• Sonuç (Success/Warning/Error), Mesaj/İstisna
• Export: .csv / .txt olarak dışa aktarım

───────────────────────────────────────────────────────────────────────────────
🧪 TEST & DOĞRULAMA ÖNERİLERİ
───────────────────────────────────────────────────────────────────────────────
• 10–50 satırlık pilot malzeme seti ile dene.
• Dosya adlandırma kuralını örneklerle doğrula.
• Logo test firmasına yaz → üretim firmasına geçmeden önce logları incele.
• Büyük batch’lerde 200’lük paketleme önerilir (timeout riskini düşürür).

───────────────────────────────────────────────────────────────────────────────
🛠 SORUN GİDERME (KISA)
───────────────────────────────────────────────────────────────────────────────
• “API KEY invalid” → App.config/User Secrets anahtarlarını doğrula.
• “Malzeme bulunamadı” → Kod normalizasyonu ve matching kurallarını kontrol et.
• “Unsupported format” → Yalnızca jpg/png/jpeg/webp desteklenir.
• “Timeout/429” → Rate limit → yeniden dene, batch boyutunu küçült.
• “Aktarım başarısız” → Logo endpoint/oturum/şirket kodunu test et.

───────────────────────────────────────────────────────────────────────────────
📄 LİSANS
───────────────────────────────────────────────────────────────────────────────
MIT License

───────────────────────────────────────────────────────────────────────────────
👤 İLETİŞİM
───────────────────────────────────────────────────────────────────────────────
Geliştirici: @dogukankosan
Hata/Öneri: GitHub Issues

# Etiketler:
# .NET Framework | Windows Forms | Stability AI | Gemini API | Logo ERP | JPlatform
