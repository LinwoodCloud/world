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
        }
    }
}