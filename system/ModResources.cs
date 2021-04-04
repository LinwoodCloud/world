using System;
using Godot;
using Godot.Collections;

namespace LinwoodWorld.System
{
    public class ModResources : Godot.Object
    {
        public Mod Mod { get; }
        public Array<Block> Blocks { get; }

        public ModResources(Mod mod, Array<Block> blocks)
        {
            Mod = mod;
            Blocks = blocks;
        }
    }
}