using Godot;
using LinwoodWorld.WorldSystem;
using System;

namespace LinwoodWorld.Main
{
    public class GrassBlock : CubitBlock
    {
        public override BlockTexture Texture => new BlockTexture("res://mods/main/textures/grass_block_side.png", "res://mods/main/textures/grass_block_top.png", "res://mods/main/textures/dirt.png");
    }
}
