using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheArtOfDev.HtmlRenderer.Adapters.Entities
{
    public struct RHeader
    {
        private double _Height;

        public double Height
        {
            get { return _Height; }
        }

        public RHeader(double height=0)
        {
            _Height = height;
        }
    }
}
