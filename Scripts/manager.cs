using Godot;
using System;

public partial class manager : Node2D
{
    Label score;

    public override void _Ready()
    {
        score = GetNode<Label>("CanvasLayer/Label");
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
            RestartGame();
        }
    }

    void RestartGame()
    {
        EventSubscriber.UnsubscribeFromEvent("OnPassColumn", OnPassColumn);
        EventSubscriber.UnsubscribeFromEvent("OnColumnHit", OnColumnHit);
        GetTree().CallDeferred("reload_current_scene");
    }
}
