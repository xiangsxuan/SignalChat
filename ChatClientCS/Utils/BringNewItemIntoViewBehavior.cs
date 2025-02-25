﻿using System.Collections.Specialized;
using System.Windows.Interactivity;
using System.Windows.Controls;
using System.Windows;

namespace ChatClientCS.Utils
{
    // todo: 学习下Behavior
    public class BringNewItemIntoViewBehavior : Behavior<ItemsControl>
    {
        private INotifyCollectionChanged notifier;

        protected override void OnAttached()
        {
            base.OnAttached();
            notifier = AssociatedObject.Items as INotifyCollectionChanged;
            notifier.CollectionChanged += ItemsControl_CollectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            notifier.CollectionChanged -= ItemsControl_CollectionChanged;
        }

        private void ItemsControl_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var newIndex = e.NewStartingIndex;
                var newElement = AssociatedObject.ItemContainerGenerator.ContainerFromIndex(newIndex);
                var item = (FrameworkElement)newElement;
                // FrameworkElement.BringIntoView: 尝试将此元素放入其所在的任何可滚动区域内的视图中。
                item?.BringIntoView();
            }
        }
    }
}