using Godot;

namespace LinwoodWorld.Inventory
{
    public abstract class Item : Godot.Object
    {
        public abstract string UITexture {get; }
        public void OnClick()
        {
            
        }
    }
}