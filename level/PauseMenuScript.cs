using Godot;
using System;

namespace LinwoodWorld.Level
{

    public class PauseMenuScript : Control
    {
        [Export]
        private NodePath playerPath;
        private Player player;
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            player = GetNode<Player>(playerPath);
        }

        public void OnQuitButtonPressed()
        {
            GetTree().Quit();
        }

        public void OnResumeButtonPressed()
        {
            player.Resume();
        }

        public void OnMainMenuButtonPressed()
        {
            GetTree().ChangeScene("res://main/menu.tscn");
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        //  public override void _Process(float delta)
        //  {
        //      
        //  }
    }
}
