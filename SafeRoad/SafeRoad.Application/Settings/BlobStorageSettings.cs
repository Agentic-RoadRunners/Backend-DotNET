
namespace SafeRoad.Core.Settings;

public class SupabaseStorageSettings
{
    public string SupabaseUrl { get; set; } = null!;
    public string SupabaseServiceKey { get; set; } = null!;
    public string BucketName { get; set; } = "incident-photos";
}