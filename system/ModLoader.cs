using Godot;
using Godot.Collections;

namespace LinwoodWorld.System
{
    public class ModLoader : Node
    {
        private Array<Addon> addons;
        public Array<Addon> Addons { get => new Array<Addon>(addons); }

        public ModLoader(){

        }
        public void LoadAddon(string name){
            ProjectSettings.LoadResourcePack($"res://Mods/{name}.pck");
            var configFile = new ConfigFile();
            configFile.Load($"res://{name}/config.cfg");
            Addon addon = new Addon(configFile);
            addons.Add(addon);
        }
    }
}