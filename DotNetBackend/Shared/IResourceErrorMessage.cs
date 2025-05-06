namespace DotNetBackend.Shared
{
    public interface IResourceErrorMessage
    {
        string GetMessage(string Key, string Param = null);
        string GetFormatedMessage(string Key, string Param, string Param2);
    }
}
