using SEG.Desktop.Control;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace DiagramDesigner
{
    public class ResizeThumb : Thumb
    {
        public ResizeThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.ResizeThumb_DragDelta);
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Control designerItem = this.DataContext as Control;

            if (designerItem != null)
            {
                double deltaVertical, deltaHorizontal;

                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        
                        if(e.VerticalChange<0)
                        {
                            deltaVertical = Math.Min(-e.VerticalChange, designerItem.ActualHeight - designerItem.MinHeight);

                            // Negative, we adjust Height and Width accordingly
                            double h = designerItem.Height - deltaVertical;

                            designerItem.Height = h;
                            designerItem.Width = h;
                        }
                        else
                        {
                            deltaVertical = e.VerticalChange;
                            
                            double h = designerItem.Height + deltaVertical;
                            if (h + Canvas.GetTop(designerItem) > ControlCenter.Instance.imgPop_snapH)
                                h = ControlCenter.Instance.imgPop_snapH - Canvas.GetTop(designerItem);
                            if (h + Canvas.GetLeft(designerItem) > ControlCenter.Instance.imgPop_snapW)
                                h = ControlCenter.Instance.imgPop_snapW - Canvas.GetLeft(designerItem);

                            designerItem.Height = h;
                            designerItem.Width = h;
                        }

                        //double h = designerItem.Height - deltaVertical;
                        //if ((h + Canvas.GetTop(designerItem))>ControlCenter.Instance.imgPop_snapH)
                        //{
                        //    h = ControlCenter.Instance.imgPop_snapH - Canvas.GetTop(designerItem);
                        //}
                        //else
                        //    h -= deltaVertical;

                        //// Check if the width is a limitation
                        //if(ControlCenter.Instance.imgPop_snapW<(h+Canvas.GetLeft(designerItem)))
                        //{
                        //    h = ControlCenter.Instance.imgPop_snapW - Canvas.GetLeft(designerItem);
                        //}
                        //designerItem.Height = h;
                        //designerItem.Width = h;

                        break;

                    case VerticalAlignment.Top:
                        deltaVertical = Math.Min(e.VerticalChange, designerItem.ActualHeight - designerItem.MinHeight);

                        double y = Canvas.GetTop(designerItem) + deltaVertical;
                        if (y < 0.0)
                        {
                            y = 0.0;
                            deltaVertical = y - Canvas.GetTop(designerItem);
                        }                        
                        Canvas.SetTop(designerItem, y);

                        designerItem.Height -= deltaVertical;
                        break;
                    default:
                        break;
                }

                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        deltaHorizontal = Math.Min(e.HorizontalChange, designerItem.ActualWidth - designerItem.MinWidth);
                        Canvas.SetLeft(designerItem, Canvas.GetLeft(designerItem) + deltaHorizontal);
                        designerItem.Width -= deltaHorizontal;
                        break;
                    case HorizontalAlignment.Right:
                        if(e.HorizontalChange<0)
                        {
                            deltaHorizontal = Math.Min(-e.HorizontalChange, designerItem.ActualWidth - designerItem.MinWidth);

                            double w = designerItem.Width - deltaHorizontal;
                            designerItem.Width = w;
                            designerItem.Height = w;
                        }
                        else
                        {
                            deltaHorizontal = e.HorizontalChange;

                            double w = designerItem.Width + deltaHorizontal;
                            if (w + Canvas.GetLeft(designerItem) > ControlCenter.Instance.imgPop_snapW)
                                w = ControlCenter.Instance.imgPop_snapW - Canvas.GetLeft(designerItem);
                            if (w + Canvas.GetTop(designerItem) > ControlCenter.Instance.imgPop_snapH)
                                w = ControlCenter.Instance.imgPop_snapH - Canvas.GetTop(designerItem);

                            designerItem.Height = w;
                            designerItem.Width = w;
                        }

                        
                        break;
                    default:
                        break;
                }
            }

            e.Handled = true;
        }
    }
}
