using System;
using Godot;
using System.Collections.Generic;

namespace LinwoodWorld.WorldSystem
{
    public class Mod : Godot.Object
    {
        public string Name { get; }
        public string Path { get; }
        public string Version { get; }
        public string Website { get; }
        public string Description { get; }
        public List<string> Blocks { get; }

        public Mod(string name, string version, string website, string description, List<string> blocks, string path)
        {
            Name = name;
            Path = path;
            Version = version;
            Website = website;
            Description = description;
            Blocks = blocks;
        }
        public Mod(){
            
        }

        public Mod(ConfigFile config, string path) :
            this(config.GetValue("general", "name") as string, config.GetValue("general", "version") as string, config.GetValue("general", "website") as string, config.GetValue("general", "description") as string, config.GetValue("general", "name") as List<string>, path)
        {

        }

        public bool Equals(Mod other)
        {
            if (other.Path == Path)
                return true;
            return false;
        }
    }
}