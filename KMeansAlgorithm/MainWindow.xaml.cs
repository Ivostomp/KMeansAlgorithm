using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace KMeansAlgorithm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<DataPoint> _DataPoints { get; set; } = new List<DataPoint>();
        private List<ClusterPoint> _Clusters { get; set; } = new List<ClusterPoint>();

        private System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();

        public Dictionary<int, Color> _ClusterColors = new Dictionary<int, Color>() {
            { 0, Color.FromArgb(255, 255, 127, 0) },
            { 1, Color.FromArgb(255, 255, 0, 127) },
            { 2, Color.FromArgb(255, 0, 255, 127) },
            { 3, Color.FromArgb(255, 127, 255, 0) },
            { 4, Color.FromArgb(255, 127, 0, 255) },
            { 5, Color.FromArgb(255, 0, 127, 255) },

            { 6, Color.FromArgb(255, 255, 0, 0) },
            { 7, Color.FromArgb(255, 0, 255, 0) },
            { 8, Color.FromArgb(255, 0, 0, 255) },
            { 9, Color.FromArgb(255, 255, 255, 0) },
            { 10, Color.FromArgb(255, 255, 0, 255) },
            { 11, Color.FromArgb(255, 0, 255, 255) },

        };

        private int _NrOfSections = 6;
        private int _MinPoints = 200;
        private int _MaxPoints = 250;

        private int _GenSpeed = 250;

        private bool processing = false;

        private bool stepSwitch = false;

        public MainWindow() {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
            this.btnReset.Click += BtnReset_Click;
            this.btnStart.Click += BtnStart_Click;
            this.btnResetDataPoints.Click += BtnResetDataPoints_Click;
            this.btnResetCluster.Click += BtnResetCluster_Click;

            _timer.Interval = _GenSpeed;
            _timer.Tick += _timer_Tick;
        }

        private void GeneratePoints() {
            _DataPoints.Clear();
            canDrawArea.Children.Clear();

            var rndPointCount = new Random();
            var nrOfPoints = rndPointCount.Next(_MinPoints, _MaxPoints);

            for (int i = 0; i < nrOfPoints; i++) {
                Thread.Sleep(1);

                var rndCoordinates = new Random();

                var tries = 0;
                var succes = true;

                var x = rndCoordinates.Next(5, 495);
                Thread.Sleep(1);
                var y = rndCoordinates.Next(5, 495);

                //Console.WriteLine("=============");
                var dPoint = new DataPoint(x, y, 10, true);
                while (_DataPoints.Any(a => a.HasCollision(dPoint))) {
                    x = rndCoordinates.Next(5, 495);
                    Thread.Sleep(1);
                    y = rndCoordinates.Next(5, 495);
                    dPoint = new DataPoint(x, y, 10, true);
                    if (tries >= 10) {
                        succes = false;
                        break;
                    }
                }

                if (succes) {
                    dPoint.MakeGraphic();
                    _DataPoints.Add(dPoint);
                    dPoint.Show(canDrawArea);
                }
            }
        }

        private void GenerateClusterPoints() {
            _Clusters.Clear();


            for (int i = 0; i < _NrOfSections; i++) {

                var rndCoordinates = new Random();

                Thread.Sleep(1);
                var x = rndCoordinates.Next(5, 495);
                Thread.Sleep(1);
                var y = rndCoordinates.Next(5, 495);

                var cluster = new ClusterPoint(i.ToString(), x, y, 10, ( i < (_ClusterColors.Count) ? _ClusterColors[i] : RandomColor()));

                var tries = 0;
                var succes = true;

                while (_Clusters.Any(a => a.HasCollision(cluster))) {
                    x = rndCoordinates.Next(5, 495);
                    Thread.Sleep(1);
                    y = rndCoordinates.Next(5, 495);
                    cluster = new ClusterPoint(i.ToString(), x, y, 10, cluster.BorderColor);
                    if (tries >= 10) {
                        succes = false;
                        break;
                    }
                }

                if (succes) {
                    cluster.MakeGraphic();
                    _Clusters.Add(cluster);
                    cluster.Show(canDrawArea);
                }
            }
        }

        private void RedrawGraphics() {
            canDrawArea.Children.Clear();

            DrawLines();

            foreach (var dp in _DataPoints) {
                dp.MakeGraphic();
                dp.Show(canDrawArea);
            }

            foreach (var cl in _Clusters) {
                cl.MakeGraphic();
                cl.Show(canDrawArea);
            }


        }

        private void DrawLines() {
            foreach (var cl in _Clusters) {
                var pointsincluster = _DataPoints.Where(q => q.BorderColor == cl.BorderColor);

                if (!pointsincluster.Any()) { continue; }

                foreach (var dp in pointsincluster) {
                    var line = new Line();

                    SolidColorBrush fillColorBrush = new SolidColorBrush();
                    fillColorBrush.Color = cl.FillColor;

                    line.Stroke = fillColorBrush;
                    line.StrokeThickness = 1;

                    line.X1 = cl.X;
                    line.Y1 = cl.Y;
                    line.X2 = dp.X;
                    line.Y2 = dp.Y;

                    canDrawArea.Children.Add(line);
                }
            }
        }

        private void StartAlgorithm() {
            _timer.Start();
        }

        private void StopAlgorithm() {
            _timer.Stop();
        }

        private void AlgorithmStep() {
            if (!stepSwitch) {
                Console.WriteLine("Step");
                foreach (var dp in _DataPoints) {

                    ClusterPoint closestCluster = null;
                    var closestDistance = 9999999.0;
                    foreach (var cl in _Clusters) {
                        var d = cl.DistanceTo(dp.X, dp.Y);
                        if (d < closestDistance) {
                            closestDistance = d;
                            closestCluster = cl;
                        }
                    }

                    if (closestCluster == null) { continue; }


                    dp.BorderColor = closestCluster.BorderColor;

                    Line line = new Line();

                }
            } else {
                Console.WriteLine("Mean");
                foreach (var cl in _Clusters) {
                    var pointsincluster = _DataPoints.Where(q => q.BorderColor == cl.BorderColor);

                    if (!pointsincluster.Any()) { continue; }

                    var meanX = pointsincluster.Average(p => p.X);
                    var meanY = pointsincluster.Average(p => p.Y);

                    cl.X = Math.Floor(meanX);
                    cl.Y = Math.Floor(meanY);
                }

                var algoritmFinished = _Clusters.All(a => !a.HasMoved());

                foreach (var cl in _Clusters) {
                    cl.previousX = cl.X;
                    cl.previousY = cl.Y;
                }

                if (algoritmFinished) {
                    _timer.Stop();
                    btnStart.Content = "Start";
                    stepSwitch = false;
                    Console.WriteLine("K-Means Finished");
                }
            }

            

            stepSwitch = !stepSwitch;
        }

        private Color RandomColor() {
            Thread.Sleep(1);
            var randonGen = new Random();

            var r = randonGen.Next(200);
            Thread.Sleep(1);
            var g = randonGen.Next(200);
            Thread.Sleep(1);
            var b = randonGen.Next(200);
            Thread.Sleep(1);

            var randomColor = Color.FromArgb(255, (byte)r, (byte)g, (byte)b);
            return randomColor;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e) {
            GeneratePoints();
            GenerateClusterPoints();
        }

        private void _timer_Tick(object sender, EventArgs e) {
            AlgorithmStep();
            RedrawGraphics();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e) {
            GeneratePoints();
            GenerateClusterPoints();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e) {
            if (!processing) {
                StartAlgorithm();
            } else {
                StopAlgorithm();
            }
            processing = !processing;
            btnStart.Content = processing ? "Stop" : "Start";
        }

        private void BtnResetCluster_Click(object sender, RoutedEventArgs e) {
            GenerateClusterPoints();
            _DataPoints.ForEach(p => p.BorderColor = Color.FromArgb(255, 0, 0, 0));
            canDrawArea.Children.Clear();
            RedrawGraphics();
        }

        private void BtnResetDataPoints_Click(object sender, RoutedEventArgs e) {
            GeneratePoints();
            canDrawArea.Children.Clear();
            RedrawGraphics();
        }

    }
}
