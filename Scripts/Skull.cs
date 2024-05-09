using Godot;
using System;

public partial class Skull : RigidBody2D
{
    int score = 0;
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("jump"))
        {
            ApplyImpulse(new Vector2(0, -400));
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void AddScore(int score)
    {
        this.score += score;
    }

    public void OnBodyEntered(Node body) {
        if(body.Name == "TopColumn" || body.Name == "BottomColumn")
        {
            EventRegistry.GetEventPublisher("OnColumnHit").RaiseEvent(this);
        }
    }
}
