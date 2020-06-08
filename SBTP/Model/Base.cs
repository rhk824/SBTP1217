using System;
using System.ComponentModel;

namespace SBTP.Model
{
    public class Base : INotifyPropertyChanged
    {

        private bool _selected;

        #region [ Property Getter And Setter ]

        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                NotifyPropertyChanged("Selected");
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
