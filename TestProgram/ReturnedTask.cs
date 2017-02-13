using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestProgram
{
    internal class Haha
    {
        public async Task GarbageMan()
        {
            var url = "http://forecast.weather.gov/MapClick.php?lat=42&lon=-75&FcstType=dwml";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Stackoverflow/1.0");
            
            //var xml = await client.GetStringAsync(url);
            var xml = client.GetStringAsync(url);

            if (xml != null)
            {
                //get bent
            }


        }
        public async Task GarbageManWithCan()
        {
            var url = "http://forecast.weather.gov/MapClick.php?lat=42&lon=-75&FcstType=dwml";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Stackoverflow/1.0");
            
            //var xml = await client.GetStringAsync(url);
            if (client.GetStringAsync(url) != null)
                return;
        }
    }
}
