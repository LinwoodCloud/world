using Godot;
using Godot.Collections;

namespace LinwoodWorld.System
{
    public abstract class CubitBlock : Block
    {
        public enum Face
        {
            TOP, BOTTOM, EAST, WEST, NORTH, SOUTH
        }
        public string Name
        {
            get;
        }
        public bool Transparent
        {
            get;
        }
        public bool Solid
        {
            get;
        }

        public override bool CausedRender(VoxelChunk chunk)
        {
            return Transparent || !Solid;
        }
    }
}