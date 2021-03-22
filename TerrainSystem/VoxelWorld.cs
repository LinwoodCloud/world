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
            var size = images.Count * 64;
            var sizeX = 512 % size;
            var sizeY = 512 / size * 64;
            img.Create(sizeX, sizeY, false, Image.Format.Rgb8);

            coords = new Array<Vector2>();
            for (int i = 0; i < images.Count; i++)
            {
                var image = images[i];
                var imageSize = i*64;
                var x = 512 % imageSize;
                var y = 512 / imageSize * 64;
                var coord = new Vector2(x, y);
                img.BlitRect(image, image.GetUsedRect(), coord);
                coords.Add(coord);
            }
            tex.CreateFromImage(img);
            img.SavePng("res://TerrainSystem/tileset.png");
            return tex;
        }
    }
}