namespace xsSettings.Forms
{
    partial class FrmSettings
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("CDDB");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("CD Audio", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("VCD/SVCD");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("DVD");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Disc", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3,
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("HTTP Proxy");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Network", new System.Windows.Forms.TreeNode[] {
            treeNode6});
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.tvMenu = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(408, 357);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(327, 357);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOkClick);
            // 
            // tvMenu
            // 
            this.tvMenu.HideSelection = false;
            this.tvMenu.Location = new System.Drawing.Point(12, 12);
            this.tvMenu.Name = "tvMenu";
            treeNode1.Name = "nodeDiscCddb";
            treeNode1.Tag = "CDDB";
            treeNode1.Text = "CDDB";
            treeNode2.Name = "nodeDiscCdda";
            treeNode2.Tag = "CDOPTIONS";
            treeNode2.Text = "CD Audio";
            treeNode3.Name = "nodeDiscVcd";
            treeNode3.Tag = "VCDOPTIONS";
            treeNode3.Text = "VCD/SVCD";
            treeNode4.Name = "nodeDiscDvd";
            treeNode4.Tag = "DVDOPTIONS";
            treeNode4.Text = "DVD";
            treeNode5.Name = "nodeDisc";
            treeNode5.Tag = "CDOPTIONS";
            treeNode5.Text = "Disc";
            treeNode6.Name = "nodeNetworkProxy";
            treeNode6.Tag = "NETPROXY";
            treeNode6.Text = "HTTP Proxy";
            treeNode7.Name = "nodeNetwork";
            treeNode7.Tag = "NETOPTIONS";
            treeNode7.Text = "Network";
            this.tvMenu.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode7});
            this.tvMenu.Size = new System.Drawing.Size(117, 324);
            this.tvMenu.TabIndex = 10;
            // 
            // FrmSettings
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 389);
            this.Controls.Add(this.tvMenu);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "xsMedia Options";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TreeView tvMenu;
    }
}