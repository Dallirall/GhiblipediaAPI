namespace GhiblipediaAPI.Services
{
    public class OmdbAPIOptions
    {
        public string? ApiKey { get; set; } //Key for OMDb API. Value is set from an environment variable or appsettings.Local.json.
    }
}
