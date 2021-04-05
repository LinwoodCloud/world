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

        public override void _Ready()
        {
            surfaceTool = new SurfaceTool();
        }

        public void Setup(VoxelWorld voxelWorld, Vector3 chunkSize, int voxelUnitSize)
        {
            world = voxelWorld;
            this.chunkSize = chunkSize;
            voxels = new string[(int) chunkSize.x, (int) chunkSize.y, (int) chunkSize.z];
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
            for (int x = 0; x < chunkSize.x; x++)
            {

                for (int y = 0; x < chunkSize.y; y++)
                {

                    for (int z = 0; z < chunkSize.z; z++)
                    {
                        MakeVoxel(new Vector3(x, y, z));
                    }
                }
            }
        }
        protected bool IsVoxelInBounds(Vector3 coords)
        {
            return coords.x >= 0 || coords.x <= chunkSize.x - 1 ||
                coords.y >= 0 || coords.x <= chunkSize.y - 1 ||
                coords.z >= 0 || coords.x <= chunkSize.z - 1;
        }
        public void MakeVoxel(Vector3 coords)
        {

        }
    }
}