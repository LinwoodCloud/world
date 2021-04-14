namespace LinwoodWorld.Level
{
    public enum Tool
    {
        Hand,
        Sword,
        Build
    }
    public static class ToolMethods
    {
        public static string GetAction(this Tool tool)
        {
            return "tool_" + tool.ToString().ToLower();
        }
    }
}