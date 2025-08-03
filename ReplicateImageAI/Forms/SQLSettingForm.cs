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
using ReplicateImageAI.Bussines;

namespace ReplicateImageAI.Forms
{
    public partial class SQLSettingForm : XtraForm
    {
        public SQLSettingForm()
        {
            InitializeComponent();
        }
        private async void btn_Save_Click(object sender, EventArgs e)
        {
            string tableName = "", columnName = "", first = "";
            if (cmb_ERPType.SelectedIndex == 0)
            {
                tableName = "INVOICE";
                columnName = "LOGICALREF";
                first = "LG";
            }
            else
            {
                tableName = "INVOICES";
                columnName = "LOGICALREF";
                first = "U";
            }
            if (!SQLSettingsValidator.ValidateSettings(txt_ServerName, txt_Port, txt_DatabaseName, txt_Username, txt_Password, txt_CompanyNo, txt_PeriodNo, cmb_ERPType))
                return;
            try
            {
                bool result = await SQLiteCrud.ConnectionStringControlAdd(
                    txt_ServerName.Text.Trim(),
                    txt_Username.Text.Trim(),
                    txt_Password.Text.Trim(),
                    txt_DatabaseName.Text.Trim(),
                    txt_Port.Text.Trim(),
                    txt_CompanyNo.Text.Trim(),
                    txt_PeriodNo.Text.Trim(), tableName, columnName, first,cmb_ERPType.Text);
                if (!result)
                {
                    XtraMessageBox.Show("MSSQL bağlantısı hatalı", "Hatalı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                XtraMessageBox.Show("MSSQL bağlantısı başarılı", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Kaydetme işlemi sırasında hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging("SQLSettingForm Save Hatası: " + ex);
            }
        }
        private async void SQLSettingForm_Load(object sender, EventArgs e)
        {
            txt_ServerName.Focus();
            txt_Port.Text = "1433";
            DataTable dt = await SQLiteCrud.GetDataFromSQLiteAsync("SELECT * FROM SQLConnectionString LIMIT 1");
            if (!DataHelper.IsDataExists(dt))
                return;
            try
            {
                txt_CompanyNo.Text = dt.Rows[0]["CompanyNo"].ToString();
                txt_PeriodNo.Text = dt.Rows[0]["PeriodNo"].ToString();
                cmb_ERPType.Text = dt.Rows[0]["ERPType"].ToString();
                string[] parameters = EncryptionHelper.Decrypt(dt.Rows[0]["ConnectString"].ToString()).Split(';');
                string port = string.Empty;
                foreach (string parameter in parameters)
                {
                    if (!string.IsNullOrWhiteSpace(parameter))
                    {
                        string[] keyValue = parameter.Split('=');
                        if (keyValue.Length < 2)
                            continue;
                        string key = keyValue[0].Trim();
                        string value = keyValue[1].Trim();
                        switch (key)
                        {
                            case "Server":
                                if (value.Contains(","))
                                {
                                    string[] serverParts = value.Split(',');
                                    txt_ServerName.Text = serverParts[0].Trim();
                                    port = serverParts[1].Trim();
                                }
                                else
                                    txt_ServerName.Text = value;
                                break;
                            case "Database":
                                txt_DatabaseName.Text = value;
                                break;
                            case "User Id":
                                txt_Username.Text = value;
                                break;
                            case "Password":
                                txt_Password.Text = value;
                                break;
                        }
                    }
                }
                if (string.IsNullOrEmpty(port))
                    txt_Port.Text = "1433";
                else
                    txt_Port.Text = port;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Ayarları okurken hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging("SQLSettingForm Load Hatası: " + ex);
            }
        }
        private void txt_CompanyNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;   
        }
        private void txt_PeriodNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }
        private void txt_Port_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }
    }
}