using Godot;
using System;

public partial class StartGameInput : Panel
{

    public override void _UnhandledInput(InputEvent @event)
    {
        if(@event.IsActionReleased("jump"))
        { 
            GetTree().Paused = false;
            GetParent().RemoveChild(this);
        }
    }
}
