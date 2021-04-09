using Godot;
using LinwoodWorld.WorldSystem;
using System;

namespace LinwoodWorld.Main
{
    public class Stone : CubitBlock
    {
        public override BlockTexture Texture => new BlockTexture("res://mods/main/textures/stone.png");
    }
}
