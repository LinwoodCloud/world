using Godot;
using System;
using Godot.Collections;

namespace LinwoodWorld.System
{
    public class VoxelChunk : Spatial
    {
        private VoxelWorld world;
        private string[,,] voxels;
        private Vector3 chunkSize;
        private int voxelSize;
        private SurfaceTool surfaceTool;
        public Array<Vector2> meshuvs;
        private MeshInstance meshInstance;
        private CollisionShape collisionShape;
        public VoxelWorld World => world;

        public override void _Ready()
        {

            surfaceTool = new SurfaceTool();
            meshInstance = GetNode<MeshInstance>("MeshInstance");
            collisionShape = GetNode<CollisionShape>("StaticBody/CollisionShape");

            meshInstance.MaterialOverride = new SpatialMaterial()
            {
                AlbedoTexture = world.Texture
            };
        }

        public void Setup(VoxelWorld voxelWorld, Vector3 chunkSize, int voxelUnitSize)
        {
            world = voxelWorld;
            this.chunkSize = chunkSize;
            voxels = new string[(int)chunkSize.x, (int)chunkSize.y, (int)chunkSize.z];
            MakeStarterTerrain();
        }
        public void MakeStarterTerrain()
        {
            for (int x = 0; x < chunkSize.x; x++)
            {
                for (int y = 0; y < chunkSize.y / 2; y++)
                {
                    for (int z = 0; z < chunkSize.z; z++)
                    {
                        if (y + 1 == chunkSize.y / 2)
                            voxels[x, y, z] = "res://mods/main/blocks/GrassBlock.cs";
                        else if (y >= chunkSize.y / 4)
                            voxels[x, y, z] = "res://mods/main/blocks/Dirt.cs";
                        else
                            voxels[x, y, z] = "res://mods/main/blocks/Stone.cs";
                    }
                }
            }
        }
        public void UpdateMesh()
        {
        }

        private void UpdateRenderMesh()
        {

            var renderVertices = new Array<Vector3>();
            var renderNormals = new Array<Vector3>();
            var renderIndices = new Array<int>();
            var renderUvs = new Array<Vector2>();
            for (int x = 0; x < chunkSize.x; x++)
            {

                for (int y = 0; x < chunkSize.y; y++)
                {

                    for (int z = 0; z < chunkSize.z; z++)
                    {
                        Array<Vector3> currentVertices;
                        Array<Vector3> currentNormals;
                        Array<int> currentIndices;
                        Array<Vector2> currentUvs;
                        var position = new Vector3(x, y, z);
                        world.GetBlock(GetVoxel(position)).CreateRenderMesh(this, position, out currentVertices, out currentNormals, out currentIndices, out currentUvs);
                        renderVertices = ObjectUtils.ConcatArrays(renderVertices, currentVertices);
                        renderNormals = ObjectUtils.ConcatArrays(renderNormals, currentNormals);
                        renderIndices = ObjectUtils.ConcatArrays(renderIndices, currentIndices);
                        renderUvs = ObjectUtils.ConcatArrays(renderUvs, currentUvs);
                    }
                }
            }
            surfaceTool.Clear();
            surfaceTool.Begin(Mesh.PrimitiveType.Triangles);
            for (int i = 0; i < renderVertices.Count; i++)
            {
                surfaceTool.AddNormal(renderNormals[i]);
                surfaceTool.AddUv(renderUvs[i]);
                surfaceTool.AddVertex(renderVertices[i]);
            }
            surfaceTool.GenerateTangents();
            meshInstance.Mesh = surfaceTool.Commit();
        }

        public bool VoxelInBounds(Vector3 position)
        {
            if (position.x < 0 || position.x + 1 > chunkSize.x)
                return false;
            if (position.y < 0 || position.y + 1 > chunkSize.x)
                return false;
            if (position.z < 0 || position.z + 1 > chunkSize.x)
                return false;
            return true;
        }

        private void UpdateCollisionMesh()
        {

            var collisionVertices = new Array<Vector3>();
            var collisionIndices = new Array<int>();
            for (int x = 0; x < chunkSize.x; x++)
            {

                for (int y = 0; x < chunkSize.y; y++)
                {

                    for (int z = 0; z < chunkSize.z; z++)
                    {
                        Array<Vector3> currentVertices;
                        Array<int> currentIndices;
                        var position = new Vector3(x, y, z);
                        world.GetBlock(GetVoxel(position)).CreateCollisionMesh(this, position, out currentVertices, out currentIndices);
                        collisionVertices = ObjectUtils.ConcatArrays(collisionVertices, currentVertices);
                        collisionIndices = ObjectUtils.ConcatArrays(collisionIndices, currentIndices);
                    }
                }
            }
            surfaceTool.Clear();
            surfaceTool.Begin(Mesh.PrimitiveType.Triangles);
            for (int i = 0; i < collisionVertices.Count; i++)
            {
                surfaceTool.AddVertex(collisionVertices[i]);
            }
            for (int i = 0; i < collisionIndices.Count; i++)
            {
                surfaceTool.AddIndex(collisionIndices[i]);
            }
            collisionShape.Shape = surfaceTool.Commit().CreateTrimeshShape();
        }
        protected bool IsVoxelInBounds(Vector3 coords)
        {
            return coords.x >= 0 || coords.x <= chunkSize.x - 1 ||
                coords.y >= 0 || coords.x <= chunkSize.y - 1 ||
                coords.z >= 0 || coords.x <= chunkSize.z - 1;
        }
        public string GetVoxel(Vector3 position)
        {
            return voxels[(int)position.x, (int)position.y, (int)position.z];
        }
        public bool CausedRender(Vector3 position)
        {
            return world.GetBlock(GetVoxel(position)).CausedRender(this, position);
        }
        public string Export()
        {
            return JSON.Print(voxels);
        }
        public void Import(string json)
        {
            var result = JSON.Parse(json);
            if (result.Error != Error.Ok)
                return;
            voxels = result.Result as string[,,];
            UpdateMesh();
        }

        public void SetVoxel(Vector3 position, string voxel)
        {
            voxels[(int)position.x, (int)position.y, (int)position.z] = voxel;
        }
    }
}