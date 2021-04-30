using Godot;
using System;

public class LoadingScreen : Control
{
    private Node currentScene;
    private ResourceInteractiveLoader loader;
    private int waitFrames;
    const int maxTime = 120;

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var root = GetTree().Root;
        currentScene = root.GetChild(root.GetChildCount() - 1);
        Visible = false;
    }

    public void Load(string path)
    {
        loader = ResourceLoader.LoadInteractive(path);
        if (loader == null)
        {
            Free();
            ShowError();
            return;
        }
        SetProcess(true);
        currentScene.QueueFree();
        // TODO: Add loading animation
        // GetNode<AnimationPlayer>("AnimationPlayer").Play("loading");
        waitFrames = 1;
        Visible = true;
    }

    private void ShowError()
    {
        GD.PrintErr("Error while loading the scene!");
    }

    public override void _Process(float delta)
    {
        if (loader == null)
        {
            SetProcess(false);
            return;
        }
        if (waitFrames > 0)
        {
            waitFrames -= 1;
            return;
        }
        var t = OS.GetTicksMsec();
        while (OS.GetTicksMsec() < t + maxTime)
        {
            var error = loader.Poll();
            if (error == Error.FileEof)
            {
                var resource = loader.GetResource();
                loader = null;
                Visible = false;
                SetNewScene(resource as PackedScene);
                break;
            }
            else if (error == Error.Ok)
                UpdateProgress();
            else
            {
                ShowError();
                loader = null;
                break;
            }
        }
    }

    private void UpdateProgress()
    {
        var progress = (float)loader.GetStage() / loader.GetStageCount();
        GetNode<TextureProgress>("ProgressBar").Value = progress;


    }

    private void SetNewScene(PackedScene resource)
    {
        currentScene = resource.Instance();
        GetNode("/root").AddChild(currentScene);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      


    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
