using System.Threading.Tasks;

namespace MyQ.Shared.Services.Abstractions
{
    public interface IFileProvider
    {
        string ApplicationRootFolderPath { get; }

        Task<string> ReadSourceFileAsync(string fileName);

        Task WriteAllTextAsync(string content, string fileName);
    }
}
