using Godot;
using Godot.Collections;
using LinwoodWorld.WorldSystem;
using System;
using System.Collections.Generic;

public class ModList : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void InitializeMods(List<Mod> mods)
    {
        foreach (Node child in GetChildren())
        {
            child.QueueFree();
        }

        foreach (var mod in mods)
        {
            AddChild(new CheckBox(){
                Text = mod.Name
            });
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
