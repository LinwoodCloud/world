using Godot;
using Godot.Collections;

namespace LinwoodWorld.System
{
    public class ModLoader : Node
    {
        private Array<Mod> mods;
        public Array<Mod> Mods { get => new Array<Mod>(mods); }


        [Signal]
        public delegate void ModsInitialized(Array<Mod> mods);

        public override void _Ready(){
            mods = new Array<Mod>();
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
            var configFile = new ConfigFile();
            configFile.Load($"res://mods/{name}/mod.cfg");
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