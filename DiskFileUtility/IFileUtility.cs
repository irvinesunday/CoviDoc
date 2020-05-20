using System.Threading.Tasks;

namespace FileService
{
    public interface IFileUtility
    {
        Task<string> ReadFromFileAsync(string filePathSource);

        Task WriteToFileAsync(string fileContents, string filePathSource);
    }
}
