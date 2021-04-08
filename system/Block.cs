using Godot;
using Godot.Collections;

namespace LinwoodWorld.System
{
    public abstract class Block : Godot.Object
    {
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
        public abstract void CreateMesh(VoxelChunk chunk, Vector3 position, int verticesCount, 
            out Array<Vector3> renderVertices,
            out Godot.Collections.Array<Vector3> renderNormals,
            out Godot.Collections.Array<int> renderIndices,
            out Godot.Collections.Array<Vector2> renderUvs,
            out Array<Vector3> collisionVertices,
            out Array<int> collisionIndices);
    }
}
