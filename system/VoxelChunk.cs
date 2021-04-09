using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace LinwoodWorld.WorldSystem
{
    public class VoxelChunk : Spatial
    {
        private VoxelWorld world;
        private string[,,] voxels;
        private Vector3 chunkSize;
        private int voxelSize;
        private SurfaceTool surfaceTool;
        public List<Vector2> meshuvs;
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
        }
        public void MakeStarterTerrain()
        {
            for (int x = 0; x < chunkSize.x; x++)
            {
                for (int y = 0; y < chunkSize.y / 2; y++)
                {
                    for (int z = 0; z < chunkSize.z; z++)
                    {
                        var position = new Vector3(x, y, z);
                        var globPos = GlobalTransform.Xform(position);
                        if (y == 1)
                            if (globPos.z % 30 == (int)(Mathf.Sin(globPos.x / 15) * 15) + 15)
                                voxels[x, y, z] = "res://mods/main/blocks/GrassBlock.cs";
                            else if (globPos.z % 30 == (int)(Mathf.Cos(globPos.x / 15) * 15) + 15)
                                voxels[x, y, z] = "res://mods/main/blocks/GrassBlock.cs";
                            else
                                voxels[x, y, z] = "res://mods/main/blocks/Dirt.cs";
                        /*if(globPos.z % 4 == 0 && globPos.x % 4 == 0 && globPos.y % 4 == 0)
                            voxels[x,y,z] = "res://mods/main/blocks/GrassBlock.cs"; */
                        /* if (y + 1 == chunkSize.y / 2 && x == chunkSize.x / 2 && z == chunkSize.z / 2)
                            voxels[x, y, z] = "res://mods/main/blocks/GrassBlock.cs";
                        else if (y >= chunkSize.y / 4)
                            voxels[x, y, z] = "res://mods/main/blocks/Dirt.cs";
                        else
                            voxels[x, y, z] = "res://mods/main/blocks/Stone.cs"; */
                    }
                }
            }
            UpdateMesh();

        }

        public bool VoxelInBounds(Vector3 position)
        {
            if (position.x < 0 || position.x >= chunkSize.x)
                return false;
            if (position.y < 0 || position.y >= chunkSize.x)
                return false;
            if (position.z < 0 || position.z >= chunkSize.x)
                return false;
            return true;
        }

        public void UpdateMesh()
        {
            var sw = new Stopwatch();
            sw.Start();
            var renderVertices = new List<Vector3>();
            var renderNormals = new List<Vector3>();
            var renderIndices = new List<int>();
            var renderUvs = new List<Vector2>();

            var collisionVertices = new List<Vector3>();
            var collisionIndices = new List<int>();

            GD.Print(sw.ElapsedMilliseconds);
            for (int x = 0; x < chunkSize.x; x++)
            {

                for (int y = 0; y < chunkSize.y; y++)
                {

                    for (int z = 0; z < chunkSize.z; z++)
                    {
                        List<Vector3> currentCollisionVertices;
                        List<int> currentCollisionIndices;
                        List<Vector3> currentRenderVertices;
                        List<Vector3> currentRenderNormals;
                        List<int> currentRenderIndices;
                        List<Vector2> currentRenderUvs;
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
            GD.Print(sw.ElapsedMilliseconds);
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
            var mesh = surfaceTool.Commit();
            meshInstance.Mesh = mesh;

            GD.Print(sw.ElapsedMilliseconds);
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
            collisionShape.CallDeferred("set_shape", surfaceTool.Commit(new ArrayMesh()).CreateTrimeshShape());
            sw.Stop();
            GD.Print(sw.ElapsedMilliseconds);
        }
        protected bool IsVoxelInBounds(Vector3 position)
        {
            return position.x >= 0 && position.x < chunkSize.x &&
                position.y >= 0 && position.y < chunkSize.y &&
                position.z >= 0 && position.z < chunkSize.z;
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
            UpdateMesh();
            return true;
        }
    }
}