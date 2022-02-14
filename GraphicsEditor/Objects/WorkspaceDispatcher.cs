using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GraphicsEditor.Interfaces;

namespace GraphicsEditor.Objects
{
    public delegate void EventRemoveUiElements(List<UIElement> uIElements);
    public delegate void EventAddUiElements(List<UIElement> uIElements);
    public static class WorkspaceDispatcher
    {
        public static event EventRemoveUiElements RemoveUiElements;
        public static event EventAddUiElements AddUiElements;
         
        public static void Remove(List<UIElement> elements)
        {
            RemoveUiElements.Invoke(elements);
        }

        public static void Add(List<UIElement> elements)
        {
            AddUiElements.Invoke(elements);
        }
    }
}
