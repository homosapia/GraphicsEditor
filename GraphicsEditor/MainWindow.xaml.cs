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
        double thick = 1;

        bool peint;
        bool transform;
        bool marker;

        List<Rectangle> markers = new();

        Point currentPoint = new();

        public MainWindow()
        {   
            InitializeComponent();
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (peint)
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
            foreach (Rectangle marker in markers)
            {
                canvas.Children.Remove(marker);
            }
            marker = false;
            transform = false;
            peint = true;
            figure = new FigureBrokenLine(canvas);
            figure.SetColor(palette.Color);
            figure.ChangeThickness(thick);
            figure.Transform += Figure_Transform;
            figure.SelectObject += СurrentFigure;
            figure.ClickMarker += ClickMarker;
            figure.RemoveMarker += Figure_RemoveMarker;
            figure.SetMarker += Figure_SetMarker;
        }
        private void rictangle_Click(object sender, RoutedEventArgs e)
        {
            foreach (Rectangle marker in markers)
            {
                canvas.Children.Remove(marker);
            }
            transform = false;
            marker = false;
            peint = true;
            figure = new Square(canvas);
            figure.SetColor(palette.Color);
            figure.ChangeThickness(thick);
            figure.Transform += Figure_Transform;
            figure.SelectObject += СurrentFigure;
            figure.ClickMarker += ClickMarker;
            figure.RemoveMarker += Figure_RemoveMarker;
            figure.SetMarker += Figure_SetMarker;
        }

        private void Figure_Transform(bool click)
        {
            transform = click;
        }

        private void Figure_SetMarker(List<Rectangle> markers)
        {
            this.markers = markers;
            foreach (Rectangle marker in markers)
            {
                canvas.Children.Add(marker);
            }
        }
        private void Figure_RemoveMarker()
        {
            foreach (Rectangle marker in markers)
            {
                canvas.Children.Remove(marker);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        public void СurrentFigure(IFigure figure)
        {
            if(this.figure != figure && !peint)
            {
                this.figure = figure;
                foreach (Rectangle marker in markers)
                {
                    canvas.Children.Remove(marker);
                }
            }
            if (!canvas.Children.Contains((UIElement)figure.Figure()))
                canvas.Children.Add((UIElement)figure.Figure());
        }

        public void ClickMarker(bool click)
        {
            marker = click;
        }

        private void palette_SelectedBrushChanged(object sender, Syncfusion.Windows.Tools.Controls.SelectedBrushChangedEventArgs e)
        {
            if (figure != null)
            {
                figure.ChangeColor(palette.Color);
            }
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.thick = slider.Value;
            if (figure != null)
            {
                figure.ChangeThickness(this.thick);
            }
        }
    }
}
