namespace ReplicateImageAI.Forms
{
    partial class ItemsFileImageForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemsFileImageForm));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.excelAlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ramProductToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btn_List = new DevExpress.XtraEditors.SimpleButton();
            this.btn_ImageFile = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.lbl_unpicture = new System.Windows.Forms.Label();
            this.lbl_picture = new System.Windows.Forms.Label();
            this.lbl_ProductCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxControl1 = new DevExpress.XtraEditors.ListBoxControl();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyErrrorProductToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControl1)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.ContextMenuStrip = this.contextMenuStrip1;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(845, 601);
            this.gridControl1.TabIndex = 3;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.excelAlToolStripMenuItem,
            this.ramProductToolStripMenuItem,
            this.imageExportToolStripMenuItem,
            this.exportImageToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(296, 108);
            // 
            // excelAlToolStripMenuItem
            // 
            this.excelAlToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("excelAlToolStripMenuItem.Image")));
            this.excelAlToolStripMenuItem.Name = "excelAlToolStripMenuItem";
            this.excelAlToolStripMenuItem.Size = new System.Drawing.Size(295, 26);
            this.excelAlToolStripMenuItem.Text = "Excel Al";
            this.excelAlToolStripMenuItem.Click += new System.EventHandler(this.excelAlToolStripMenuItem_Click);
            // 
            // ramProductToolStripMenuItem
            // 
            this.ramProductToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("ramProductToolStripMenuItem.Image")));
            this.ramProductToolStripMenuItem.Name = "ramProductToolStripMenuItem";
            this.ramProductToolStripMenuItem.Size = new System.Drawing.Size(295, 26);
            this.ramProductToolStripMenuItem.Text = "Seçili Malzeme Kodunu Kopyala";
            this.ramProductToolStripMenuItem.Click += new System.EventHandler(this.ramProductToolStripMenuItem_Click);
            // 
            // imageExportToolStripMenuItem
            // 
            this.imageExportToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("imageExportToolStripMenuItem.Image")));
            this.imageExportToolStripMenuItem.Name = "imageExportToolStripMenuItem";
            this.imageExportToolStripMenuItem.Size = new System.Drawing.Size(295, 26);
            this.imageExportToolStripMenuItem.Text = "Görselleri Dışarı Al";
            this.imageExportToolStripMenuItem.Click += new System.EventHandler(this.imageExportToolStripMenuItem_Click);
            // 
            // exportImageToolStripMenuItem
            // 
            this.exportImageToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportImageToolStripMenuItem.Image")));
            this.exportImageToolStripMenuItem.Name = "exportImageToolStripMenuItem";
            this.exportImageToolStripMenuItem.Size = new System.Drawing.Size(295, 26);
            this.exportImageToolStripMenuItem.Text = "Seçili Görseli Dışarı Al";
            this.exportImageToolStripMenuItem.Click += new System.EventHandler(this.exportImageToolStripMenuItem_Click);
            // 
            // gridView1
            // 
            this.gridView1.DetailHeight = 431;
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
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
            this.btn_List.Location = new System.Drawing.Point(860, 84);
            this.btn_List.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_List.Name = "btn_List";
            this.btn_List.Size = new System.Drawing.Size(212, 49);
            this.btn_List.TabIndex = 5;
            this.btn_List.Text = "Listeyi Yenile";
            this.btn_List.Click += new System.EventHandler(this.btn_List_Click);
            // 
            // btn_ImageFile
            // 
            this.btn_ImageFile.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Warning;
            this.btn_ImageFile.Appearance.Font = new System.Drawing.Font("Tahoma", 12.25F, System.Drawing.FontStyle.Bold);
            this.btn_ImageFile.Appearance.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.btn_ImageFile.Appearance.Options.UseBackColor = true;
            this.btn_ImageFile.Appearance.Options.UseFont = true;
            this.btn_ImageFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_ImageFile.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_ImageFile.ImageOptions.Image")));
            this.btn_ImageFile.Location = new System.Drawing.Point(860, 14);
            this.btn_ImageFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_ImageFile.Name = "btn_ImageFile";
            this.btn_ImageFile.Size = new System.Drawing.Size(212, 49);
            this.btn_ImageFile.TabIndex = 6;
            this.btn_ImageFile.Text = "Logoya Aktar";
            this.btn_ImageFile.Click += new System.EventHandler(this.btn_ImageFile_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("groupControl1.CaptionImageOptions.Image")));
            this.groupControl1.Controls.Add(this.lbl_unpicture);
            this.groupControl1.Controls.Add(this.lbl_picture);
            this.groupControl1.Controls.Add(this.lbl_ProductCount);
            this.groupControl1.Controls.Add(this.label3);
            this.groupControl1.Controls.Add(this.label2);
            this.groupControl1.Controls.Add(this.label1);
            this.groupControl1.Location = new System.Drawing.Point(860, 159);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(351, 158);
            this.groupControl1.TabIndex = 7;
            this.groupControl1.Text = "Malzeme Bilgisi";
            // 
            // lbl_unpicture
            // 
            this.lbl_unpicture.AutoSize = true;
            this.lbl_unpicture.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.lbl_unpicture.Location = new System.Drawing.Point(283, 108);
            this.lbl_unpicture.Name = "lbl_unpicture";
            this.lbl_unpicture.Size = new System.Drawing.Size(20, 22);
            this.lbl_unpicture.TabIndex = 9;
            this.lbl_unpicture.Text = "0";
            // 
            // lbl_picture
            // 
            this.lbl_picture.AutoSize = true;
            this.lbl_picture.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.lbl_picture.Location = new System.Drawing.Point(246, 76);
            this.lbl_picture.Name = "lbl_picture";
            this.lbl_picture.Size = new System.Drawing.Size(20, 22);
            this.lbl_picture.TabIndex = 9;
            this.lbl_picture.Text = "0";
            // 
            // lbl_ProductCount
            // 
            this.lbl_ProductCount.AutoSize = true;
            this.lbl_ProductCount.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.lbl_ProductCount.Location = new System.Drawing.Point(149, 43);
            this.lbl_ProductCount.Name = "lbl_ProductCount";
            this.lbl_ProductCount.Size = new System.Drawing.Size(20, 22);
            this.lbl_ProductCount.TabIndex = 8;
            this.lbl_ProductCount.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.label3.Location = new System.Drawing.Point(9, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(268, 22);
            this.label3.TabIndex = 9;
            this.label3.Text = "Görseli Olmayan Malzeme Sayısı:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.label2.Location = new System.Drawing.Point(9, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(231, 22);
            this.label2.TabIndex = 8;
            this.label2.Text = "Görsel Olan Malzeme Sayısı:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 10.25F);
            this.label1.Location = new System.Drawing.Point(9, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 22);
            this.label1.TabIndex = 7;
            this.label1.Text = "Malzeme Sayısı:";
            // 
            // listBoxControl1
            // 
            this.listBoxControl1.ContextMenuStrip = this.contextMenuStrip2;
            this.listBoxControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxControl1.Location = new System.Drawing.Point(2, 33);
            this.listBoxControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listBoxControl1.Name = "listBoxControl1";
            this.listBoxControl1.Size = new System.Drawing.Size(347, 190);
            this.listBoxControl1.TabIndex = 8;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyErrrorProductToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(296, 30);
            // 
            // copyErrrorProductToolStripMenuItem
            // 
            this.copyErrrorProductToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyErrrorProductToolStripMenuItem.Image")));
            this.copyErrrorProductToolStripMenuItem.Name = "copyErrrorProductToolStripMenuItem";
            this.copyErrrorProductToolStripMenuItem.Size = new System.Drawing.Size(295, 26);
            this.copyErrrorProductToolStripMenuItem.Text = "Seçili Malzeme Kodunu Kopyala";
            this.copyErrrorProductToolStripMenuItem.Click += new System.EventHandler(this.copyErrrorProductToolStripMenuItem_Click);
            // 
            // groupControl2
            // 
            this.groupControl2.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("groupControl2.CaptionImageOptions.Image")));
            this.groupControl2.Controls.Add(this.listBoxControl1);
            this.groupControl2.Location = new System.Drawing.Point(860, 334);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(351, 225);
            this.groupControl2.TabIndex = 9;
            this.groupControl2.Text = "Görsel Aktarım";
            // 
            // ItemsFileImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1200, 601);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.btn_ImageFile);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.btn_List);
            this.IconOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("ItemsFileImageForm.IconOptions.LargeImage")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "ItemsFileImageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Resim Dosyası Aktar";
            this.Load += new System.EventHandler(this.ItemsFileImageForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControl1)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.SimpleButton btn_List;
        private DevExpress.XtraEditors.SimpleButton btn_ImageFile;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem excelAlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ramProductToolStripMenuItem;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private System.Windows.Forms.Label lbl_unpicture;
        private System.Windows.Forms.Label lbl_picture;
        private System.Windows.Forms.Label lbl_ProductCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ListBoxControl listBoxControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem copyErrrorProductToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageExportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportImageToolStripMenuItem;
    }
}