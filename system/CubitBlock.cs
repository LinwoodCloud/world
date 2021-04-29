using Godot;
using LinwoodWorld.Main;
using System.Collections.Generic;

namespace LinwoodWorld.WorldSystem
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
        public override string UITexture => Texture.North;

        public override bool CausedRender(VoxelChunk chunk, Vector3 position)
        {
            return Transparent || !Solid;
        }
        public override void CreateMesh(VoxelChunk chunk, Vector3 position, int verticesCount,
            out List<Vector3> renderVertices,
            out List<Vector3> renderNormals,
            out List<int> renderIndices,
            out List<Vector2> renderUvs,
            out List<Vector3> collisionVertices,
            out List<int> collisionIndices, bool renderAllFaces = false)
        {
            renderVertices = new List<Vector3>();
            renderNormals = new List<Vector3>();
            renderIndices = new List<int>();
            renderUvs = new List<Vector2>();
            collisionVertices = new List<Vector3>();
            collisionIndices = new List<int>();
            if (chunk != null && chunk.GetVoxel(position) == null)
                return;

            var topPos = new Vector3(position.x, position.y + 1, position.z);
            var bottomPos = new Vector3(position.x, position.y - 1, position.z);
            var eastPos = new Vector3(position.x + 1, position.y, position.z);
            var westPos = new Vector3(position.x - 1, position.y, position.z);
            var northPos = new Vector3(position.x, position.y, position.z + 1);
            var southPos = new Vector3(position.x, position.y, position.z - 1);

            var topRenderVertices = new List<Vector3>();
            var topRenderNormals = new List<Vector3>();
            var topRenderIndices = new List<int>();
            var topRenderUvs = new List<Vector2>();
            var topCollisionVertices = new List<Vector3>();
            var topCollisionIndices = new List<int>();
            if (!renderAllFaces && chunk.VoxelInBounds(topPos))
            {
                if (chunk.CausedRender(topPos))
                    CreateFace(chunk, position, Face.TOP, verticesCount, out topRenderVertices, out topRenderNormals, out topRenderIndices, out topRenderUvs, out topCollisionVertices, out topCollisionIndices);
            }
            else
                CreateFace(chunk, position, Face.TOP, verticesCount, out topRenderVertices, out topRenderNormals, out topRenderIndices, out topRenderUvs, out topCollisionVertices, out topCollisionIndices);
            verticesCount += topRenderVertices.Count;
            renderVertices.AddRange(topRenderVertices);
            renderNormals.AddRange(topRenderNormals);
            renderIndices.AddRange(topRenderIndices);
            renderUvs.AddRange(topRenderUvs);
            collisionVertices.AddRange(topCollisionVertices);
            collisionIndices.AddRange(topCollisionIndices);

            var bottomRenderVertices = new List<Vector3>();
            var bottomRenderNormals = new List<Vector3>();
            var bottomRenderIndices = new List<int>();
            var bottomRenderUvs = new List<Vector2>();
            var bottomCollisionVertices = new List<Vector3>();
            var bottomCollisionIndices = new List<int>();
            if (!renderAllFaces && chunk.VoxelInBounds(bottomPos))
            {
                if (chunk.CausedRender(bottomPos))
                    CreateFace(chunk, position, Face.BOTTOM, verticesCount, out bottomRenderVertices, out bottomRenderNormals, out bottomRenderIndices, out bottomRenderUvs, out bottomCollisionVertices, out bottomCollisionIndices);
            }
            CreateFace(chunk, position, Face.BOTTOM, verticesCount, out bottomRenderVertices, out bottomRenderNormals, out bottomRenderIndices, out bottomRenderUvs, out bottomCollisionVertices, out bottomCollisionIndices);
            verticesCount += bottomRenderVertices.Count;
            renderVertices.AddRange(bottomRenderVertices);
            renderNormals.AddRange(bottomRenderNormals);
            renderIndices.AddRange(bottomRenderIndices);
            renderUvs.AddRange(bottomRenderUvs);
            collisionVertices.AddRange(bottomCollisionVertices);
            collisionIndices.AddRange(bottomCollisionIndices);

            var eastRenderVertices = new List<Vector3>();
            var eastRenderNormals = new List<Vector3>();
            var eastRenderIndices = new List<int>();
            var eastRenderUvs = new List<Vector2>();
            var eastCollisionVertices = new List<Vector3>();
            var eastCollisionIndices = new List<int>();
            if (chunk.VoxelInBounds(eastPos) && !renderAllFaces)
            {
                if (chunk.CausedRender(eastPos))
                    CreateFace(chunk, position, Face.EAST, verticesCount, out eastRenderVertices, out eastRenderNormals, out eastRenderIndices, out eastRenderUvs, out eastCollisionVertices, out eastCollisionIndices);
            }
            else
                CreateFace(chunk, position, Face.EAST, verticesCount, out eastRenderVertices, out eastRenderNormals, out eastRenderIndices, out eastRenderUvs, out eastCollisionVertices, out eastCollisionIndices);
            verticesCount += eastRenderVertices.Count;
            renderVertices.AddRange(eastRenderVertices);
            renderNormals.AddRange(eastRenderNormals);
            renderIndices.AddRange(eastRenderIndices);
            renderUvs.AddRange(eastRenderUvs);
            collisionVertices.AddRange(eastCollisionVertices);
            collisionIndices.AddRange(eastCollisionIndices);

            var westRenderVertices = new List<Vector3>();
            var westRenderNormals = new List<Vector3>();
            var westRenderIndices = new List<int>();
            var westRenderUvs = new List<Vector2>();
            var westCollisionVertices = new List<Vector3>();
            var westCollisionIndices = new List<int>();
            if (!renderAllFaces && chunk.VoxelInBounds(westPos))
            {
                if (chunk.CausedRender(westPos))
                    CreateFace(chunk, position, Face.WEST, verticesCount, out westRenderVertices, out westRenderNormals, out westRenderIndices, out westRenderUvs, out westCollisionVertices, out westCollisionIndices);
            }
            else
                CreateFace(chunk, position, Face.WEST, verticesCount, out westRenderVertices, out westRenderNormals, out westRenderIndices, out westRenderUvs, out westCollisionVertices, out westCollisionIndices);
            verticesCount += westRenderVertices.Count;
            renderVertices.AddRange(westRenderVertices);
            renderNormals.AddRange(westRenderNormals);
            renderIndices.AddRange(westRenderIndices);
            renderUvs.AddRange(westRenderUvs);
            collisionVertices.AddRange(westCollisionVertices);
            collisionIndices.AddRange(westCollisionIndices);

            var northRenderVertices = new List<Vector3>();
            var northRenderNormals = new List<Vector3>();
            var northRenderIndices = new List<int>();
            var northRenderUvs = new List<Vector2>();
            var northCollisionVertices = new List<Vector3>();
            var northCollisionIndices = new List<int>();
            if (!renderAllFaces && chunk.VoxelInBounds(northPos))
            {
                if (chunk.CausedRender(northPos))
                    CreateFace(chunk, position, Face.NORTH, verticesCount, out northRenderVertices, out northRenderNormals, out northRenderIndices, out northRenderUvs, out northCollisionVertices, out northCollisionIndices);
            }
            else
                CreateFace(chunk, position, Face.NORTH, verticesCount, out northRenderVertices, out northRenderNormals, out northRenderIndices, out northRenderUvs, out northCollisionVertices, out northCollisionIndices);
            verticesCount += northRenderVertices.Count;
            renderVertices.AddRange(northRenderVertices);
            renderNormals.AddRange(northRenderNormals);
            renderIndices.AddRange(northRenderIndices);
            renderUvs.AddRange(northRenderUvs);
            collisionVertices.AddRange(northCollisionVertices);
            collisionIndices.AddRange(northCollisionIndices);

            var southRenderVertices = new List<Vector3>();
            var southRenderNormals = new List<Vector3>();
            var southRenderIndices = new List<int>();
            var southRenderUvs = new List<Vector2>();
            var southCollisionVertices = new List<Vector3>();
            var southCollisionIndices = new List<int>();
            if (!renderAllFaces && chunk.VoxelInBounds(southPos))
            {
                if (chunk.CausedRender(southPos))
                    CreateFace(chunk, position, Face.SOUTH, verticesCount, out southRenderVertices, out southRenderNormals, out southRenderIndices, out southRenderUvs, out southCollisionVertices, out southCollisionIndices);
            }
            else
                CreateFace(chunk, position, Face.SOUTH, verticesCount, out southRenderVertices, out southRenderNormals, out southRenderIndices, out southRenderUvs, out southCollisionVertices, out southCollisionIndices);
            verticesCount += southRenderVertices.Count;
            renderVertices.AddRange(southRenderVertices);
            renderNormals.AddRange(southRenderNormals);
            renderIndices.AddRange(southRenderIndices);
            renderUvs.AddRange(southRenderUvs);
            collisionVertices.AddRange(southCollisionVertices);
            collisionIndices.AddRange(southCollisionIndices);
        }

        protected void CreateFace(VoxelChunk chunk, Vector3 position, Face face, int verticesCount,
            out List<Vector3> renderVertices, out List<Vector3> renderNormals, out List<int> renderIndices, out List<Vector2> renderUvs, out List<Vector3> collisionVertices, out List<int> collisionIndices)
        {
            var uvPos = chunk.World.GetTexturePosition(Texture.GetTextureByFace(face));
            var vSize = chunk.World.voxelUnitSize;
            var sizedPos = position * vSize;

            renderVertices = new List<Vector3>();
            renderNormals = new List<Vector3>();
            renderIndices = new List<int>();
            renderUvs = new List<Vector2>();
            collisionIndices = new List<int>();
            collisionVertices = new List<Vector3>();
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