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
using System.IO;
using ReplicateImageAI.Classes;

namespace ReplicateImageAI.Forms
{
    public partial class LogsForm :XtraForm
    {
        public LogsForm()
        {
            InitializeComponent();
        }
        private class LogItem
        {
            public string UILog { get; set; }
        }
        private string LogFilePath => Path.Combine(Application.StartupPath, "Logs", "UILog.txt");
        private List<LogItem> ReadLogs()
        {
            List<LogItem> list = new List<LogItem>();
            try
            {
                if (File.Exists(LogFilePath))
                {
                    string[] lines = File.ReadAllLines(LogFilePath);
                    foreach (string line in lines)
                        list.Add(new LogItem { UILog = line });
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Log okuma hatası:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging("Log okuma hatası: " + ex.ToString());
            }
            return list;
        }
        private void excelAlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "Excel Dosyası (*.xlsx)|*.xlsx",
                    Title = "Excel'e Aktar",
                    FileName = "UILog.xlsx"
                })
                {
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        gridView1.OptionsPrint.PrintDetails = true;
                        gridControl1.ExportToXlsx(saveDialog.FileName);
                        XtraMessageBox.Show("Excel dosyası başarıyla oluşturuldu.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Excel aktarım hatası:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging("Excel aktarım hatası: " + ex.ToString());
            }
        }
        private void temizleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(LogFilePath))
                    File.WriteAllText(LogFilePath, string.Empty);
                gridControl1.DataSource = ReadLogs();
                gridView1.RefreshData();
                XtraMessageBox.Show("Log dosyası başarıyla temizlendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Log temizleme hatası:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging("Log temizleme hatası: " + ex.ToString());
            }
        }
        private void LogsForm_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource = ReadLogs();
            GridViewDesigner.CustomizeGrid(gridView1);
            gridView1.Columns["UILog"].Caption = "Log Mesajı";
            gridView1.OptionsBehavior.ReadOnly = true;
            gridView1.OptionsBehavior.Editable = false;
        }
    }
}