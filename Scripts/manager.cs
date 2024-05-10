using Godot;
using System;

public partial class manager : Node2D
{
    Label score;
    AudioStreamPlayer2D DeathSound;
    
    public override void _Ready()
    {
        GetTree().Paused = true;
        score = GetNode<Label>("CanvasLayer/Label");
        DeathSound = GetNode<AudioStreamPlayer2D>("%DeathSound");
        RegisterEvents();
    }

    void RegisterEvents()
    {
        EventRegistry.RegisterEvent("OnPassColumn");
        EventSubscriber.SubscribeToEvent("OnPassColumn", OnPassColumn);        
        
        EventRegistry.RegisterEvent("OnColumnHit");
        EventSubscriber.SubscribeToEvent("OnColumnHit", OnColumnHit);
    }

    void OnPassColumn(object sender, object obj)
    {
        if(obj is Skull skull)
        {
            score.Text = $"Score: {skull.GetScore()}";
        }
    }

    void OnColumnHit(object sender, object obj)
    {
        if (obj is Skull skull)
        {
            // Pauses every node, except DeathSound because we changed the process to never pause in the inspector (Node->Process->Mode)
            GetTree().Paused = true;
            DeathSound.Play();
            // RestartGame(); -> DeathSound Signal when finished Restarts the Game 
        }
    }

    void RestartGame()
    {
        EventSubscriber.UnsubscribeFromEvent("OnPassColumn", OnPassColumn);
        EventSubscriber.UnsubscribeFromEvent("OnColumnHit", OnColumnHit);
        GetTree().CallDeferred("reload_current_scene");
    }
}
