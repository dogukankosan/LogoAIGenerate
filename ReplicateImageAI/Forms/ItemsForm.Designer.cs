namespace ReplicateImageAI.Forms
{
    partial class ItemsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemsForm));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btn_LogoSave = new DevExpress.XtraEditors.SimpleButton();
            this.btn_List = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(724, 572);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // btn_LogoSave
            // 
            this.btn_LogoSave.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
            this.btn_LogoSave.Appearance.Font = new System.Drawing.Font("Tahoma", 12.25F, System.Drawing.FontStyle.Bold);
            this.btn_LogoSave.Appearance.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.btn_LogoSave.Appearance.Options.UseBackColor = true;
            this.btn_LogoSave.Appearance.Options.UseFont = true;
            this.btn_LogoSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_LogoSave.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_LogoSave.ImageOptions.Image")));
            this.btn_LogoSave.Location = new System.Drawing.Point(730, 26);
            this.btn_LogoSave.Name = "btn_LogoSave";
            this.btn_LogoSave.Size = new System.Drawing.Size(182, 40);
            this.btn_LogoSave.TabIndex = 1;
            this.btn_LogoSave.Text = "Logoya Aktar";
            this.btn_LogoSave.Click += new System.EventHandler(this.btn_LogoSave_Click);
            // 
            // btn_List
            // 
            this.btn_List.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Question;
            this.btn_List.Appearance.Font = new System.Drawing.Font("Tahoma", 12.25F, System.Drawing.FontStyle.Bold);
            this.btn_List.Appearance.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.btn_List.Appearance.Options.UseBackColor = true;
            this.btn_List.Appearance.Options.UseFont = true;
            this.btn_List.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_List.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_List.ImageOptions.Image")));
            this.btn_List.Location = new System.Drawing.Point(730, 84);
            this.btn_List.Name = "btn_List";
            this.btn_List.Size = new System.Drawing.Size(182, 40);
            this.btn_List.TabIndex = 2;
            this.btn_List.Text = "Listeyi Yenile";
            this.btn_List.Click += new System.EventHandler(this.btn_List_Click);
            // 
            // ItemsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1030, 572);
            this.Controls.Add(this.btn_List);
            this.Controls.Add(this.btn_LogoSave);
            this.Controls.Add(this.gridControl1);
            this.IconOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("ItemsForm.IconOptions.LargeImage")));
            this.MaximizeBox = false;
            this.Name = "ItemsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Malzeme Listesi";
            this.Load += new System.EventHandler(this.ItemsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.SimpleButton btn_LogoSave;
        private DevExpress.XtraEditors.SimpleButton btn_List;
    }
}