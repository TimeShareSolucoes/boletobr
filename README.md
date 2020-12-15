BoletoBr
==========

Geração de Boletos, Remessa, Leitura de Retorno para Bancos Brasileiros, no padrão CNAB 240 e CNAB 400.

Direcionado para .NET 4.5

Me inspirei na biblioteca: https://boletonet.codeplex.com/

## Diferenças da biblioteca
- Eliminei dependências de VB.NET (Microsoft.VisualBasic) para utilização da função Strings.Mid no cálculo do módulo 11.
- Renderização de boletos em projetos separados, eliminando dependências. Ex: Aplicativo Windows Forms utilizando biblioteca System.Web.

## Como utilizar

> Consulte nosso [Wiki](https://github.com/TimeShareSolucoes/boletobr/wiki)

## Compatibilidade

- <b>Bancos Brasileiros</b>
	- <b>001-9 | Banco do Brasil - (CNAB 400)</b>
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
	- <b>003-5 | Banco da Amazônia - (CNAB 400)</b>
		- Implementação / Testes
			- Carteiras
				- [x] CNR - Cobrança Não Registrada
	- <b>033-7 | Banco Santander - (CNAB 400)</b>
		- Implementação / Testes
			- Carteiras
				- [x] Carteira 101 - Banco Emite - cod. 1 - Eletrônica com registro
                - [x] Carteira 101 - Beneficiário Emite - cod. 5 - Rápida com registro
                - [x] Carteira 201 - Banco Emite - cod. 3 - Caucionada eletrônica
                - [x] Carteira 201 - Beneficiário Emite - cod. 6 - Caucionada rápida
                - [x] Carteira 102 - cod. 4 - Cobrança sem registro
                - [x] Carteira 104 - cod. 7 - Descontada eletrônica
	- <b>104-0 | Caixa Econômica Federal - (CNAB 240)</b>
		- Implementação / Testes 
			- Carteiras
				- [x] RG - Carteira Registrada 
				- [x] RG - Carteira Registrada com emissão pelo banco 
				- [x] SR - Carteira Sem Registro 
	- <b>237-2 | Banco Bradesco - (CNAB 400)</b>
		- Implementação / Testes
			- Carteiras
				- [x] Carteira 02
				- [x] Carteira 03
				- [x] Carteira 04 (*Implementado em 07/07/2017 by @kalleyaoliveira*)
				- [x] Carteira 06 - Sem Registro
				- [x] Carteira 09 (*Homologado com Banco - 02/12/2015 by @kalleyaoliveira**)
				- [x] Carteira 19
	- <b>341-7 | Banco Itaú - (CNAB 400)</b>
		- Implementação / Testes 
			- Carteira Direta
				- [ ] 108 - Direta Eletrônica (Carnê) 
			- Carteira Escritural
				- [ ] Carteira 104 - Escritural Eletrônica (Carnê)
				- [x] Carteira 112 - Escritural Eletrônica (Simples) (*Homologado com Banco - 30/06/2016 by @kalleyaoliveira**)
				- [ ] Carteira 138 - Escritural Eletrônica (Mensagem Colorida)
				- [ ] Carteira 147 - Escritural Eletrônica (Dólar)
			- Carteira Sem Registro
				- [x] 103 - Sem Registro (Carnê)
				- [x] 173 - Sem Registro
				- [ ] 196 - Sem Registro (15 Posições)
			- Carteira COM Registro
				- [x] 109 - Com Registro (*Homologado com Banco - 02/12/2015 by @kalleyaoliveira**)
	- <b>399-9 | Banco Hsbc - (CNAB 400)</b>
		- Implementação / Testes
			- Carteiras
				- [x] CNR - Cobrança Não Registrada
				- [x] CSB - Cobrança Registrada (*Homologado com Banco - 03/06/2016 by @kalleyaoliveira**)
	- <b>070-1 | Banco BRB - (CNAB 400)</b>
		- Implementação / Testes
			- Carteiras
				- [x] TIPO 1 - COBRANÇA DIRETA SEM REGISTRO				
				- [x] TIPO 2 - COBRANÇA DIRETA COM REGISTRO
	- <b>756-0 | Banco Cooperativo do Brasil - SICOOB</b>
		- Implementação / Testes
			- Carteiras
				- [x] 1/01 - CNAB 400 - Simples com Registro (*Homologado com o Banco - 04/08/2016 by @kalleyaoliveira**)
				- [x] 1/01 - CNAB 240 -Simples com Registro (*Homologado com o Banco - 20/03/2018 by @marcelodossantosaraujo**)
	- <b>422-7 | Banco (Solicitação de remoção do nome pelo Banco) - (CNAB 400)</b>
		- Implementação / Testes
			- Carteiras
				- [x] 1 - COBRANÇA SIMPLES (*Em homologação com o Banco - by @kalleyaoliveira**)
	- <b>707-2 | Banco Daycoval - (CNAB 400)</b>
		- Implementação / Testes
			- Carteiras
				- [x] 3 - COBRANÇA (*Homologado com o Banco 10/2017 - by @kalleyaoliveira**)
				- [x] 4 - COBRANÇA (*Homologado com o Banco 10/2017 - by @kalleyaoliveira**)
	- <b>041-8 | Banco Banrisul - (CNAB 400)</b>
		- Implementação / Testes
			- Carteiras 
            			- [x]  1 – Cobrança Simples (8050.76)
			        - [x]  3 – Cobrança Caucionada (8150.55) Reservado
			        - [x]  4 – Cobrança em IGPM (8450.94) *
			        - [x]  5 – Cobrança Caucionada CGB Especial (8355.01) Reservado
			        - [x]  6 – Cobrança Simples Seguradora (8051.57)
			        - [x]  7 – Cobrança em UFIR (8257.86) *
			        - [x]  8 – Cobrança em IDTR (8356.84) *
			        - [x]  C – Cobrança Vinculada (8250.34)
			        - [x]  D – Cobrança CSB (8258.67)
			        - [x]  E – Cobrança Caucionada Câmbio (8156.24)
			        - [x]  F – Cobrança Vendor (8152.17) Reservado
			        - [x]  H – Cobrança Caucionada Dólar (8157.05) Reservado **
			        - [x]  I – Cobrança Caucionada Compror (8351.46) Reservado
			        - [x]  K – Cobrança Simples INCC-M (8153.06)
			        - [x]  M – Cobrança Partilhada (8154.70)
			        - [x]  N – Capital de Giro CGB ICM (6130.96) Reservado
			        - [x]  R – Desconto de Duplicata (6030.15) ***
			        - [x]  S – Vendor Eletrônico – Valor Final (Corrigido) (6032.79) 
				
## Como contribuir

Para compilar o projeto Nuget, utilizamos a extensão (NuGet Package Project for Visual Studio 2017)[
https://marketplace.visualstudio.com/items?itemName=NuProjTeam.NuProj2017#review-details]

## Instalação
O BoletoBr também é distribuído através de um pacote Nuget. Dessa forma não é necessário baixar o código fonte atualizado e compilar sempre que desejar uma versão mais recente.  
Na sua aplicação .NET, instale o seguinte pacote:  
> PM> Install-Package BoletoBr
