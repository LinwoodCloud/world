using Godot;
using Godot.Collections;
using LinwoodWorld.Main;

namespace LinwoodWorld.System
{
    public abstract class CubitBlock : Block
    {
        public enum Face
        {
            TOP, BOTTOM, EAST, WEST, NORTH, SOUTH
        }
        public struct BlockTexture
        {
            public string North { get; }
            public string South { get; }
            public string West { get; }
            public string East { get; }
            public string Top { get; }
            public string Bottom { get; }
            public BlockTexture(string all)
            {
                North = all;
                South = all;
                West = all;
                East = all;
                Top = all;
                Bottom = all;
            }
            public BlockTexture(string side, string topBottom)
            {
                North = side;
                South = side;
                West = side;
                East = side;
                Top = topBottom;
                Bottom = topBottom;
            }
            public BlockTexture(string side, string top, string bottom)
            {
                North = side;
                South = side;
                West = side;
                East = side;
                Top = top;
                Bottom = bottom;
            }
            public BlockTexture(string north, string south, string west, string east, string top, string bottom)
            {
                North = north;
                South = south;
                West = west;
                East = east;
                Top = top;
                Bottom = bottom;
            }
            public string GetTextureByFace(Face face)
            {
                switch (face)
                {
                    case Face.NORTH:
                        return North;
                    case Face.SOUTH:
                        return South;
                    case Face.WEST:
                        return West;
                    case Face.EAST:
                        return East;
                    case Face.TOP:
                        return Top;
                    case Face.BOTTOM:
                        return Bottom;
                    default:
                        return null;
                }
            }
        }
        public string Name
        {
            get;
        }
        public bool Transparent => false;
        public bool Solid => true;
        public abstract BlockTexture Texture { get; }

