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
        public VisibleRegionChangedEventArgs(int firstVisibleIndex, int lastVisibleIndex, int totalLength)
        {
            FirstVisibleIndex = firstVisibleIndex;
            LastVisibleIndex = lastVisibleIndex;
            TotalLength = totalLength;
        }

        public int FirstVisibleIndex { get; private set; }
        public int LastVisibleIndex { get; private set; }
        public int TotalLength { get; private set; }
    }

    public delegate void VisibleRegionChangedEventHandler(object sender, VisibleRegionChangedEventArgs e);

    #endregion
}
