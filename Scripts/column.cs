using Godot;
using System;

public partial class column : Node2D
{
    StaticBody2D TopColumn { get; set; }
    StaticBody2D BottomColumn { get; set; }
    Area2D Score { get; set; }
    [Export] int minPos = 0;
    [Export] int maxPos = 0;

    Vector2 TopColumnInitialPosition { get; set; }
    Vector2 BottomColumnInitialPosition { get; set; }
    Vector2 InitialPosition { get; set; }

    public override void _Ready()
    {
        TopColumn = GetNode<StaticBody2D>("TopColumn");
        BottomColumn = GetNode<StaticBody2D>("BottomColumn");
        TopColumnInitialPosition = TopColumn.Position;
        BottomColumnInitialPosition = BottomColumn.Position;
        InitialPosition = Position;
        SetInitialPosition();
    }

    public override void _Process(double delta)
    {
        MovePilars();
    }

    private void SetInitialPosition()
    {
        TopColumn.Position = TopColumnInitialPosition;
        BottomColumn.Position = BottomColumnInitialPosition;
        Position = InitialPosition;
        SetRandomSpacing();
    }

    private void SetRandomSpacing()
    {
        TopColumn.Position -= new Vector2(0, new Random().Next(minPos, maxPos));
        BottomColumn.Position += new Vector2(0, new Random().Next(minPos, maxPos));
        Position += new Vector2(0, new Random().Next(-150, 150));
    }

    private void MovePilars()
    {
        if(Position.X < -100) { SetInitialPosition(); }
        Position -= new Vector2(2, 0);
    }

    public void OnBodyEntered(Node2D body)
    {
        if (body is Skull skull)
        {
            skull.AddScore(1);
            EventRegistry.GetEventPublisher("OnPassColumn").RaiseEvent(skull);
        }
    }
}
