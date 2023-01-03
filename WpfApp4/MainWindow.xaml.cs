using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAPP4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point start, dest;
        Color strokeColor = Colors.Red;
        Color fillColor = Colors.Yellow;
        Brush currentStrokeBrush;
        Brush currentFillBrush;
        int strokeThickness = 3;
        string ShapeType = "Line";

        public MainWindow()
        {
            InitializeComponent();
            strokeColorPicker.SelectedColor = strokeColor;
            fillColorPicker.SelectedColor = fillColor;
        }

        private void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            myCanvas.Cursor = Cursors.Cross;
            start = e.GetPosition(myCanvas);
            switch (ShapeType)
            {
                case "Line":
                    DrawLine(Brushes.Gray, 1);
                    break;
                case "Rectangle":
                    DrawRectangle(Brushes.Gray, Brushes.LightGray, 1);
                    break;
                case "Ellipse":
                    DrawEllipse(Brushes.Gray, Brushes.LightGray, 1);
                    break;
                case "Polyline":
                    break;
            }
            DisplayStatus();
        }

        private void myCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            dest = e.GetPosition(myCanvas);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                switch (ShapeType)
                {
                    case "Line":
                        var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                        line.X2 = dest.X;
                        line.Y2 = dest.Y;
                        break;
                    case "Rectangle":
                        var rect = myCanvas.Children.OfType<Rectangle>().LastOrDefault();
                        UpdateShape(rect);
                        break;
                    case "Ellipse":
                        var ellipse = myCanvas.Children.OfType<Ellipse>().LastOrDefault();
                        UpdateShape(ellipse);
                        break;
                    case "Polyline":
                        break;
                }
            }
            DisplayStatus();
        }

        private void myCanvas_MouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (ShapeType)
            {
                case "Line":
                    var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                    UpdateShape(line, currentStrokeBrush, strokeThickness);
                    break;
                case "Rectangle":
                    var rect = myCanvas.Children.OfType<Rectangle>().LastOrDefault();
                    UpdateShape(rect, currentStrokeBrush, currentFillBrush, strokeThickness);
                    break;
                case "Ellipse":
                    var ellipse = myCanvas.Children.OfType<Ellipse>().LastOrDefault();
                    UpdateShape(ellipse, currentStrokeBrush, currentFillBrush, strokeThickness);
                    break;
            }
            myCanvas.Cursor = Cursors.Arrow;
            DisplayStatus();
        }

        private void UpdateShape(Shape shape)
        {
            Point origin;
            origin.X = Math.Min(start.X, dest.X);
            origin.Y = Math.Min(start.Y, dest.Y);
            double width = Math.Abs(dest.X - start.X);
            double height = Math.Abs(dest.Y - start.Y);

            shape.Width = width;
            shape.Height = height;
            shape.SetValue(Canvas.LeftProperty, origin.X);
            shape.SetValue(Canvas.TopProperty, origin.Y);
        }

        private void UpdateShape(Shape shape, Brush stroke, int thickness)
        {
            shape.Stroke = stroke;
            shape.StrokeThickness = thickness;
        }

        private void UpdateShape(Shape shape, Brush stroke, Brush fill, int thickness)
        {
            shape.Stroke = stroke;
            shape.Fill = fill;
            shape.StrokeThickness = thickness;
        }

        private void DrawEllipse(Brush stroke, Brush fill, int thickness)
        {
            Ellipse ellipse = new Ellipse()
            {
                Stroke = stroke,
                Fill = fill,
                StrokeThickness = thickness
            };
            UpdateShape(ellipse);
            myCanvas.Children.Add(ellipse);
        }

        private void DrawRectangle(Brush stroke, Brush fill, int thickness)
        {
            Rectangle rect = new Rectangle()
            {
                Stroke = stroke,
                Fill = fill,
                StrokeThickness = thickness
            };
            UpdateShape(rect);
            myCanvas.Children.Add(rect);
        }

        private void DrawLine(Brush stroke, int thickness)
        {
            Line line = new Line()
            {
                X1 = start.X,
                Y1 = start.Y,
                X2 = dest.X,
                Y2 = dest.Y,
                Stroke = stroke,
                StrokeThickness = thickness
            };
            myCanvas.Children.Add(line);
        }

        private void strokeColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            strokeColor = (Color)e.NewValue;
            currentStrokeBrush = new SolidColorBrush(strokeColor);
        }

        private void fillColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            fillColor = (Color)e.NewValue;
            currentFillBrush = new SolidColorBrush(fillColor);
        }

        private void thicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            strokeThickness = (int)e.NewValue;
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton targetRadioButton = sender as RadioButton;
            ShapeType = targetRadioButton.Content.ToString();
        }

        private void DisplayStatus()
        {
            int lineCount = myCanvas.Children.OfType<Line>().Count();
            int rectangleCount = myCanvas.Children.OfType<Rectangle>().Count();
            int ellipseCount = myCanvas.Children.OfType<Ellipse>().Count();
            int polylineCount = myCanvas.Children.OfType<Polyline>().Count();
            coordinateLabel.Content = $"座標點 : ({Math.Round(start.X)}, {Math.Round(start.Y)}) : ({Math.Round(dest.X)}, {Math.Round(dest.Y)})";
            shapeLabel.Content = $"Line: {lineCount}, Rectangle: {rectangleCount}, Ellipse: {ellipseCount}, Polyline: {polylineCount}";
        }
    }
}
