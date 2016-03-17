using SEG.Desktop.Control;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace DiagramDesigner
{
    public class MoveThumb : Thumb
    {
        public MoveThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Control designerItem = this.DataContext as Control;

            if (designerItem != null)
            {
                double left = Canvas.GetLeft(designerItem) + e.HorizontalChange;
                if (left < 0)
                    left = 0;
                if (left + designerItem.Width > ControlCenter.Instance.imgPop_snapW)
                    left = ControlCenter.Instance.imgPop_snapW - designerItem.Width;

                double top = Canvas.GetTop(designerItem) + e.VerticalChange;
                if (top < 0)
                    top = 0;
                if (top + designerItem.Height > ControlCenter.Instance.imgPop_snapH)
                    top = ControlCenter.Instance.imgPop_snapH - designerItem.Height;


                Canvas.SetLeft(designerItem, left);
                Canvas.SetTop(designerItem, top);
            }
        }
    }
}