        public override bool CausedRender(VoxelChunk chunk, Vector3 position)
        {
            return Transparent || !Solid;
        }
        public override void CreateMesh(VoxelChunk chunk, Vector3 position, int verticesCount,
            out Array<Vector3> renderVertices,
            out Godot.Collections.Array<Vector3> renderNormals,
            out Godot.Collections.Array<int> renderIndices,
            out Godot.Collections.Array<Vector2> renderUvs,
            out Array<Vector3> collisionVertices,
            out Array<int> collisionIndices)
        {
            renderVertices = new Array<Vector3>();
            renderNormals = new Array<Vector3>();
            renderIndices = new Array<int>();
            renderUvs = new Array<Vector2>();
            collisionVertices = new Array<Vector3>();
            collisionIndices = new Array<int>();
            if (chunk.GetVoxel(position) == null)
                return;

            var topPos = new Vector3(position.x, position.y + 1, position.z);
            var bottomPos = new Vector3(position.x, position.y - 1, position.z);
            var eastPos = new Vector3(position.x + 1, position.y, position.z);
            var westPos = new Vector3(position.x - 1, position.y, position.z);
            var northPos = new Vector3(position.x, position.y, position.z + 1);
            var southPos = new Vector3(position.x, position.y, position.z - 1);

            var topRenderVertices = new Array<Vector3>();
            var topRenderNormals = new Array<Vector3>();
            var topRenderIndices = new Array<int>();
            var topRenderUvs = new Array<Vector2>();
            var topCollisionVertices = new Array<Vector3>();
            var topCollisionIndices = new Array<int>();
            if (chunk.VoxelInBounds(topPos))
            {
                if (chunk.CausedRender(topPos))
                    CreateFace(chunk, position, Face.TOP, verticesCount, out topRenderVertices, out topRenderNormals, out topRenderIndices, out topRenderUvs, out topCollisionVertices, out topCollisionIndices);
            }
            else
                CreateFace(chunk, position, Face.TOP, verticesCount, out topRenderVertices, out topRenderNormals, out topRenderIndices, out topRenderUvs, out topCollisionVertices, out topCollisionIndices);
            verticesCount += topRenderVertices.Count;

            var bottomRenderVertices = new Array<Vector3>();
            var bottomRenderNormals = new Array<Vector3>();
            var bottomRenderIndices = new Array<int>();
            var bottomRenderUvs = new Array<Vector2>();
            var bottomCollisionVertices = new Array<Vector3>();
            var bottomCollisionIndices = new Array<int>();
            if (chunk.VoxelInBounds(bottomPos))
            {
                if (chunk.CausedRender(bottomPos))
                    CreateFace(chunk, position, Face.BOTTOM, verticesCount, out bottomRenderVertices, out bottomRenderNormals, out bottomRenderIndices, out bottomRenderUvs, out bottomCollisionVertices, out bottomCollisionIndices);
            }
            CreateFace(chunk, position, Face.BOTTOM, verticesCount, out bottomRenderVertices, out bottomRenderNormals, out bottomRenderIndices, out bottomRenderUvs, out bottomCollisionVertices, out bottomCollisionIndices);
            verticesCount += bottomRenderVertices.Count;

            var eastRenderVertices = new Array<Vector3>();
            var eastRenderNormals = new Array<Vector3>();
            var eastRenderIndices = new Array<int>();
            var eastRenderUvs = new Array<Vector2>();
            var eastCollisionVertices = new Array<Vector3>();
            var eastCollisionIndices = new Array<int>();
            if (chunk.VoxelInBounds(eastPos))
            {
                if (chunk.CausedRender(eastPos))
                    CreateFace(chunk, position, Face.EAST, verticesCount, out eastRenderVertices, out eastRenderNormals, out eastRenderIndices, out eastRenderUvs, out eastCollisionVertices, out eastCollisionIndices);
            }
            else
                CreateFace(chunk, position, Face.EAST, verticesCount, out eastRenderVertices, out eastRenderNormals, out eastRenderIndices, out eastRenderUvs, out eastCollisionVertices, out eastCollisionIndices);
            verticesCount += eastRenderVertices.Count;

            var westRenderVertices = new Array<Vector3>();
            var westRenderNormals = new Array<Vector3>();
            var westRenderIndices = new Array<int>();
            var westRenderUvs = new Array<Vector2>();
            var westCollisionVertices = new Array<Vector3>();
            var westCollisionIndices = new Array<int>();
            if (chunk.VoxelInBounds(westPos))
            {
                if (chunk.CausedRender(westPos))
                    CreateFace(chunk, position, Face.WEST, verticesCount, out westRenderVertices, out westRenderNormals, out westRenderIndices, out westRenderUvs, out westCollisionVertices, out westCollisionIndices);
            }
            else
                CreateFace(chunk, position, Face.WEST, verticesCount, out westRenderVertices, out westRenderNormals, out westRenderIndices, out westRenderUvs, out westCollisionVertices, out westCollisionIndices);
            verticesCount += westRenderVertices.Count;

            var northRenderVertices = new Array<Vector3>();
            var northRenderNormals = new Array<Vector3>();
            var northRenderIndices = new Array<int>();
            var northRenderUvs = new Array<Vector2>();
            var northCollisionVertices = new Array<Vector3>();
            var northCollisionIndices = new Array<int>();
            if (chunk.VoxelInBounds(northPos))
            {
                if (chunk.CausedRender(northPos))
                    CreateFace(chunk, position, Face.NORTH, verticesCount, out northRenderVertices, out northRenderNormals, out northRenderIndices, out northRenderUvs, out northCollisionVertices, out northCollisionIndices);
            }
            else
                CreateFace(chunk, position, Face.NORTH, verticesCount, out northRenderVertices, out northRenderNormals, out northRenderIndices, out northRenderUvs, out northCollisionVertices, out northCollisionIndices);
            verticesCount += northRenderVertices.Count;

            var southRenderVertices = new Array<Vector3>();
            var southRenderNormals = new Array<Vector3>();
            var southRenderIndices = new Array<int>();
            var southRenderUvs = new Array<Vector2>();
            var southCollisionVertices = new Array<Vector3>();
            var southCollisionIndices = new Array<int>();
            if (chunk.VoxelInBounds(southPos))
            {
                if (chunk.CausedRender(southPos))
                    CreateFace(chunk, position, Face.SOUTH, verticesCount, out southRenderVertices, out southRenderNormals, out southRenderIndices, out southRenderUvs, out southCollisionVertices, out southCollisionIndices);
            }
            else
                CreateFace(chunk, position, Face.SOUTH, verticesCount, out southRenderVertices, out southRenderNormals, out southRenderIndices, out southRenderUvs, out southCollisionVertices, out southCollisionIndices);
            verticesCount += southRenderVertices.Count;

            renderVertices = ObjectUtils.ConcatArrays(topRenderVertices, bottomRenderVertices, eastRenderVertices, westRenderVertices, northRenderVertices, southRenderVertices);
            renderNormals = ObjectUtils.ConcatArrays(topRenderNormals, bottomRenderNormals, eastRenderNormals, westRenderNormals, northRenderNormals, southRenderNormals);
            renderIndices = ObjectUtils.ConcatArrays(topRenderIndices, bottomRenderIndices, eastRenderIndices, westRenderIndices, northRenderIndices, southRenderIndices);
            renderUvs = ObjectUtils.ConcatArrays(topRenderUvs, bottomRenderUvs, eastRenderUvs, westRenderUvs, northRenderUvs, southRenderUvs);
            collisionVertices = ObjectUtils.ConcatArrays(topCollisionVertices, bottomCollisionVertices, eastCollisionVertices, westCollisionVertices, northCollisionVertices, southCollisionVertices);
            collisionIndices = ObjectUtils.ConcatArrays(topCollisionIndices, bottomCollisionIndices, eastCollisionIndices, westCollisionIndices, northCollisionIndices, southCollisionIndices);
        }

