using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Model
{
   public class ProjectModel : INotifyPropertyChanged
    {
        string project_name = "";
        string project_location = "";
        /// <summary>
        /// 工程名
        /// </summary>
        public string PROJECT_NAME { set { project_name = value; NotifyPropertyChanged("PROJECT_NAME"); } get => "SBTP-" + (string.IsNullOrEmpty(project_name) ? "*" : project_name); }
        /// <summary>
        /// 工程路径
        /// </summary>
        public string PROJECT_LOCATION { set { project_location = value; NotifyPropertyChanged("PROJECT_LOCATION"); } get => project_location; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
