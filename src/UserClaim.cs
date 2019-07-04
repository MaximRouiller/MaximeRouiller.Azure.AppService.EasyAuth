using Newtonsoft.Json;

public class UserClaim
{
    [JsonProperty("typ")]
    public string Type { get; set; }
    [JsonProperty("val")]
    public string Value { get; set; }
}
