using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace ExemploDLLCSharp
{

    public partial class FormPrincipal : Form
    {
        public enum enTipo
        {
            CREDITO = 1,
            CREDITO_A_VISTA = 2,
            CREDITO_PARC_LOJA = 3,
            CREDITO_PAR_ADM = 4,
            DEBITO = 5,
            ADM = 6,
            CANCELAR = 7,
            ATV = 8,
            RELATORIO = 9,
            SOLICITAR_CPF = 10,
            LINK_PAGO = 21,
            PARCELE_MAIS = 22,
            SPLIT_PGTO = 23,
            REIMPRESSAO = 24,
            REIMPRESSAODIRETO = 25,
            CONFIRMA = 26,
            DEBITO_A_VISTA = 27,
            VENDE_CARTEIRA_DIGITAL_PIX = 28,
            LISTAR_LINK_PAGO = 29,
            MANUTENCAO_LINK_PAGO = 30

        }

        public string sCNPJCliente;
        public string sCNPJParceiro;
        string sTelefone = "";
        string sTexto = "";
        string sData = "";
        string sControle = "";
        string sRetorno = string.Empty;
        string sItens = "";


        double dValor = 0;
        int iCupom = 0;
        int iParcelas = 0;
        int iQtdeItens;

        #region Importação DLL

        [DllImport("TefClientmc.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        static extern string VendeCredito(string sCNPJCliente, string sCNPJParceiro, double dValor, int iNumeroCupom, int iLeitor);


        [DllImport("TefClientmc.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        static extern string VendeCreditoVista(string sCNPJCliente, string sCNPJParceiro, double dValor, int iNumeroCupom, int iLeitor);

        [DllImport("TefClientmc.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        static extern string VendeCreditoParcLoja(string sCNPJCliente, string sCNPJParceiro, int iParcelas, double dValor, int iNumeroCupom, int iLeitor);

        [DllImport("TefClientmc.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        static extern string VendeCreditoParcAdm(string sCNPJCliente, string sCNPJParceiro, int iParcelas, double dValor, int iNumeroCupom, int iLeitor);

        [DllImport("TefClientmc.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        static extern string VendeDebito(string sCNPJCliente, string sCNPJParceiro, double dValor, int iNumeroCupom, int iLeitor);

        [DllImport("TefClientmc.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        static extern string VendeDebitoAVista(string sCNPJCliente, string sCNPJParceiro, double dValor, int iNumeroCupom, int iLeitor);

        [DllImport("TefClientmc.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        static extern string Confirmar(string sCNPJCliente, string sCNPJParceiro, int iNumeroCupom);

        [DllImport("TefClientmc.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        static extern string Cancelar(string sCNPJCliente, string sCNPJParceiro, double dValor, int iCupom, string sControle, int iLeitor);

        [DllImport("TefClientmc.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        static extern string Desfazimento(string sCNPJCliente, string sCNPJParceiro, int iNumeroCupom);

        [DllImport("TefClientmc.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        static extern string VendeCarteiraDigitalPix(string sCNPJCliente, string sCNPJParceiro, double dValor, int iNumeroCupom, int iLeitor);

        [DllImport("TefClientmc.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        static extern string LinkPagamento(string sCNPJCliente, string sCNPJParceiro, int Parcelas, double dValor,
            int iNumeroCupom, int iQtdeItens, string sItens, string sTelefone, string sTexto, int iLeitor);

        [DllImport("TefClientmc.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        static extern string ListarLinkPagamentoPago(string sCNPJCliente, string sCNPJParceiro, double dValor, int iNumeroCupom, int iLeitor);

        [DllImport("TefClientmc.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        static extern string ManutencaoLinkPagamento(string sCNPJCliente, string sCNPJParceiro, int iLeitor);

        [DllImport("TefClientmc.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        static extern string Adm(string sCNPJCliente, string sCNPJParceiro, double dValor, int iNumeroCupom, int iLeitor);

        [DllImport("TefClientmc.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        static extern string Atv(string sCNPJCliente, string sCNPJParceiro, int iNumeroCupom, int iLeitor);

        [DllImport("TefClientmc.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        static extern string SolicitarCPF(string sCNPJCliente, string sCNPJParceiro, int iNumeroCupom);

        [DllImport("TefClientmc.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        static extern string Reimpressao(string sCNPJCliente, string sCNPJParceiro, double dValor, int iNumeroCupom, int iLeitor);

        [DllImport("TefClientmc.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        static extern string ReimpressaoDireto(string sCNPJCliente, string sCNPJParceiro, string sNSU, string sData, int iNumeroCupom, int iLeitor);

        [DllImport("TefClientmc.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        static extern string Relatorio(string sCNPJCliente, string sCNPJParceiro, int iNumeroCupom, int iLeitor);

        [DllImport("TefClientmc.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        static extern string ParceleMais(string sCNPJCliente, string sCNPJParceiro, double dValor, int iNumeroCupom, int iLeitor);

        #endregion Importação DLL

        public FormPrincipal()
        {
            InitializeComponent();
            textData.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void Transacionar(enTipo tipo)
        {
            try
            {
                List<string> vLinhas = new List<string>();
                List<string> vTipos = new List<string>();

                sRetorno = string.Empty;
                sCNPJCliente = txbCnpj.Text.Trim();
                sCNPJParceiro = txbCnpjParceiro.Text.Trim();
                sTelefone = textTelefone.Text.Trim();
                sTexto = textTexto.Text.Trim();
                sData = textData.Text.Trim();
                sControle = txbControle.Text.Trim();

                int auxQtde = 0;

                Int32.TryParse(tbQtdeItens.Text, out auxQtde);

                iQtdeItens = Convert.ToInt32(auxQtde);
                sItens = tbItens.Text;

                string sRetornoTransacao = string.Empty;
                string sMensagemTransacao = string.Empty;
                string sComprovanteTransacao = string.Empty;

                double.TryParse(txbValor.Text, out dValor);
                int.TryParse(txbCupom.Text, out iCupom);
                int.TryParse(txbParcelas.Text, out iParcelas);


                if (string.IsNullOrWhiteSpace(sCNPJCliente) || string.IsNullOrWhiteSpace(sCNPJParceiro))
                {
                    MessageBox.Show("Os campos de CNPJ devem ser preenchidos");
                    return;
                }

                switch (tipo)
                {
                    case enTipo.CREDITO:
                        if (string.IsNullOrWhiteSpace(sCNPJCliente) ||
                            string.IsNullOrWhiteSpace(sCNPJParceiro) ||
                            dValor == 0 ||
                            iCupom == 0)
                        {
                            MessageBox.Show("Verifique os campos solicitados.");
                            return;
                        }
                        sRetorno = VendeCredito(sCNPJCliente, sCNPJParceiro, dValor, iCupom, 0);
                        break;

                    case enTipo.CREDITO_A_VISTA:
                        if (string.IsNullOrWhiteSpace(sCNPJCliente) ||
                            string.IsNullOrWhiteSpace(sCNPJParceiro) ||
                            dValor == 0 ||
                            iCupom == 0)
                        {
                            MessageBox.Show("Verifique os campos solicitados.");
                            return;
                        }
                        sRetorno = VendeCreditoVista(sCNPJCliente, sCNPJParceiro, dValor, iCupom, 0);
                        break;

                    case enTipo.CONFIRMA:
                        if (string.IsNullOrWhiteSpace(sCNPJCliente) ||
                            string.IsNullOrWhiteSpace(sCNPJParceiro) ||
                            iCupom == 0)
                        {
                            MessageBox.Show("Verifique os campos solicitados.");
                            return;
                        }
                        sRetorno = Confirmar(sCNPJCliente, sCNPJParceiro, iCupom);
                        break;

                    case enTipo.CANCELAR:
                        if (string.IsNullOrWhiteSpace(sCNPJCliente) ||
                            string.IsNullOrWhiteSpace(sCNPJParceiro) ||
                            string.IsNullOrWhiteSpace(sControle) ||
                            dValor == 0 ||
                            iCupom == 0)
                        {
                            MessageBox.Show("Verifique os campos solicitados.");
                            return;
                        }
                        sRetorno = Cancelar(sCNPJCliente, sCNPJParceiro, dValor, iCupom, sControle, 0);
                        break;

                    case enTipo.CREDITO_PARC_LOJA:
                        if (string.IsNullOrWhiteSpace(sCNPJCliente) ||
                            string.IsNullOrWhiteSpace(sCNPJParceiro) ||
                            iParcelas == 0 ||
                            dValor == 0 ||
                            iCupom == 0)
                        {
                            MessageBox.Show("Verifique os campos solicitados.");
                            return;
                        }
                        sRetorno = VendeCreditoParcLoja(sCNPJCliente, sCNPJParceiro, iParcelas, dValor, iCupom, 0);
                        break;

                    case enTipo.CREDITO_PAR_ADM:
                        if (string.IsNullOrWhiteSpace(sCNPJCliente) ||
                            string.IsNullOrWhiteSpace(sCNPJParceiro) ||
                            iParcelas == 0 ||
                            dValor == 0 ||
                            iCupom == 0)
                        {
                            MessageBox.Show("Verifique os campos solicitados.");
                            return;
                        }
                        sRetorno = VendeCreditoParcAdm(sCNPJCliente, sCNPJParceiro, iParcelas, dValor, iCupom, 0);
                        break;

                    case enTipo.DEBITO:
                        if (string.IsNullOrWhiteSpace(sCNPJCliente) ||
                            string.IsNullOrWhiteSpace(sCNPJParceiro) ||
                            dValor == 0 ||
                            iCupom == 0)
                        {
                            MessageBox.Show("Verifique os campos solicitados.");
                            return;
                        }
                        sRetorno = VendeDebito(sCNPJCliente, sCNPJParceiro, dValor, iCupom, 0);
                        break;

                    case enTipo.DEBITO_A_VISTA:
                        if (string.IsNullOrWhiteSpace(sCNPJCliente) ||
                            string.IsNullOrWhiteSpace(sCNPJParceiro) ||
                            dValor == 0 ||
                            iCupom == 0)
                        {
                            MessageBox.Show("Verifique os campos solicitados.");
                            return;
                        }
                        sRetorno = VendeDebitoAVista(sCNPJCliente, sCNPJParceiro, dValor, iCupom, 0);
                        break;

                    case enTipo.VENDE_CARTEIRA_DIGITAL_PIX:
                        if (string.IsNullOrWhiteSpace(sCNPJCliente) ||
                            string.IsNullOrWhiteSpace(sCNPJParceiro) ||
                            dValor == 0 ||
                            iCupom == 0)
                        {
                            MessageBox.Show("Verifique os campos solicitados.");
                            return;
                        }
                        sRetorno = VendeCarteiraDigitalPix(sCNPJCliente, sCNPJParceiro, dValor, iCupom, 0);
                        break;

                    case enTipo.LINK_PAGO:
                        if (string.IsNullOrWhiteSpace(sCNPJCliente) ||
                              string.IsNullOrWhiteSpace(sCNPJParceiro) ||
                              string.IsNullOrWhiteSpace(sTelefone) ||
                              string.IsNullOrWhiteSpace(sTexto) ||
                              iParcelas == 0 ||
                              dValor == 0 ||
                              iCupom == 0)
                        {
                            MessageBox.Show("Verifique os campos solicitados.");
                            return;
                        }
                        sRetorno = LinkPagamento(sCNPJCliente, sCNPJParceiro, iParcelas, dValor, iCupom, iQtdeItens, sItens, sTelefone, sTexto, 0);
                        break;

                    case enTipo.LISTAR_LINK_PAGO:
                        if (string.IsNullOrWhiteSpace(sCNPJCliente) ||
                           string.IsNullOrWhiteSpace(sCNPJParceiro) ||
                           dValor == 0 ||
                           iCupom == 0)
                        {
                            MessageBox.Show("Verifique os campos solicitados.");
                            return;
                        }
                        sRetorno = ListarLinkPagamentoPago(sCNPJCliente, sCNPJParceiro, dValor, iCupom, 0);
                        break;

                    case enTipo.MANUTENCAO_LINK_PAGO:
                        if (string.IsNullOrWhiteSpace(sCNPJCliente) ||
                           string.IsNullOrWhiteSpace(sCNPJParceiro))
                        {
                            MessageBox.Show("Verifique os campos solicitados.");
                            return;
                        }
                        sRetorno = ManutencaoLinkPagamento(sCNPJCliente, sCNPJParceiro, 0);
                        break;

                    case enTipo.ADM:
                        if (string.IsNullOrWhiteSpace(sCNPJCliente) ||
                           string.IsNullOrWhiteSpace(sCNPJParceiro) ||
                           dValor == 0 ||
                           iCupom == 0)
                        {
                            MessageBox.Show("Verifique os campos solicitados.");
                            return;
                        }
                        sRetorno = Adm(sCNPJCliente, sCNPJParceiro, dValor, iCupom, 0);
                        break;

                    case enTipo.ATV:
                        if (string.IsNullOrWhiteSpace(sCNPJCliente) ||
                           string.IsNullOrWhiteSpace(sCNPJParceiro) ||
                           iCupom == 0)
                        {
                            MessageBox.Show("Verifique os campos solicitados.");
                            return;
                        }
                        sRetorno = Atv(sCNPJCliente, sCNPJParceiro, iCupom, 0);
                        break;

                    case enTipo.SOLICITAR_CPF:
                        if (string.IsNullOrWhiteSpace(sCNPJCliente) ||
                           string.IsNullOrWhiteSpace(sCNPJParceiro) ||
                           iCupom == 0)
                        {
                            MessageBox.Show("Verifique os campos solicitados.");
                            return;
                        }
                        sRetorno = SolicitarCPF(sCNPJCliente, sCNPJParceiro, iCupom);
                        break;

                    case enTipo.REIMPRESSAO:
                        if (string.IsNullOrWhiteSpace(sCNPJCliente) ||
                           string.IsNullOrWhiteSpace(sCNPJParceiro) ||
                           dValor == 0 ||
                           iCupom == 0)
                        {
                            MessageBox.Show("Verifique os campos solicitados.");
                            return;
                        }
                        sRetorno = Reimpressao(sCNPJCliente, sCNPJParceiro, dValor, iCupom, 0);
                        break;

                    case enTipo.REIMPRESSAODIRETO:
                        if (string.IsNullOrWhiteSpace(sCNPJCliente) ||
                           string.IsNullOrWhiteSpace(sCNPJParceiro) ||
                           string.IsNullOrWhiteSpace(sControle) ||
                           string.IsNullOrWhiteSpace(sData) ||
                           iCupom == 0)
                        {
                            MessageBox.Show("Verifique os campos solicitados.");
                            return;
                        }
                        sRetorno = ReimpressaoDireto(sCNPJCliente, sCNPJParceiro, sControle, sData, iCupom, 0);
                        break;

                    case enTipo.RELATORIO:
                        if (string.IsNullOrWhiteSpace(sCNPJCliente) ||
                           string.IsNullOrWhiteSpace(sCNPJParceiro) ||
                           iCupom == 0)
                        {
                            MessageBox.Show("Verifique os campos solicitados.");
                            return;
                        }
                        sRetorno = Relatorio(sCNPJCliente, sCNPJParceiro, iCupom, 0);
                        break;

                    case enTipo.PARCELE_MAIS:
                        if (string.IsNullOrWhiteSpace(sCNPJCliente) ||
                           string.IsNullOrWhiteSpace(sCNPJParceiro) ||
                           dValor == 0 ||
                           iCupom == 0)
                        {
                            MessageBox.Show("Verifique os campos solicitados.");
                            return;
                        }
                        sRetorno = ParceleMais(sCNPJCliente, sCNPJParceiro, dValor, iCupom, 0);
                        break;
                }

                if (string.IsNullOrWhiteSpace(sRetorno))
                    return;

                // RETORNO
                sRetorno = sRetorno.Replace(Environment.NewLine, "¬");
                vLinhas = sRetorno.Split('¬').ToList();
                foreach (var sLinha in vLinhas)
                {
                    vTipos = sLinha.Split(';').ToList();
                    if (vTipos[0] == "S")
                    {
                        sRetornoTransacao = vTipos[1];
                        sMensagemTransacao = vTipos[2];

                        if (sRetornoTransacao == "0")
                        {
                            MessageBox.Show(sMensagemTransacao);
                            return;
                        }
                    }
                    else
                    {
                        if (vTipos[0] == "I")
                            sComprovanteTransacao = sComprovanteTransacao + vTipos[1] + Environment.NewLine;
                    }
                }

                MessageBox.Show(sMensagemTransacao + Environment.NewLine + sComprovanteTransacao);
                sRetorno = Confirmar(sCNPJCliente, sCNPJParceiro, 12345);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        #region Funções 
        private void Credito_Click(object sender, EventArgs e)
        {
            Transacionar(enTipo.CREDITO_A_VISTA);
        }
        private void CreditoParcelamentoLoja_Click(object sender, EventArgs e)
        {
            Transacionar(enTipo.CREDITO_PARC_LOJA);
        }
        private void CreditoParcelamentoADM_Click(object sender, EventArgs e)
        {
            Transacionar(enTipo.CREDITO_PAR_ADM);
        }
        private void VendeDebito_Click(object sender, EventArgs e)
        {
            Transacionar(enTipo.DEBITO);
        }
        private void VendeDebitoAVista_Click(object sender, EventArgs e)
        {
            Transacionar(enTipo.DEBITO_A_VISTA);
        }

        private void CancelarDebito_Click(object sender, EventArgs e)
        {
            Transacionar(enTipo.CANCELAR);
        }
        private void VendeCarteiraDigitalPIX_Click(object sender, EventArgs e)
        {
            Transacionar(enTipo.VENDE_CARTEIRA_DIGITAL_PIX);
        }
        private void LinkPagamento_Click(object sender, EventArgs e)
        {
            Transacionar(enTipo.LINK_PAGO);
        }
        private void ListarLinkPagamento_Click(object sender, EventArgs e)
        {
            Transacionar(enTipo.LISTAR_LINK_PAGO);
        }
        private void ADM_Click(object sender, EventArgs e)
        {
            Transacionar(enTipo.ADM);
        }
        private void ATV_Click(object sender, EventArgs e)
        {
            Transacionar(enTipo.ATV);
        }
        private void SolicitarCPF_Click(object sender, EventArgs e)
        {
            Transacionar(enTipo.SOLICITAR_CPF);
        }
        private void Reimpressao_Click(object sender, EventArgs e)
        {
            Transacionar(enTipo.REIMPRESSAO);
        }
        private void ReimpressaoDireta_Click(object sender, EventArgs e)
        {
            Transacionar(enTipo.REIMPRESSAODIRETO);
        }
        private void Relatorio_Click(object sender, EventArgs e)
        {
            Transacionar(enTipo.RELATORIO);
        }
        private void ParceleMais_Click(object sender, EventArgs e)
        {
            Transacionar(enTipo.PARCELE_MAIS);
        }
        private void CANCELARPIX_Click(object sender, EventArgs e)
        {
            Transacionar(enTipo.CANCELAR);
        }
        private void btnManutLink_Click(object sender, EventArgs e)
        {
            Transacionar(enTipo.MANUTENCAO_LINK_PAGO);
        }
        #endregion Funções
    }

}
