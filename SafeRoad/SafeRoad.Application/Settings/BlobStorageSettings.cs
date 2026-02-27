
namespace SafeRoad.Core.Settings;

public class BlobStorageSettings
{
    public string ConnectionString { get; set; } = null!;
    public string ContainerName { get; set; } = "incident-photos";
}