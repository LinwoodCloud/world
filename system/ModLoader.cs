using Godot;
using System.Collections.Generic;

namespace LinwoodWorld.WorldSystem
{
    public class ModLoader : Node
    {
        private List<Mod> mods;
        public List<Mod> Mods { get => new List<Mod>(mods); }


        [Signal]
        public delegate void ModsInitialized(List<Mod> mods);

        public override void _Ready(){
            mods = new List<Mod>();
            GD.Print("LOADING MODLOADER...");
            EnableMod("main");
            EmitSignal(nameof(ModsInitialized), mods);
        }

        public void LoadAddon(string name, bool enable = true)
        {
            ProjectSettings.LoadResourcePack($"res://mods/{name}.pck");
            if (enable)
                EnableMod(name);
        }

        public void EnableMod(string name)
        {
            GD.Print("ENABLE MOD!");
            var configFile = new ConfigFile();
            var path = $"res://mods/{name}/mod.cfg";
            GD.Print(path);
            var file = new File();
            GD.Print(file.Open(path, File.ModeFlags.Read));
            GD.Print(file.GetAsText());
            file.Close();
            GD.Print(configFile.Load(path));
            Mod mod = new Mod(configFile, name);
            mods.Add(mod);
            var locales = TranslationServer.GetLoadedLocales();
            foreach (var locale in locales)
            {
                var translation = ResourceLoader.Load<Translation>($"res://mods/{name}/localization.{locale}.translation");
                if (translation != null){
                    TranslationServer.AddTranslation(translation);
                }
            }
        }
        public void DisableMod(string name)
        {
            var configFile = new ConfigFile();
            configFile.Load($"res://mods/{name}/mod.cfg");
            mods.Remove(new Mod(configFile, name));
            var locales = TranslationServer.GetLoadedLocales();
            foreach (var locale in locales)
            {
                var translation = ResourceLoader.Load<Translation>($"res://mods/{name}/localization.{locale}.translation");
                if (translation != null)
                    TranslationServer.RemoveTranslation(translation);
            }
        }
    }
}