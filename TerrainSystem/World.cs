using Godot;
using System;
using Godot.Collections;

public class VoxelWorld
{
    ImageTexture BuildTileSet(Array<Image> images, out Array<Vector2> coords)
    {
        var tex = new ImageTexture();
        var img = new Image();
        img.Lock();
        img.Create(400, 400, false, Image.Format.Rgb8);

        coords = new Array<Vector2>();
        foreach (var image in images)
        {
            var coord = new Vector2(0,0);
            img.BlitRect(image, image.GetUsedRect(), coord);
            coords.Add(coord);
        }
        tex.CreateFromImage(img);
        img.SavePng("res://TerrainSystem/tileset.png");
        return tex;
    }
}