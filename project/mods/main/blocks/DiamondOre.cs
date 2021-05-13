using Godot;
using LinwoodWorld.WorldSystem;
using System;

namespace LinwoodWorld.Main
{
    public class DiamondOre : CubitBlock
    {
        public override BlockTexture Texture => new BlockTexture("res://mods/main/textures/diamond_ore.png");
    }
}
