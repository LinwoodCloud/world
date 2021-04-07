using Godot;
using Godot.Collections;

namespace LinwoodWorld.System
{
    public abstract class CubitBlock : Block
    {
        public struct Texture {
            public string North {get; }
            public string South {get; }
            public string West {get; }
            public string East {get; }
            public string Top {get; }
            public string Bottom {get; }
            public Texture(string all){
                North = all;
                South = all;
                West = all;
                East = all;
                Top = all;
                Bottom = all;
            }
            public Texture(string side, string topBottom){
                North = side;
                South = side;
                West = side;
                East = side;
                Top = topBottom;
                Bottom = topBottom;
            }
            public Texture(string side, string top, string bottom){
                North = side;
                South = side;
                West = side;
                East = side;
                Top = top;
                Bottom = bottom;
            }
            public Texture(string north, string south, string west, string east, string top, string bottom){
                North = north;
                South = south;
                West = west;
                East = east;
                Top = top;
                Bottom = bottom;
            }
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
        public abstract Texture BlockTexture { get; }

        public override bool CausedRender(VoxelChunk chunk)
        {
            return Transparent || !Solid;
        }
        public override void CreateCollisionMesh(VoxelChunk chunk, Vector3 position, out Array<Vector3> vertices, out Array<int> indicees)
        {
            throw new global::System.NotImplementedException();
        }
        public override void CreateRenderMesh(VoxelChunk chunk, Vector3 position, out Array<Vector3> vertices, out Array<Vector3> normals, out Array<int> indices, out Array<Vector2> uvs)
        {
            throw new global::System.NotImplementedException();
        }
    }
}