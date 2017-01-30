using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KMeansAlgorithm
{
    public class DataPoint {
        

        public double X { get; set; } = 0;
        public double Y { get; set; } = 0;

        public Shape Graphic { get; set; }

        public double Size { get; set; } = 10;

        public bool IsEllipse { get; set; } = true;

        public Color FillColor { get; set; }
        public Color BorderColor { get; set; }

        public ClusterPoint ParentCluster { get; set; }

        public DataPoint(double x, double y, double size, bool isEllipse) {
            X = x; Y = y; Size = size; IsEllipse = isEllipse;

            FillColor = Color.FromArgb(255, 255, 255, 255);
            BorderColor = Color.FromArgb(255, 0, 0, 0);

            //Console.WriteLine("{0}-{1}", X, Y);
        }

        public void MakeGraphic() {
            Ellipse ellipse = new Ellipse();

            SolidColorBrush fillColorBrush = new SolidColorBrush();
            fillColorBrush.Color = FillColor;

            SolidColorBrush borderColorBrush = new SolidColorBrush();
            borderColorBrush.Color = BorderColor;

            ellipse.Name = $"Shape_{X}_{Y}_{IsEllipse}";
            ellipse.Fill = fillColorBrush;

            ellipse.StrokeThickness = 2;
            ellipse.Stroke = borderColorBrush;

            ellipse.Width = Size;
            ellipse.Height = Size;

            ellipse.SetValue(Canvas.LeftProperty, (X - (ellipse.Width / 2)));
            ellipse.SetValue(Canvas.TopProperty, (Y - (ellipse.Height / 2)));

            Graphic = ellipse;
        }

        public void Show(Canvas canvas) {
            canvas.Children.Add(Graphic);
        }

        public double DistanceTo(double x, double y) {
            var distance = Math.Sqrt((
                Math.Pow(Math.Abs(x - X), 2) + Math.Pow(Math.Abs(y - Y), 2)
                ));
            return distance;
        }

        public bool HasCollision(DataPoint other) {
            var collision = DistanceTo(other.X, other.Y) <= Size;

            return collision;
        }
    }
}
