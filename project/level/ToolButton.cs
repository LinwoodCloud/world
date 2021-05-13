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
            UpdateDisplay(player.CurrentTool);
            player.Connect("ToolChanged", this, nameof(ToolChanged));
        }
        private void UpdateDisplay(Tool current)
        {
            selectedRect.Visible = current == tool;
        }

        public void OnPressed()
        {
            ChangeTool();
        }

        public override void _Input(InputEvent @event)
        {
            if (@event.IsActionPressed(tool.GetAction()))
                ChangeTool();
        }

        private void ChangeTool()
        {
            player.CurrentTool = tool;
        }

        public void ToolChanged(Tool last, Tool current)
        {
            UpdateDisplay(current);
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        //  public override void _Process(float delta)
        //  {
        //      
        //  }
    }
}