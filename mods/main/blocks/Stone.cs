using Godot;
using LinwoodWorld.System;
using System;

namespace LinwoodWorld.Main
{
    public class Stone : CubitBlock
    {
        public override BlockTexture Texture => new BlockTexture("res://mods/main/textures/stone.png");
    }
}
