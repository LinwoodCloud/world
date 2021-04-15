using Godot;

namespace LinwoodWorld.Level
{
    public class BackpackScript : WindowDialog
    {

        public override void _Input(InputEvent @event)
        {
            if(@event.IsActionPressed("backpack")){
                Visible = !Visible;
            }
        }
    }
}