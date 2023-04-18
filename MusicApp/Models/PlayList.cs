using MusicApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class PlayList
    {
        public string Name { get; set; }
        public string Picture { get; set; }

        public List<Song> Songs { get; set; }

    }
}
