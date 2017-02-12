using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheArtOfDev.HtmlRenderer.Adapters.Entities
{
    public struct RMargin
    {
        private double _Top;
        private double _Bottom;
        private double _Left;
        private double _Right;

        public double Top
        {
            get { return _Top; }
            set { _Top = value; }
        }

        public double Bottom
        {
            get { return _Bottom; }
            set { _Bottom = value; }
        }

        public double Left
        {
            get { return _Left; }
            set { _Left = value; }
        }

        public double Right
        {
            get { return _Right; }
            set { _Right = value; }
        }


        public RMargin(double top=0,
                       double bottom = 0,
                       double left = 0,
                       double right = 0
                      )
        {
            _Top = top;
            _Bottom = bottom;
            _Left = left;
            _Right = right;
        }
    }
}
