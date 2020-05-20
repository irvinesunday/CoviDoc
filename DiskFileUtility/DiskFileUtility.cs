using System.IO;
using System.Threading.Tasks;

namespace FileService
{
    public class DiskFileUtility : IFileUtility
    {
        public async Task<string> ReadFromFileAsync(string filePathSource)
        {
            using (StreamReader streamReader = new StreamReader(filePathSource))
            {
                return await streamReader.ReadToEndAsync();
            }
        }

        public async Task WriteToFileAsync(string fileContents, string filePathSource)
        {
            using (StreamWriter streamWriter = new StreamWriter(filePathSource))
            {
                await streamWriter.WriteLineAsync(fileContents);
            }
        }
    }
}
