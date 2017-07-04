using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BoletoBr.View.Win.Dominio;
using DevExpress.XtraEditors;

namespace BoletoBr.View.Win
{
    public partial class frmConsCarteira : DevExpress.XtraEditors.XtraForm
    {
        public frmConsCarteira()
        {
            InitializeComponent();
            AtualizarDados();
        }

        private void AtualizarDados()
        {
            var connection = new ConnectionSqlite();
            gridControl1.DataSource = connection.GetAll();
        }

        private void barbtnCadastrar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var frmCadCarteira = new frmCadCarteira();
            if (frmCadCarteira.ShowDialog() == DialogResult.OK)
                AtualizarDados();
        }

        private void barbtnAlterar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var objSelecionado = gridView1.GetFocusedRow() as CarteiraBoleto;
            if (objSelecionado == null)
            {
                MessageBox.Show("Selecione uma carteira de boleto.");
                return;
            }
            var frmCadCarteira = new frmCadCarteira();
            frmCadCarteira.Carregar(objSelecionado);
            if (frmCadCarteira.ShowDialog() == DialogResult.OK)
                AtualizarDados();
        }

        private void barbtnExcluir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var objSelecionado = gridView1.GetFocusedRow() as CarteiraBoleto;

            if (objSelecionado == null)
            {
                MessageBox.Show("Selecione uma carteira de boleto.");
                return;
            }

            try
            {
                if (
                    MessageBox.Show("Deseja excluir a carteira?", "Atenção", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var con = new ConnectionSqlite();
                    con.ExcluirCarteiraBoleto(objSelecionado);
                    AtualizarDados();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}