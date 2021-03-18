using Godot;
using System;
using Godot.Collections;

namespace LinwoodWorld.System
{
    public class VoxelWorld : Spatial
    {
        private Array<Addon> addons;

        public Array<Addon> Addons { get => new Array<Addon>(addons); }


        public void RegisterAddon(Addon addon){
            addons.Add(addon);
        }
        public void UnregisterAddon(Addon addon){
            addons.Add(addon);
        }

        private ImageTexture BuildTileSet(Array<Image> images, out Array<Vector2> coords)
        {
            var tex = new ImageTexture();
            var img = new Image();
            img.Lock();
            img.Create(400, 400, false, Image.Format.Rgb8);

            coords = new Array<Vector2>();
            foreach (var image in images)
            {
                var coord = new Vector2(0, 0);
                img.BlitRect(image, image.GetUsedRect(), coord);
                coords.Add(coord);
            }
            tex.CreateFromImage(img);
            img.SavePng("res://TerrainSystem/tileset.png");
            return tex;
        }
    }
}