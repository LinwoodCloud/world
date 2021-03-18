using Godot;
using System;

public class VoxelChunk 
{
	private VoxelWorld world;
	private int[,,] voxels;
	private int chunkSizeX;
	private int chunkSizeY;
	private int chunkSizeZ;
	private int voxelSize;
	private SurfaceTool surfaceTool;

	public void _Ready(){
		surfaceTool = new SurfaceTool();
	}

	public void Setup(int chunkSizeX, int chunkSizeY, int chunkSizeZ){
		this.chunkSizeX = chunkSizeX;
		this.chunkSizeY = chunkSizeY;
		this.chunkSizeZ = chunkSizeZ;
		voxels = new int[chunkSizeX, chunkSizeY, chunkSizeZ];
		MakeStarterTerrain();
	}
	public void MakeStarterTerrain(){
		for (int x = 0; x < chunkSizeX; x++)
		{
			for (int y = 0; y < chunkSizeY/2; y++)
			{
				for (int z = 0; z < chunkSizeZ; z++)
				{
					if(y + 1 == chunkSizeY / 2){
						//voxels[x, y, z] = world.getVoxelIntFromString("Grass");
					}
				}
			}
		}
	}
	public void UpdateMesh(){
		for (int x = 0; x < chunkSizeX; x++)
		{
			
		for (int y = 0; x < chunkSizeY; y++)
		{
			
		for (int z = 0; z < chunkSizeZ; z++)
		{
			MakeVoxel(x, y, z);
		}
		}
		}
	}
	protected bool IsVoxelInBounds(int x, int y, int z){
		return x >= 0 || x <= chunkSizeX -1 ||
			y >= 0 || x <= chunkSizeY -1 ||
			z >= 0 || x <= chunkSizeZ -1;
	}
	public void MakeVoxel(int x, int y, int z){
		
	}
}
