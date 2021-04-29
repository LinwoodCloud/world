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
        private Sprite tileset;
        public override void _Ready()
        {
            player = GetNode<Player>(playerPath);
            selectedRect = GetNode<TextureRect>(new NodePath("SelectedTexture"));
            tileset = GetNode<Sprite>(new NodePath("Tileset"));
            UpdateDisplay(player.CurrentSlot);
            player.Connect("SlotChanged", this, nameof(SlotChanged));
            player.Connect("HotbarChanged", this, nameof(HotbarChanged));
        }

        private void UpdateDisplay(int currentSlot)
        {
            selectedRect.Visible = currentSlot == slot;
            tileset.Visible = player.hotbar[slot] != null;
            if(player.hotbar[slot] != null){
                tileset.RegionEnabled = true;
                var block = player.CurrentWorld.GetBlock(player.hotbar[slot]);
                var texture = block.UITexture;
                var position = player.CurrentWorld.GetTexturePosition(texture);
                tileset.RegionRect = new Rect2(position * 64, new Vector2(64, 64));
            }
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

        public void SlotChanged(int last, int current)
        {
            UpdateDisplay(current);
        }
        public void HotbarChanged(string last, string current)
        {
            UpdateDisplay(player.CurrentSlot);
        }
    }
}