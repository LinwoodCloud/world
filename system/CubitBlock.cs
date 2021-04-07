using Godot;
using Godot.Collections;

namespace LinwoodWorld.System
{
    public abstract class CubitBlock : Block
    {
        public struct Texture
        {
            public string North { get; }
            public string South { get; }
            public string West { get; }
            public string East { get; }
            public string Top { get; }
            public string Bottom { get; }
            public Texture(string all)
            {
                North = all;
                South = all;
                West = all;
                East = all;
                Top = all;
                Bottom = all;
            }
            public Texture(string side, string topBottom)
            {
                North = side;
                South = side;
                West = side;
                East = side;
                Top = topBottom;
                Bottom = topBottom;
            }
            public Texture(string side, string top, string bottom)
            {
                North = side;
                South = side;
                West = side;
                East = side;
                Top = top;
                Bottom = bottom;
            }
            public Texture(string north, string south, string west, string east, string top, string bottom)
            {
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
        public bool Solid => true;
        public abstract Texture BlockTexture { get; }

        public override bool CausedRender(VoxelChunk chunk, Vector3 position)
        {
            return Transparent || !Solid;
        }
        public override void CreateCollisionMesh(VoxelChunk chunk, Vector3 position, out Array<Vector3> vertices, out Array<int> indicees)
        {
            throw new global::System.NotImplementedException();
        }
        public override void CreateRenderMesh(VoxelChunk chunk, Vector3 position, out Array<Vector3> vertices, out Array<Vector3> normals, out Array<int> indices, out Array<Vector2> uvs)
        {
            vertices = new Array<Vector3>();
            normals = new Array<Vector3>();
            indices = new Array<int>();
            uvs = new Array<Vector2>();
            if(chunk.GetVoxel(position) == null)
                return;
            var topPos = new Vector3(position.x, position.y +1, position.z);
            var bottomPos = new Vector3(position.x, position.y -1, position.z);
            var eastPos = new Vector3(position.x + 1, position.y, position.z);
            var westPos = new Vector3(position.x - 1, position.y, position.z);
            var northPos = new Vector3(position.x, position.y, position.z + 1);
            var southPos = new Vector3(position.x, position.y, position.z - 1);
            
            var topVertices = new Array<Vector3>();
            var topNormals = new Array<Vector3>();
            var topIndices = new Array<int>();
            var topUvs = new Array<Vector2>();
            if(chunk.VoxelInBounds(topPos))
                if(chunk.CausedRender(topPos))
                    CreateRenderFace(chunk, position, Face.TOP, out topVertices, out topNormals, out topIndices, out topUvs);
            
            var bottomVertices = new Array<Vector3>();
            var bottomNormals = new Array<Vector3>();
            var bottomIndices = new Array<int>();
            var bottomUvs = new Array<Vector2>();
            if(chunk.VoxelInBounds(bottomPos))
                if(chunk.CausedRender(bottomPos))
                    CreateRenderFace(chunk, position, Face.BOTTOM, out bottomVertices, out bottomNormals, out bottomIndices, out bottomUvs);
            
            var eastVertices = new Array<Vector3>();
            var eastNormals = new Array<Vector3>();
            var eastIndices = new Array<int>();
            var eastUvs = new Array<Vector2>();
            if(chunk.VoxelInBounds(eastPos))
                if(chunk.CausedRender(eastPos))
                    CreateRenderFace(chunk, position, Face.EAST, out eastVertices, out eastNormals, out eastIndices, out eastUvs);
            
            var westVertices = new Array<Vector3>();
            var westNormals = new Array<Vector3>();
            var westIndices = new Array<int>();
            var westUvs = new Array<Vector2>();
            if(chunk.VoxelInBounds(westPos))
                if(chunk.CausedRender(westPos))
                    CreateRenderFace(chunk, position, Face.WEST, out westVertices, out westNormals, out westIndices, out westUvs);
            
            var northVertices = new Array<Vector3>();
            var northNormals = new Array<Vector3>();
            var northIndices = new Array<int>();
            var northUvs = new Array<Vector2>();
            if(chunk.VoxelInBounds(northPos))
                if(chunk.CausedRender(northPos))
                    CreateRenderFace(chunk, position, Face.NORTH, out northVertices, out northNormals, out northIndices, out northUvs);
            
            var southVertices = new Array<Vector3>();
            var southNormals = new Array<Vector3>();
            var southIndices = new Array<int>();
            var southUvs = new Array<Vector2>();
            if(chunk.VoxelInBounds(southPos))
                if(chunk.CausedRender(southPos))
                    CreateRenderFace(chunk, position, Face.SOUTH, out southVertices, out southNormals, out southIndices, out southUvs);
            
            vertices = ObjectUtils.ConcatArrays(topVertices, bottomVertices, eastVertices, westVertices, northVertices, southVertices);
            normals = ObjectUtils.ConcatArrays(topNormals, bottomNormals, eastNormals, westNormals, northNormals, southNormals);
            indices = ObjectUtils.ConcatArrays(topIndices, bottomIndices, eastIndices, westIndices, northIndices, southIndices);
            uvs = ObjectUtils.ConcatArrays(topUvs, bottomUvs, eastUvs, westUvs, northUvs, southUvs);
        }

        protected void CreateRenderFace(VoxelChunk chunk, Vector3 position, Face face, out Array<Vector3> vertices, out Array<Vector3> normals, out Array<int> indices, out Array<Vector2> uvs)
        {
            throw new global::System.NotImplementedException();
        }
        protected void CreateCollisionFace(VoxelChunk chunk, Vector3 position, Face face, out Array<Vector3> vertices, out Array<int> indicees)
        {

            throw new global::System.NotImplementedException();
        }
    }
}