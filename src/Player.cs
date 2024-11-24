using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public partial class Player : Node
{

    // Backing field for the IsActive property
    private bool _isActive;

    // Exported property with get and set accessors
    [Export]
    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
            GD.Print($"Player {Name} isActive set to: {_isActive}");
            // Trigger any necessary updates or signals here
        }
    }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
    public void Draw(int num, string drawnDeck, string drawToDeck)
    {
        List<Card> cardsDrawn = GetNode<Deck>("%"+drawnDeck).DrawTopCards(num);
        foreach(Card card in cardsDrawn)
            GetNode<Deck>("%"+drawToDeck).AddCard(card);
    }

    public Deck GetDeck(string nameOfDec)
    {
        return GetNode<Deck>("%"+nameOfDec);
    }
}
