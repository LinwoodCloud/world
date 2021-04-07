using Godot;
using LinwoodWorld.System;
using System;

namespace LinwoodWorld.Main
{
    public class GrassBlock : CubitBlock
    {
        public override Texture BlockTexture => new Texture("res://mods/main/textures/grass_block_side.png", "res://mods/main/textures/grass_block_top.png", "res://mods/main/textures/dirt.png");
    }
}
