using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.DataVisualization.Charting;

namespace SBTP.BLL
{
    public class NewDataPoint : DataPoint
    {
        public event RoutedEventHandler Click;
    }
}
