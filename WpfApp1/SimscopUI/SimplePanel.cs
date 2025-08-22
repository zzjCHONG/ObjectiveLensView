using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.SimscopUI
{
    public class SimplePanel : Panel
    {
        /// <summary>
        /// 传入父容器分配的可用空间，返回该容器根据其子元素大小计算确定的在布局过程中所需的大小。
        /// 用于计算本身及其子控件的大小
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            var maxSize = new Size();

            foreach (UIElement child in InternalChildren)
                if (child != null)
                {
                    child.Measure(availableSize);
                    maxSize.Width = Math.Max(maxSize.Width, child.DesiredSize.Width);
                    maxSize.Height = Math.Max(maxSize.Height, child.DesiredSize.Height);
                }

            return maxSize;
        }

        /// <summary>
        /// 传入父容器最终分配的控件大小，返回使用的实际大小
        /// 用于布局本身及其子控件的位置和大小
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement child in InternalChildren)
                child?.Arrange(new Rect(finalSize));

            return finalSize;
        }
    }
}
