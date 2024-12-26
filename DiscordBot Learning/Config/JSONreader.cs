using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot_Learning.Config
{
    public class JSONreader
    {
        public string token { get; set; }
        public string prefix { get; set; }

        public async Task ReadJSON()
        {
            using (StreamReader sr  = new StreamReader("Config.json"))
            {
                string json = await sr.ReadToEndAsync();
                jsonStructure data = JsonConvert.DeserializeObject<jsonStructure>(json);

                this.token = data.token;
                this.prefix = data.prefix;
            }
        }
    }

    internal sealed class jsonStructure
    {
        public string token { get; set; }
        public string prefix { get; set; }
    }
}
