using Godot;
using System;

public abstract class Block
{
    public enum Face {
        TOP, BOTTOM, EAST, WEST, NORTH, SOUTH
    }
    public string name
    {
        get;
    }
    public bool Transparent
    {
        get;
    }
    public bool Solid
    {
        get;
    }

    public abstract void BuildVoxelFace(Chunk chunk);

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
