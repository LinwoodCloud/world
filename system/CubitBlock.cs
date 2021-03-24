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

        public override void Render(VoxelChunk chunk, out Array<Vector3> verticies, out Array<Vector3> normals, out Array<Vector3> collision)
        {
            verticies = new Array<Vector3>();
            normals = new Array<Vector3>();
            collision = new Array<Vector3>();
        }
    }
}