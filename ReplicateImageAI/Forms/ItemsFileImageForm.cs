using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using ReplicateImageAI.Classes;
using DevExpress.XtraGrid;
using ClosedXML.Excel;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;

namespace ReplicateImageAI.Forms
{
    public partial class ItemsFileImageForm : XtraForm
    {
        private DataTable dtSettings;
        private DataTable dtGrid;
        private ToolTipController toolTipController1;
        private CancellationTokenSource _cts;
        public ItemsFileImageForm()
        {
            InitializeComponent();
        }
        private async void ItemsFileImageForm_Load(object sender, EventArgs e)
        {
            await InitializeAsync();
        }
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
            gridView1.DoubleClick += gridView1_DoubleClick;
            gridView1.RowStyle += gridView1_RowStyle;
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
                foreach (string colName in new[] { "Malzeme Kodu", "Malzeme Açıklaması", "Durum" })
                {
                    GridColumn col = gridView1.Columns[colName];
                    if (col != null) col.OptionsColumn.AllowMove = false;
                    if (colName == "Durum" && col != null) col.Width = 200;
                }
                gridView1.OptionsView.ShowGroupPanel = false;
                gridView1.OptionsCustomization.AllowGroup = false;
                gridView1.OptionsCustomization.AllowColumnMoving = false;
                gridView1.OptionsCustomization.AllowQuickHideColumns = false;
                gridView1.OptionsMenu.EnableGroupPanelMenu = false;
                gridView1.OptionsSelection.MultiSelect = false;
            }
            finally
            {
                gridView1.EndUpdate();
            }
        }
        private void ToolTipController1_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            if (e.SelectedControl != gridControl1) return;
            GridHitInfo hit = gridView1.CalcHitInfo(e.ControlMousePosition);
            if (!hit.InRowCell || hit.Column?.FieldName != "ERP Görsel") return;
            if (gridView1.GetRowCellValue(hit.RowHandle, hit.Column) is byte[] bytes && bytes.Length > 0)
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream(bytes))
                    using (Image img = Image.FromStream(ms))
                    {
                        ToolTipControlInfo info = new ToolTipControlInfo(hit.RowHandle.ToString() + hit.Column.FieldName, "");
                        SuperToolTip superTip = new SuperToolTip();
                        superTip.Items.Add(new ToolTipItem { Image = new Bitmap(img), Text = "" });
                        info.SuperTip = superTip;
                        e.Info = info;
                    }
                }
                catch {  }
            }
        }
        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;
            string durum = Convert.ToString(gridView1.GetRowCellValue(e.RowHandle, "Durum"));
            if (!string.IsNullOrEmpty(durum))
            {
                e.Appearance.ForeColor = durum.Equals("Başarılı", StringComparison.OrdinalIgnoreCase)
                    ? Color.Green
                    : Color.Red;
            }
        }
        private async void btn_ImageFile_Click(object sender, EventArgs e)
        {
            string[] files = PickImageFiles();
            if (files.Length == 0)
            {
                XtraMessageBox.Show("Klasörde uygun formatta (.jpg, .jpeg, .png, .bmp) görsel bulunamadı.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            HashSet<string> codesSet = new HashSet<string>(files.Select(p => Path.GetFileNameWithoutExtension(p)), StringComparer.OrdinalIgnoreCase);
            ClearDurum();
            int success = 0, fail = 0, notMatched = 0;
            Dictionary<string, string> statusDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            listBoxControl1.Items.Clear();
            foreach (var file in files)
            {
                string code = Path.GetFileNameWithoutExtension(file);
                var rows = dtGrid.Select($"[Malzeme Kodu] = '{code.Replace("'", "''")}'");
                if (rows.Length == 0)
                {
                    TextLog.TextLogging($"[Resim: {code}] Malzeme bulunamadı.");
                    notMatched++;
                    fail++;
                    listBoxControl1.Items.Add($"{code} - Eşleşmedi");
                    continue;
                }
                DataRow row = rows[0];
                row["Durum"] = DBNull.Value;
                try
                {
                    byte[] imgData = File.ReadAllBytes(file);
                    bool ok = await SaveImage(row, imgData);
                    row["Durum"] = ok ? "Başarılı" : "SQL Hatası";
                    if (ok)
                        success++;
                    else
                    {
                        fail++;
                        listBoxControl1.Items.Add($"{code} - SQL Hatası");
                    }
                }
                catch (Exception ex)
                {
                    TextLog.TextLogging($"[Dosya: {file}] Hata: {ex}");
                    row["Durum"] = "SQL Hatası";
                    fail++;
                    listBoxControl1.Items.Add($"{code} - SQL Hatası");
                }
            }
            foreach (DataRow r in dtGrid.Rows)
                statusDict[r.Field<string>("Malzeme Kodu")] = r.Field<string>("Durum");
            await ReloadGridWithStatus(statusDict, codesSet);
            if (success == 0 && fail == 0)
            {
                XtraMessageBox.Show("Hiçbir malzeme kodu eşleşmedi. İşlem yapılmadı.",
                    "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            XtraMessageBox.Show($"{success} başarılı, {fail} hata. (Eşleşmeyen: {notMatched})",
                "Sonuç", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private string[] PickImageFiles()
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() != DialogResult.OK) return Array.Empty<string>();
            return Directory.GetFiles(dlg.SelectedPath, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                             || f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)
                             || f.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                             || f.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
                    .ToArray();
        }
        private void ClearDurum()
        {
            foreach (DataRow r in dtGrid.Rows)
                r["Durum"] = DBNull.Value;
        }
        private async Task<bool> SaveImage(DataRow row, byte[] data)
        {
            int refId = Convert.ToInt32(row["ID"]);
            string name = Convert.ToString(row["Malzeme Açıklaması"]);
            return dtSettings.Rows[0]["ERPType"].ToString() == "TIGER ERP"
                ? await SaveToTigerERPAsync(dtSettings, refId, name, data)
                : await SaveToJPlatformAsync(dtSettings, refId, name, data);
        }
        private async Task ReloadGridWithStatus(Dictionary<string, string> statusMap, HashSet<string> codes)
        {
            await LoadGridAsync(_cts.Token);
            ConfigureGrid();
            foreach (DataRow r in dtGrid.Rows)
            {
                string code = r.Field<string>("Malzeme Kodu");
                if (statusMap.TryGetValue(code, out string status))
                    r["Durum"] = status;
                else
                    r["Durum"] = codes.Contains(code) ? "Malzeme Kodu Bulunamadı" : "Klasörde Görsel Yok";
            }
            gridControl1.RefreshDataSource();
            UpdateMaterialInfo();
        }
        private async void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridHitInfo hit = gridView1.CalcHitInfo(gridControl1.PointToClient(Control.MousePosition));
                if (!hit.InRow) return;
                DataRow row = gridView1.GetDataRow(hit.RowHandle);
                if (row == null) return;
                using (OpenFileDialog ofd = new OpenFileDialog
                {
                    Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp",
                    Title = "Malzeme görseli seç",
                    Multiselect = false
                })
                {
                    if (ofd.ShowDialog() != DialogResult.OK) return;
                    byte[] img = File.ReadAllBytes(ofd.FileName);
                    bool ok = await SaveImage(row, img);
                    if (ok)
                    {
                        row["ERP Görsel"] = img;
                        row["Durum"] = "Başarılı";
                        gridView1.RefreshRow(hit.RowHandle);
                        UpdateMaterialInfo();
                        XtraMessageBox.Show("Görsel güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        row["Durum"] = "SQL Hatası"; 
                        gridView1.RefreshRow(hit.RowHandle);
                        XtraMessageBox.Show("Görsel güncellenemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                TextLog.TextLogging($"[DoubleClick Hata] {ex}");
                XtraMessageBox.Show("Görsel okunamadı veya kaydedilemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void btn_LogoSave_Click(object sender, EventArgs e)
        {
            int[] selected = gridView1.GetSelectedRows();
            if (selected.Length == 0)
            {
                XtraMessageBox.Show("Lütfen bir satır seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Seçili malzemeye görsel seç",
                Multiselect = false
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            byte[] img;
            try
            {
                img = File.ReadAllBytes(ofd.FileName);
            }
            catch (Exception ex)
            {
                TextLog.TextLogging($"[LogoSave] Dosya okuma hatası: {ex}");
                XtraMessageBox.Show("Dosya okunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int success = 0, fail = 0;
            foreach (int handle in selected)
            {
                DataRow row = gridView1.GetDataRow(handle);
                if (row == null) { fail++; continue; }
                bool ok = await SaveImage(row, img);
                row["Durum"] = ok ? "Başarılı" : "SQL Hatası";
                if (ok) success++; else fail++;
            }
            XtraMessageBox.Show($"{success} başarılı, {fail} hata.", "Sonuç", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await ReloadGridWithStatus(
                dtGrid.AsEnumerable().ToDictionary(r => r.Field<string>("Malzeme Kodu"), r => r.Field<string>("Durum")),
                new HashSet<string>(dtGrid.AsEnumerable().Select(r => r.Field<string>("Malzeme Kodu")))
            );
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
        private void excelAlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.RowCount == 0)
                {
                    XtraMessageBox.Show("Aktarılacak veri bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DataTable export = dtGrid.Clone(); 
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    int rowHandle = gridView1.GetVisibleRowHandle(i);
                    if (rowHandle >= 0)
                    {
                        DataRow row = ((DataRowView)gridView1.GetRow(rowHandle)).Row;
                        export.ImportRow(row);
                    }
                }
                if (export.Rows.Count == 0)
                {
                    XtraMessageBox.Show("Filtreye uygun veri bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                using (SaveFileDialog dlg = new SaveFileDialog
                {
                    Filter = "Excel Dosyası (*.xlsx)|*.xlsx",
                    Title = "Excel'e Aktar",
                    FileName = "MalzemeListesi.xlsx"
                })
                {
                    if (dlg.ShowDialog() != DialogResult.OK)
                        return;
                    if (export.Columns.Contains("Durum"))
                        export.Columns.Remove("Durum");
                    if (!export.Columns.Contains("Görsel"))
                        export.Columns.Add("Görsel", typeof(string));
                    if (export.Columns.Contains("ERP Görsel"))
                    {
                        foreach (DataRow r in export.Rows)
                        {
                            byte[] img = r["ERP Görsel"] as byte[];
                            r["Görsel"] = (img != null && img.Length > 0) ? "Var" : "Yok";
                        }
                        export.Columns.Remove("ERP Görsel");
                    }
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
        private async Task<bool> SaveToTigerERPAsync(DataTable SQLSettings, int logicalRef, string malzemeAdi, byte[] imageData)
        {
            try
            {
                string companyNo = SQLSettings.Rows[0]["CompanyNo"].ToString();
                string checkQuery = $@"
            SELECT LDATA 
            FROM LG_{companyNo}_FIRMDOC WITH (NOLOCK) 
            WHERE INFOREF = @InfoRef AND INFOTYP = 20 AND DOCTYP = 0 AND DOCNR = 11";
                Dictionary<string, object> checkParams = new Dictionary<string, object> { { "@InfoRef", logicalRef } };
                object ldataValue = await SQLCrud.ExecuteScalarAsync(checkQuery, checkParams);
                bool exists = ldataValue != null && ldataValue != DBNull.Value;
                string query = exists
                    ? $@"UPDATE LG_{companyNo}_FIRMDOC 
                 SET LDATA = @ImageData 
                 WHERE INFOREF = @InfoRef AND INFOTYP = 20 AND DOCTYP = 0 AND DOCNR = 11"
                    : $@"INSERT INTO LG_{companyNo}_FIRMDOC 
                 (INFOTYP, INFOREF, DOCTYP, DOCNR, LDATA) 
                 VALUES (20, @InfoRef, 0, 11, @ImageData)";
                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@InfoRef", logicalRef },
            { "@ImageData", imageData }
        };
                bool ok = await SQLCrud.ExecuteCrudAsync(query, parameters);
                if (!ok)
                {
                    TextLog.TextLogging($"[Malzeme: {malzemeAdi}] FIRMDOC yazım başarısız.");
                    return false;
                }
                string incQuery = $@"UPDATE LG_{companyNo}_ITEMS SET IMAGEINC = 1 WHERE LOGICALREF = @LogicalRef";
                Dictionary<string, object> incParams = new Dictionary<string, object> { { "@LogicalRef", logicalRef } };
                bool incOk = await SQLCrud.ExecuteCrudAsync(incQuery, incParams);
                if (!incOk)
                {
                    TextLog.TextLogging($"[Malzeme: {malzemeAdi}] IMAGEINC güncellenemedi.");
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
                string companyNo = SQLSettings.Rows[0]["CompanyNo"].ToString();
                string checkQuery = $@"
            SELECT COUNT(*) 
            FROM U_{companyNo}_COMPANYDOCS WITH (NOLOCK) 
            WHERE INFOTYPE = 20 AND INFOREF = @InfoRef AND DOCTYPE = 0 AND DOCNR = 1";
                Dictionary<string, object> checkParams = new Dictionary<string, object> { { "@InfoRef", logicalRef } };
                object result = await SQLCrud.ExecuteScalarAsync(checkQuery, checkParams);
                int count = (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;
                if (count > 0)
                {
                    string updateQuery = $@"
                UPDATE U_{companyNo}_COMPANYDOCS 
                SET LDATA = @ImageData 
                WHERE INFOTYPE = 20 AND INFOREF = @InfoRef AND DOCTYPE = 0 AND DOCNR = 1";
                    Dictionary<string, object> updateParams = new Dictionary<string, object>
            {
                { "@InfoRef", logicalRef },
                { "@ImageData", imageData }
            };
                   return await SQLCrud.ExecuteCrudAsync(updateQuery, updateParams);
                }
                object newRef = await SQLCrud.ExecuteScalarAsync($"SELECT NEXT VALUE FOR U_{companyNo}_COMPANYDOCSSEQ", null);
                if (newRef == null || newRef == DBNull.Value)
                {
                    TextLog.TextLogging($"[Malzeme: {malzemeAdi}] Yeni LOGICALREF alınamadı.");
                    return false;
                }
                int newLogicalRef = Convert.ToInt32(newRef);
                string insertQuery = $@"
            INSERT INTO U_{companyNo}_COMPANYDOCS 
            (LOGICALREF, INFOTYPE, INFOREF, DOCTYPE, DOCNR, LDATA, DESCRIPTION, ISMAIN, 
             TE_RECSTATUS, TE_LABELS, TE_SUBCOMPANY, TE_WPIID, TE_WFIID, TE_RIGHTS)
            VALUES 
            (@NewLogicalRef, 20, @InfoRef, 0, 1, @ImageData, '', 0, -1, NULL, 0, 0, '', 0)";
                Dictionary<string, object> insertParams = new Dictionary<string, object>
        {
            { "@NewLogicalRef", newLogicalRef },
            { "@InfoRef", logicalRef },
            { "@ImageData", imageData }
        };
                return await SQLCrud.ExecuteCrudAsync(insertQuery, insertParams);
            }
            catch (Exception ex)
            {
                TextLog.TextLogging($"[SaveToJPlatformAsync] {malzemeAdi} hata: {ex}");
                return false;
            }
        }
        private async void btn_List_Click(object sender, EventArgs e)
        {
            try
            {
                _cts?.Cancel();
                _cts = new CancellationTokenSource();
                await LoadGridAsync(_cts.Token);
                ConfigureGrid();
            }
            catch (Exception ex)
            {
                TextLog.TextLogging($"[btn_List_Click] Hata: {ex}");
                XtraMessageBox.Show("Liste yenilenirken hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void imageExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dtGrid == null || dtGrid.Rows.Count == 0)
            {
                XtraMessageBox.Show("Aktarılacak veri bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog { Description = "Resimlerin kaydedileceği klasörü seçin" })
            {
                if (folderDialog.ShowDialog() != DialogResult.OK) return;
                string savePath = folderDialog.SelectedPath;
                int success = 0, fail = 0;
                foreach (DataRow row in dtGrid.Rows)
                {
                    try
                    {
                        string code = row["Malzeme Kodu"].ToString();
                        if (row["ERP Görsel"] is byte[] imageBytes && imageBytes.Length > 0)
                        {
                            string fileName = Path.Combine(savePath, $"{code}.jpg");

                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            using (Image img = Image.FromStream(ms))
                            {
                                img.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                                success++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        TextLog.TextLogging($"[Dışarı Aktarım Hatası] {ex}");
                        fail++;
                    }
                }
                XtraMessageBox.Show($"{success} görsel dışarı aktarıldı, {fail} hata oluştu.", "Sonuç", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void exportImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] selectedRows = gridView1.GetSelectedRows();
            if (selectedRows.Length == 0)
            {
                XtraMessageBox.Show("Lütfen bir satır seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DataRow row = gridView1.GetDataRow(selectedRows[0]);
            if (row == null || !(row["ERP Görsel"] is byte[] imageBytes) || imageBytes.Length == 0)
            {
                XtraMessageBox.Show("Seçili satırda geçerli bir görsel bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog { Description = "Resmin kaydedileceği klasörü seçin" })
            {
                if (folderDialog.ShowDialog() != DialogResult.OK) return;
                try
                {
                    string code = row["Malzeme Kodu"].ToString();
                    string filePath = Path.Combine(folderDialog.SelectedPath, $"{code}.jpg");
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    using (Image img = Image.FromStream(ms))
                    {
                        img.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    XtraMessageBox.Show("Görsel başarıyla dışarı aktarıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    TextLog.TextLogging($"[Tekli Dışa Aktarım Hatası] {ex}");
                    XtraMessageBox.Show("Görsel dışa aktarılırken hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}