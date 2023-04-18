using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Models
{
    public class Song
    {
        public string Title { get; set; }
        public string MusicLabel {get; set; }
        public string Artist { get; set; }

        public string Picture { get; set; }

        public string Album { get; set; }

        public string MusicURL { get; set; }
    }
}
