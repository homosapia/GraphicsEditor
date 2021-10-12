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
        IPaint paint;
        IFigure figure;

        Point currentPoint = new();

        public MainWindow()
        {
            paint = new Paint();
            
            InitializeComponent();
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentPoint = e.GetPosition(canvas);

            paint.AppNewObject(canvas, figure);

            paint.StartObject(currentPoint);

        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            currentPoint = e.GetPosition(canvas);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                paint.NowObject(currentPoint);
            }

            if (e.RightButton == MouseButtonState.Pressed)
            {
                figure.ChangePosition(currentPoint);
            }
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            currentPoint = e.GetPosition(canvas);
            paint.EndObject(currentPoint);
        }

        private void segment_Click(object sender, RoutedEventArgs e)
        {   
            figure = new Segment();
        }

        private void rictangle_Click(object sender, RoutedEventArgs e)
        {
            figure = new Square();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
