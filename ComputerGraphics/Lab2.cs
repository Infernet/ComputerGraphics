using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputerGraphics
{
    class Lab2
    {
        //Поля для рисования
        /// <summary>
        /// Карандаш для рисования
        /// </summary>
        private Pen Pen;
        /// <summary>
        /// Видеостраница 1
        /// </summary>
        private Bitmap PicturePage;
        /// <summary>
        /// Объект Graphics для первой видеостраницы
        /// </summary>
        private Graphics GraphicsDraw;
        /// <summary>
        /// Конечная форма для отображения рисунка
        /// </summary>
        private PictureBox Picture;

        /// <summary>
        /// Коснтруктор
        /// </summary>
        /// <param name="pictureBox">Ссылка на объект PictureBox для отображения рисунка</param>
        public Lab2(ref PictureBox pictureBox)
        {
            Picture = pictureBox;
            Pen = new Pen(Color.Red, 1);
            PicturePage = new Bitmap(Picture.Width, Picture.Height);
            GraphicsDraw = Graphics.FromImage(PicturePage);

        }

        //Типы отрисовок

        /// <summary>
        /// Алгоритм Брезенхема для отрисовки линии
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        public void DrawLineBresenham(int arc)
        {
            GraphicsDraw.Clear(Picture.BackColor);

            Point[] Points = GetLineCoord(arc);
            DrawGrid();

            bool steep = Math.Abs(Points[1].Y - Points[0].Y) > Math.Abs(Points[1].X - Points[0].X); // Проверяем рост отрезка по оси X и по оси Y
            // Отражаем линию по диагонали, если угол наклона слишком большой
            if (steep)
                Swap(ref Points[0], ref Points[1],true); // Перетасовка координат вынесена в отдельную функцию для красоты
            // Если линия растёт не слева направо, то меняем начало и конец отрезка местами
            if (Points[0].X > Points[1].X)
                Swap(ref Points[0], ref Points[1]);

            int dx = Points[1].X - Points[0].X;
            int dy = Math.Abs(Points[1].Y - Points[0].Y);
            int error = dx / 2; // Здесь используется оптимизация с умножением на dx, чтобы избавиться от лишних дробей
            int ystep = (Points[0].Y < Points[1].Y) ? 1 : -1; // Выбираем направление роста координаты y
            int y = Points[0].Y;
            for (int x = Points[0].X; x <= Points[1].X; x++)
            {
                // Не забываем вернуть координаты на место
                PicturePage.SetPixel((steep ? y : x), (steep ? x : y), Color.Red);
                error -= dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }

            Picture.Image = PicturePage;
            Picture.Refresh();
        }
        /// <summary>
        /// Алгоритм Брезенхема для отрисовки окружности
        /// </summary>
        /// <param name="arc"></param>
        /// <param name="radius"></param>
        public void DrawCircleBresenham(int radius)
        {
            GraphicsDraw.Clear(Picture.BackColor);
            DrawGrid();
            //центр окружности
            Point pointCenter = new Point(Picture.Width / 2 - 1, Picture.Height / 2 - 1);
            int x = 0;
            int y = radius;
            int gap = 0;
            int delta = 2 - 2 * radius;

            while (y >= 0)
            {
                PicturePage.SetPixel(pointCenter.X + x, pointCenter.Y + y, Color.Yellow);
                PicturePage.SetPixel(pointCenter.X + x, pointCenter.Y - y, Color.Red);
                PicturePage.SetPixel(pointCenter.X - x, pointCenter.Y + y, Color.Green);
                PicturePage.SetPixel(pointCenter.X - x, pointCenter.Y - y, Color.Violet);

                gap = 2 * (delta + y) - 1;
                if (delta < 0 && gap <= 0)
                {
                    x++;
                    delta += 2 * x + 1;
                    continue;
                }
                if (delta > 0 && gap > 0)
                {
                    y--;
                    delta -= 2 * y + 1;
                    continue;
                }
                x++;
                delta += 2 * (x - y);
                y--;
            }

            Picture.Image = PicturePage;
            Picture.Refresh();
        }
        /// <summary>
        /// Алгоритм Брезенхема для отрисовки дуги
        /// </summary>
        /// <param name="arc"></param>
        /// <param name="radius"></param>
        public void DrawArcBresenham(int arc,int radius)
        {
            GraphicsDraw.Clear(Picture.BackColor);
            DrawGrid();
            //центр окружности
            Point pointCenter = new Point(Picture.Width / 2 - 1, Picture.Height / 2 - 1);
            int x = 0;
            int y = radius;
            int gap = 0;
            int delta = 2 - 2 * radius;

            while (y >= 0)
            {
                switch (arc)
                {
                    case 1:
                        PicturePage.SetPixel(pointCenter.X + x, pointCenter.Y - y, Color.Red);
                        break;
                    case 3:
                        PicturePage.SetPixel(pointCenter.X - x, pointCenter.Y - y, Color.Violet);
                        break;
                    case 5:
                        PicturePage.SetPixel(pointCenter.X - x, pointCenter.Y + y, Color.Green);
                        break;
                    case 7:
                        PicturePage.SetPixel(pointCenter.X + x, pointCenter.Y + y, Color.Yellow);
                        break;
                    default:
                        break;
                }
                gap = 2 * (delta + y) - 1;
                if (delta < 0 && gap <= 0)
                {
                    x++;
                    delta += 2 * x + 1;
                    continue;
                }
                if (delta > 0 && gap > 0)
                {
                    y--;
                    delta -= 2 * y + 1;
                    continue;
                }
                x++;
                delta += 2 * (x - y);
                y--;
            }

            Picture.Image = PicturePage;
            Picture.Refresh();
        }






        //Вспомогательные методы
        /// <summary>
        /// Определяет по октанту и длине координат для рисования линии
        /// </summary>
        /// <param name="Arc"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private Point[] GetLineCoord(int Arc)
        {
            Point[] Result = new Point[2];
            switch (Arc)
            {
                case 1:
                case 5:
                    Result[0] = new Point(600, 150);
                    Result[1] = new Point(0, 450);
                    break;
                case 2:
                case 6:
                    Result[0] = new Point(450, 0);
                    Result[1] = new Point(150, 600);
                    break;
                case 3:
                case 7:
                    Result[0] = new Point(150, 0);
                    Result[1] = new Point(450, 600);
                    break;
                case 4:
                case 8:
                    Result[0] = new Point(0, 150);
                    Result[1] = new Point(600, 450);
                    break;
                default:
                    break;
            }
            return Result;

        }
        /// <summary>
        /// Выполняет специальную отрисовку сетки
        /// </summary>
        /// <param name="radius"></param>
        private void DrawGrid()
        {
            Pen.Color = Color.Blue;
            //Разделение сектора на октанты
            GraphicsDraw.DrawLine(Pen, new Point(0, 0), new Point(Picture.Width - 1, Picture.Height - 1));
            GraphicsDraw.DrawLine(Pen, new Point(Picture.Width - 1, 0), new Point(0, Picture.Height - 1));
            GraphicsDraw.DrawLine(Pen, new Point(Picture.Width / 2, 0), new Point(Picture.Width / 2, Picture.Height - 1));
            GraphicsDraw.DrawLine(Pen, new Point(0, Picture.Height / 2), new Point(Picture.Width - 1, Picture.Height / 2));
        }
        /// <summary>
        /// Замена координат местами
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void Swap(ref Point x, ref Point y,bool deal=false)
        {
            //если не отражаем по диагонали
            if (!deal)
            {
                Point tmp = x;
                x = y;
                y = tmp;
            }
            else
            {
                int tmp = x.X;
                x.X = x.Y;
                x.Y = tmp;

                tmp = y.X;
                y.X = y.Y;
                y.Y = tmp;
            }
        }


    }
}