        protected void CreateFace(VoxelChunk chunk, Vector3 position, Face face, int verticesCount,
            out Array<Vector3> renderVertices, out Array<Vector3> renderNormals, out Array<int> renderIndices, out Array<Vector2> renderUvs, out Array<Vector3> collisionVertices, out Array<int> collisionIndices)
        {
            var uvPos = chunk.World.GetTexturePosition(Texture.GetTextureByFace(face));
            var vSize = chunk.World.voxelUnitSize;
            var sizedPos = position * vSize;

            renderVertices = new Array<Vector3>();
            renderNormals = new Array<Vector3>();
            renderIndices = new Array<int>();
            renderUvs = new Array<Vector2>();
            collisionIndices = new Array<int>();
            collisionVertices = new Array<Vector3>();
            switch (face)
            {
                case Face.TOP:
                    renderVertices.Add(new Vector3(sizedPos.x, sizedPos.y + vSize, sizedPos.z));
                    renderVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y + vSize, sizedPos.z));
                    renderVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y + vSize, sizedPos.z + vSize));
                    renderVertices.Add(new Vector3(sizedPos.x, sizedPos.y + vSize, sizedPos.z + vSize));
                    renderNormals.Add(new Vector3(0, 1, 0));
                    renderNormals.Add(new Vector3(0, 1, 0));
                    renderNormals.Add(new Vector3(0, 1, 0));
                    renderNormals.Add(new Vector3(0, 1, 0));
                    if (Solid)
                    {
                        collisionVertices.Add(new Vector3(sizedPos.x, sizedPos.y + vSize, sizedPos.z + vSize));
                        collisionVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y + vSize, sizedPos.z + vSize));
                        collisionVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y + vSize, sizedPos.z));
                        collisionVertices.Add(new Vector3(sizedPos.x, sizedPos.y + vSize, sizedPos.z));
                    }
                    break;
                case Face.BOTTOM:
                    renderVertices.Add(new Vector3(sizedPos.x, sizedPos.y, sizedPos.z + vSize));
                    renderVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y, sizedPos.z + vSize));
                    renderVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y, sizedPos.z));
                    renderVertices.Add(new Vector3(sizedPos.x, sizedPos.y, sizedPos.z));
                    renderNormals.Add(new Vector3(0, -1, 0));
                    renderNormals.Add(new Vector3(0, -1, 0));
                    renderNormals.Add(new Vector3(0, -1, 0));
                    renderNormals.Add(new Vector3(0, -1, 0));
                    if (Solid)
                    {
                        collisionVertices.Add(new Vector3(sizedPos.x, sizedPos.y, sizedPos.z + vSize));
                        collisionVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y, sizedPos.z + vSize));
                        collisionVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y, sizedPos.z));
                        collisionVertices.Add(new Vector3(sizedPos.x, sizedPos.y, sizedPos.z));
                    }
                    break;
                case Face.NORTH:
                    renderVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y, sizedPos.z + vSize));
                    renderVertices.Add(new Vector3(sizedPos.x, sizedPos.y, sizedPos.z + vSize));
                    renderVertices.Add(new Vector3(sizedPos.x, sizedPos.y + vSize, sizedPos.z + vSize));
                    renderVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y + vSize, sizedPos.z + vSize));
                    renderNormals.Add(new Vector3(0, 0, 1));
                    renderNormals.Add(new Vector3(0, 0, 1));
                    renderNormals.Add(new Vector3(0, 0, 1));
                    renderNormals.Add(new Vector3(0, 0, 1));
                    if (Solid)
                    {
                        collisionVertices.Add(new Vector3(sizedPos.x, sizedPos.y, sizedPos.z + vSize));
                        collisionVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y, sizedPos.z + vSize));
                        collisionVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y + vSize, sizedPos.z + vSize));
                        collisionVertices.Add(new Vector3(sizedPos.x, sizedPos.y + vSize, sizedPos.z + vSize));
                    }
                    break;
                case Face.SOUTH:
                    renderVertices.Add(new Vector3(sizedPos.x, sizedPos.y, sizedPos.z));
                    renderVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y, sizedPos.z));
                    renderVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y + vSize, sizedPos.z));
                    renderVertices.Add(new Vector3(sizedPos.x, sizedPos.y + vSize, sizedPos.z));
                    renderNormals.Add(new Vector3(0, 0, -1));
                    renderNormals.Add(new Vector3(0, 0, -1));
                    renderNormals.Add(new Vector3(0, 0, -1));
                    renderNormals.Add(new Vector3(0, 0, -1));
                    if (Solid)
                    {
                        collisionVertices.Add(new Vector3(sizedPos.x, sizedPos.y, sizedPos.z));
                        collisionVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y, sizedPos.z));
                        collisionVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y + vSize, sizedPos.z));
                        collisionVertices.Add(new Vector3(sizedPos.x, sizedPos.y + vSize, sizedPos.z));
                    }
                    break;
                case Face.EAST:
                    renderVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y, sizedPos.z));
                    renderVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y, sizedPos.z + vSize));
                    renderVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y + vSize, sizedPos.z + vSize));
                    renderVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y + vSize, sizedPos.z));
                    renderNormals.Add(new Vector3(1, 0, 0));
                    renderNormals.Add(new Vector3(1, 0, 0));
                    renderNormals.Add(new Vector3(1, 0, 0));
                    renderNormals.Add(new Vector3(1, 0, 0));
                    if (Solid)
                    {
                        collisionVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y, sizedPos.z + vSize));
                        collisionVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y, sizedPos.z));
                        collisionVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y + vSize, sizedPos.z));
                        collisionVertices.Add(new Vector3(sizedPos.x + vSize, sizedPos.y + vSize, sizedPos.z + vSize));
                    }
                    break;
                case Face.WEST:
                    renderVertices.Add(new Vector3(sizedPos.x, sizedPos.y, sizedPos.z + vSize));
                    renderVertices.Add(new Vector3(sizedPos.x, sizedPos.y, sizedPos.z));
                    renderVertices.Add(new Vector3(sizedPos.x, sizedPos.y + vSize, sizedPos.z));
                    renderVertices.Add(new Vector3(sizedPos.x, sizedPos.y + vSize, sizedPos.z + vSize));
                    renderNormals.Add(new Vector3(-1, 0, 0));
                    renderNormals.Add(new Vector3(-1, 0, 0));
                    renderNormals.Add(new Vector3(-1, 0, 0));
                    renderNormals.Add(new Vector3(-1, 0, 0));
                    if (Solid)
                    {
                        collisionVertices.Add(new Vector3(sizedPos.x, sizedPos.y, sizedPos.z + vSize));
                        collisionVertices.Add(new Vector3(sizedPos.x, sizedPos.y, sizedPos.z));
                        collisionVertices.Add(new Vector3(sizedPos.x, sizedPos.y + vSize, sizedPos.z));
                        collisionVertices.Add(new Vector3(sizedPos.x, sizedPos.y + vSize, sizedPos.z + vSize));
                    }
                    break;
            }

            var textureUnitX = 1.0f / ((float)chunk.AlbedoTexture.GetWidth() / 64f);
            var textureUnitY = 1.0f / ((float)chunk.AlbedoTexture.GetHeight() / 64f);
            var sizedUvPos = uvPos * new Vector2(textureUnitX, textureUnitY);
            renderUvs.Add(new Vector2(sizedUvPos.x, sizedUvPos.y + textureUnitY));
            renderUvs.Add(new Vector2(sizedUvPos.x + textureUnitX, sizedUvPos.y + textureUnitY));
            renderUvs.Add(new Vector2(sizedUvPos.x + textureUnitX, sizedUvPos.y));
            renderUvs.Add(new Vector2(sizedUvPos.x, sizedUvPos.y));

            renderIndices.Add(renderVertices.Count + verticesCount - 4);
            renderIndices.Add(renderVertices.Count + verticesCount - 3);
            renderIndices.Add(renderVertices.Count + verticesCount - 1);
            renderIndices.Add(renderVertices.Count + verticesCount - 3);
            renderIndices.Add(renderVertices.Count + verticesCount - 2);
            renderIndices.Add(renderVertices.Count + verticesCount - 1);

            if (Solid)
            {
                collisionIndices.Add(renderVertices.Count + verticesCount - 4);
                collisionIndices.Add(renderVertices.Count + verticesCount - 3);
                collisionIndices.Add(renderVertices.Count + verticesCount - 1);
                collisionIndices.Add(renderVertices.Count + verticesCount - 3);
                collisionIndices.Add(renderVertices.Count + verticesCount - 2);
                collisionIndices.Add(renderVertices.Count + verticesCount - 1);
            }
        }
    }
}