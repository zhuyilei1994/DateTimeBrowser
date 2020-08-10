using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace DateTimeBrowser
{
    class MainVM:ViewModelBase
    {
        private int _hour;
        private int _minute;
        private int _second;

        public MainVM()
        {
            Hour = 10;
            Minute = 10;
            Second = 10;
            Date=DateTime.Parse("2000/10/1");
        }

        public DateTime? Date { set; get; }

        public int Hour
        {
            set => Set(ref _hour , value);
            get => _hour;
        }

        public int Minute
        {
            set => Set(ref _minute , value);
            get => _minute;
        }

        public int Second
        {
            set => Set(ref _second , value);
            get => _second;
        }
    }
}
