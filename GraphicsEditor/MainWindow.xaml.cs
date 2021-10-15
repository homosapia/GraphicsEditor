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

        bool transform;

        Point currentPoint = new();

        public MainWindow()
        {   
            InitializeComponent();
        }

        private void canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            figure.ChangePosition(e.GetPosition(canvas));
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(!canvas.Children.Contains((UIElement)figure.Figure()))
            {
                transform = true;
                figure.CreateFigure(e.GetPosition(canvas));
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed && transform)
            {
                figure.ChangePosition(e.GetPosition(canvas));
            }
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            transform = false;
        }

        private void segment_Click(object sender, RoutedEventArgs e)
        {
            figure = new FigureBrokenLine(canvas);
            figure.ReceiveFigure += СurrentFigure;
            figure.ClickMarker += ShowMarker;
            figure.RemoveFigure += Figure_RemoveFigure;
        }

        private void rictangle_Click(object sender, RoutedEventArgs e)
        {
            figure = new Square(canvas);
            figure.ReceiveFigure += СurrentFigure;
            figure.ClickMarker += ShowMarker;
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
            if(!transform && this.figure != figure)
                this.figure = figure;
            if (!canvas.Children.Contains((UIElement)this.figure.Figure()))
                canvas.Children.Add((UIElement)this.figure.Figure());
        }

        public void ShowMarker(bool click)
        {
            transform = click;
        }
    }
}
