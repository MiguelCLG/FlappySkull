using Godot;
using System;

public partial class CloudController : Node2D
{
    [Export] Sprite2D BackCloud;
    [Export] Sprite2D MidCloud;
    [Export] Sprite2D FrontCloud;
    [Export] float BackCloudBaseSpeed { get; set; } = .5f;
    [Export] float MidCloudBaseSpeed { get; set; } = 1f;
    [Export] float FrontCloudBaseSpeed { get; set; } = 1.5f;

    Vector2 InitialPosition { get; set; }

    public override void _Ready()
    {
        BackCloud = GetNode<Sprite2D>("BackCloud");
        MidCloud = GetNode<Sprite2D>("MidCloud");
        FrontCloud = GetNode<Sprite2D>("FrontCloud");
        InitialPosition = BackCloud.GlobalPosition;
        SetCloudsColor(BackCloud);
        SetCloudsPosition(BackCloud);
        SetCloudsColor(MidCloud);
        SetCloudsPosition(MidCloud);
        SetCloudsColor(FrontCloud);
        SetCloudsPosition(FrontCloud);
    }

    public override void _Process(double delta)
    {
        if (BackCloud.GlobalPosition.X < -100)
        {
            ResetPosition(BackCloud);
            SetCloudsColor(BackCloud);
            SetCloudsPosition(BackCloud);
        }
        if (MidCloud.GlobalPosition.X < -100)
        {
            ResetPosition(MidCloud);
            SetCloudsColor(MidCloud);
            SetCloudsPosition(MidCloud);
        }
        if (FrontCloud.GlobalPosition.X < -100)
        {
            ResetPosition(FrontCloud);
            SetCloudsColor(FrontCloud);
            SetCloudsPosition(FrontCloud);
        }
        MoveClouds(BackCloud, BackCloudBaseSpeed);
        MoveClouds(MidCloud, MidCloudBaseSpeed);
        MoveClouds(FrontCloud, FrontCloudBaseSpeed);
    }

    private void MoveClouds(Sprite2D cloud, float speed)
    {
        cloud.GlobalPosition = new Vector2(cloud.GlobalPosition.X - speed, cloud.GlobalPosition.Y);
    }
    private void ResetPosition(Sprite2D cloud)
    {
        cloud.GlobalPosition = InitialPosition;
    }

    private void SetCloudsColor(Sprite2D cloud)
    {
        var rand = (float)(new Random().NextDouble());
        var grayscaleRand = rand < 0.2 ? 0.5f : rand;
        Color color = new Color(grayscaleRand, grayscaleRand, grayscaleRand, 0.8f);
        cloud.Modulate = color;
    }

    private void SetCloudsPosition(Sprite2D cloud)
    {
        cloud.GlobalPosition = new Vector2(cloud.GlobalPosition.X, new Random().Next(100, 400));
    }

}
