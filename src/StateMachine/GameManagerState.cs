using Godot;
using System;

// This state is used as a bridge between the GameManager and the other states.
// All other states should inherit this one.
// When the GameManager initializes, it fetches All Objects of "GameManagetState" and
// gives them a reference to the Game Manager, this way, states can call the GameManager to change states, or do other stuff.

public partial class GameManagerState : SimpleState
{
    public GameManager GM;    
}