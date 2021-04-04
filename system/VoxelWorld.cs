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

        private ImageTexture BuildTileSet(Array<Texture> textures, out Array<Vector2> coords)
        {
            var tex = new ImageTexture();
            var img = new Image();
            var size = textures.Count * 64;
            var sizeX = 512;
            var sizeY = (size / 512 + 1) * 64;
            GD.Print(sizeX, ", ", sizeY);
            img.Create(sizeX, sizeY, false, Image.Format.Rgba8);

            coords = new Array<Vector2>();
            for (int i = 0; i < textures.Count; i++)
            {
                var texture = textures[i];
                var image = texture.GetData();
                image.Convert(Image.Format.Rgba8);
                var x = i == 0 ? 0 : i * image.GetWidth() % sizeX;
                var y = i == 0 ? 0 : i * image.GetWidth() / sizeX * 64;
                var coord = new Vector2(x, y);
                GD.Print(coord, ", ", image.GetUsedRect());
                img.BlitRect(image, image.GetUsedRect(), coord);
                coords.Add(coord);
            }
            img.SavePng("res://textures/tileset.png");
            tex.CreateFromImage(img);
            return tex;
        }
        public override void _Ready()
        {
            Array<Vector2> coords;
            var textures = new Array<Texture>();
            textures.Add(GD.Load<StreamTexture>("res://mods/main/blocks/textures/dirt.png"));
            textures.Add(GD.Load<StreamTexture>("res://mods/main/blocks/textures/grass_block_side.png"));
            textures.Add(GD.Load<StreamTexture>("res://mods/main/blocks/textures/grass_block_top.png"));
            textures.Add(GD.Load<StreamTexture>("res://mods/main/blocks/textures/stone.png"));
            textures.Add(GD.Load<StreamTexture>("res://mods/main/blocks/textures/stone.png"));
            textures.Add(GD.Load<StreamTexture>("res://mods/main/blocks/textures/stone.png"));
            textures.Add(GD.Load<StreamTexture>("res://mods/main/blocks/textures/stone.png"));
            textures.Add(GD.Load<StreamTexture>("res://mods/main/blocks/textures/stone.png"));
            textures.Add(GD.Load<StreamTexture>("res://mods/main/blocks/textures/stone.png"));
            textures.Add(GD.Load<StreamTexture>("res://mods/main/blocks/textures/stone.png"));
            textures.Add(GD.Load<StreamTexture>("res://mods/main/blocks/textures/stone.png"));
            textures.Add(GD.Load<StreamTexture>("res://mods/main/blocks/textures/stone.png"));
            textures.Add(GD.Load<StreamTexture>("res://mods/main/blocks/textures/stone.png"));
            textures.Add(GD.Load<StreamTexture>("res://mods/main/blocks/textures/stone.png"));
            textures.Add(GD.Load<StreamTexture>("res://mods/main/blocks/textures/stone.png"));
            textures.Add(GD.Load<StreamTexture>("res://mods/main/blocks/textures/stone.png"));
            textures.Add(GD.Load<StreamTexture>("res://mods/main/blocks/textures/stone.png"));
            textures.Add(GD.Load<StreamTexture>("res://mods/main/blocks/textures/dirt.png"));
            textures.Add(GD.Load<StreamTexture>("res://mods/main/blocks/textures/grass_block_side.png"));
            textures.Add(GD.Load<StreamTexture>("res://mods/main/blocks/textures/dirt.png"));
            textures.Add(GD.Load<StreamTexture>("res://mods/main/blocks/textures/grass_block_side.png"));
            Texture = BuildTileSet(textures, out coords);
            LoadMods();
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
            var error = directory.Open($"res://mods/{mod.Path}/blocks");
            if(error != Error.Ok)
                return;
            directory.ListDirBegin();
            var fileName = directory.GetNext();
            var blocks = new Array<Block>();
            while(!fileName.Empty() && fileName != "."){
                GD.Print(fileName);
            }
            modResources.Add(new ModResources(mod, blocks));
        }

        internal void SetWorldVoxel(object v)
        {
            throw new NotImplementedException();
        }
    }
}