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
            
            var renderVerticies = new Array<Vector3>();
            var renderNormals = new Array<Vector3>();
            var renderIndices = new Array<int>();
            var renderUvs = new Array<Vector2>();
            for (int x = 0; x < chunkSize.x; x++)
            {

                for (int y = 0; x < chunkSize.y; y++)
                {

                    for (int z = 0; z < chunkSize.z; z++)
                    {
                        Array<Vector3> currentVerticies;
                        Array<Vector3> currentNormals;
                        Array<int> currentIndices;
                        Array<Vector2> currentUvs;
                        world.GetBlock(GetVoxel(new Vector3(x, y, z))).CreateRenderMesh(this, out currentVerticies, out currentNormals, out currentIndices, out currentUvs);
                        renderVerticies = ConcatArrays(renderVerticies, currentVerticies);
                        renderNormals = ConcatArrays(renderNormals, currentNormals);
                        renderIndices = ConcatArrays(renderIndices, currentIndices);
                        renderUvs = ConcatArrays(renderUvs, currentUvs);
                    }
                }
            }
            surfaceTool.Clear();
            surfaceTool.Begin(Mesh.PrimitiveType.Triangles);
            for (int i = 0; i < renderVerticies.Count; i++)
            {
                surfaceTool.AddNormal(renderNormals[i]);
                surfaceTool.AddUv(renderUvs[i]);
                surfaceTool.AddVertex(renderVerticies[i]);
            }
            surfaceTool.GenerateTangents();
            meshInstance.Mesh = surfaceTool.Commit();
        }
        private void UpdateCollisionMesh()
        {
            
            var collisionVerticies = new Array<Vector3>();
            var collisionIndices = new Array<int>();
            for (int x = 0; x < chunkSize.x; x++)
            {

                for (int y = 0; x < chunkSize.y; y++)
                {

                    for (int z = 0; z < chunkSize.z; z++)
                    {
                        Array<Vector3> currentVerticies;
                        Array<int> currentIndices;
                        world.GetBlock(GetVoxel(new Vector3(x, y, z))).CreateCollisionMesh(this, out currentVerticies, out currentIndices);
                        collisionVerticies = ConcatArrays(collisionVerticies, currentVerticies);
                        collisionIndices = ConcatArrays(collisionIndices, currentIndices);
                    }
                }
            }
            surfaceTool.Clear();
            surfaceTool.Begin(Mesh.PrimitiveType.Triangles);
            for (int i = 0; i < collisionVerticies.Count; i++)
            {
                surfaceTool.AddVertex(collisionVerticies[i]);
            }
            for (int i = 0; i < collisionIndices.Count; i++)
            {
                surfaceTool.AddIndex(collisionIndices[i]);
            }
            collisionShape.Shape = surfaceTool.Commit().CreateTrimeshShape();
        }
        private Array<T> ConcatArrays<T>(Array<T> firstArray, Array<T> secondArray)
        {
            Array<T> array = new Array<T>();
            foreach (var item in firstArray)
            {
                array.Add(item);
            }
            foreach (var item in secondArray)
            {
                array.Add(item);
            }
            return array;
        }
        protected bool IsVoxelInBounds(Vector3 coords)
        {
            return coords.x >= 0 || coords.x <= chunkSize.x - 1 ||
                coords.y >= 0 || coords.x <= chunkSize.y - 1 ||
                coords.z >= 0 || coords.x <= chunkSize.z - 1;
        }
        public string GetVoxel(Vector3 coords)
        {
            return voxels[(int)coords.x, (int)coords.y, (int)coords.z];
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

        internal bool SetVoxel(Vector3 position, string voxel)
        {
            throw new NotImplementedException();
        }
    }
}