using Godot;
using Godot.Collections;

namespace LinwoodWorld.System
{
    public abstract class Addon {
        public abstract string Name {get;}
        public abstract string Version {get;}
        public abstract Array<Block> Blocks {get;}
    }
}