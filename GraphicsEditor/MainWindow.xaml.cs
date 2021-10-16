using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphicsEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IFigure figure;

        bool peint;
        bool transform;
        bool marker;

        List<Rectangle> markers = new();

        Point currentPoint = new();

        public MainWindow()
        {   
            InitializeComponent();
        }

        private void canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (peint)
            {
                figure.ChangePosition(e.GetPosition(canvas));
            }
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(peint)
            {
                figure.CreateFigure(e.GetPosition(canvas));
            }
            if (!transform && !marker)
            {
                foreach (Rectangle marker in markers)
                {
                    canvas.Children.Remove(marker);
                }
            }
            if (!marker)
                transform = false;
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && peint)
            {
                figure.DrawFigure(e.GetPosition(canvas));
            }

            if(e.LeftButton == MouseButtonState.Pressed && marker)
            {
                figure.ChangePosition(e.GetPosition(canvas));
            }
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (peint)
            {
                peint = false;
                figure = null;
            }
        }

        private void segment_Click(object sender, RoutedEventArgs e)
        {
            peint = true;
            figure = new FigureBrokenLine(canvas);
            figure.Transform += Figure_Transform;
            figure.SelectObject += СurrentFigure;
            figure.ClickMarker += ClickMarker;
            figure.RemoveFigure += Figure_RemoveFigure;
            figure.SetMarker += Figure_SetMarker;
        }

        private void Figure_Transform(bool click)
        {
            transform = click;
        }

        private void Figure_SetMarker(Rectangle marker)
        {
            markers.Add(marker);
            canvas.Children.Add(marker);
        }

        private void rictangle_Click(object sender, RoutedEventArgs e)
        {
            peint = true;
            figure = new Square(canvas);
            figure.SelectObject += СurrentFigure;
            figure.ClickMarker += ClickMarker;
            figure.RemoveFigure += Figure_RemoveFigure;
        }

        private void Figure_RemoveFigure(UIElement figure)
        {
            canvas.Children.Remove(figure);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        public void СurrentFigure(IFigure figure)
        {
            if(this.figure != figure)
                this.figure = figure;
            if (!canvas.Children.Contains((UIElement)this.figure.Figure()))
                canvas.Children.Add((UIElement)this.figure.Figure());
        }

        public void ClickMarker(bool click)
        {
            marker = click;
        }
    }
}
