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

namespace ReplicateImageAI.Forms
{
    public partial class HomeForm : XtraForm
    {
        public HomeForm()
        {
            InitializeComponent();
        }
        internal void OpenFormInContainer(Form form)
        {
            if (form == null) return;
            try
            {
                panelControl1.Controls.Clear();
                form.TopLevel = false;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = DockStyle.Fill;
                panelControl1.Controls.Add(form);
                form.Show();
            }
            catch (Exception)
            {

            }
        }
        private void btn_productPicture_Click(object sender, EventArgs e)
        {
            OpenFormInContainer(new ItemsForm());
        }
        private void btn_SQLSettings_Click(object sender, EventArgs e)
        {
            OpenFormInContainer(new SQLSettingForm());
        }
        private void btn_Logs_Click(object sender, EventArgs e)
        {
            OpenFormInContainer(new LogsForm());
        }
        private void btn_SQLiteCommand_Click(object sender, EventArgs e)
        {
            OpenFormInContainer(new SQLiteCommandForm());
        }
        private void Default_StyleChanged(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.ThemaName = DevExpress.LookAndFeel.UserLookAndFeel.Default.ActiveSkinName;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Tema kaydetme hatası:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging("Tema kaydetme hatası: " + ex.ToString());
            }
        }
        private void HomeForm_Load(object sender, EventArgs e)
        {
            try
            {
                DevExpress.UserSkins.BonusSkins.Register();
                DevExpress.Skins.SkinManager.EnableFormSkins();
                string savedTheme = Properties.Settings.Default.ThemaName;
                if (!string.IsNullOrWhiteSpace(savedTheme))
                    DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(savedTheme);
                DevExpress.LookAndFeel.UserLookAndFeel.Default.StyleChanged += Default_StyleChanged;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Tema yükleme hatası:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging("Tema yükleme hatası: " + ex.ToString());
            }
        }
        private void btn_Thema_Click(object sender, EventArgs e)
        {
            popupMenu2.ShowPopup(Cursor.Position);
        }
        private void btn_ImageSetting_Click(object sender, EventArgs e)
        {
            OpenFormInContainer(new ImageGenerateSettingForm());
        }
    }
}