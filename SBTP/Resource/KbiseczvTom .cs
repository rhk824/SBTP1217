﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SBTP.Resource
{
    public class KbiseczvTom : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            var size = new Size();
            foreach (var temp in Children.Cast<UIElement>())
            {
                temp.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                size = new Size(size.Width + temp.DesiredSize.Width, Math.Max(size.Height, temp.DesiredSize.Height));
            }
            return size;
        }

        protected override Size ArrangeOverride(Size availableSize)
        {
            var size = availableSize;

            var width = size.Width / Children.Count;

            for (int i = 0; i < Children.Count; i++)
            {
                var temp = Children[i];
                temp.Arrange(new Rect(new Point(i * width, 0), new Size(width, size.Height)));
            }

            return size;
        }
    }
}
