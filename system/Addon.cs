using Godot;
using Godot.Collections;

namespace LinwoodWorld.System
{
    public struct Addon
    {
        private string Name { get; }
        private string Version { get; }
        private string Website { get; }
        private string Description { get; }
        private Array<string> Blocks { get; }

        public Addon(string name, string version, string website, string description, Array<string> blocks)
        {
            Name = name;
            Version = version;
            Website = website;
            Description = description;
            Blocks = blocks;
        }

        public Addon(ConfigFile config) :
            this(config.GetValue("general", "name") as string, config.GetValue("general", "version") as string, config.GetValue("general", "website") as string, config.GetValue("general", "description") as string, config.GetValue("general", "name") as Array<string>)
        {

        }
    }
}