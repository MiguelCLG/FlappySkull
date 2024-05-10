using Godot;
using System;

public partial class Skull : RigidBody2D
{
    int score = 0;
    int highScore = 0;
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
    
    public int GetHighScore()
    {
        return highScore;
    }
    
    public void SetHighScore(int newHighScore)
    {
        highScore = newHighScore;
    }


    public void AddScore(int score)
    {
        this.score += score;
    }

    public Godot.Collections.Dictionary<string, Variant> Save()
    {
        return new Godot.Collections.Dictionary<string, Variant>()
        {
            { 
                "HighScore", highScore 
            },
        };
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
