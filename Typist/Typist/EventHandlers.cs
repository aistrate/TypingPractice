using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Typist
{
    #region StatusChanged

    public class StatusChangedEventArgs : EventArgs
    {
        public StatusChangedEventArgs(string statusMessage)
        {
            StatusMessage = statusMessage;
        }

        public string StatusMessage { get; private set; }
    }

    public delegate void StatusChangedEventHandler(object sender, StatusChangedEventArgs e);

    #endregion


    #region VisibleRegionChanged

    public class VisibleRegionChangedEventArgs : EventArgs
    {
        public VisibleRegionChangedEventArgs(int firstVisibleRow, int lastVisibleRow, int totalRowCount)
        {
            FirstVisibleRow = firstVisibleRow;
            LastVisibleRow = lastVisibleRow;
            TotalRowCount = totalRowCount;
        }

        public int FirstVisibleRow { get; private set; }
        public int LastVisibleRow { get; private set; }
        public int TotalRowCount { get; private set; }
    }

    public delegate void VisibleRegionChangedEventHandler(object sender, VisibleRegionChangedEventArgs e);

    #endregion
}
