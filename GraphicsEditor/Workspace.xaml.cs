using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GraphicsEditor
{
    public partial class Workspace : UserControl
    {
        public Paint paint;
        public Workspace()
        {
            InitializeComponent();
        }

        private void Сanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            paint.SetInitialValues(e.GetPosition(canvas));
        }

        private void Сanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                paint.ChangeFigure();
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                paint.MoveEverything();
            }
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            paint = new(canvas);
        }

        private void canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            paint.DeselectAnObject();
        }
    }
}
