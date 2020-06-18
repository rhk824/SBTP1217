using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Model
{
    public class qkcs : INotifyPropertyChanged
    {
        double ycwd = 0;
        double yckhd = 0;
        double ycph = 0;
        double ycyl = 0;
        double sn = 0;
        double yn = 0;
        double ym = 0;
        string fs;
        double qtn = 0;
        double qtgn = 0;
        double jlmin = 0;
        double jlmax = 0;
        double lb = 0;
        double hf = 0;
        double jq = 0;
        double cyybhd = 0;
        double sxxsddz = 0;
        double jn = 0;
        double mvalue = 0.5;

        public double Ycwd { get => ycwd; set { ycwd = value; NotifyPropertyChanged("Ycwd"); } }
        public double Yckhd { get => yckhd; set { yckhd = value; NotifyPropertyChanged("Yckhd"); } }
        public double Ycph { get => ycph; set { ycph = value; NotifyPropertyChanged("Ycph"); } }
        public double Ycyl { get => ycyl; set { ycyl = value; NotifyPropertyChanged("Ycyl"); } }
        public double Sn { get => sn; set { sn = value; NotifyPropertyChanged("Sn"); } }
        public double Yn { get => yn; set { yn = value; NotifyPropertyChanged("Yn"); } }
        public double Ym { get => ym; set { ym = value; NotifyPropertyChanged("Ym"); } }
        public string Fs { get => fs; set { fs = value; NotifyPropertyChanged("Fs"); } }
        public double Qtn { get => qtn; set { qtn = value; NotifyPropertyChanged("Qtn"); } }
        public double Qtgn { get => qtgn; set { qtgn = value; NotifyPropertyChanged("Qtgn"); } }
        public double Jlmin { get => jlmin; set { jlmin = value; NotifyPropertyChanged("Jlmin"); } }
        public double Jlmax { get => jlmax; set { jlmax = value; NotifyPropertyChanged("Jlmax"); } }
        public double Lb { get => lb; set { lb = value; NotifyPropertyChanged("Lb"); } }
        public double Hf { get => hf; set { hf = value; NotifyPropertyChanged("Hf"); } }
        public double Jq { get => jq; set { jq = value; NotifyPropertyChanged("Jq"); } }
        public double Cyybhd { get => cyybhd; set { cyybhd = value; NotifyPropertyChanged("Cyybhd"); } }
        public double Sxxsddz { get => sxxsddz; set { sxxsddz = value; NotifyPropertyChanged("Sxxsddz"); } }

        public double Jn { get => jn; set { jn = value; NotifyPropertyChanged("Jn"); } }
        public double Mvalue { get => mvalue; set { mvalue = value; NotifyPropertyChanged("Mvalue"); } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
