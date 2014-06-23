BoletoBr
==========

Geração de Boleto e Remessa, Leitura de Retorno para Bancos Brasileiros, padrão CNAB 240 e 400.

Direcionado para .NET 4.5

Me inspirei na biblioteca: https://boletonet.codeplex.com/

## Diferenças da biblioteca:
* Eliminei dependências de VB.NET (Microsoft.VisualBasic) para utilização da função Strings.Mid no cálculo do módulo 11.
* Renderização de boletos em projetos separados, eliminando dependências. Ex: Aplicativo windows forms utilizando biblioteca System.Web.

Tarefas/Implementações:
* Arquitetura
	- [ ] Classes base
	- [ ] Common methods
* Bancos Brasileiros
	- [ ] Banco Itaú
	- [ ] Banco Hsbc
		- [ ] Testes
			- Carteira CNR
				- [x] Tipo Identificador 4 (boletos com data de vencimento)
				- [ ] Tipo Identificador 5 (boletos sem data de vencimento)
			- Carteira CSB (Em Andamento)
