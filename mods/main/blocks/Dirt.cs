using Godot;
using LinwoodWorld.WorldSystem;
using System;

namespace LinwoodWorld.Main
{
    public class Dirt : CubitBlock
    {
        public override BlockTexture Texture => new BlockTexture("res://mods/main/textures/dirt.png");
    }
}
