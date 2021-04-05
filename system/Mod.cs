using System;
using Godot;
using Godot.Collections;

namespace LinwoodWorld.System
{
    public class Mod : Godot.Object
    {
        public string Name { get; }
        public string Path { get; }
        public string Version { get; }
        public string Website { get; }
        public string Description { get; }
        public Array<string> Blocks { get; }

        public Mod(string name, string version, string website, string description, Array<string> blocks, string path)
        {
            Name = name;
            Path = path;
            Version = version;
            Website = website;
            Description = description;
            Blocks = blocks;
        }

        public Mod(ConfigFile config, string path) :
            this(config.GetValue("general", "name") as string, config.GetValue("general", "version") as string, config.GetValue("general", "website") as string, config.GetValue("general", "description") as string, config.GetValue("general", "name") as Array<string>, path)
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