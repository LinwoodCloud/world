using Godot;
using System;

public class MainMenuScript : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private Button continueButton;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        continueButton = GetNode<Button>(new NodePath("Panel/ScrollContainer/VBoxContainer/ContinueButton"));
        continueButton.Disabled = true;
    }

    private void Open(string uri)
    {
        OS.ShellOpen(uri);
    }

    public void OnCreateButtonPressed()
    {
        var loader = GetNode<LoadingScreen>("/root/LoadingScreen");
        GD.Print("START LOADING");
        loader.Load("res://level/Main_Scene.tscn");
    }

    public void OnNewsButtonPressed()
    {
        Open("https://linwood.tk/blog");
    }
    public void OnSourceButtonPressed()
    {
        Open("https://github.com/LinwoodCloud/world.git");
    }
    public void OnWebsiteButtonPressed()
    {
        Open("https://linwood.tk/docs/world/overview");
    }
    public void OnModsButtonPressed()
    {
        GetNode<WindowDialog>("ModsDialog").Popup_();
    }
    public void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
