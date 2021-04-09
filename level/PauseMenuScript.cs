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
            Resume();
            GetTree().Quit();
        }
        public void Resume()
        {
            Visible = false;
            GetTree().Paused = false;
            Input.SetMouseMode(Input.MouseMode.Captured);
        }

        public void OnResumeButtonPressed()
        {
            Resume();
        }

        public void Pause()
        {
            Visible = true;
            GetTree().Paused = true;
            Input.SetMouseMode(Input.MouseMode.Visible);
        }

        public void OnMainMenuButtonPressed()
        {
            GetTree().Paused = false;
            GetTree().ChangeScene("res://main/menu.tscn");
        }
        public override void _Input(InputEvent @event)
        {
            if (Input.IsActionJustPressed("ui_cancel"))
                if (Input.GetMouseMode() == Input.MouseMode.Visible)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        //  public override void _Process(float delta)
        //  {
        //      
        //  }
    }
}
