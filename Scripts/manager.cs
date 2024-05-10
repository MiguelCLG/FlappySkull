using Godot;

public partial class manager : Node2D
{
    Label score;
    Label HighScore;
    AudioStreamPlayer2D DeathSound;
    
    public override void _Ready()
    {
        GetTree().Paused = true;
        score = GetNode<Label>("CanvasLayer/ScoreLabel");
        HighScore = GetNode<Label>("CanvasLayer/HighScoreLabel");
        DeathSound = GetNode<AudioStreamPlayer2D>("%DeathSound");
        RegisterEvents();
        LoadGame();
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
            HighScore.Text = $"High Score: {skull.GetHighScore()}";
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
        SaveGame();
        EventSubscriber.UnsubscribeFromEvent("OnPassColumn", OnPassColumn);
        EventSubscriber.UnsubscribeFromEvent("OnColumnHit", OnColumnHit);
        GetTree().CallDeferred("reload_current_scene");
    }

    void LoadGame()
    {
        if (!FileAccess.FileExists("user://savegame.save"))
        {
            GD.PrintErr("No save file.");
            return; // Error! We don't have a save to load.
        }
       
        using var saveGame = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Read);
        while (saveGame.GetPosition() < saveGame.GetLength())
        {
            var jsonString = saveGame.GetLine();

            var json = new Json();
            var parseResult = json.Parse(jsonString);

            if(parseResult != Error.Ok)
            {
                GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in {jsonString} at line {json.GetErrorLine()}");
                continue;
            }
            var nodeData = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)json.Data);

            foreach(var (key, value) in nodeData)
            {
                HighScore.Text = $"High Score: {value}";
                GetNode<Skull>("Skull").SetHighScore((int)value);
            }
        }
    }

    void SaveGame()
    {
        using var saveGame = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Write);
        var saveNodes = GetTree().GetNodesInGroup("Persist");

        foreach(var node in saveNodes)
        {
            var nodeData = node.Call("Save");
            var jsonString = Json.Stringify(nodeData);

            saveGame.StoreLine(jsonString);
        }
    }

    public Godot.Collections.Dictionary<string, Variant> Save()
    {
        return new Godot.Collections.Dictionary<string, Variant>()
        {
            { "HighScore", HighScore.Text },
        };
    }
}
