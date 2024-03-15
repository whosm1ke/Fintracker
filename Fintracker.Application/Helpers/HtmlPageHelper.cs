using Fintracker.Application.Contracts.Helpers;

namespace Fintracker.Application.Helpers;
public class HtmlPageHelper : IHtmlPageHelper
{
    private readonly string _webRoot;

    public HtmlPageHelper(string webRoot) => _webRoot = webRoot; 

    public async Task<string> GetPageContent(string page)
    {
        return await File.ReadAllTextAsync(Path.Combine(_webRoot, "html", page));
    }
}