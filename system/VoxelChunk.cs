using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;

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
        public Texture AlbedoTexture => (meshInstance.MaterialOverride as SpatialMaterial).AlbedoTexture;

        public override void _Ready()
        {

        }

        public void Setup(VoxelWorld voxelWorld, Vector3 chunkSize, int voxelUnitSize)
        {
            world = voxelWorld;
            this.chunkSize = chunkSize;
            voxels = new string[(int)chunkSize.x, (int)chunkSize.y, (int)chunkSize.z];
            surfaceTool = new SurfaceTool();
            meshInstance = GetNode<MeshInstance>("MeshInstance");
            collisionShape = GetNode<CollisionShape>("StaticBody/CollisionShape");

            meshInstance.MaterialOverride = new SpatialMaterial()
            {
                AlbedoTexture = world.Texture
            };
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
                        if (y + 1 == chunkSize.y / 2 && x == chunkSize.x / 2 && z == chunkSize.z / 2)
                            voxels[x, y, z] = "res://mods/main/blocks/GrassBlock.cs";
                        else if (y >= chunkSize.y / 4)
                            voxels[x, y, z] = "res://mods/main/blocks/Dirt.cs";
                        else
                            voxels[x, y, z] = "res://mods/main/blocks/Stone.cs";
                    }
                }
            }
            UpdateMesh();
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

        public void UpdateMesh()
        {
            var renderVertices = new List<Vector3>();
            var renderNormals = new List<Vector3>();
            var renderIndices = new List<int>();
            var renderUvs = new List<Vector2>();

            var collisionVertices = new List<Vector3>();
            var collisionIndices = new List<int>();

            for (int x = 0; x < chunkSize.x; x++)
            {

                for (int y = 0; y < chunkSize.y; y++)
                {

                    for (int z = 0; z < chunkSize.z; z++)
                    {
                        Array<Vector3> currentCollisionVertices;
                        Array<int> currentCollisionIndices;
                        Array<Vector3> currentRenderVertices;
                        Array<Vector3> currentRenderNormals;
                        Array<int> currentRenderIndices;
                        Array<Vector2> currentRenderUvs;
                        var position = new Vector3(x, y, z);
                        var block = world.GetBlock(GetVoxel(position));
                        if (block != null)
                        {
                            block.CreateMesh(this, position, renderVertices.Count, out currentRenderVertices, out currentRenderNormals, out currentRenderIndices, out currentRenderUvs, 
                                out currentCollisionVertices, out currentCollisionIndices);
                            renderVertices.AddRange(currentRenderVertices);
                            renderNormals.AddRange(currentRenderNormals);
                            renderIndices.AddRange(currentRenderIndices);
                            renderUvs.AddRange(currentRenderUvs);
                            collisionVertices.AddRange(currentCollisionVertices);
                            collisionIndices.AddRange(currentCollisionIndices);
                        }
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
            for (int i = 0; i < renderIndices.Count; i++)
            {
                surfaceTool.AddIndex(renderIndices[i]);
            }
            surfaceTool.GenerateTangents();
            meshInstance.Mesh = surfaceTool.Commit();

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
        protected bool IsVoxelInBounds(Vector3 position)
        {
            return position.x >= 0 && position.x <= chunkSize.x - 1 &&
                position.y >= 0 && position.x <= chunkSize.y - 1 &&
                position.z >= 0 && position.x <= chunkSize.z - 1;
        }
        public string GetVoxel(Vector3 position)
        {
            if (IsVoxelInBounds(position))
                return voxels[(int)position.x, (int)position.y, (int)position.z];
            return null;
        }
        public bool CausedRender(Vector3 position)
        {
            var block = world.GetBlock(GetVoxel(position));
            if (block == null)
                return true;
            return block.CausedRender(this, position);
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

        public bool SetVoxel(Vector3 position, string voxel)
        {
            if (IsVoxelInBounds(position))
                voxels[(int)position.x, (int)position.y, (int)position.z] = voxel;
            else
                return false;
            return true;
        }
    }
}