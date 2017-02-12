using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TheArtOfDev.HtmlRenderer.Adapters.Entities
{
    public class PageList
    {
        private int _iCurrent = -1;

        private int iCurrent
        {
            get { return _iCurrent; }
            set { _iCurrent = value; }
        }


        private List<RPage> _Page = new List<RPage>();

        public RPage Screen_Page = new RPage();

        public RPage Page(int i)
        {
            if ((_Page.Count > 0) && (i >= 0) && (i <= _Page.Count))
            {
                return _Page[i];
            }
            else
            {
                return Screen_Page;
            }
        }


        public void Add(RPage page)
        {
            _Page.Add(page);
        }

        public void Clear()
        {
            _Page.Clear();
        }


        public int Count
        {
            get { return _Page.Count; }
        }

        public float Height
        {
            get
            {
                double y = 0;
                int i = 0;
                if (_Page.Count > 0)
                {
                    for (i = 0; i < _Page.Count; i++)
                    {

                        y += _Page[i].VerticalDistanceBetweenTwoPages + _Page[i].Height;
                    }
                    y += _Page[i - 1].VerticalDistanceBetweenTwoPages;
                }
                return Convert.ToSingle(y);
            }
        }

        public float Width
        {
            get
            {
                double y = 0;
                int i = 0;
                if (_Page.Count > 0)
                {
                    for (i = 0; i < _Page.Count; i++)
                    {
                        if (y < _Page[i].Width)
                        {
                            y = _Page[i].Width;
                        }
                    }
                }
                return Convert.ToSingle(y);
            }
        }
    }
}