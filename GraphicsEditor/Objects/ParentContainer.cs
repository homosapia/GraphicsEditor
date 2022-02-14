using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using GraphicsEditor.Interfaces;
using GraphicsEditor.Objects;
using Newtonsoft.Json;
using System.Windows.Input;
using GraphicsEditor.Resources;

namespace GraphicsEditor.Objects
{
    public class ParentContainer
    {
        private Canvas canvas;
        public ParentContainer(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void Remove(List<UIElement> elements)
        {
            foreach (var element in elements)
            {
                canvas.Children.Remove(element);
            }
        }

        public void Add(List<UIElement> elements)
        {
            foreach (var element in elements)
            {
                if (!canvas.Children.Contains(element))
                    canvas.Children.Add(element);
            }
        }
    }
}
