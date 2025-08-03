using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ReplicateImageAI.Classes;
using ReplicateImageAI.Models;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.Utils;
using System.IO;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraSplashScreen;

namespace ReplicateImageAI.Forms
{
    public partial class ItemsForm : XtraForm
    {
        public ItemsForm()
        {
            InitializeComponent();
        }
        DataTable dt = null;
        private void ToolTipController1_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            if (e.SelectedControl != gridControl1) return;
            GridHitInfo hitInfo = gridView1.CalcHitInfo(e.ControlMousePosition);
            if (hitInfo.InRowCell && hitInfo.Column.FieldName == "Görsel")
            {
                byte[] imgBytes = gridView1.GetRowCellValue(hitInfo.RowHandle, "Görsel") as byte[];
                if (imgBytes != null && imgBytes.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(imgBytes))
                    {
                        Image img = new Bitmap(Image.FromStream(ms));
                        ToolTipControlInfo info = new ToolTipControlInfo(hitInfo.RowHandle.ToString() + hitInfo.Column.FieldName, "");
                        SuperToolTip superTip = new SuperToolTip();
                        ToolTipItem item = new ToolTipItem
                        {
                            Image = img,
                            Text = ""
                        };
                        superTip.Items.Add(item);
                        info.SuperTip = superTip;
                        e.Info = info;
                    }
                }
            }
        }
        private ToolTipController toolTipController1;
        private async void List()
        {
            DataTable dtGrid = null;
            if (dt.Rows[0]["ERPType"].ToString() == "TIGER ERP")
            {
                dtGrid = await SQLCrud.GetDataTableAsync($@"
                    SELECT ITM.LOGICALREF 'ID',ITM.CODE 'Malzeme Kodu',ITM.NAME 'Malzeme Açıklaması',DOC.LDATA 'Görsel' FROM LG_001_ITEMS ITM WITH (NOLOCK)
                    LEFT JOIN LG_001_FIRMDOC DOC ON DOC.INFOREF = ITM.LOGICALREF AND DOC.INFOTYP = 20 AND DOC.DOCTYP = 0 AND DOC.DOCNR = 11
                    WHERE ITM.ACTIVE = 0  AND ITM.NAME <> '' ORDER BY 3");
            }
            else
            {
                dtGrid = await SQLCrud.GetDataTableAsync($@"SELECT ITM.LOGICALREF 'ID',ITM.CODE 'Malzeme Kodu',ITM.DESCRIPTION 'Malzeme Açıklaması',DOCS.LDATA 'Görsel' FROM U_{dt.Rows[0]["CompanyNo"].ToString()}_ITEMS ITM WITH (NOLOCK) 
LEFT JOIN U_{dt.Rows[0]["CompanyNo"].ToString()}_COMPANYDOCS DOCS WITH(NOLOCK) ON DOCS.INFOREF = ITM.LOGICALREF AND DOCS.INFOTYPE = 20 AND DOCS.DOCTYPE = 0 AND DOCS.DOCNR = 1
WHERE ITM.BOSTATUS = 0 AND CODE <> 'ÿ' AND ITM.DESCRIPTION <> '' ORDER BY 2");
            }
            gridControl1.DataSource = dtGrid;
            RepositoryItemPictureEdit pictureEdit = new RepositoryItemPictureEdit
            {
                SizeMode = PictureSizeMode.Zoom,
                NullText = ""
            };
            gridView1.Columns["Görsel"].ColumnEdit = pictureEdit;
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
        }
        private async void ItemsForm_Load(object sender, EventArgs e)
        {
            toolTipController1 = new ToolTipController();
            toolTipController1.GetActiveObjectInfo += ToolTipController1_GetActiveObjectInfo;
            gridControl1.ToolTipController = toolTipController1;
            GridViewDesigner.CustomizeGrid(gridView1);
            dt = await SQLiteCrud.GetDataFromSQLiteAsync("SELECT * FROM SQLConnectionString LIMIT 1");
            if (!DataHelper.IsDataExists(dt))
            {
                XtraMessageBox.Show("SQL Bilgilerini Lütfen Giriniz !!", "Hatalı SQL Bağlantı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            List();
        }
        private async void btn_LogoSave_Click(object sender, EventArgs e)
        {
            if (gridView1.GetSelectedRows().Length == 0)
            {
                XtraMessageBox.Show("Lütfen en az bir malzeme seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.Enabled = false;
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true);
            try
            {
                SplashScreenManager.Default.SendCommand(WaitForm1.SplashScreenCommand.SetCaption, "SQL ayarları kontrol ediliyor...");
                DataTable SQLSettings = await SQLiteCrud.GetDataFromSQLiteAsync("SELECT * FROM SQLConnectionString LIMIT 1");
                if (!DataHelper.IsDataExists(SQLSettings))
                {
                    XtraMessageBox.Show("SQL Bilgilerini Lütfen Giriniz !!", "Hatalı SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                SplashScreenManager.Default.SendCommand(WaitForm1.SplashScreenCommand.SetCaption, "API ayarları kontrol ediliyor...");
                DataTable dtAISettings = await SQLiteCrud.GetDataFromSQLiteAsync("SELECT ImagePrompt FROM ImageGenerateSetting LIMIT 1");
                if (!DataHelper.IsDataExists(dtAISettings))
                {
                    XtraMessageBox.Show("Resim API Key Bilgilerini Lütfen Giriniz !!", "Hatalı API Key", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                SplashScreenManager.Default.SendCommand(WaitForm1.SplashScreenCommand.SetCaption, "Seçili malzemeler hazırlanıyor...");
                List<(string MalzemeAdi, int LogicalRef)> selectedItems = new List<(string, int)>();
                foreach (int rowHandle in gridView1.GetSelectedRows())
                {
                    if (!gridView1.IsDataRow(rowHandle)) continue;
                    string malzeme = gridView1.GetRowCellValue(rowHandle, "Malzeme Açıklaması")?.ToString();
                    object refObj = gridView1.GetRowCellValue(rowHandle, "ID");
                    if (!string.IsNullOrWhiteSpace(malzeme) && int.TryParse(refObj?.ToString(), out int logicalRef))
                        selectedItems.Add((malzeme, logicalRef));
                }
                int successCount = 0;
                int errorCount = 0;
                foreach (var item in selectedItems)
                {
                    SplashScreenManager.Default.SendCommand(WaitForm1.SplashScreenCommand.SetCaption, $"Malzeme işleniyor: {item.MalzemeAdi}...");
                    string translatedPrompt = await GeminiTranslator.TranslateToEnglishAsync("", item.MalzemeAdi);
                    if (string.IsNullOrEmpty(translatedPrompt))
                    {
                        TextLog.TextLogging($"[Malzeme: {item.MalzemeAdi}] Malzeme Açıklaması çevirisi yapılamadı.");
                        errorCount++;
                        continue;
                    }
                    string imageStyle = dtAISettings.Rows[0]["ImagePrompt"]?.ToString().Trim();
                    if (!imageStyle.ToLower().Contains("realistic"))
                        imageStyle = $"realistic, {imageStyle}";
                    string finalPrompt = $"{imageStyle}. This image should clearly contain: {translatedPrompt.Trim()}.";
                    var input = new ImageGenerationInput
                    {
                        Prompt = finalPrompt,
                        Width = 1024,
                        Height = 1024,
                        GuidanceScale = 6.5f,
                        NumInferenceSteps = 35,
                        Samples = 1
                    };
                    var resultDict = await ImageCreateAI.GenerateImagesAsync(new List<ImageGenerationInput> { input });
                    if (!resultDict.TryGetValue(input.Prompt, out byte[] imageData) || imageData == null)
                    {
                        TextLog.TextLogging($"[Malzeme: {item.MalzemeAdi}] Görsel oluşturulamadı.");
                        errorCount++;
                        continue;
                    }
                    bool processResult = false;
                    if (SQLSettings.Rows[0]["ERPType"]?.ToString() == "TIGER ERP")
                        processResult = await SaveToTigerERPAsync(SQLSettings, item.LogicalRef, item.MalzemeAdi, imageData);
                    else
                        processResult = await SaveToJPlatformAsync(SQLSettings, item.LogicalRef, item.MalzemeAdi, imageData);
                    if (processResult)
                        successCount++;
                    else
                        errorCount++;
                }
                if (successCount > 0 && errorCount == 0)
                    XtraMessageBox.Show("Tüm seçili malzemeler başarıyla işlendi.", "Tamamlandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (successCount > 0 && errorCount > 0)
                    XtraMessageBox.Show($"{successCount} malzeme başarıyla işlendi, {errorCount} malzemede hata oluştu. Detaylar log dosyasında.", "Kısmi Başarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    XtraMessageBox.Show("Hiçbir malzeme işlenemedi. Detaylar log dosyasında.", "Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
                List();
            }
            catch (Exception ex)
            {
                TextLog.TextLogging($"[btn_LogoSave_Click] {ex}");
                XtraMessageBox.Show(ex.Message, "İşlem Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (SplashScreenManager.Default != null && SplashScreenManager.Default.IsSplashFormVisible)
                    SplashScreenManager.CloseForm();
                this.Enabled = true;
            }
        }
        private async Task<bool> SaveToTigerERPAsync(DataTable SQLSettings, int logicalRef, string malzemeAdi, byte[] imageData)
        {
            try
            {
                Dictionary<string, object> checkParams = new Dictionary<string, object> { { "@InfoRef", logicalRef } };
                object ldataValue = await SQLCrud.ExecuteScalarAsync(
                    $"SELECT LDATA FROM LG_{SQLSettings.Rows[0]["CompanyNo"]}_FIRMDOC WITH (NOLOCK) WHERE INFOREF = @InfoRef AND INFOTYP = 20 AND DOCTYP = 0 AND DOCNR = 11",
                    checkParams
                );
                bool ldataVar = ldataValue != null && ldataValue != DBNull.Value;
                string query = ldataVar
                    ? $@"UPDATE LG_{SQLSettings.Rows[0]["CompanyNo"]}_FIRMDOC SET LDATA = @ImageData WHERE INFOREF = @InfoRef AND INFOTYP = 20 AND DOCTYP = 0 AND DOCNR = 11"
                    : $@"INSERT INTO LG_{SQLSettings.Rows[0]["CompanyNo"]}_FIRMDOC (INFOTYP, INFOREF, DOCTYP, DOCNR, LDATA) VALUES (20, @InfoRef, 0, 11, @ImageData)";
                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@InfoRef", logicalRef },
            { "@ImageData", imageData }
        };
                bool success = await SQLCrud.ExecuteCrudAsync(query, parameters);
                if (!success)
                {
                    TextLog.TextLogging($"[Malzeme: {malzemeAdi}] FIRMDOC işlem başarısız.");
                    return false;
                }
                string updateImageIncQuery = $"UPDATE LG_{SQLSettings.Rows[0]["CompanyNo"]}_ITEMS SET IMAGEINC = 1 WHERE LOGICALREF = @LogicalRef";
                bool imageIncSuccess = await SQLCrud.ExecuteCrudAsync(updateImageIncQuery, new Dictionary<string, object> { { "@LogicalRef", logicalRef } });
                if (!imageIncSuccess)
                {
                    TextLog.TextLogging($"[Malzeme: {malzemeAdi}] IMAGEINC güncelleme başarısız.");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                TextLog.TextLogging($"[SaveToTigerERPAsync] {malzemeAdi} hata: {ex}");
                return false;
            }
        }
        private async Task<bool> SaveToJPlatformAsync(DataTable SQLSettings, int logicalRef, string malzemeAdi, byte[] imageData)
        {
            try
            {
                Dictionary<string, object> checkParams = new Dictionary<string, object> { { "@InfoRef", logicalRef } };
                object recordCountObj = await SQLCrud.ExecuteScalarAsync(
                    $@"SELECT COUNT(*) FROM U_{SQLSettings.Rows[0]["CompanyNo"]}_COMPANYDOCS WITH (NOLOCK) 
               WHERE INFOTYPE = 20 AND INFOREF = @InfoRef AND DOCTYPE = 0 AND DOCNR = 1",
                    checkParams
                );
                int recordCount = recordCountObj != null && recordCountObj != DBNull.Value ? Convert.ToInt32(recordCountObj) : 0;
                bool recordExists = recordCount > 0;
                if (recordExists)
                {
                    string updateQuery = $@"UPDATE U_{SQLSettings.Rows[0]["CompanyNo"]}_COMPANYDOCS 
                                    SET LDATA = @ImageData
                                    WHERE INFOTYPE = 20 AND INFOREF = @InfoRef AND DOCTYPE = 0 AND DOCNR = 1";
                    return await SQLCrud.ExecuteCrudAsync(updateQuery, new Dictionary<string, object>
            {
                { "@InfoRef", logicalRef },
                { "@ImageData", imageData }
            });
                }
                else
                {
                    object newLogicalRefObj = await SQLCrud.ExecuteScalarAsync(
                        $"SELECT NEXT VALUE FOR U_{SQLSettings.Rows[0]["CompanyNo"]}_COMPANYDOCSSEQ", null
                    );
                    if (newLogicalRefObj == null || newLogicalRefObj == DBNull.Value)
                    {
                        TextLog.TextLogging($"[Malzeme: {malzemeAdi}] Yeni LOGICALREF alınamadı.");
                        return false;
                    }
                    int newLogicalRef = Convert.ToInt32(newLogicalRefObj);
                    string insertQuery = $@"
                INSERT INTO U_{SQLSettings.Rows[0]["CompanyNo"]}_COMPANYDOCS 
                (LOGICALREF, INFOTYPE, INFOREF, DOCTYPE, DOCNR, LDATA, DESCRIPTION, ISMAIN, 
                 TE_RECSTATUS, TE_LABELS, TE_SUBCOMPANY, TE_WPIID, TE_WFIID, TE_RIGHTS)
                VALUES
                (@NewLogicalRef, 20, @InfoRef, 0, 1, @ImageData, '', 0, -1, NULL, 0, 0, '', 0)";
                    return await SQLCrud.ExecuteCrudAsync(insertQuery, new Dictionary<string, object>
            {
                { "@NewLogicalRef", newLogicalRef },
                { "@InfoRef", logicalRef },
                { "@ImageData", imageData }
            });
                }
            }
            catch (Exception ex)
            {
                TextLog.TextLogging($"[SaveToJPlatformAsync] {malzemeAdi} hata: {ex}");
                return false;
            }
        }
        private void btn_List_Click(object sender, EventArgs e)
        {
            List();
        }
    }
}