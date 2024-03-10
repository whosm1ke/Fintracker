using Fintracker.Application.Contracts.Helpers;
using Microsoft.AspNetCore.Hosting;
namespace Fintracker.Application.Helpers;
public class HtmlPagesHelper : IHtmlPageHelper
{
    private readonly IWebHostEnvironment _hostEnvironment;

    public  HtmlPagesHelper(IWebHostEnvironment host) => _hostEnvironment = host;

    public async Task<string> GetPageContent(string page)
    {
        return await File.ReadAllTextAsync(Path.Combine(_hostEnvironment.WebRootPath, "html", page));
    }
}