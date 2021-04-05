using Godot;
using System;
using Godot.Collections;

namespace LinwoodWorld.System
{
    public class VoxelWorld : TextureRect
    {
        [Export]
        private NodePath modLoader;
        private Array<ModResources> modResources;
        private Array<string> textures = new Array<string>();
        private Dictionary<string, Vector2> textureCoords = new Dictionary<string, Vector2>();

        private ImageTexture BuildTileSet()
        {
            var tex = new ImageTexture();
            var img = new Image();
            var size = textures.Count * 64;
            var sizeX = 512;
            var sizeY = (size / 512 + 1) * 64;
            GD.Print(sizeX, ", ", sizeY);
            img.Create(sizeX, sizeY, false, Image.Format.Rgba8);
            
            for (int i = 0; i < textures.Count; i++)
            {
                var texture = textures[i];
                var resource = GD.Load<StreamTexture>(texture);
                var image = resource.GetData();
                image.Convert(Image.Format.Rgba8);
                var x = i == 0 ? 0 : i * image.GetWidth() % sizeX;
                var y = i == 0 ? 0 : i * image.GetWidth() / sizeX * 64;
                var coord = new Vector2(x, y);
                GD.Print(coord, ", ", image.GetUsedRect());
                img.BlitRect(image, image.GetUsedRect(), coord);
                textureCoords[texture] = coord;
            }
            img.SavePng("res://textures/tileset.png");
            tex.CreateFromImage(img);
            return tex;
        }
        public override void _Ready()
        {
            GD.Print(textureCoords);
            LoadMods();
            Texture = BuildTileSet();
        }

        private void LoadMods()
        {
            var modLoaderNode = GetNode<ModLoader>(modLoader);
            modLoaderNode.Setup();
            var mods = modLoaderNode.Mods;
            modResources = new Array<ModResources>();
            foreach (var mod in mods)
            {
                LoadMod(mod);
            }
        }
        private void LoadMod(Mod mod)
        {
            var directory = new Directory();
            var error = directory.Open($"res://mods/{mod.Path}/blocks/");
            if(error != Error.Ok)
                return;
            directory.ListDirBegin(true);
            var fileName = directory.GetNext();
            var blocks = new Array<Block>();
            while(!fileName.Empty()){
                fileName = directory.GetNext();
            }

            
            directory = new Directory();
            error = directory.Open($"res://mods/{mod.Path}/textures/");
            if(error != Error.Ok)
                return;
            directory.ListDirBegin(true);
            fileName = directory.GetNext();
            while(!fileName.Empty()){
                GD.Print(fileName);
                var resource = GD.Load<StreamTexture>($"res://mods/{mod.Path}/textures/{fileName}");
                if(resource != null)
                    textures.Add(resource.ResourcePath);
                fileName = directory.GetNext();
            }
            GD.Print(textures);
            modResources.Add(new ModResources(mod, blocks));
        }

        internal void SetWorldVoxel(object v)
        {
            throw new NotImplementedException();
        }

        public Vector2 GetTextureCoord(String path){
            return textureCoords[path];
        }
    }
}