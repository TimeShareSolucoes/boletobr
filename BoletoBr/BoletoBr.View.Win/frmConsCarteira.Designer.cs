namespace BoletoBr.View.Win
{
    partial class frmConsCarteira
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConsCarteira));
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barbtnCarteira = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnGerarBoleto = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnCadastrar = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnAlterar = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnExcluir = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.barbtnCarteira,
            this.barbtnGerarBoleto,
            this.barbtnCadastrar,
            this.barbtnAlterar,
            this.barbtnExcluir});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 6;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ribbonControl1.Size = new System.Drawing.Size(1394, 145);
            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // barbtnCarteira
            // 
            this.barbtnCarteira.Caption = "Carteiras de boletos";
            this.barbtnCarteira.Glyph = ((System.Drawing.Image)(resources.GetObject("barbtnCarteira.Glyph")));
            this.barbtnCarteira.Id = 1;
            this.barbtnCarteira.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barbtnCarteira.LargeGlyph")));
            this.barbtnCarteira.Name = "barbtnCarteira";
            // 
            // barbtnGerarBoleto
            // 
            this.barbtnGerarBoleto.Caption = "Gerar boleto";
            this.barbtnGerarBoleto.Glyph = ((System.Drawing.Image)(resources.GetObject("barbtnGerarBoleto.Glyph")));
            this.barbtnGerarBoleto.Id = 2;
            this.barbtnGerarBoleto.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barbtnGerarBoleto.LargeGlyph")));
            this.barbtnGerarBoleto.Name = "barbtnGerarBoleto";
            // 
            // barbtnCadastrar
            // 
            this.barbtnCadastrar.Caption = "Cadastrar";
            this.barbtnCadastrar.Glyph = ((System.Drawing.Image)(resources.GetObject("barbtnCadastrar.Glyph")));
            this.barbtnCadastrar.Id = 3;
            this.barbtnCadastrar.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barbtnCadastrar.LargeGlyph")));
            this.barbtnCadastrar.Name = "barbtnCadastrar";
            this.barbtnCadastrar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnCadastrar_ItemClick);
            // 
            // barbtnAlterar
            // 
            this.barbtnAlterar.Caption = "Alterar";
            this.barbtnAlterar.Glyph = ((System.Drawing.Image)(resources.GetObject("barbtnAlterar.Glyph")));
            this.barbtnAlterar.Id = 4;
            this.barbtnAlterar.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barbtnAlterar.LargeGlyph")));
            this.barbtnAlterar.Name = "barbtnAlterar";
            this.barbtnAlterar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnAlterar_ItemClick);
            // 
            // barbtnExcluir
            // 
            this.barbtnExcluir.Caption = "Excluir";
            this.barbtnExcluir.Glyph = ((System.Drawing.Image)(resources.GetObject("barbtnExcluir.Glyph")));
            this.barbtnExcluir.Id = 5;
            this.barbtnExcluir.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barbtnExcluir.LargeGlyph")));
            this.barbtnExcluir.Name = "barbtnExcluir";
            this.barbtnExcluir.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnExcluir_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Opções";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barbtnCadastrar);
            this.ribbonPageGroup1.ItemLinks.Add(this.barbtnAlterar);
            this.ribbonPageGroup1.ItemLinks.Add(this.barbtnExcluir);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "Cadastros";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 145);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.MenuManager = this.ribbonControl1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1394, 565);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // frmConsCarteira
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1394, 710);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.ribbonControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.Name = "frmConsCarteira";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dados carteiras";
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.BarButtonItem barbtnCarteira;
        private DevExpress.XtraBars.BarButtonItem barbtnGerarBoleto;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.BarButtonItem barbtnCadastrar;
        private DevExpress.XtraBars.BarButtonItem barbtnAlterar;
        private DevExpress.XtraBars.BarButtonItem barbtnExcluir;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
    }
}