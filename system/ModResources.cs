using System;
using Godot;
using System.Collections.Generic;

namespace LinwoodWorld.WorldSystem
{
    public class ModResources : Godot.Object
    {
        public Mod Mod { get; }
        public List<Block> Blocks { get; }

        public ModResources(Mod mod, List<Block> blocks)
        {
            Mod = mod;
            Blocks = blocks;
        }
    }
}