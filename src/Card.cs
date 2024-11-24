using Godot;
using System;


public enum TiposCarta {
	Peixe, 
	Ferramenta
};


public partial class Card : MarginContainer
{

	public TiposCarta tipo;
	public string nome;
	public int? numPontos;
	public int? numPescado;

	public string texto;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void SetCardProperties(string tipo, string nome, int? numPontos, int? numPescado, string texto, CardDatabase.CardTypes enumCarta)
	{
		GD.Print(" = CARD -- SetCardProperties - Name:["+nome+"]");
		if(tipo == "Peixe")
			this.tipo = TiposCarta.Peixe;
		if(tipo == "Ferramenta")
			this.tipo = TiposCarta.Ferramenta;

		this.nome = nome;
		this.numPontos = numPontos;
		this.numPescado = numPescado;
		this.texto = texto;
		LoadCardTextureFromName(enumCarta);
		GetNode<Label>("%CardName").Text = nome;
	}	


	/// <summary>
	/// Giant Switch case :)
	/// </summary>
	/// <param name="enumCarta"></param>
	public void LoadCardTextureFromName(CardDatabase.CardTypes enumCarta)
    {
        TextureRect cardTexture = GetNode<TextureRect>("%CardTexture");
        Texture2D newTexture = null;

        switch (enumCarta)
        {
            case CardDatabase.CardTypes.AmeijoaBoa:
                newTexture = (Texture2D)GD.Load("res://art/ameijoa.png");
                break;
            case CardDatabase.CardTypes.AtumClaro:
                newTexture = (Texture2D)GD.Load("res://art/atum.png");
                break;
            case CardDatabase.CardTypes.BaleiaCorcunda:
                newTexture = (Texture2D)GD.Load("res://art/baleia.png");
                break;
            case CardDatabase.CardTypes.Cavala:
                newTexture = (Texture2D)GD.Load("res://art/cavala.png");
                break;
            case CardDatabase.CardTypes.Choco:
                newTexture = (Texture2D)GD.Load("res://art/choco.png");
                break;
            case CardDatabase.CardTypes.Espadarte:
                newTexture = (Texture2D)GD.Load("res://art/espadarte.png");
                break;
            case CardDatabase.CardTypes.PolvoComum:
                newTexture = (Texture2D)GD.Load("res://art/polvo.png");
                break;
            case CardDatabase.CardTypes.Sapateira:
                newTexture = (Texture2D)GD.Load("res://art/sapateira.png");
                break;
            case CardDatabase.CardTypes.Sardinha:
                newTexture = (Texture2D)GD.Load("res://art/sardinha.png");
                break;
            default:
                GD.PrintErr("Invalid card type: " + enumCarta.ToString());
                break;
        }
        if (newTexture != null)
            cardTexture.Texture = newTexture;
    }


	private void OnCardPressed()
    {
        GD.Print("Card was Pressed!");
        EventRegistry.GetEventPublisher("OnCardPressed").RaiseEvent(this);
    }

}
