using Godot;

namespace LinwoodWorld.System
{
    public abstract class Block
    {
		public abstract bool CausedRender(VoxelChunk chunk);
        //public abstract void BuildVoxelFace(Chunk chunk);

        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";


        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        //  public override void _Process(float delta)
        //  {
        //      
        //  }
		public abstract void Render(VoxelChunk chunk, out Godot.Collections.Array<Vector3> verticies, out Godot.Collections.Array<Vector3> normals, out Godot.Collections.Array<Vector3> collision);
    }
}
