using Godot;
using System;

namespace LinwoodWorld.Level
{
    public class ToolButton : TextureButton
    {
        [Export]
        public Tool tool;
        [Export]
        public NodePath playerPath;
        private Player player;
        private TextureRect selectedRect;

        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            player = GetNode<Player>(playerPath);
            selectedRect = GetNode<TextureRect>(new NodePath("SelectedTexture"));
            UpdateDisplay();
        }
        private void UpdateDisplay()
        {
            selectedRect.Visible = player.CurrentTool == tool;
        }

        public override void _PhysicsProcess(float delta)
        {
            if(Input.IsActionJustPressed(tool.GetAction())){
                player.CurrentTool = tool;
                UpdateDisplay();
            }
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        //  public override void _Process(float delta)
        //  {
        //      
        //  }
    }
}