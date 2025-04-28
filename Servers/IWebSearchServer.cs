public interface IWebSearchServer
{
    ValueTask<IEnumerable<string>> SearchAsync(string query);
}