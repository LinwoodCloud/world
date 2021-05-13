using Godot;
using LinwoodWorld.WorldSystem;
using System;

namespace LinwoodWorld.Main
{
    public class IronOre : CubitBlock
    {
        public override BlockTexture Texture => new BlockTexture("res://mods/main/textures/iron_ore.png");
    }
}
