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
using DevExpress.XtraLayout.Utils;

namespace BoletoBr.View.Win
{
    public partial class frmCadCarteira : DevExpress.XtraEditors.XtraForm
    {
        public CarteiraBoleto _carteiraBoleto { get; set; }

        public frmCadCarteira()
        {
            InitializeComponent();

            var listTipoArquivo = new List<string>() {"CNAB240", "CNAB400"};
            cbbTipoArquivoRetorno.Properties.Items.AddRange(listTipoArquivo);
            cbbTipoArquivoRemessa.Properties.Items.AddRange(listTipoArquivo);

            var listaModelosBoletos = new List<string> {"CARNE", "FATURA", "PADRAO", "FATURA-CARTA", "CARNE-A5"};
            cbbModeloBoleto.Properties.Items.AddRange(listaModelosBoletos);

            cbbBanco.Properties.DataSource = Banco.GetBancos();
        }

        public void Carregar(CarteiraBoleto carteiraBoleto)
        {
            _carteiraBoleto = carteiraBoleto;

            cbbBanco.EditValue = _carteiraBoleto.CodigoBanco;
            edtNumeroAgencia.EditValue = _carteiraBoleto.NumeroAgencia;
            edtDigitoAgencia.EditValue = _carteiraBoleto.DigitoAgencia;
            edtNumeroConta.EditValue = _carteiraBoleto.NumeroConta;
            edtDigitoConta.EditValue = _carteiraBoleto.DigitoConta;
            edtDescricao.EditValue = _carteiraBoleto.DescricaoCarteira;
            edtNumeroCarteira.EditValue = _carteiraBoleto.NumeroCarteira;
            cbbModeloBoleto.EditValue = _carteiraBoleto.ModeloBoleto;
            cbbTipoArquivoRetorno.EditValue = _carteiraBoleto.TipoArquivoRetorno;
            cbbTipoArquivoRemessa.EditValue = _carteiraBoleto.TipoArquivoRemessa;
            edtCodigoCedente.EditValue = _carteiraBoleto.CodigoCedente;
            edtDigitoCedente.EditValue = _carteiraBoleto.DigitoCodigoCedente;
            edtConvenio.EditValue = _carteiraBoleto.NumeroConvenio;
            edtCodigoTransmissao.EditValue = _carteiraBoleto.CodigoTransmissao;
            edtBancoGeraBoletos.Checked = _carteiraBoleto.BancoGeraBoleto;
            edtCedenteNome.EditValue = _carteiraBoleto.NomeCedente;
            edtCedenteCpfCnpj.EditValue = _carteiraBoleto.CpfCnpjCedente;
            edtCedenteEndereco.EditValue = _carteiraBoleto.EnderecoCedente;
            edtCedenteComplemento.EditValue = _carteiraBoleto.ComplementoCedente;
            edtCedenteNumero.EditValue = _carteiraBoleto.NumeroCedente;
            edtCedenteBairro.EditValue = _carteiraBoleto.BairroCedente;
            edtCedenteCidade.EditValue = _carteiraBoleto.CidadeCedente;
            edtCedenteUF.EditValue = _carteiraBoleto.UfCedente;
            edtCedenteCEP.EditValue = _carteiraBoleto.CepCedente;
            edtValorJuro.EditValue = _carteiraBoleto.ValorJuros;
            edtValorMulta.EditValue = _carteiraBoleto.ValorMulta;
            edtInstrucao1.EditValue = _carteiraBoleto.Instrucao1;
            edtInstrucao2.EditValue = _carteiraBoleto.Instrucao2;
            edtInstrucao3.EditValue = _carteiraBoleto.Instrucao3;
            edtInstrucao4.EditValue = _carteiraBoleto.Instrucao4;
            edtInstrucao5.EditValue = _carteiraBoleto.Instrucao5;
            edtInstrucao6.EditValue = _carteiraBoleto.Instrucao6;
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Validar()) return;
                SetCampos();

