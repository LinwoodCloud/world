using Godot;
using LinwoodWorld.WorldSystem;
using System;

namespace LinwoodWorld.Main
{
    public class CopperOre : CubitBlock
    {
        public override BlockTexture Texture => new BlockTexture("res://mods/main/textures/copper_ore.png");
    }
}
