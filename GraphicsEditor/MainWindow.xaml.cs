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

        Point currentPoint = new();

        Line line;
        Rectangle rectangle;

        int numberLine = 0;

        public MainWindow()
        {
            paint = new Paint();
            
            InitializeComponent();
        }

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                currentPoint = e.GetPosition(canvas);

                paint.StartObject(currentPoint);
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                currentPoint = e.GetPosition(canvas);

                paint.NowObject(currentPoint);
            }
        }

        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (line != null)
            {
                line.Name = "line" + numberLine;
                line.X2 = e.GetPosition(canvas).X;
                line.Y2 = e.GetPosition(canvas).Y;
                numberLine++;

                paint.AppNewObject(canvas, new Segment());
                line = (Line)paint.GetCurrentItem();
            }
        }

        private void segment_Click(object sender, RoutedEventArgs e)
        {   
            paint.AppNewObject(canvas, new Segment());

            line = (Line)paint.GetCurrentItem();
        }

        private void rictangle_Click(object sender, RoutedEventArgs e)
        {
            paint.AppNewObject(canvas, new Square());

            rectangle = (Rectangle)paint.GetCurrentItem();
        }
    }
}
