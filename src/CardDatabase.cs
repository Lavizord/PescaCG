using System;
using System.Collections.Generic;


///
/// Class contem um Dicionário de cartas que têm os dados necessários para 
/// serem carregadas para uma scena.
/// Novas Cartas têm de ser adiconadas, mantendo a estutura existente.

//                  0      1         2       3           4      5
// # Cardinfo = [Tipo, Nome, NumCartas, Pontos, NumPescado, Texto]



public static class CardDatabase
{
    public enum CardTypes
    {
        AtumClaro,
        BaleiaCorcunda,
        Espadarte,
        Choco,
        PolvoComum,
        Sardinha,
        Carapau,
        Cavala,
        AmeijoaBoa,
        Sapateira,
        Rede,
        Plankton,
        Anzol,
        Gaiola,
        RedeSecagem
    }
    
    public static Dictionary<CardTypes, object[]> DATA = new Dictionary<CardTypes, object[]>
    {
        { CardTypes.AtumClaro, new object[] { "Peixe", "Atum Claro", 5, 2, null, "No início do turno, devora um pescado no teu oceano excepto Crustáceos, outros Atuns-Albacora, o Espadarte e a Baleia-Corcunda. Ganha 2 pontos por cada pescado devorado;" } },
        { CardTypes.AmeijoaBoa, new object[] { "Peixe", "Amêijoa Boa", 10, 1, null, " Pode-se Pescar até duas sem uso de Ferramenta. Podem ser comidas pela Sapateira." } },
        { CardTypes.BaleiaCorcunda, new object[] { "Peixe", "Baleia Corcunda", 3, 10, null, "Durante a Fase de Ferramenta, o jogador pode usar 3 Plankton do Oceano para pescar a Baleia, o jogador pode pescar a Baleia do Oceano ou diretamente da Mão ou do seu Baralho. Esta ação tem de ser a única ação do jogador na fase de ferramenta.;" } },
        { CardTypes.Carapau, new object[] { "Peixe", "Carapau", 5, 1, null, " No início de cada ano, se estiver no Oceano, os pontos do Carapau são duplicados. Depois de ser pescado e pontuado a carta volta a valer 1 ponto." } },
        { CardTypes.Cavala, new object[] { "Peixe", "Cavala", 12, 1, null, "Quando pescado com outras cartas deCavala, aumenta o nº de pontos.;" } },
        { CardTypes.Choco, new object[] { "Peixe", "Choco", 8, 2, null, "Uma vez por turno, depois de vendido o jogador escolhe uma carta de Ferramenta da pilha de descarte e adiciona-a á sua mão, incluindo a ferramenta usada para o pescar." } },
        { CardTypes.Espadarte, new object[] { "Peixe", "Espadarte", 5, 4, null, "No início de cada Fase de Oceano e apenas uma vez, o jogador escolhe outro peixe do seu Oceano (Sardinha, Cavala, Carapau, Choco, Polvo ou Atum). Esse peixe fica bloqueado até á próxima Fase de Oceano. " } },
        { CardTypes.PolvoComum, new object[] { "Peixe", "Polvo Comum", 8, 2, null, "Depois de Pescado, pode ser vendido em qualquer lota até ao final de cada ano. A carta de Polvo Comum é guardada junto da pilha de descarte até ser vendida. " } },
        { CardTypes.Sapateira, new object[] { "Peixe", "Sapateira", 7, 2, null, "Apenas uma vez, no inicio da Fase de Oceano, pode comer Ameijoa Boa ou Plankton, ao fazer isto adiciona ( 2 ) aos pontos da carta. Se Pescado com Gaiola adiciona mais ( 2 )." } },
        { CardTypes.Sardinha, new object[] { "Peixe", "Sardinha", 12, 1, null, "Uma vez por turno, ao jogar uma Sardinha da mão o jogador pode ir procurar uma Sardinha do baralho e adicionar á sua mão." } },
        { CardTypes.Rede, new object[] { "Ferramenta", "Rede", 13, null, 3, "Pode capturar até 3 Pescado. Útil para capturar Atum-Albacora, Carapau, Cavala, Sardinha e Sapateira;" } },
        { CardTypes.Anzol, new object[] { "Ferramenta", "Anzol", 8, null, 2, "Pode Pescar até 2 Peixes; Útil para Pescar Choco, Espadarte e Polvo-Comum. " } },
        { CardTypes.Gaiola, new object[] { "Ferramenta", "Gaiola", 4, null, 1, "Pode Pescar 1 Peixe Útil para Pescar Choco, Polvo-Comum e Sapateira." } },
        { CardTypes.RedeSecagem, new object[] { "Ferramenta", "Rede de Secagem", 10, null, 1, "Não conta para o limite de uso de Ferramenta na fase de captura. Quando um peixe é pescado pode ser utilizada uma Rede de Secagem para adicionar mais 2 pontos ao valor do Peixe. Útil para Carapau, Polvo-Comum e Sardinha. " } },
        { CardTypes.Plankton, new object[] { "Ferramenta", "Plankton", 10, null, 1, "Pode Pescar 1 pescado; Pode ser colocado no Oceano. Uma vez no oceano pode ser utilizada como ferramenta e não conta para o limite de uso de Ferramenta. Pode ser descartado apenas na fase de Captura e por cada Plankton descartado biscas o mesmo nº de cartas do teu baralho. Útil para Pescar Cavala e Baleia-Corcunda" } }
    };
}
