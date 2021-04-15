using System;
using Godot;

namespace LinwoodWorld.Level
{
    public class SlotButton : TextureButton
    {
        [Export]
        public int slot;
        [Export]
        public NodePath playerPath;
        private Player player;
        private TextureRect selectedRect;
        public override void _Ready()
        {
            player = GetNode<Player>(playerPath);
            selectedRect = GetNode<TextureRect>(new NodePath("SelectedTexture"));
            UpdateDisplay(player.CurrentSlot);
        }

        private void UpdateDisplay(int currentSlot)
        {
            selectedRect.Visible = currentSlot == slot;
        }
        public override void _Input(InputEvent @event)
        {
            if(@event.IsActionPressed("slot_" + slot))
                ChangeSlot();
        }
        private void ChangeSlot()
        {
            player.CurrentSlot = slot;
        }

        public void ToolChanged(int last, int current)
        {
            UpdateDisplay(current);
        }
    }
}