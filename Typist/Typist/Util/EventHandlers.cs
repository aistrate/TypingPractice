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

        public override bool Equals(object obj)
        {
            VisibleRegionChangedEventArgs other = obj as VisibleRegionChangedEventArgs;

            return other != null &&
                   this.FirstVisibleIndex == other.FirstVisibleIndex &&
                   this.LastVisibleIndex == other.LastVisibleIndex &&
                   this.TotalLength == other.TotalLength;
        }
    }

    public delegate void VisibleRegionChangedEventHandler(object sender, VisibleRegionChangedEventArgs e);

    #endregion


    #region CursorPositionChanged

    public class CursorPositionChangedEventArgs : EventArgs
    {
        public CursorPositionChangedEventArgs(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public int Row { get; private set; }
        public int Column { get; private set; }

        public override bool Equals(object obj)
        {
            CursorPositionChangedEventArgs other = obj as CursorPositionChangedEventArgs;

            return other != null &&
                   this.Row == other.Row &&
                   this.Column == other.Column;
        }
    }

    public delegate void CursorPositionChangedEventHandler(object sender, CursorPositionChangedEventArgs e);

    #endregion
}