                var connection = new ConnectionSqlite();
                connection.SalvarCarteiraBoleto(_carteiraBoleto);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void SetCampos()
        {
            var bancoSelecionado = GetBancoSelecionado();
            if (_carteiraBoleto == null) _carteiraBoleto = new CarteiraBoleto();

            _carteiraBoleto.CodigoBanco = bancoSelecionado?.Codigo;
            _carteiraBoleto.NomeBanco = bancoSelecionado?.Descricao;
            _carteiraBoleto.NumeroAgencia = edtNumeroAgencia.EditValue?.ToString();
            _carteiraBoleto.DigitoAgencia = edtDigitoAgencia.EditValue?.ToString();
            _carteiraBoleto.NumeroConta = edtNumeroConta.EditValue?.ToString();
            _carteiraBoleto.DigitoConta = edtDigitoConta.EditValue?.ToString();
            _carteiraBoleto.DescricaoCarteira = edtDescricao.EditValue?.ToString();
            _carteiraBoleto.NumeroCarteira = edtNumeroCarteira.EditValue?.ToString();
            _carteiraBoleto.ModeloBoleto = cbbModeloBoleto.EditValue?.ToString();
            _carteiraBoleto.TipoArquivoRetorno = cbbTipoArquivoRetorno.EditValue?.ToString();
            _carteiraBoleto.TipoArquivoRemessa = cbbTipoArquivoRemessa.EditValue?.ToString();
            _carteiraBoleto.CodigoCedente = edtCodigoCedente.EditValue?.ToString();
            _carteiraBoleto.DigitoCodigoCedente = edtDigitoCedente.EditValue?.ToString();
            _carteiraBoleto.NumeroConvenio = edtConvenio.EditValue?.ToString();
            _carteiraBoleto.CodigoTransmissao = edtCodigoTransmissao.EditValue?.ToString();
            _carteiraBoleto.BancoGeraBoleto = edtBancoGeraBoletos.Checked;
            _carteiraBoleto.NomeCedente = edtCedenteNome.EditValue?.ToString();
            _carteiraBoleto.CpfCnpjCedente = edtCedenteCpfCnpj.EditValue?.ToString();
            _carteiraBoleto.EnderecoCedente = edtCedenteEndereco.EditValue?.ToString();
            _carteiraBoleto.ComplementoCedente = edtCedenteComplemento.EditValue?.ToString();
            _carteiraBoleto.NumeroCedente = edtCedenteNumero.EditValue?.ToString();
            _carteiraBoleto.BairroCedente = edtCedenteBairro.EditValue?.ToString();
            _carteiraBoleto.CidadeCedente = edtCedenteCidade.EditValue?.ToString();
            _carteiraBoleto.UfCedente = edtCedenteUF.EditValue?.ToString();
            _carteiraBoleto.CepCedente = edtCedenteCEP.EditValue?.ToString();
            _carteiraBoleto.ValorJuros = edtValorJuro.EditValue == null ? 0 : Convert.ToDecimal(edtValorJuro.EditValue);
            _carteiraBoleto.ValorMulta = edtValorMulta.EditValue == null
                ? 0
                : Convert.ToDecimal(edtValorMulta.EditValue);
            _carteiraBoleto.Instrucao1 = edtInstrucao1.EditValue?.ToString();
            _carteiraBoleto.Instrucao2 = edtInstrucao2.EditValue?.ToString();
            _carteiraBoleto.Instrucao3 = edtInstrucao3.EditValue?.ToString();
            _carteiraBoleto.Instrucao4 = edtInstrucao4.EditValue?.ToString();
            _carteiraBoleto.Instrucao5 = edtInstrucao5.EditValue?.ToString();
            _carteiraBoleto.Instrucao6 = edtInstrucao6.EditValue?.ToString();
        }

        private bool Validar()
        {
            var bancoSelecionado = GetBancoSelecionado();
            if (bancoSelecionado == null)
            {
                MessageBox.Show("Informe o banco.");
                return false;
            }
            if (edtNumeroAgencia.EditValue == null)
            {
                MessageBox.Show("Informe a agência.");
                return false;
            }
            if (edtDigitoAgencia.EditValue == null)
            {
                MessageBox.Show("Informe o dígito da agência.");
                return false;
            }
            if (edtNumeroConta.EditValue == null)
            {
                MessageBox.Show("Informe o número da conta.");
                return false;
            }
            if (edtDigitoConta.EditValue == null)
            {
                MessageBox.Show("Informe o dígito da conta.");
                return false;
            }
            if (edtDescricao.EditValue == null)
            {
                MessageBox.Show("Informe a descrição para a carteira.");
                return false;
            }
            if (edtNumeroCarteira.EditValue == null)
            {
                MessageBox.Show("Informe o número da carteira.");
                return false;
            }

            return true;
        }

        private void cbbBanco_EditValueChanged(object sender, EventArgs e)
        {
            layoutControlItemCodigoCedente.Visibility = LayoutVisibility.Never;
            layoutControlItemDigitoCedente.Visibility = LayoutVisibility.Never;
            layoutControlItemConvenio.Visibility = LayoutVisibility.Never;
            layoutControlItemCodigoTranmissao.Visibility = LayoutVisibility.Never;
            
            var bancoSelecionado = GetBancoSelecionado();

            switch (bancoSelecionado?.Codigo)
            {
                case "001":
                    layoutControlItemConvenio.Visibility = LayoutVisibility.Always;
                    break;
                case "003":
                    layoutControlItemConvenio.Visibility = LayoutVisibility.Always;
                    break;
                case "033":
                    layoutControlItemCodigoCedente.Visibility = LayoutVisibility.Always;
                    layoutControlItemConvenio.Visibility = LayoutVisibility.Always;
                    layoutControlItemCodigoTranmissao.Visibility = LayoutVisibility.Always;
                    break;
                case "070":
                    break;
                case "104":
                    layoutControlItemCodigoCedente.Visibility = LayoutVisibility.Always;
                    layoutControlItemDigitoCedente.Visibility = LayoutVisibility.Always;
                    break;
                case "237":
                    layoutControlItemCodigoCedente.Visibility = LayoutVisibility.Always;
                    layoutControlItemConvenio.Visibility = LayoutVisibility.Always;
                    break;
                case "341":
                    layoutControlItemCodigoCedente.Visibility = LayoutVisibility.Always;
                    layoutControlItemDigitoCedente.Visibility = LayoutVisibility.Always;
                    break;
                case "399":
                    layoutControlItemCodigoCedente.Visibility = LayoutVisibility.Always;
                    layoutControlItemDigitoCedente.Visibility = LayoutVisibility.Always;
                    break;
                case "756":
                    layoutControlItemCodigoCedente.Visibility = LayoutVisibility.Always;
                    layoutControlItemDigitoCedente.Visibility = LayoutVisibility.Always;
                    break;
            }
        }

        private Banco GetBancoSelecionado()
        {
            var bancos = cbbBanco.Properties.DataSource as List<Banco>;
            return bancos?.FirstOrDefault(x => x.Codigo == cbbBanco.EditValue?.ToString());
        }
    }
}