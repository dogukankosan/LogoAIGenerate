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
using System.Threading;
using ClosedXML.Excel;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace ReplicateImageAI.Forms
{
    public partial class ItemsForm : XtraForm
    {
        public ItemsForm()
        {
            InitializeComponent();
        }
        private DataTable dtSettings;
        private DataTable dtGrid;
        private ToolTipController toolTipController1;
        private CancellationTokenSource _cts;
        DataTable dt = null;
        private async Task InitializeAsync()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            dtSettings = await SQLiteCrud.GetDataFromSQLiteAsync("SELECT * FROM SQLConnectionString LIMIT 1");
            if (!DataHelper.IsDataExists(dtSettings))
            {
                XtraMessageBox.Show("SQL bilgisi eksik. Lütfen ayarları tamamlayınız.", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            SetupGridEvents();
            await LoadGridAsync(_cts.Token);
            ConfigureGrid();
        }
        private void SetupGridEvents()
        {
            toolTipController1 = new ToolTipController();
            toolTipController1.GetActiveObjectInfo += ToolTipController1_GetActiveObjectInfo;
            gridControl1.ToolTipController = toolTipController1;
            GridViewDesigner.CustomizeGrid(gridView1);
            gridView1.RowStyle += gridView1_RowStyle;
        }
        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;
            string durum = Convert.ToString(gridView1.GetRowCellValue(e.RowHandle, "Durum"))?.Trim();

            if (string.IsNullOrEmpty(durum)) return;

            if (durum.Equals("Başarılı", StringComparison.OrdinalIgnoreCase) ||
                durum.Equals("Güncellendi", StringComparison.OrdinalIgnoreCase) ||
                durum.StartsWith("Başarılı", StringComparison.OrdinalIgnoreCase) ||
                durum.StartsWith("Güncellendi", StringComparison.OrdinalIgnoreCase))
            {
                e.Appearance.ForeColor = Color.Green;
            }
            else
                e.Appearance.ForeColor = Color.Red;
        }
        private void ToolTipController1_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            if (e.SelectedControl != gridControl1) return;
            GridHitInfo hitInfo = gridView1.CalcHitInfo(e.ControlMousePosition);
            if (hitInfo.InRowCell && hitInfo.Column.FieldName == "ERP Görsel")
            {
                byte[] imgBytes = gridView1.GetRowCellValue(hitInfo.RowHandle, "ERP Görsel") as byte[];
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
        private async void List()
        {
            if (dt == null || dt.Rows.Count == 0) return;
            string erpType = dt.Rows[0]["ERPType"].ToString();
            string companyNo = dt.Rows[0]["CompanyNo"].ToString();
            if (erpType == "TIGER ERP")
            {
                dtGrid = await SQLCrud.GetDataTableAsync($@"
            SELECT ITM.LOGICALREF 'ID', ITM.CODE 'Malzeme Kodu', ITM.NAME 'Malzeme Açıklaması', DOC.LDATA 'ERP Görsel'
            FROM LG_001_ITEMS ITM WITH (NOLOCK)
            LEFT JOIN LG_001_FIRMDOC DOC ON DOC.INFOREF = ITM.LOGICALREF AND DOC.INFOTYP = 20 AND DOC.DOCTYP = 0 AND DOC.DOCNR = 11
            WHERE ITM.ACTIVE = 0 AND ITM.NAME <> ''
            ORDER BY 3");
            }
            else
            {
                dtGrid = await SQLCrud.GetDataTableAsync($@"
            SELECT ITM.LOGICALREF 'ID', ITM.CODE 'Malzeme Kodu', ITM.DESCRIPTION 'Malzeme Açıklaması', DOCS.LDATA 'ERP Görsel'
            FROM U_{companyNo}_ITEMS ITM WITH (NOLOCK)
            LEFT JOIN U_{companyNo}_COMPANYDOCS DOCS WITH (NOLOCK) ON DOCS.INFOREF = ITM.LOGICALREF AND DOCS.INFOTYPE = 20 AND DOCS.DOCTYPE = 0 AND DOCS.DOCNR = 1
            WHERE ITM.BOSTATUS = 0 AND CODE <> 'ÿ' AND ITM.DESCRIPTION <> ''
            ORDER BY 2");
            }
            if (!dtGrid.Columns.Contains("Durum"))
                dtGrid.Columns.Add("Durum", typeof(string));
            gridControl1.DataSource = dtGrid;
            GridColumn durumCol = gridView1.Columns["Durum"];
            if (durumCol != null)
                durumCol.Width = 150;
            RepositoryItemPictureEdit pictureEdit = new RepositoryItemPictureEdit
            {
                SizeMode = PictureSizeMode.Zoom,
                NullText = ""
            };
            gridView1.Columns["ERP Görsel"].ColumnEdit = pictureEdit;
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            UpdateMaterialInfo(); 
            await InitializeAsync();
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
           DataTable keys = await SQLiteCrud.GetDataFromSQLiteAsync("SELECT * FROM ImageGenerateSetting LIMIT 1");
            if (!DataHelper.IsDataExists(keys))
            {
                XtraMessageBox.Show("API Key Bilgilerini Lütfen Giriniz !!", "Hatalı Key Bağlantı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            List();
            UpdateMaterialInfo();
            ConfigureGrid();
        }
        private async void btn_LogoSave_Click(object sender, EventArgs e)
        {
            if (gridView1.GetSelectedRows().Length == 0)
            {
                XtraMessageBox.Show("Lütfen en az bir malzeme seçin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.Enabled = false;
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true);
            listBoxControl1.Items.Clear();
            try
            {
                SplashScreenManager.Default.SendCommand(WaitForm1.SplashScreenCommand.SetCaption, "SQL ayarları kontrol ediliyor...");
                DataTable SQLSettings = await SQLiteCrud.GetDataFromSQLiteAsync("SELECT * FROM SQLConnectionString LIMIT 1");
                if (!DataHelper.IsDataExists(SQLSettings))
                {
                    XtraMessageBox.Show("SQL Bilgilerini Lütfen Giriniz !!", "Hatalı SQL",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                SplashScreenManager.Default.SendCommand(WaitForm1.SplashScreenCommand.SetCaption, "API ayarları kontrol ediliyor...");
                DataTable dtAISettings = await SQLiteCrud.GetDataFromSQLiteAsync("SELECT ImagePrompt FROM ImageGenerateSetting LIMIT 1");
                if (!DataHelper.IsDataExists(dtAISettings))
                {
                    XtraMessageBox.Show("Resim API Key Bilgilerini Lütfen Giriniz !!", "Hatalı API Key",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var selected = new List<(string Kod, string Ad, int Ref, int RowHandle)>();
                foreach (int rowHandle in gridView1.GetSelectedRows())
                {
                    if (!gridView1.IsDataRow(rowHandle)) continue;
                    string kod = gridView1.GetRowCellValue(rowHandle, "Malzeme Kodu")?.ToString();
                    string ad = gridView1.GetRowCellValue(rowHandle, "Malzeme Açıklaması")?.ToString();
                    object refObj = gridView1.GetRowCellValue(rowHandle, "ID");
                    if (!string.IsNullOrWhiteSpace(kod) &&
                        !string.IsNullOrWhiteSpace(ad) &&
                        int.TryParse(refObj?.ToString(), out int logicalRef))
                    {
                        selected.Add((kod, ad, logicalRef, rowHandle));
                    }
                }
                int successCount = 0, errorCount = 0;
                Dictionary<string, string> statusMap = new Dictionary<string, string>();
                HashSet<string> codes = new HashSet<string>(selected.Select(s => s.Kod));
                foreach (var item in selected)
                {
                    SplashScreenManager.Default.SendCommand(WaitForm1.SplashScreenCommand.SetCaption, $"Malzeme işleniyor: {item.Ad}...");
                    string translatedPrompt = await GeminiTranslator.TranslateToEnglishAsync("", item.Ad);
                    if (string.IsNullOrWhiteSpace(translatedPrompt))
                    {
                        string msg = "Çeviri Hatası";
                        listBoxControl1.Items.Add($"{item.Kod} - {item.Ad} - {msg}");
                        statusMap[item.Kod] = msg;
                        gridView1.SetRowCellValue(item.RowHandle, "Durum", msg);
                        errorCount++;
                        continue;
                    }
                    string imageStyle = dtAISettings.Rows[0]["ImagePrompt"]?.ToString().Trim();
                    if (string.IsNullOrEmpty(imageStyle)) imageStyle = "realistic";
                    else if (!imageStyle.ToLower().Contains("realistic")) imageStyle = $"realistic, {imageStyle}";
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
                        string msg = "Görsel oluşturulamadı";
                        TextLog.TextLogging($"[Malzeme: {item.Ad}] {msg}.");
                        listBoxControl1.Items.Add($"{item.Kod} - {item.Ad} - {msg}");
                        statusMap[item.Kod] = msg;
                        gridView1.SetRowCellValue(item.RowHandle, "Durum", msg);
                        errorCount++;
                        continue;
                    }
                    bool ok = (SQLSettings.Rows[0]["ERPType"]?.ToString() == "TIGER ERP")
                        ? await SaveToTigerERPAsync(SQLSettings, item.Ref, item.Ad, imageData)
                        : await SaveToJPlatformAsync(SQLSettings, item.Ref, item.Ad, imageData);

                    if (ok)
                    {
                        successCount++;
                        gridView1.SetRowCellValue(item.RowHandle, "ERP Görsel", imageData);
                        string msg = "Başarılı";
                        statusMap[item.Kod] = msg;
                        gridView1.SetRowCellValue(item.RowHandle, "Durum", msg);
                    }
                    else
                    {
                        string msg = "SQL Hatası";
                        TextLog.TextLogging($"[Malzeme: {item.Ad}] Görsel veritabanına kaydedilemedi.");
                        listBoxControl1.Items.Add($"{item.Kod} - {item.Ad} - {msg}");
                        statusMap[item.Kod] = msg;
                        gridView1.SetRowCellValue(item.RowHandle, "Durum", msg);
                        errorCount++;
                    }
                }
                if (successCount > 0 && errorCount == 0)
                    XtraMessageBox.Show("Tüm seçili malzemeler başarıyla işlendi.", "Tamamlandı",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (successCount > 0 && errorCount > 0)
                    XtraMessageBox.Show($"{successCount} başarılı, {errorCount} hata. Detaylar listede ve log dosyasında.", "Kısmi Başarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    XtraMessageBox.Show("Hiçbir malzeme işlenemedi. Detaylar listede ve log dosyasında.", "Başarısız",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                await ReloadGridWithStatus(statusMap);
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
        private async Task ReloadGridWithStatus(Dictionary<string, string> statusMap)
        {
            await LoadGridAsync(_cts.Token);
            foreach (DataRow r in dtGrid.Rows)
            {
                string code = r.Field<string>("Malzeme Kodu");
                if (statusMap.TryGetValue(code, out string status))
                    r["Durum"] = status; 
            }
            gridControl1.RefreshDataSource();
            UpdateMaterialInfo();
        }
        private void ConfigureGrid()
        {
            gridView1.BeginUpdate();
            try
            {
                RepositoryItemPictureEdit pictureEditor = new RepositoryItemPictureEdit
                {
                    SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom,
                    NullText = ""
                };
                GridColumn imgCol = gridView1.Columns["ERP Görsel"];
                if (imgCol != null)
                {
                    imgCol.ColumnEdit = pictureEditor;
                    imgCol.OptionsColumn.AllowEdit = false;
                    imgCol.Width = 120;
                    imgCol.MinWidth = 120;
                    imgCol.MaxWidth = 120;
                }
                GridColumn idCol = gridView1.Columns["ID"];
                if (idCol != null)
                {
                    idCol.Visible = false;
                    idCol.OptionsColumn.ShowInCustomizationForm = false;
                }
                GridColumn durumCol = gridView1.Columns["Durum"];
                if (durumCol != null)
                    durumCol.Width = 150;
                gridView1.OptionsBehavior.Editable = false;
                gridView1.OptionsView.EnableAppearanceEvenRow = true;
                gridView1.OptionsView.EnableAppearanceOddRow = true;
                gridView1.FocusRectStyle = DrawFocusRectStyle.RowFullFocus;
                gridView1.OptionsView.ShowGroupPanel = false;
                gridView1.OptionsCustomization.AllowGroup = false;
                gridView1.OptionsCustomization.AllowColumnMoving = false;
                gridView1.OptionsCustomization.AllowQuickHideColumns = false;
                gridView1.OptionsMenu.EnableColumnMenu = false;
                gridView1.OptionsMenu.EnableGroupPanelMenu = false;
                gridView1.OptionsMenu.EnableFooterMenu = false;
                gridView1.OptionsSelection.MultiSelect = true;
                gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            }
            finally
            {
                gridView1.EndUpdate();
            }
        }
        private async Task LoadGridAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            var table = await FetchGridDataAsync();
            dtGrid = table;
            if (!dtGrid.Columns.Contains("Durum"))
                dtGrid.Columns.Add("Durum", typeof(string));
            gridControl1.DataSource = dtGrid;
            UpdateMaterialInfo();
        }
        private async Task<DataTable> FetchGridDataAsync()
        {
            string companyNo = dtSettings.Rows[0]["CompanyNo"].ToString();
            string erpType = dtSettings.Rows[0]["ERPType"].ToString();
            if (erpType == "TIGER ERP")
            {
                return await SQLCrud.GetDataTableAsync($@"
                    SELECT ITM.LOGICALREF AS ID, ITM.CODE AS [Malzeme Kodu], ITM.NAME AS [Malzeme Açıklaması], DOC.LDATA AS [ERP Görsel]
                    FROM LG_{companyNo}_ITEMS ITM WITH (NOLOCK)
                    LEFT JOIN LG_{companyNo}_FIRMDOC DOC
                        ON DOC.INFOREF = ITM.LOGICALREF AND DOC.INFOTYP = 20 AND DOC.DOCTYP = 0 AND DOC.DOCNR = 11
                    WHERE ITM.ACTIVE = 0 AND ITM.NAME <> '' 
                    ORDER BY ITM.CODE");
            }
            return await SQLCrud.GetDataTableAsync($@"
                SELECT ITM.LOGICALREF AS ID, ITM.CODE AS [Malzeme Kodu], ITM.DESCRIPTION AS [Malzeme Açıklaması], DOCS.LDATA AS [ERP Görsel]
                FROM U_{companyNo}_ITEMS ITM WITH (NOLOCK)
                LEFT JOIN U_{companyNo}_COMPANYDOCS DOCS WITH (NOLOCK)
                    ON DOCS.INFOREF = ITM.LOGICALREF AND DOCS.INFOTYPE = 20 AND DOCS.DOCTYPE = 0 AND DOCS.DOCNR = 1
                WHERE ITM.BOSTATUS = 0 AND ITM.DESCRIPTION <> ''
                ORDER BY ITM.CODE");
        }
        private void UpdateMaterialInfo()
        {
            if (dtGrid == null) return;
            int total = dtGrid.Rows.Count;
            int withPic = dtGrid.AsEnumerable().Count(r => r["ERP Görsel"] is byte[] b && b?.Length > 0);
            lbl_ProductCount.Text = total.ToString();
            lbl_picture.Text = withPic.ToString();
            lbl_unpicture.Text = (total - withPic).ToString();
        }
        private void excelAlToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog dlg = new SaveFileDialog
                {
                    Filter = "Excel Dosyası (*.xlsx)|*.xlsx",
                    Title = "Excel'e Aktar",
                    FileName = "MalzemeListesi.xlsx"
                })
                {
                    if (dlg.ShowDialog() != DialogResult.OK)
                        return;
                    DataTable export = dtGrid.Clone();
                    for (int i = 0; i < gridView1.RowCount; i++)
                    {
                        int rowHandle = gridView1.GetVisibleRowHandle(i);
                        if (rowHandle >= 0)
                        {
                            DataRow row = gridView1.GetDataRow(rowHandle);
                            if (row != null)
                                export.ImportRow(row);
                        }
                    }
                    if (export.Rows.Count == 0)
                    {
                        XtraMessageBox.Show("Filtreye uygun veri bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    export.Columns.Add("Görsel Durumu", typeof(string));
                    foreach (DataRow r in export.Rows)
                    {
                        byte[] img = r["ERP Görsel"] as byte[];
                        r["Görsel Durumu"] = (img != null && img.Length > 0) ? "Var" : "Yok";
                    }
                    if (export.Columns.Contains("ERP Görsel"))
                        export.Columns.Remove("ERP Görsel");
                    if (export.Columns.Contains("Durum"))
                        export.Columns.Remove("Durum");
                    using (XLWorkbook workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Malzeme Listesi");
                        worksheet.Cell(1, 1).InsertTable(export);
                        workbook.SaveAs(dlg.FileName);
                    }
                    XtraMessageBox.Show("Excel dosyası başarıyla oluşturuldu.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                TextLog.TextLogging("Excel aktarım hatası: " + ex);
                XtraMessageBox.Show("Excel aktarım hatası:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ramProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] sel = gridView1.GetSelectedRows();
            if (sel.Length == 0)
            {
                XtraMessageBox.Show("Lütfen bir malzeme seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DataRow row = gridView1.GetDataRow(sel[0]);
            string code = row?["Malzeme Kodu"] as string;
            if (!string.IsNullOrEmpty(code))
            {
                Clipboard.SetText(code);
                XtraMessageBox.Show($"Malzeme kodu kopyalandı: {code}", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                XtraMessageBox.Show("Malzeme kodu boş.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void imageExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] selectedRows = gridView1.GetSelectedRows();
            if (selectedRows.Length == 0)
            {
                XtraMessageBox.Show("Lütfen en az bir satır seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog { Description = "Resimlerin kaydedileceği klasörü seçin" })
            {
                if (folderDialog.ShowDialog() != DialogResult.OK) return;
                string savePath = folderDialog.SelectedPath;
                int success = 0, fail = 0;
                foreach (int rowHandle in selectedRows)
                {
                    try
                    {
                        DataRow row = gridView1.GetDataRow(rowHandle);
                        if (row == null) continue;
                        string code = row["Malzeme Kodu"].ToString();
                        byte[] imageBytes = row["ERP Görsel"] as byte[];
                        if (imageBytes == null || imageBytes.Length == 0) continue;
                        string fileName = Path.Combine(savePath, $"{code}.jpg");
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        using (Image img = Image.FromStream(ms))
                        {
                            img.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                            success++;
                        }
                    }
                    catch (Exception ex)
                    {
                        TextLog.TextLogging($"[Toplu Dışa Aktarım Hatası] {ex}");
                        fail++;
                    }
                }
                XtraMessageBox.Show($"{success} görsel dışarı aktarıldı, {fail} hata oluştu.", "Sonuç", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void copyErrrorProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBoxControl1.SelectedItem == null)
            {
                XtraMessageBox.Show("Lütfen listeden bir ürün seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string selectedText = listBoxControl1.SelectedItem.ToString();
            string code = selectedText.Split('-')[0].Trim();
            if (!string.IsNullOrEmpty(code))
            {
                Clipboard.SetText(code);
                XtraMessageBox.Show($"Malzeme kodu kopyalandı: {code}", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                XtraMessageBox.Show("Malzeme kodu alınamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void btn_List_Click_1(object sender, EventArgs e)
        {
            List();
        }
    }
}