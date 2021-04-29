using Godot;
using LinwoodWorld.WorldSystem;
using System;
using System.Collections.Generic;

namespace LinwoodWorld.Particles
{

    public class BlockManipulation : MeshInstance
    {
        private SurfaceTool surfaceTool;
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        }

        public void Setup(VoxelChunk chunk, Vector3 position)
        {
            ((ShaderMaterial) MaterialOverride).SetShaderParam("block_texture", chunk.AlbedoTexture);
            surfaceTool = new SurfaceTool();
            var renderVertices = new List<Vector3>();
            var renderNormals = new List<Vector3>();
            var renderIndices = new List<int>();
            var renderUvs = new List<Vector2>();

            var collisionVertices = new List<Vector3>();
            var collisionIndices = new List<int>();
            chunk.GetBlock(position).CreateMesh(chunk, new Vector3(0, 0, 0), 0, out renderVertices, out renderNormals, out renderIndices, out renderUvs, out collisionVertices, out collisionIndices, true);

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
            Mesh = mesh;
        }

        public void OnTimeout()
        {
            QueueFree();
        }
    }
}
