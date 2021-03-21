using System;
using System.Text;
using Godot;
using NLua;

namespace LinwoodWorld.System
{
    public class ScriptingNode : Spatial
    {
        public override void _Ready()
        {
            GD.Print("Lua engine start");
            using (Lua lua = new Lua())
            {
                lua.State.Encoding = Encoding.UTF8;
                lua.DoString("res = 'HELLO WORLD'");
                string res = (string)lua["res"];

                GD.Print(res);
            }
            GD.Print("Lua engine stop");
        }
    }
}