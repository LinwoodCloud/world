using Godot;
using Godot.Collections;

namespace LinwoodWorld.System
{
    public abstract class Block
    {
        public enum Face
        {
            TOP, BOTTOM, EAST, WEST, NORTH, SOUTH
        }
        public abstract bool CausedRender(VoxelChunk chunk, Vector3 position);
        //public abstract void BuildVoxelFace(Chunk chunk);

        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";


        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        //  public override void _Process(float delta)
        //  {
        //      
        //  }
        public abstract void CreateRenderMesh(VoxelChunk chunk, Vector3 position, 
            out Array<Vector3> vertices,
            out Godot.Collections.Array<Vector3> normals,
            out Godot.Collections.Array<int> indices,
            out Godot.Collections.Array<Vector2> uvs);
        public abstract void CreateCollisionMesh(VoxelChunk chunk, Vector3 position, 
            out Array<Vector3> vertices,
            out Array<int> indicees);
    }
}
