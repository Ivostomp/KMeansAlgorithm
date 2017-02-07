using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KMeansAlgorithm
{
    public class ClusterPoint : DataPoint
    {
        public string Key { get; set; }

        public double previousX { get; set; }
        public double previousY { get; set; }

        public ClusterPoint(string key, double x, double y, double size, Color color) : base(x, y, size, false) {
            Key = key;
            BorderColor = color;
            FillColor = Color.FromArgb(255, 255, 255, 255);
        }

        public bool HasMoved() {
            return (X != previousX) && (Y != previousY);
        }

    }
}
