using Godot;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

public partial class GameManager : SimpleStateMachine
{

	private int numOfPlayers = 0;
	private int activePlayerCounter = 0;


    public override void _Ready()
    {
        base._Ready();

		List<GameManagerState> gameManagetStates = GetNode<Node>("States").GetChildren().OfType<GameManagerState>().ToList();
		foreach (GameManagerState GMS in gameManagetStates)
			GMS.GM = this;
		
		// We Initialize our number of Players from the Scene
		numOfPlayers = GetNode<Node>("%PlayerContainer").GetChildren().OfType<Player>().ToList().Count();
		RegisterEvents();

		GD.Print("Game Manager Inicialziado. Como não temos UI, vai já passar para o MenuState.");
		ChangeState("MenuState");
    }


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}


	private void RegisterEvents()
	{
		EventRegistry.RegisterEvent("OnCardPressed");
		EventRegistry.RegisterEvent("OnPlayerChange");
	}


	/// <summary>
	/// Method to initialize the public deck of cards, should be called when we are starting a new Game.
	/// The initialized deck contains every card in the database, of wich the NumCartas property is greater than 0.
	/// </summary>
	public void InitializePublicDeck()
	{
		GD.Print("---");
		GD.Print("Intializing Public Deck of Cards....  (Probably we are at the game SetUp phase)");
		Deck toolDeck = GetNode<Deck>("%ToolCards");
		Deck fishDeck = GetNode<Deck>("%FishCards");

		// We fetch hall the data from our dictionary database.
		foreach (var cardData in CardDatabase.DATA)
		{
			// Divide data into Key / Values[]
			CardDatabase.CardTypes cardType = cardData.Key;
			object[] values = cardData.Value;
			// Get Number of cards to generate based on data.
			int numCartasGerar = (int) values[2];
			GD.Print("");
			GD.Print($"Carta: ["+values[1]+"] carregada. NumCartas no deck Inicial: ["+values[2]+"].");
			
			// Loop Through the card and generate a new Card Scene from the Card.tscn
			for (int i = 0; i < numCartasGerar; i++)
			{
				PackedScene cardPs = GD.Load<PackedScene>("res://scenes/Card.tscn");
				var cardInstance = cardPs.Instantiate<Card>();

				string tipo		= (string) values[0];  
				string nome		= (string) values[1];
				int? numPontos 	= (int?) values[3];
				int? numPescado 	= (int?)values[4];
				string texto 	= (string) values[5];  

				// We also set its properties from the dataset.
				cardInstance.SetCardProperties(
					tipo, nome, numPontos, numPescado, texto, cardType
				);
				// We Hide the card.
				cardInstance.Hide();

				cardInstance.Name = nome+(i+1).ToString();
				// Now we add the cards to certain nodes, for easy acess.
				if(cardInstance.tipo == TiposCarta.Ferramenta)
					toolDeck.AddCard(cardInstance);
				if(cardInstance.tipo == TiposCarta.Peixe)
					fishDeck.AddCard(cardInstance);
			}
		}
		toolDeck.Shuffle();
		fishDeck.Shuffle();
	}


	/// <summary>
	/// Method that Initializes the player decks with the starting values.
	/// Player Initial Cards are taken from the Fish and Tool decks.
	/// </summary>
	public void InitializePlayerDecks()
	{
		// We get the Players added to the PlayerContainer.
		List<Player> players = GetNode<Node>("%PlayerContainer").GetChildren().OfType<Player>().ToList();
		
		// Now we loop the players and give them the initial cards, taken from the FishCards and ToolCards. 
		foreach (Player player in players)
		{	
			// We fetch the PlaerDeck.
			Deck playerDeck = player.GetNode<Deck>("%Deck");
			
			// We fetch 3 cards from the tool cards 
			List<Card> drawnCards = GetNode<Deck>("%ToolCards").DrawTopCards(3);
			// We add the 3 cards to the playerDeck.
			foreach (Card card in drawnCards)
				playerDeck.AddCard(card);
			
			
			// We fetch 6 cards from the tool cards 
			drawnCards = GetNode<Deck>("%FishCards").DrawTopCards(6);
			// We add the 3 cards to the playerDeck.
			foreach (Card card in drawnCards)
				playerDeck.AddCard(card);
		}
	}


	/// <summary>
	/// For now, this just checks for a player count and sets the active player up...
	/// </summary> <summary>
	/// 
	/// </summary>
	public void SetInitialPlayer()
	{
		activePlayerCounter = 0;
		Player p = GetNode<Node>("%PlayerContainer").GetChildren().OfType<Player>().ToList()[0];
		p.IsActive = true;
		EventRegistry.GetEventPublisher("OnPlayerChange").RaiseEvent(GetActivePlayer());
	}


	public void ShuffleAllDecks()
	{
		ShuffleAllPlayerDecks();
		ShufflePublicDecks();
	}


	public void ShufflePublicDecks()
	{
		GetNode<Deck>("%ToolCards").Shuffle();
		GetNode<Deck>("%FishCards").Shuffle();
	}


	public void ShuffleAllPlayerDecks()
	{
		List<Player> players = GetNode<Node>("%PlayerContainer").GetChildren().OfType<Player>().ToList();
		foreach (Player player in players)
		{
			player.GetNode<Deck>("%Deck").Shuffle();
			GD.Print("Game Manager -- Shuffling All Decks - "+player.Name);
		}
	}


	public Player GetActivePlayer()
	{
		return GetNode<Node>("%PlayerContainer").GetChildren().OfType<Player>().ToList().Where(p => p.IsActive == true).FirstOrDefault();
	}

	public bool IsLastPlayerTurn()
	{
		if((activePlayerCounter+1) == numOfPlayers)
			return true;
		return false;
	}

	public void ChangeToNextPlayer()
	{
		GD.Print("GM.Changing player...");

		int pcountBefore = activePlayerCounter;		
		List<Player> players = GetNode<Node>("%PlayerContainer").GetChildren().OfType<Player>().ToList();
		players[activePlayerCounter].IsActive = false;
		activePlayerCounter += 1;
		players[activePlayerCounter].IsActive = true;

		EventRegistry.GetEventPublisher("OnPlayerChange").RaiseEvent(GetActivePlayer());

	}


	/// <summary>
	/// Just adds a card to the ActivePlayers main deck.
	/// </summary>
	/// <param name="card"></param>
	public void AddCardToActivePlayer(Card card)
	{
		GetActivePlayer().GetNode<Deck>("%Deck").AddCard(card);
	}

	public void AllPlayersDraw(int num, string drawnDeck, string drawToDeck)
	{
		foreach(Player p in GetNode<Node>("%PlayerContainer").GetChildren().OfType<Player>().ToList())
			p.Draw(4, "Deck" ,"Hand");
	}	


	public List<Card> DrawCardsFromPublicDecks(string name, int numCards) 
	{
		List<Card> cardsDrawn = new();
		if(name == "FishCards")
			cardsDrawn.AddRange(GetNode<Deck>("%FishCards").DrawTopCards(numCards));
		if(name == "ToolCards")
			cardsDrawn.AddRange(GetNode<Deck>("%ToolCards").DrawTopCards(numCards));

		return cardsDrawn;
	}


	public void AddCardsToDeck(string name, List<Card> cartas) 
	{
		if(name == "FishCards")
			foreach(Card card in cartas)
				GetNode<Deck>("%FishCards").AddCard(card);
		if(name == "ToolCards")
			foreach(Card card in cartas)
				GetNode<Deck>("%ToolCards").AddCard(card);
	}

}
