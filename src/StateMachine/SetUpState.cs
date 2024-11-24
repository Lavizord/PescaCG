using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SetUpState : GameManagerState
{

	public override void OnStart(Dictionary<string, object> message)
	{
		base.OnStart(message);
		GD.Print("Start of State: [SetUpState]");
		GM.InitializePublicDeck();
		GM.SetInitialPlayer();
		GM.InitializePlayerDecks();
		GM.ChangeState("YearStartState");
	}


	public override void OnUpdate()
	{ 
		base.OnUpdate();
	}


	public override void UpdateState(float delta)
	{
		base.UpdateState(delta);
	}


	public override void OnExit(string nextState)
	{
		base.OnExit(nextState);
		GD.Print("Exiting of State: [SetUpState]");
	}


}
