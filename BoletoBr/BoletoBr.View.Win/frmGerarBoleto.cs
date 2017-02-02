using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BoletoBr.View.Win.Dominio;
using BoletoBr.View.XtraReport;
using DevExpress.XtraEditors;

namespace BoletoBr.View.Win
{
    public partial class frmGerarBoleto : DevExpress.XtraEditors.XtraForm
    {
        public frmGerarBoleto()
        {
            InitializeComponent();

            var connection = new ConnectionSqlite();
            cbbCarteira.Properties.DataSource = connection.GetAll();
        }

        private void btnGerarBoleto_Click(object sender, EventArgs e)
        {
            if (!Validar()) return;

            try
            {
                var carteira = GetCarteiraSelecionada();
                var sacado = new SacadoBoleto(edtCedenteNome.EditValue?.ToString(),
                    edtCedenteCpfCnpj.EditValue?.ToString(), edtCedenteEndereco.EditValue?.ToString(),
                    edtCedenteComplemento.EditValue?.ToString(), edtCedenteNumero.EditValue?.ToString(),
                    edtCedenteBairro.EditValue?.ToString(), edtCedenteCidade.EditValue?.ToString(),
                    edtCedenteUF.EditValue?.ToString(), edtCedenteCEP.EditValue?.ToString());

                var boleto = Funcoes.GerarBoleto(carteira, sacado, Convert.ToDecimal(edtValor.EditValue?.ToString()),
                    edtDataVencimento.DateTime, edtNumeroDocumento.EditValue?.ToString());

                var modeloUtilizar = Funcoes.RetornaModeloBoletoUtilizar(carteira.ModeloBoleto);

                var objVisualizadorBoleto = new VisualizadorBoleto(modeloUtilizar, new List<Boleto>() {boleto});
                objVisualizadorBoleto.ExibirNaTela();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Validar()
        {
            var carteira = GetCarteiraSelecionada();
            if (carteira == null)
            {
                MessageBox.Show("Selecione a carteira");
                return false;
            }
            if (Convert.ToInt64(edtNumeroDocumento.EditValue?.ToString()) <= 0)
            {
                MessageBox.Show("Informe o número do documento (somente número).");
                return false;
            }
            if (Convert.ToDecimal(edtValor.EditValue?.ToString()) <= 0)
            {
                MessageBox.Show("Informe o valor.");
                return false;
            }
            if (edtDataVencimento.EditValue == null)
            {
                MessageBox.Show("Informe a data de vencimento.");
                return false;
            }

            return true;
        }

        private CarteiraBoleto GetCarteiraSelecionada()
        {
            var carteiras = cbbCarteira.Properties.DataSource as List<CarteiraBoleto>;
            return
                carteiras?.FirstOrDefault(x => x.IdCarteiraBoleto == Convert.ToInt16(cbbCarteira.EditValue?.ToString()));
        }

        private void btnGerarRemessa_Click(object sender, EventArgs e)
        {
            if (!Validar()) return;

            try
            {
                var carteira = GetCarteiraSelecionada();
                var sacado = new SacadoBoleto(edtCedenteNome.EditValue?.ToString(),
                    edtCedenteCpfCnpj.EditValue?.ToString(), edtCedenteEndereco.EditValue?.ToString(),
                    edtCedenteComplemento.EditValue?.ToString(), edtCedenteNumero.EditValue?.ToString(),
                    edtCedenteBairro.EditValue?.ToString(), edtCedenteCidade.EditValue?.ToString(),
                    edtCedenteUF.EditValue?.ToString(), edtCedenteCEP.EditValue?.ToString());

                var caminhoArquivo =
                    Funcoes.GerarArquivoRemessa(carteira, sacado, Convert.ToDecimal(edtValor.EditValue?.ToString()),
                        edtDataVencimento.DateTime, edtNumeroDocumento.EditValue?.ToString());

                if (
                    MessageBox.Show(string.Format(@"Remessa gerada com sucesso!
                    {0}Local do arquivo:{0}{1}{0}
                    Deseja abrir a pasta onde o arquivo foi gerado?", Environment.NewLine, caminhoArquivo), "Aviso",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    Process.Start(Path.GetDirectoryName(caminhoArquivo));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}