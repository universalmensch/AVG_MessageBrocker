using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Geocoding{
     public static async Task Main(string[] args){
        List<KeyValuePair<string, string>> geoDaten = await getGeoDaten("DE","Karlsruhe","40+Moltkestraße","76133");
        string latitude = geoDaten[0].Value;
        string longitude = geoDaten[1].Value;
        Console.WriteLine($"Latitude: {latitude}\nLongitude: {longitude}");
    }
    
    public static async Task<List<KeyValuePair<string, string>>> getGeoDaten(string country, string city, string street, string postalcode)
    {
        using var client = new HttpClient();
        var geoDaten = await client.GetStringAsync($"https://geocode.maps.co/search?country={country}&city={city}&street={street}&postalcode={postalcode}");
        //Console.WriteLine(geoDaten);
        JArray jsonArray = JArray.Parse(geoDaten);
        List<KeyValuePair<string, string>> latundLon = new List<KeyValuePair<string, string>>();

        foreach (JObject item in jsonArray)
        {
            if (item.ContainsKey("lat") && item.ContainsKey("lon"))
            {
                string latitude = item["lat"].ToString();
                string longitude = item["lon"].ToString();
                latundLon.Add(new KeyValuePair<string, string>("latitude", latitude));
                latundLon.Add(new KeyValuePair<string, string>("longitude", longitude));
            }
        }
        return latundLon;
    }
}