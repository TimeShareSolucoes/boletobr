using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.BarCode;
using DevExpress.XtraReports.UI;

namespace BoletoBr.View.XtraReport
{
    public enum ModeloBoleto
    {
        Carne,
        Fatura,
        Normal,
        FaturaCarta,
        CarneA5
    }

    /// <summary>
    /// Objetivos:
    /// Identificar o banco e obter a logo do banco
    /// Receber uma lista de boletos
    /// Renderizar os boletos.
    /// </summary>
    public class VisualizadorBoleto
    {
        private readonly List<BoletoBr.Boleto> _boletos;
        private readonly ModeloBoleto _modeloBoleto;

        public VisualizadorBoleto(ModeloBoleto modeloBoleto, List<Boleto> boletos)
        {
            _boletos = boletos;
            _modeloBoleto = modeloBoleto;
        }

        public DevExpress.XtraReports.UI.XtraReport AtualizarLogoDoBancoNoModelo()
        {
            if (_boletos == null)
                throw new Exception("Não há boletos para exibir.");

            /* Atualiza logotipos dos bancos, na lista de boletos */
            foreach (var boletoAtualizarLogo in _boletos)
            {
                var imagemLogoBanco = BancoDeLogos.ObterLogoBanco(boletoAtualizarLogo.BancoBoleto.CodigoBanco);
                boletoAtualizarLogo.BancoBoleto.LogotipoBancoParaExibicao = imagemLogoBanco;
            }

            var rpt = CarregaModeloBoleto(_modeloBoleto);

            rpt.DataSource = _boletos;

            return rpt;
        }

        public void ExibirNaTela()
        {
            var rpt = AtualizarLogoDoBancoNoModelo();

            var pt = new ReportPrintTool(rpt);

            pt.AutoShowParametersPanel = true;
            pt.ShowRibbonPreviewDialog();
        }

        public void ExportarEmPdf(string path)
        {
            var rpt = AtualizarLogoDoBancoNoModelo();

            rpt.DataSource = _boletos;
            rpt.ExportToPdf(path);
        }

        public MemoryStream GerarPdfEmMemoria()
        {
            MemoryStream mem = new MemoryStream();
            var rpt = AtualizarLogoDoBancoNoModelo();

            rpt.ExportToPdf(mem);

            return mem;
        }

        public DevExpress.XtraReports.UI.XtraReport CarregaModeloBoleto(ModeloBoleto mb)
        {
            DevExpress.XtraReports.UI.XtraReport rptXtraReport;

            switch (mb)
            {
                case ModeloBoleto.Carne:
                    rptXtraReport = new CarneBoletoGenericoRpt();
                    break;
                case ModeloBoleto.Fatura:
                    rptXtraReport = new BoletoGenericoRpt();
                    break;
                case ModeloBoleto.Normal:
                    rptXtraReport = new BoletoGenericoRpt();
                    break;
                case ModeloBoleto.FaturaCarta:
                    rptXtraReport = new BoletoFaturaCarta();
                    break;
                case ModeloBoleto.CarneA5:
                    rptXtraReport = new CarneBoletoA5Rpt();
                    break;
                default:
                    throw new Exception("O modelo utilizado não foi reconhecido.");
            }

            return rptXtraReport;
        }
    }
}


