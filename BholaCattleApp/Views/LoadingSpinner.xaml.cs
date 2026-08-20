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

namespace BholaCattleApp.Views
{
    public partial class LoadingSpinner : UserControl
    {
        public LoadingSpinner()
        {
            InitializeComponent();
            Loaded += (s, e) => DrawArc();
        }
        private void DrawArc()
        {
            const double cx = 35, cy = 35, r = 32;
            const double startAngleDeg = 0;
            const double sweepAngleDeg = 270; // 270° gap gives clear open end

            double startRad = startAngleDeg * Math.PI / 180;
            double endRad = (startAngleDeg + sweepAngleDeg) * Math.PI / 180;

            Point startPoint = new Point(
                cx + r * Math.Cos(startRad),
                cy + r * Math.Sin(startRad));

            Point endPoint = new Point(
                cx + r * Math.Cos(endRad),
                cy + r * Math.Sin(endRad));

            var figure = new PathFigure { StartPoint = startPoint, IsClosed = false };
            figure.Segments.Add(new ArcSegment
            {
                Point = endPoint,
                Size = new Size(r, r),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = sweepAngleDeg > 180
            });

            SpinnerArc.Data = new PathGeometry(new[] { figure });

            SpinnerArc.Stroke = new LinearGradientBrush
            {
                StartPoint = new Point(1, 1), // ← swapped
                EndPoint = new Point(0, 0), // ← swapped
                GradientStops = new GradientStopCollection
                {
                    new GradientStop(Color.FromArgb(0,   74, 144, 226), 0.0), // transparent tail
                    new GradientStop(Color.FromArgb(180, 74, 144, 226), 0.5), // mid
                    new GradientStop(Color.FromArgb(255, 74, 144, 226), 1.0)  // solid head
                }
            };
        }
    }
}
