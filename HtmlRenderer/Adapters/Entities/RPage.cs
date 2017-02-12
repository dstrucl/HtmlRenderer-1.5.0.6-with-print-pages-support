using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheArtOfDev.HtmlRenderer.Adapters.Entities
{
    public struct RPage
    {
        private double _yOffset;
        private double _Width;
        private double _Height;
        private RMargin _Margin;
        private RHeader _Header;
        private RFooter _Footer;
        internal Core.Dom.CssBox _cssbox;

        private double _VerticalDistanceBetweenTwoPages;

        public double YOffset
        {
            get { return _yOffset; }
        }

        public double VerticalDistanceBetweenTwoPages
        {
            get { return _VerticalDistanceBetweenTwoPages;}
        }

        public double Width
        {
            get { return _Width; }
        }

        public double Height
        {
            get { return _Height; }
        }

        public RMargin Margin
        {
            get { return _Margin; }
        }

        public RHeader Header
        {
            get { return _Header; }
        }

        public RFooter Footer
        {
            get { return _Footer; }
        }


        internal RPage(Core.Dom.CssBox cssBox, double _ydisplaypos = 0, double width = int.MaxValue, double height = int.MaxValue, double verticalDistanceBetweenTwoPages = 0, RMargin margin = new RMargin(), RHeader header= new RHeader(), RFooter footer= new RFooter())
        {
            _cssbox = cssBox;
            _yOffset = _ydisplaypos;
            _Width = width;
            _Height = height;
            _VerticalDistanceBetweenTwoPages=verticalDistanceBetweenTwoPages;
            _Margin = margin;
            _Header = header;
            _Footer = footer;
        }
    }
}
