namespace LinwoodWorld.Level
{
    public enum Tool
    {
        Hand,
        Sword,
        Build,
        Wrench
    }
    public static class ToolMethods
    {
        public static string GetAction(this Tool tool)
        {
            return $"{tool.ToString().ToLower()}_tool";
        }
    }
}