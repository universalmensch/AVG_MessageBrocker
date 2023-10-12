using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Geocoding{
     public static async Task Main(string[] args){
        List<KeyValuePair<string, string>> geoDaten = await getGeoDaten("DE","Karlsruhe","40+Moltkestraße");
        string latitude = geoDaten[0].Value;
        string longitude = geoDaten[1].Value;
        Console.WriteLine($"Latitude: {latitude}\nLongitude: {longitude}");
    }
    
    public static async Task<List<KeyValuePair<string, string>>> getGeoDaten(string country, string city, string street)
    {
        using var client = new HttpClient();
        var geoDaten = await client.GetStringAsync($"https://geocode.maps.co/search?country={country}&city={city}&street={street}");
        //Console.WriteLine(geoDaten);
        JArray geoArray = JArray.Parse(geoDaten);
        List<KeyValuePair<string, string>> latundLon = new List<KeyValuePair<string, string>>();

        foreach (JObject geoObject in geoArray)
        {
            if (geoObject.ContainsKey("lat") && geoObject.ContainsKey("lon"))
            {
                string latitude = geoObject["lat"].ToString();
                string longitude = geoObject["lon"].ToString();
                latundLon.Add(new KeyValuePair<string, string>("latitude", latitude));
                latundLon.Add(new KeyValuePair<string, string>("longitude", longitude));
            }
        }
        return latundLon;
    }
}