using Godot;
using System;

public partial class Skull : RigidBody2D
{
    int score = 0;
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("jump"))
        {
            LinearVelocity = Vector2.Zero;
            ApplyImpulse(new Vector2(0, -300));
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

    public void OnDeathCollisionsEnter(Node body)
    {
        if(body.Name == "Skull")
            EventRegistry.GetEventPublisher("OnColumnHit").RaiseEvent(this);
    }
    public void OnBodyEntered(Node body) {
        if(body.Name == "TopColumn" || body.Name == "BottomColumn")
        {
            EventRegistry.GetEventPublisher("OnColumnHit").RaiseEvent(this);
        }
    }
}
