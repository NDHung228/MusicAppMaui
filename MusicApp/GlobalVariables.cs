using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Models;
using MusicApp.Models;

namespace MusicApp
{
    public static class GlobalVariables
    {

        public static List<PlayList> playlist = new List<PlayList>();
        public static string[] listMusic = new string[] { "Chay-Ngay-Di-Son-Tung-M-TP.mp3", "Lac-Troi-Masew-Mix-Son-Tung-M-TP-Masew.mp3",
        "Day-Dut-Noi-Dau-Mr-Siro.mp3","Ngoai-30-Thai-Hoc.mp3","Anh-Sai-Roi-Son-Tung-M-TP.mp3","Vo-Hinh-Trong-Tim-Em-Mr-Siro.mp3",
            "Tinh-Yeu-Chap-Va-Mr-Siro.mp3" ,"Lang-Nghe-Nuoc-Mat-Mr-Siro.mp3","Duoi-Nhung-Con-Mua-Mr-Siro.mp3"};

        public static ObservableCollection<Song> ListAllMusic;
    }
}
