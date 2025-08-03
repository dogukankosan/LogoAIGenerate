﻿using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReplicateImageAI.Classes
{
    internal static class GeminiTranslator
    {
        private static string apiKey = "";
        private static readonly string model = "gemini-1.5-flash";
        private static string endpoint = "";
        internal static async Task<string> TranslateToEnglishAsync(string prompt="",string procudt="",string key="")
        {
            DataTable dt = null;
            if (string.IsNullOrEmpty(key))
            {
                 dt = await SQLiteCrud.GetDataFromSQLiteAsync("SELECT GeminiKey FROM ImageGenerateSetting LIMIT 1");
                if (!DataHelper.IsDataExists(dt))
                {
                    XtraMessageBox.Show("Gemini API Key Bilgilerini Lütfen Giriniz !!", "Hatalı API Key", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                apiKey = EncryptionHelper.Decrypt(dt.Rows[0][0]?.ToString());
            }
            else
                apiKey = key;
            string value = "";
            if (!string.IsNullOrEmpty(procudt))
            {
                value = $"Please translate the following product name to English only, without explanation: \"{procudt}\"";
            }
            else
                value = prompt;
            endpoint = $"https://generativelanguage.googleapis.com/v1/models/{model}:generateContent?key={apiKey}";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var requestBody = new
                {
                    contents = new[]
                    {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = value
                        }
                    }
                }
            }
                };
                string json = JsonSerializer.Serialize(requestBody);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage response = await client.PostAsync(endpoint, content);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    TextLog.TextLogging($"Gemini API Yanıt Kodu: {(int)response.StatusCode} - {response.StatusCode}");
                    TextLog.TextLogging("Gemini API Gönderilen JSON: " + json);
                    TextLog.TextLogging("Gemini API Yanıtı: " + responseBody);
                    JsonDocument doc = JsonDocument.Parse(responseBody);
                    if (doc.RootElement.TryGetProperty("candidates", out var candidates) &&
                        candidates.GetArrayLength() > 0 &&
                        candidates[0].TryGetProperty("content", out var contentJson) &&
                        contentJson.TryGetProperty("parts", out var partsJson) &&
                        partsJson.GetArrayLength() > 0 &&
                        partsJson[0].TryGetProperty("text", out var textElement))
                    {
                        string answer = textElement.GetString()?.Trim();
                        return answer;
                    }
                    else
                    {
                        TextLog.TextLogging("Gemini API: Beklenen JSON yapısı bulunamadı.");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    TextLog.TextLogging("Gemini API Hata: " + ex.Message);
                    TextLog.TextLogging("Gemini API Exception: " + ex.ToString());
                    XtraMessageBox.Show($"Hata: {ex.Message}", "Hatalı Çeviri", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }
    }
}