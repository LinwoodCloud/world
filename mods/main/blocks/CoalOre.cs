using Godot;
using LinwoodWorld.WorldSystem;
using System;

namespace LinwoodWorld.Main
{
    public class CoalOre : CubitBlock
    {
        public override BlockTexture Texture => new BlockTexture("res://mods/main/textures/coal_ore.png");
    }
}
