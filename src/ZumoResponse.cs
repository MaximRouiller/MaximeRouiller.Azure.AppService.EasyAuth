using Newtonsoft.Json;

public class ZumoResponse
{
    [JsonProperty("authenticationToken")]
    public string AuthenticationToken { get; set; }
}
