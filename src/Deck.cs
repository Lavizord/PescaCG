using Godot;
using System;
using System.Collections.Generic;

public partial class Deck : Node
{

	[Export] bool hideCardsOnAdd = true;
	public List<Card> cards = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cards = new();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	/// <summary>
	/// We add a card to our node and to our list. If a card has a parent we remove it 
	/// and se the parent to the deck.
	/// </summary>
	/// <param name="cardToAdd"></param> <summary>
	/// 
	/// </summary>
	/// <param name="cardToAdd"></param>
	public void AddCard(Card cardToAdd)
	{
		// We add our card to our Deck.
		cards.Add(cardToAdd);
		// We get the card parent.
		Node cardParent = cardToAdd.GetParent();
		// If the card has a parent we remove the card from being a children.
		if(cardParent != null)
			cardParent.RemoveChild(cardToAdd);

		// We add the card as a child of our Deck.
		AddChild(cardToAdd);
		if(hideCardsOnAdd)
			cardToAdd.Hide();
	}

	/// <summary>
	/// Shuffles the cards in the deck list.
	/// </summary>
	public void Shuffle()
	{
		Random random = new Random();  
		int n = cards.Count;  

		for(int i= cards.Count - 1; i > 1; i--)
		{
			int rnd = random.Next(i + 1);  

			var value = cards[rnd];  
			cards[rnd] = cards[i];  
			cards[i] = value;
		}
	}

	/// <summary>
	/// Returns an X number of cards from the "top" of the deck, index 0 is considered top.
	/// The drawn cards are also removed from the parent object.
	/// </summary>
	/// <param name="numberToGet">Number of cards to return, if possible.</param>
	/// <returns></returns>
	public List<Card> DrawTopCards(int numberToGet)
	{
		List<Card> returnCards = new();
		for (int i = 0; i < numberToGet; i++)
		{
			Card card = cards[0];
			Node cardParent = card.GetParent();
			returnCards.Add(card);
			// Remove it from parent 
			cardParent.RemoveChild(card);
			// Remove as cartas da nossa lista.
			cards.RemoveAt(0);
		}
		return returnCards;
	}

	/// <summary>
	/// Finds a card in the deck as the specified card, we remove its parent, we remove it from the list of cards and finally return it.
	/// </summary>
	/// <param name="card"></param>
	/// <returns></returns>
	public Card DrawSpecificCard(Card card)
	{
		Card cardFound = cards.Find( c => c.nome == card.nome);
		Node cardParent = cardFound.GetParent();
		
		// Remove it from parent 
		cardParent.RemoveChild(cardFound);
		// Remove as cartas da nossa lista.
		cards.Remove(cardFound);
	
		return cardFound;
	}/* TODO: Pode vir a dar jeito algo com string ou tipo.
	public Card DrawSpecificCard(String name)
	{
		Card cardFound = cards.Find( c => c == card);
		Node cardParent = cardFound.GetParent();

		// Remove it from parent 
		cardParent.RemoveChild(cardFound);
		// Remove as cartas da nossa lista.
		cards.Remove(cardFound);
	
		return cardFound;
	}*/
	
}
