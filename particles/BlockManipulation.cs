using Godot;
using LinwoodWorld.WorldSystem;
using System;
using System.Collections.Generic;

namespace LinwoodWorld.Particles
{

    public class BlockManipulation : MeshInstance
    {
        private SurfaceTool surfaceTool;
        private Tween tween;
        private string block;
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            GetNode<AnimationPlayer>("AnimationPlayer").Play("fade_in");
            tween = GetNode<Tween>("Tween");
        }

        public void Setup(VoxelChunk voxelChunk, string block, Vector3 currentPos)
        {
            this.currentPos = currentPos;
            this.block = block;
            Translation = currentPos;
            ((ShaderMaterial)MaterialOverride).SetShaderParam("block_texture", voxelChunk.World.Texture);
            surfaceTool = new SurfaceTool();
            var renderVertices = new List<Vector3>();
            var renderNormals = new List<Vector3>();
            var renderIndices = new List<int>();
            var renderUvs = new List<Vector2>();

            var collisionVertices = new List<Vector3>();
            var collisionIndices = new List<int>();
            voxelChunk.World.GetBlock(block).CreateMesh(voxelChunk, new Vector3(0, 0, 0), 0, out renderVertices, out renderNormals, out renderIndices, out renderUvs, out collisionVertices, out collisionIndices, renderAllFaces: true);

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

        public void Stop()
        {
            GetNode<AnimationPlayer>("AnimationPlayer").Play("fade_out");
        }
        public void OnAnimationFinished(String name)
        {
            if (name == "fade_out")
            {
                EmitSignal(nameof(OnStop), currentPos, block);
                QueueFree();
            }
        }

        [Signal]
        public delegate void OnStop(Vector3 position, string block);
        private Vector3 currentPos;

        internal void ChangePosition(Vector3 position)
        {
            if (currentPos != position){
                //    GlobalTransform = new Transform(GlobalTransform.basis, position);
                tween.InterpolateProperty(this, "translation", Translation, position, 0.5f, Tween.TransitionType.Linear, Tween.EaseType.InOut);
                tween.Start();
            }
            currentPos = position;
        }
    }
}
