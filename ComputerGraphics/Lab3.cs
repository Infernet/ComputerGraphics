using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputerGraphics
{
    class Lab3
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

        private Stack<Point> StackFill=new Stack<Point>();

        /// <summary>
        /// Цвет фона
        /// </summary>
        private Color BackgroundColor;
        /// <summary>
        /// Цвет границы фигуры
        /// </summary>
        private Color BoardColor;
        /// <summary>
        /// Цвет затравки
        /// </summary>
        private Color FillingColor;

        /// <summary>
        /// Коснтруктор
        /// </summary>
        /// <param name="pictureBox">Ссылка на объект PictureBox для отображения рисунка</param>
        public Lab3(ref PictureBox pictureBox)
        {
            Picture = pictureBox;
            Pen = new Pen(Color.FromArgb(255,0,0), 1);
            PicturePage = new Bitmap(Picture.Width, Picture.Height);
            GraphicsDraw = Graphics.FromImage(PicturePage);
            BackgroundColor = Picture.BackColor;
            BoardColor = Pen.Color;
            FillingColor = Color.FromArgb(254, 0, 0);
        }

        //Типы отрисовок

        /// <summary>
        /// Вывод незатравленной фигуры на экран
        /// </summary>
        public void DrawFigure()
        {
            GraphicsDraw.Clear(Picture.BackColor);
            Point[] points = new Point[8];
            points[0] = new Point() { X = 100, Y = 100 };
            points[1] = new Point() { X = 500, Y = 100 };
            points[2] = new Point() { X = 500, Y = 250 };
            points[3] = new Point() { X = 350, Y = 250 };
            points[4] = new Point() { X = 350, Y = 380 };
            points[5] = new Point() { X = 500, Y = 380 };
            points[6] = new Point() { X = 500, Y = 490 };
            points[7] = new Point() { X = 100, Y = 490 };
            GraphicsDraw.DrawPolygon(Pen, points);
            Picture.Image = PicturePage;
            Picture.Refresh();
            StackFill.Clear();
            StackFill.Push(new Point(101, 101));
        }
        /// <summary>
        /// Запуск первого алгоритма
        /// </summary>
        public void StartFillingOne()
        {
            //пока стек не опустеет
            while (StackFill.Count != 0)
            {
                Point vPoint = StackFill.Peek();
                StackFill.Pop();
                //Окрашивание вытолкнутого из стека пикселя
                PicturePage.SetPixel(vPoint.X,vPoint.Y, FillingColor);

                //Проверка соседних пикселей на границу и необходимость затравки

                //1 
                if (PicturePage.GetPixel(vPoint.X + 1, vPoint.Y) != BoardColor && PicturePage.GetPixel( vPoint.X + 1,vPoint.Y) != FillingColor)
                    StackFill.Push(new Point(vPoint.X + 1, vPoint.Y));
                //2
                if (PicturePage.GetPixel(vPoint.X - 1, vPoint.Y) != BoardColor && PicturePage.GetPixel(vPoint.X - 1, vPoint.Y) != FillingColor)
                    StackFill.Push(new Point(vPoint.X - 1, vPoint.Y));
                //3
                if (PicturePage.GetPixel(vPoint.X, vPoint.Y+1) != BoardColor && PicturePage.GetPixel(vPoint.X, vPoint.Y+1) != FillingColor)
                    StackFill.Push(new Point(vPoint.X, vPoint.Y+1));
                //4
                if (PicturePage.GetPixel(vPoint.X, vPoint.Y-1) != BoardColor && PicturePage.GetPixel(vPoint.X, vPoint.Y-1) != FillingColor)
                    StackFill.Push(new Point(vPoint.X, vPoint.Y-1));



                //Если точка не граничит с границей фигуры то проверяем диагональные точки на возможность затравки
                if (!IsBoard(vPoint))
                {
                    //5
                    if (PicturePage.GetPixel(vPoint.X + 1, vPoint.Y+1) != BoardColor && PicturePage.GetPixel(vPoint.X + 1, vPoint.Y+1) != FillingColor)
                        StackFill.Push(new Point(vPoint.X + 1, vPoint.Y+1));
                    //6
                    if (PicturePage.GetPixel(vPoint.X - 1, vPoint.Y - 1) != BoardColor && PicturePage.GetPixel(vPoint.X - 1, vPoint.Y - 1) != FillingColor)
                        StackFill.Push(new Point(vPoint.X - 1, vPoint.Y - 1));
                    //7
                    if (PicturePage.GetPixel(vPoint.X + 1, vPoint.Y - 1) != BoardColor && PicturePage.GetPixel(vPoint.X + 1, vPoint.Y - 1) != FillingColor)
                        StackFill.Push(new Point(vPoint.X + 1, vPoint.Y - 1));
                    //8
                    if (PicturePage.GetPixel(vPoint.X - 1, vPoint.Y + 1) != BoardColor && PicturePage.GetPixel(vPoint.X - 1, vPoint.Y + 1) != FillingColor)
                        StackFill.Push(new Point(vPoint.X - 1, vPoint.Y + 1));
                }
            }
            Picture.Image = PicturePage;
            Picture.Refresh();
        }
        /// <summary>
        /// Запуск второго алгоритма
        /// </summary>
        public void StartFillingTwo()
        {
            PicturePage.SetPixel(101, 101, FillingColor);
            //пока стек не опустеет
            while (StackFill.Count != 0)
            {
                Point vPoint = StackFill.Peek();
                StackFill.Pop();
                //Окрашивание вытолкнутого из стека пикселя
                PicturePage.SetPixel(vPoint.X, vPoint.Y, FillingColor);

                //Проверка соседних пикселей на границу и необходимость затравки

                //1 
                if (PicturePage.GetPixel(vPoint.X + 1, vPoint.Y) != BoardColor && PicturePage.GetPixel(vPoint.X + 1, vPoint.Y) != FillingColor)
                {
                    PicturePage.SetPixel(vPoint.X + 1, vPoint.Y, FillingColor);
                    StackFill.Push(new Point(vPoint.X + 1, vPoint.Y));
                }
                    
                //2
                if (PicturePage.GetPixel(vPoint.X - 1, vPoint.Y) != BoardColor && PicturePage.GetPixel(vPoint.X - 1, vPoint.Y) != FillingColor)
                {
                    PicturePage.SetPixel(vPoint.X - 1, vPoint.Y, FillingColor);
                    StackFill.Push(new Point(vPoint.X - 1, vPoint.Y));
                }
                //3
                if (PicturePage.GetPixel(vPoint.X, vPoint.Y + 1) != BoardColor && PicturePage.GetPixel(vPoint.X, vPoint.Y + 1) != FillingColor)
                {
                    PicturePage.SetPixel(vPoint.X, vPoint.Y + 1, FillingColor);
                    StackFill.Push(new Point(vPoint.X, vPoint.Y + 1));
                }
                //4
                if (PicturePage.GetPixel(vPoint.X, vPoint.Y - 1) != BoardColor && PicturePage.GetPixel(vPoint.X, vPoint.Y - 1) != FillingColor)
                {
                    PicturePage.SetPixel(vPoint.X, vPoint.Y - 1, FillingColor);
                    StackFill.Push(new Point(vPoint.X, vPoint.Y - 1));
                }



                //Если точка не граничит с границей фигуры то проверяем диагональные точки на возможность затравки
                if (!IsBoard(vPoint))
                {
                    //5
                    if (PicturePage.GetPixel(vPoint.X + 1, vPoint.Y + 1) != BoardColor && PicturePage.GetPixel(vPoint.X + 1, vPoint.Y + 1) != FillingColor)
                    {
                        PicturePage.SetPixel(vPoint.X + 1, vPoint.Y + 1, FillingColor);
                        StackFill.Push(new Point(vPoint.X + 1, vPoint.Y + 1));
                    }
                    //6
                    if (PicturePage.GetPixel(vPoint.X - 1, vPoint.Y - 1) != BoardColor && PicturePage.GetPixel(vPoint.X - 1, vPoint.Y - 1) != FillingColor)
                    {
                        PicturePage.SetPixel(vPoint.X - 1, vPoint.Y - 1, FillingColor);
                        StackFill.Push(new Point(vPoint.X - 1, vPoint.Y - 1));
                    }
                    //7
                    if (PicturePage.GetPixel(vPoint.X + 1, vPoint.Y - 1) != BoardColor && PicturePage.GetPixel(vPoint.X + 1, vPoint.Y - 1) != FillingColor)
                    {
                        PicturePage.SetPixel(vPoint.X + 1, vPoint.Y - 1, FillingColor);
                        StackFill.Push(new Point(vPoint.X + 1, vPoint.Y - 1));
                    }
                    //8
                    if (PicturePage.GetPixel(vPoint.X - 1, vPoint.Y + 1) != BoardColor && PicturePage.GetPixel(vPoint.X - 1, vPoint.Y + 1) != FillingColor)
                    {
                        PicturePage.SetPixel(vPoint.X - 1, vPoint.Y + 1, FillingColor);
                        StackFill.Push(new Point(vPoint.X - 1, vPoint.Y + 1));
                    }
                }
            }
            Picture.Image = PicturePage;
            Picture.Refresh();
        }


        /// <summary>
        /// Проверка граничит ли точка с границей фигуры
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private bool IsBoard(Point point)
        {
            if (PicturePage.GetPixel(point.X + 1, point.Y) == BoardColor ||
                PicturePage.GetPixel(point.X - 1, point.Y) == BoardColor ||
                PicturePage.GetPixel(point.X, point.Y + 1) == BoardColor ||
                PicturePage.GetPixel(point.X, point.Y - 1) == BoardColor ||
                PicturePage.GetPixel(point.X + 1, point.Y + 1) == BoardColor ||
               PicturePage.GetPixel(point.X - 1, point.Y - 1) == BoardColor ||
                PicturePage.GetPixel(point.X + 1, point.Y - 1) == BoardColor ||
                PicturePage.GetPixel(point.X - 1, point.Y + 1) == BoardColor)
                return true;
            else return false;
        }
    }
}
