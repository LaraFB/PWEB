﻿using System.Text.Json.Serialization;

namespace RCLApi.DTO;
public class Token
{
    //public string?  accesstoken {  get; set; }
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; }
    public string? tokentype { get; set; }
 //   public int? ExpiresIn { get; set; }
 //   public string? RefreshToken { get; set; }
    public string? utilizadorid { get; set; }
    public string? utilizadornome { get; set; }
}

