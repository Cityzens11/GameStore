using System.Text.RegularExpressions;

namespace GameStore.Web;

public static class Settings
{
    public static string ApiRoot = "http://localhost:5062/api";

    public static string IdentityRoot = "http://localhost:5148";
    public static string ClientId = "frontend";
    public static string ClientSecret = "secret";

    public static readonly Regex JwtTokenPattern = new Regex(@"^([A-Za-z0-9-_]+\.){2}[A-Za-z0-9-_]+\b");
}
