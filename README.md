BoletoBr
==========

Geração de Boletos, Remessa, Leitura de Retorno para Bancos Brasileiros, no padrão CNAB 240 e CNAB 400.

Direcionado para .NET 4.5

Me inspirei na biblioteca: https://boletonet.codeplex.com/

## Diferenças da biblioteca
- Eliminei dependências de VB.NET (Microsoft.VisualBasic) para utilização da função Strings.Mid no cálculo do módulo 11.
- Renderização de boletos em projetos separados, eliminando dependências. Ex: Aplicativo Windows Forms utilizando biblioteca System.Web.

## Tarefas/Implementações

- <b>Arquitetura</b>
	- [ ] Classes base
	- [ ] Common methods
- <b>Bancos Brasileiros</b>
	- <b>001-9 | Banco do Brasil</b>
		- Implementação / Testes
			- Carteiras
				- [x] Carteira 11 
				- [x] Carteira 16
				- [ ] Carteira 17
				- [ ] Carteira 17-019
				- [ ] Carteira 18
				- [ ] Carteira 18-019
				- [ ] Carteira 18-027
				- [ ] Carteira 18-035
				- [ ] Carteira 18-140
				- [ ] Carteira 31
	- <b>003-5 | Banco da Amazônia</b>
		- Implementação / Testes
			- Carteiras
				- [x] CNR - Cobrança Não Registrada
	- <b>033-7 | Banco Santander</b>
		- Implementação / Testes
			- Carteiras
				- [x] 101 - Cobrança Simples COM Registro
				- [x] 102 - Cobrança Simples SEM Registro
				- [ ] 201 - Penhor Rápida COM Registro
	- <b>104-0 | Caixa Econômica Federal</b>
		- Implementação / Testes 
			- Carteiras
				- [x] RG - Carteira Registrada
				- [x] SR - Carteira Sem Registro
	- <b>237-2 | Banco Bradesco</b>
		- Implementação / Testes
			- Carteiras
				- [ ] Carteira 02
				- [ ] Carteira 03
				- [x] Carteira 06 - Sem Registro
				- [ ] Carteira 09
				- [ ] Carteira 19
	- <b>341-7 | Banco Itaú</b>
		- Implementação / Testes 
			- Carteira Direta
				- [ ] 108 - Direta Eletrônica (Carnê) 
			- Carteira Escritural
				- [ ] 104 - Escritural Eletrônica (Carnê)
				- [ ] 112 - Escritural Eletrônica (Simples)
				- [ ] 138 - Escritural Eletrônica (Mensagem Colorida)
				- [ ] 147 - Escritural Eletrônica (Dólar)
			- Carteira Sem Registro
				- [x] 103 - Sem Registro (Carnê)
				- [x] 173 - Sem Registro
				- [ ] 196 - Sem Registro (15 Posições)
	- <b>399-9 | Banco Hsbc</b>
		- Implementação / Testes
			- Carteiras
				- [x] CNR - Cobrança Não Registrada
				- [ ] CSB - Cobrança Registrada
	- <b>070-1 | Banco BRB</b>
		- Implementação / Testes
			- Carteiras
				- [x] TIPO 1 - COBRANÇA DIRETA SEM REGISTRO				
				- [x] TIPO 2 - COBRANÇA DIRETA COM REGISTRO
