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
	- [x] Classes base
	- [x] Common methods
- <b>Bancos Brasileiros</b>
	- <b>001-9 | Banco do Brasil</b>
		- Implementação / Testes
			- Carteiras
				- [x] Carteira 11 		(*Envio de remessa homologado com o banco em: 27-08-2015*)
				- [x] Carteira 11-019 	(*Homologado com o Banco - 02/12/2015 by @kalleyaoliveira*)
				- [x] Carteira 16
				- [x] Carteira 17		(*Homologado com o Banco - 03/02/2016 by @kalleyaoliveira*)
				- [x] Carteira 17-019 	(*Homologado com o Banco - 03/02/2016 by @kalleyaoliveira*)
				- [x] Carteira 17-035 	(*Homologado com o Banco - 03/02/2016 by @kalleyaoliveira*)
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
				- [x] 101 - Cobrança Simples COM Registro	(*Homologado com o Banco - 04/03/2016 by @kalleyaoliveira*)
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
				- [x] Carteira 02
				- [ ] Carteira 03
				- [x] Carteira 04 (*Implementado em 07/07/2017 by @kalleyaoliveira*)
				- [x] Carteira 06 - Sem Registro
				- [x] Carteira 09 (*Homologado com Banco - 02/12/2015 by @kalleyaoliveira**)
				- [ ] Carteira 19
	- <b>341-7 | Banco Itaú</b>
		- Implementação / Testes 
			- Carteira Direta
				- [ ] 108 - Direta Eletrônica (Carnê) 
			- Carteira Escritural
				- [ ] 104 - Escritural Eletrônica (Carnê)
				- [x] 112 - Escritural Eletrônica (Simples) (*Homologado com Banco - 30/06/2016 by @kalleyaoliveira**)
				- [ ] 138 - Escritural Eletrônica (Mensagem Colorida)
				- [ ] 147 - Escritural Eletrônica (Dólar)
			- Carteira Sem Registro
				- [x] 103 - Sem Registro (Carnê)
				- [x] 173 - Sem Registro
				- [ ] 196 - Sem Registro (15 Posições)
			- Carteira COM Registro
				- [x] 109 - Com Registro (*Homologado com Banco - 02/12/2015 by @kalleyaoliveira**)
	- <b>399-9 | Banco Hsbc</b>
		- Implementação / Testes
			- Carteiras
				- [x] CNR - Cobrança Não Registrada
				- [x] CSB - Cobrança Registrada (*Homologado com Banco - 03/06/2016 by @kalleyaoliveira**)
	- <b>070-1 | Banco BRB</b>
		- Implementação / Testes
			- Carteiras
				- [x] TIPO 1 - COBRANÇA DIRETA SEM REGISTRO				
				- [x] TIPO 2 - COBRANÇA DIRETA COM REGISTRO
	- <b>756-0 | Banco Cooperativo do Brasil - SICOOB</b>
		- Implementação / Testes
			- Carteiras
				- [x] 1/01 - Simples com Registro (*Homologado com o Banco - 04/08/2016 by @kalleyaoliveira**)
	- <b>422-7 | Banco Safra</b>
		- Implementação / Testes
			- Carteiras
				- [x] 1 - COBRANÇA SIMPLES (*Em homologação com o Banco - by @kalleyaoliveira**)
