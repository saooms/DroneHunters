using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace API
{
    public static class Map
    {
        public static async Task<byte[]> GetMapSegment(Vector3 coordinate) {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (platform; rv:geckoversion) Gecko/geckotrail Firefox/firefoxversion");
                client.DefaultRequestHeaders.Add("Accept", "*/*");
                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");

                HttpResponseMessage response = await client.GetAsync("https://tile.openstreetmap.org/" + coordinate.z + "/" + coordinate.x + "/" + coordinate.y + ".png");
                
                if (response.IsSuccessStatusCode)
                {
                    byte[] content = await response.Content.ReadAsByteArrayAsync();

                    return content;
                }
            }

            return null;
        }
    }
}
