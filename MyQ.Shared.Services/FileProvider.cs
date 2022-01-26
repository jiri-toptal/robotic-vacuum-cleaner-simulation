using MyQ.Shared.Services.Abstractions;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace MyQ.Shared.Services
{
    // todo: move to services folder...
    public class FileProvider : IFileProvider
    {
        public string ApplicationRootFolderPath => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        public Task<string> ReadSourceFileAsync(string fileName) => File.ReadAllTextAsync(Path.Combine(ApplicationRootFolderPath, fileName));

        public Task WriteAllTextAsync(string content, string fileName) => File.WriteAllTextAsync(Path.Combine(ApplicationRootFolderPath, fileName), content);
    }
}
