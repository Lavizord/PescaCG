using Godot;
using System;
using System.Collections.Generic;

public partial class MenuState : GameManagerState
{
	public override void OnStart(Dictionary<string, object> message)
	{
		base.OnStart(message);
        GD.Print("Start of State: [MenuState]. Como não temos UI, vamos já saltar para o SetUpState.");
	}

	public override void OnUpdate()
	{ 
		base.OnUpdate();
		GM.ChangeState("SetUpState");
	}

	public override void UpdateState(float delta)
	{
		base.UpdateState(delta);
	}

	public override void OnExit(string nextState)
	{
		base.OnExit(nextState);
	}
}
