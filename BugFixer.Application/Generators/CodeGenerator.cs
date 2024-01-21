namespace BugFixer.Application.Generators
{
    public static class CodeGenerator
    {
        public static string CreateActivationCode()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
