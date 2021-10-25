using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsEditor.Objects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GraphicsEditor
{
    class FigureRectangle : IFigure
    {
        public event EventClickMarker ClickMarker;
        public event EventSetUIElement UIElement;
        public event EventTransform Transform;
        public event EventSelectFigure SelectObject;
        public event EventRemoveUiElemrnt RemoveUiElemrnt;
        public event EventFindPositionMouse FindPositionMouse;

        public square rectangle = new();
        
        public Point StartObject = new Point();

        double distanceX;
        double distanceY;
        double pointY;

        bool transform;
        bool turn;
        bool move;

        public FigureRectangle()
        {
            transform = true;
            rectangle.Rectangle.MouseLeftButtonDown += Rectangle_MouseLeftButtonDown;
            rectangle.Rectangle.MouseLeave += Rectangle_MouseLeave;

            rectangle.Marker.MouseLeftButtonDown += Marker_MouseLeftButtonDown;
            rectangle.Marker.MouseLeftButtonUp += Marker_MouseLeftButtonUp;
        }

        public string SerializeFigure()
        {
            List<string> objects = new();

            objects.Add(JsonConvert.SerializeObject(rectangle.CopyElements()));
            objects.Add(JsonConvert.SerializeObject(StartObject));

            string f = JsonConvert.SerializeObject(objects);
            List<string> list = JsonConvert.DeserializeObject<List<string>>(f);
            List<object> fd = JsonConvert.DeserializeObject<List<object>>(list[0]);
            rectangle.InsertElements(fd);

            return JsonConvert.SerializeObject(objects);
        }

        public void DeserializeFigure(List<string> objects)
        {
            rectangle.InsertElements(JsonConvert.DeserializeObject<List<object>>(objects[0]));

            StartObject = JsonConvert.DeserializeObject<Point>(objects[1]);
        }

        public IFigure GetCopyIFigure()
        {
            FigureRectangle figureRectangle = new();
            return figureRectangle;
        }

        public void TuneElements()
        {
            rectangle.CreateObject();
            rectangle.SetPosition(StartObject);

            rectangle.Rectangle.MouseLeftButtonDown += Rectangle_MouseLeftButtonDown;
            rectangle.Rectangle.MouseLeave += Rectangle_MouseLeave;

            rectangle.Marker.MouseLeftButtonDown += Marker_MouseLeftButtonDown;
            rectangle.Marker.MouseLeftButtonUp += Marker_MouseLeftButtonUp;
        }

        private void Rectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!transform)
            {
                turn = true;
                transform = false;
                move = false;
            }
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            move = true;
            turn = false;
            transform = false;

            rectangle.ShowMarker();

            Transform(true);

            SelectObject(this);
            FindPositionMouse();
        }
        private void Marker_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            transform = false;
        }

        private void Marker_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            transform = true;
            move = false;
            turn = false;
        }

        public object NewObject(Canvas canvas)
        {
            return canvas;
        }

        public object Figure()
        {
            return rectangle.Substrate;
        }

        public void ChangePosition(Point point)
        {
            if (transform)
            {
                Point pointSubstrate = Mouse.GetPosition(rectangle.Substrate);
                rectangle.resize(pointSubstrate);
            }
            if(turn)
            {
                rectangle.HideMarker();
                double rotat =  point.Y - pointY;
                rectangle.ChangeTurn(rotat);
            }
            if(move)
            {
                MoveFigure(point);
            }
        }

        public void ChangeColor(Color color)
        {
            rectangle.ChangeColor(color);
        }

        public void ChangeThickness(double thick)
        {
            rectangle.ChangeThickness(thick);
        }

        public void DeselectShape()
        {
            rectangle.HideMarker();
            transform = false;
            move = false;
            turn = false;
        }

        public void StartingPoint(Point point)
        {
            StartObject = point;
            rectangle.CreateObject();
            rectangle.SetPosition(point);
            SelectObject(this);
        }

        public void CurrentPositionMouseOnCanvas(Point point)
        {
            distanceX = point.X - StartObject.X;
            distanceY = point.Y - StartObject.Y;

            pointY = point.Y;
        }

        public List<UIElement> GetAllUIElements()
        {
            List<UIElement> uIElements = new();
            uIElements.Add(rectangle.Substrate);
            return uIElements;
        }

        public void MoveFigure(Point point)
        {
            StartObject.X = point.X - distanceX;
            StartObject.Y = point.Y - distanceY;
            rectangle.SetPosition(StartObject);
        }
    }
}
