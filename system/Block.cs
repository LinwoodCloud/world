using Godot;
using System.Collections.Generic;

namespace LinwoodWorld.WorldSystem
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
            out List<Vector3> renderVertices,
            out List<Vector3> renderNormals,
            out List<int> renderIndices,
            out List<Vector2> renderUvs,
            out List<Vector3> collisionVertices,
            out List<int> collisionIndices);
    }
}
