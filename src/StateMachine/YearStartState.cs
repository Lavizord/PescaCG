using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


public partial class YearStartState : GameManagerState
{

	private int cardsChosen = 0;
	[Export] public int maxCardsPerSetup = 3;

	private Card cardToTrade = null;
	private string cardToTradeOrigin = "";

	
	public override void OnStart(Dictionary<string, object> message)
	{
		base.OnStart(message);
		GD.Print("Start of State: [YearStartState]");
		RevealCardsToChose();
		SubscriveToStateEvents(true);
	}



	private void SubscriveToStateEvents(bool subscribe)
	{	
		if(subscribe)
		{
			EventSubscriber.SubscribeToEvent("OnPlayerChange", OnPlayerChange);
		}
		if(!subscribe)
		{
			EventSubscriber.UnsubscribeFromEvent("OnPlayerChange", OnPlayerChange);
		}
	}


	public override void OnUpdate()
	{ 
		base.OnUpdate();
		UpdatePlayerDeckGrid();
	}

	public override void UpdateState(float delta)
	{
		base.UpdateState(delta);
	}


	public override void OnExit(string nextState)
	{
		base.OnExit(nextState);
		GD.Print("[Year Start state] -- EXIT");
		SubscriveToStateEvents(false);
	}
	

	public void OnCardChosenToAdd(object sender, object obj)
	{
		GD.Print("YearStartState.OnCardChosenToAdd");
		if(obj is Card card)
		{
			GM.AddCardToActivePlayer(card);
			cardsChosen += 1;
			
		}
		if(cardsChosen == maxCardsPerSetup)
		{
			GD.Print("YearStart - Max Cards Chosen!");
			CleanUpNotChosenCards();
			GM.ShufflePublicDecks();
			cardsChosen = 0;
			GM.ChangeToNextPlayer();
			RevealCardsToChose();
		}
		// TODO: I think this is Bugged... The Final Player Logic.
		if(GM.IsLastPlayerTurn())
		{
			GD.Print("YearStart - Last Player has chosen the last card! ");
			GM.ShuffleAllPlayerDecks();
			GM.AllPlayersDraw(4, "Deck", "Hand");
			GM.ChangeState("OceanState");
		}
		UpdatePlayerDeckGrid();
	}


	//TODO: This is Bugged, rethink the way I am handling the cards....
	public void OnCardChosenToTrade(object sender, object obj)
	{
		string newCardOrigin = "";
		// Determine Object origin
		if(obj is Card card )
		{
			// Se já tivermos uma carta selecionada e se o pai da nova for diferente, vamos fazer a troca.
			if(cardToTrade != null && cardToTrade.GetParent().Name != card.GetParent().Name)
			{
				SwitchCards(cardToTrade, card);
				cardToTrade = null;
			}
			// Basicamente estamos a selecionar uma carta do mesmo local, portanto substituimos.
			// Ou estamos a selecionar uma carta pela primeira vez, portanto guardamos.
			else 
			{
				cardToTrade = card;
			}
		}	
		UpdatePlayerDeckGrid();
	}


	// TODO: some of this still is not producing the right result. Debug some more.
	private void SwitchCards(Card card1, Card card2)
	{
		string card1Origin = "";
		string card2Origin = "";

		// Determine card origins.
		if(card1.GetParent().Name == "DeckContainer")
				card1Origin	 = "playerdeck";
		if(card2.GetParent().Name == "DeckContainer")
				card2Origin	 = "playerdeck";

		if(card1.GetParent().Name == "CardChoiceContainer")
				card1Origin = "grid";
		if(card2.GetParent().Name == "CardChoiceContainer")
				card2Origin = "grid";

		// Handling of the change to the card1
		if(card1Origin == "playerdeck")
		{
			// There is a caviat here, the cards displayed are a copy of the deck.
			// Therefore, if we simply add them to the Fish // Tool decks, we are not really removing them from the player deck.
			// I think the simple solution is to remove a card from a player deck. And delete it.
			if(card1.tipo == TiposCarta.Peixe)
				GM.AddCardsToDeck("FishCards", new List<Card> { card1 });
			if(card1.tipo == TiposCarta.Ferramenta)
				GM.AddCardsToDeck("ToolCards", new List<Card> { card1 });

			// The DrawSpecificCard method should handle removing the card from the player deck.
			GM.GetActivePlayer().GetDeck("Deck").DrawSpecificCard(card1).QueueFree();
		}
		if(card1Origin == "grid")
		{
			GM.AddCardToActivePlayer(card1);
		}
		//--
		// Handling of the change to the card2
		if(card2Origin == "playerdeck")
		{
			// There is a caviat here, the cards displayed are a copy of the deck.
			// Therefore, if we simply add them to the Fish // Tool decks, we are not really removing them from the player deck.
			// I think the simple solution is to remove a card from a player deck. And delete it.
			if(card2.tipo == TiposCarta.Peixe)
				GM.AddCardsToDeck("FishCards", new List<Card> { card2 });
			if(card2.tipo == TiposCarta.Ferramenta)
				GM.AddCardsToDeck("ToolCards", new List<Card> { card2 });

			// The DrawSpecificCard method should handle removing the card from the player deck.
			GM.GetActivePlayer().GetDeck("Deck").DrawSpecificCard(card1).QueueFree();
		}
		if(card2Origin == "grid")
		{
			GM.AddCardToActivePlayer(card2);
		}
	}


	public void OnPlayerChange(object sender, object obj)
	{
		GD.Print("YearStartState.OnPlayerChange() triggered.");
		if(obj is Player player)
		{
			GetNode<Label>("%PlayerName").Text = player.Name;
		}
	}

	
	public void RevealCardsToChose(string choice = null)
	{
		List<Card> cardsToChoce = GM.DrawCardsFromPublicDecks("FishCards", 3);
		cardsToChoce.AddRange(GM.DrawCardsFromPublicDecks("ToolCards", 3));
		foreach (Card card in cardsToChoce)
		{
			GetNode<GridContainer>("%CardChoiceContainer").AddChild(card);
			card.SetSize(new Vector2(250f, 350f));
			card.Show();
		}
	}

	// TODO: Think about this, i believe this duplication of the card is in the origin of our issue...
	private void UpdatePlayerDeckGrid()
	{
		List<Card> pCards = GM.GetActivePlayer().GetDeck("Deck").cards;
		foreach (Node node in GetNode<GridContainer>("%DeckContainer").GetChildren())
			node.QueueFree();
		
		foreach(Card card in pCards)
		{
			Node dupcard = card.Duplicate();
			if(dupcard is Card duprealcard)
			{
				GetNode<GridContainer>("%DeckContainer").AddChild(duprealcard);
				duprealcard.SetSize(new Vector2(250f, 350f));
				duprealcard.Show();
			}
		}
	}


	public void CleanUpNotChosenCards()
	{
		List<Card> cardsToChose = GetNode<GridContainer>("%CardChoiceContainer").GetChildren().OfType<Card>().ToList();
		GM.AddCardsToDeck("FishCards", cardsToChose.Where(c => c.tipo == TiposCarta.Peixe).ToList());
		GM.AddCardsToDeck("ToolCards", cardsToChose.Where(c => c.tipo == TiposCarta.Ferramenta).ToList());
	}


	public void OnTradeCardButtonPressed()
	{
		EventSubscriber.SubscribeToEvent("OnCardPressed", OnCardChosenToTrade);
	}
	public void OnAddCardBtnPressed()
	{
		EventSubscriber.SubscribeToEvent("OnCardPressed", OnCardChosenToAdd);
	}


}
