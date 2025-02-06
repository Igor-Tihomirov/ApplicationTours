using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Приложение_Турагенства.Classes
{
    public class clCaptchaGenerator
    {
        private static readonly Random rnd = new Random();
        public string CaptchaText { get; private set; }

        public BitmapSource GenerateCaptcha(double widthParam, double heightParam, int textLength = 5)
        {
            int width = (int)Math.Floor(widthParam);
            int height = (int)Math.Floor(heightParam);

            CaptchaText = GenerateRandomText(textLength);
            DrawingVisual drawingVisual = new DrawingVisual();

            using (DrawingContext dc = drawingVisual.RenderOpen())
            {
                // Фон
                dc.DrawRectangle(Brushes.Gray, null, new Rect(0, 0, width, height));

                // Цвета текста
                Brush[] colors = { Brushes.Black, Brushes.Red, Brushes.RoyalBlue, Brushes.Green };

                // Отрисовка текста с разными размерами и наклоном
                double charX = (width - (20 * textLength)) / 2;
                for (int i = 0; i < CaptchaText.Length; i++)
                {
                    double fontSize = rnd.Next(22, 38);
                    double angle = rnd.Next(-30, 30);
                    //Brush textColor = colors[rnd.Next(colors.Length)];
                    Brush textColor = Brushes.Black;

                    FormattedText formattedText = new FormattedText(
                        CaptchaText[i].ToString(),
                        System.Globalization.CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface("Arial"),
                        fontSize,
                        textColor,
                        1.25
                    );

                    RotateTransform rotateTransform = new RotateTransform(angle, charX + fontSize / 2, height / 2);
                    dc.PushTransform(rotateTransform);
                    dc.DrawText(formattedText, new Point(charX, rnd.Next(10, height - 40)));
                    dc.Pop();

                    charX += formattedText.Width + 7;
                }

                // Линии-помехи
                for (int i = 0; i < 6; i++)
                {
                    Pen linePen = new Pen(colors[rnd.Next(colors.Length)], rnd.Next(1, 3));
                    dc.DrawLine(linePen,
                                new Point(rnd.Next(0, width), rnd.Next(0, height)),
                                new Point(rnd.Next(0, width), rnd.Next(0, height)));
                }

                // Белые точки (шум)
                for (int i = 0; i < width; ++i)
                    for (int j = 0; j < height; ++j)
                        if (rnd.Next() % 20 == 0)
                            dc.DrawRectangle(Brushes.White, null, new Rect(i, j, 1, 1));
            }

            RenderTargetBitmap bitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(drawingVisual);
            return bitmap;
        }

        private string GenerateRandomText(int length)
        {
            const string chars = "1234567890QWERTYUOPASDFGHJKLZXCVBNM";
            char[] textArray = new char[length];
            for (int i = 0; i < length; i++)
            {
                textArray[i] = chars[rnd.Next(chars.Length)];
            }                
            return new string(textArray);
        }
    }
}










//=============================================
//for (int i = 0; i < 6; i++)
//{
//    // Определение пути кривой Безье
//    PathFigure pathFigure = new PathFigure
//    {
//        StartPoint = new Point(rnd.Next(0, width), rnd.Next(0, height)) // Начальная точка
//    };

//    // Кубическая кривая Безье (три контрольные точки)
//    BezierSegment bezierSegment = new BezierSegment(
//        new Point(rnd.Next(0, width), rnd.Next(0, height)),      // Первая контрольная точка
//        new Point(rnd.Next(0, width), rnd.Next(0, height)),  // Вторая контрольная точка
//        new Point(rnd.Next(0, width), rnd.Next(0, height)), // Конечная точка
//        true
//    );

//    pathFigure.Segments.Add(bezierSegment);

//    PathGeometry pathGeometry = new PathGeometry();
//    pathGeometry.Figures.Add(pathFigure);

//    // Отрисовка кривой
//    dc.DrawGeometry(null, new Pen(colors[rnd.Next(colors.Length)], rnd.Next(1, 3)), pathGeometry);
//}
//===================================

