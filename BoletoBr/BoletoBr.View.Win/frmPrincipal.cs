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

namespace BoletoBr.View.Win
{
    public partial class frmPrincipal : DevExpress.XtraEditors.XtraForm
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void barbtnCarteira_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var frmCadCarteira = new frmConsCarteira();
            frmCadCarteira.ShowDialog();
        }

        private void barbtnGerarBoleto_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var frmGerarBoleto = new frmGerarBoleto();
            frmGerarBoleto.ShowDialog();
        }
    }
}