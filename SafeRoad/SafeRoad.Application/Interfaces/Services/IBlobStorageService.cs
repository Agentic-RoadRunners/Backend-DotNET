using System.IO;
using System.Threading.Tasks;

namespace SafeRoad.Core.Interfaces.Services;

public interface IBlobStorageService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);
    Task DeleteAsync(string blobUrl);
}