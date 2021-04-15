using Godot;

namespace LinwoodWorld.Inventory
{
    public abstract class Item : Godot.Object
    {
        public abstract Texture UITexture {get; }
        public void OnClick()
        {
            
        }
    }
}