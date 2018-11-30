using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputerGraphics
{
    class Lab4
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
        /// Псевдогенератор случайных чисел для генерации цвета линий
        /// </summary>

        /// <summary>
        /// Цвет формы
        /// </summary>
        public Color BackgroundColor { get; set; }
        /// <summary>
        /// Цвет окна
        /// </summary>
        public Color PlaceColor { get; set; }
        /// <summary>
        /// Цвет границ
        /// </summary>
        public Color BoardColor { get; set; }

        //Поля для отсечения
        /// <summary>
        /// Отсекающая часть по ширине
        /// </summary>
        private int WidthPart;
        /// <summary>
        /// Отсекающая часть по высоте
        /// </summary>
        private int HeightPart;

        public Lab4(ref PictureBox picture)
        {
            Picture = picture;
            Pen = new Pen(Color.Red, 1);
            PicturePage = new Bitmap(Picture.Width, Picture.Height);
            GraphicsDraw = Graphics.FromImage(PicturePage);
            WidthPart = PicturePage.Width / 3;
            HeightPart = PicturePage.Height / 3;
        }


        //Методы для рисования
        /// <summary>
        /// Метод отрисовки линии без проверки
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="lineColor"></param>
        public void DrawLine(Point startPoint,Point endPoint,Color lineColor)
        {
            Pen.Color = lineColor;
            GraphicsDraw.DrawLine(Pen, startPoint, endPoint);
            Picture.Image = PicturePage;
            Picture.Refresh();
        }
        /// <summary>
        /// Метод отрисовки линии с урезанием под окно
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public void DrawLineWithAlgorithm(Point startPoint,Point endPoint,Color lineColor)
        {
            //Цвет линии
            Pen.Color = lineColor;
            //определение кодов точек
            int CodeStartPoint = GetCodePoint(startPoint);
            int CodeEndPoint = GetCodePoint(endPoint);
            //определение необходимости отсечения, отрисовки или игнорирования линии
            //если оба кода ==0 то вся линия находится в области отрисовки
            if (CodeStartPoint == 0 && CodeEndPoint == 0)
            {
                GraphicsDraw.DrawLine(Pen, startPoint, endPoint);
                Picture.Image = PicturePage;
                Picture.Refresh();
            }
            //определение результата коньюнкции двух кодов (если результат 0 - то линию необходимо отсекать, 
            //если >0 то линия находится вне области отрисовки и опускается
            int codeResult = CodeStartPoint & CodeEndPoint;
            //если ==0 требуется отсечение и отрисовка в пределах ограничиваемого окна
            if (codeResult == 0)
                LineAnalyze(ref startPoint, ref endPoint, ref CodeStartPoint, ref CodeEndPoint);
            //в противном случае отрисовка не нужна, т.к. линия находится за пределами отрисовки
        }
        /// <summary>
        /// Рисование границы с областью видимости
        /// </summary>
        public void DrawWindowBoard()
        {
            GraphicsDraw.Clear(BackgroundColor);
            Picture.BackColor = BackgroundColor;
            //Массив отсекающих линий
            Point[] points = new Point[] {
                new Point(WidthPart,0),new Point(WidthPart,PicturePage.Height),
                new Point(WidthPart*2,0),new Point(WidthPart*2,PicturePage.Height),
                new Point(0,HeightPart),new Point(PicturePage.Width,HeightPart),
                new Point(0,HeightPart*2),new Point(PicturePage.Width,HeightPart*2)
            };
            //Установка цвета границ
            Pen.Color = BoardColor;
            //Рисование отсекающих линий
            GraphicsDraw.DrawLine(Pen, points[0], points[1]);
            GraphicsDraw.DrawLine(Pen, points[2], points[3]);
            GraphicsDraw.DrawLine(Pen, points[4], points[5]);
            GraphicsDraw.DrawLine(Pen, points[6], points[7]);
            //Установка цвета окна
            Pen.Color = PlaceColor;
            GraphicsDraw.FillRectangle(Pen.Brush, new Rectangle(WidthPart + 1, HeightPart + 1, WidthPart -1, HeightPart-1));
            //Отображение на форме
            Picture.Image = PicturePage;
            Picture.Refresh();
        }
        /// <summary>
        /// Сброс отображаемых линий
        /// </summary>
        public void ResetDraw()
        {
            GraphicsDraw.Clear(BackgroundColor);
            DrawWindowBoard();
        }

        //Сервисные методы 
        /// <summary>
        /// Проверка пересекает ли линия окно, урезание или запрет отрисовки
        /// </summary>
        /// <param name="firstPoint">стартовая точка</param>
        /// <param name="secondPoint">конечная точка</param>
        /// <param name="CodeFirst">код первой точки</param>
        /// <param name="CodeSecond">код второй точки</param>
        private void LineAnalyze(ref Point firstPoint, ref Point secondPoint, ref int CodeFirst, ref int CodeSecond)
        {
            //проверка 2х частных случаев, когда линии вертикальные или горизонтальные с целью увеличения быстродействия
            //если линия горизонтальная
            if (firstPoint.Y == secondPoint.Y)
            {
                //разбор по каждой точке, урезаются лишь те, что не попадают в область видимости
                //если необходимо урезать первую точку
                if (CodeFirst != 0)
                {
                    if (IsBordersOnTheBoard(CodeFirst, Board.Левая))
                        firstPoint.X = WidthPart;
                    else
                        firstPoint.X = WidthPart * 2;
                }
                //если необходимо урезать вторую точку
                if (CodeSecond != 0)
                {
                    if (IsBordersOnTheBoard(CodeSecond, Board.Левая))
                        secondPoint.X = WidthPart;
                    else
                        secondPoint.X = WidthPart * 2;
                }
                GraphicsDraw.DrawLine(Pen, firstPoint, secondPoint);
                Picture.Image = PicturePage;
                Picture.Refresh();
                return;
            }
            //если линия вертикальная
            else if (firstPoint.X == secondPoint.X)
            {
                //разбор по каждой точке, урезаются лишь те, что не попадают в область видимости
                //если необходимо урезать первую точку
                if (CodeFirst != 0)
                {
                    if (IsBordersOnTheBoard(CodeFirst, Board.Верхняя))
                        firstPoint.Y = HeightPart;
                    else
                        firstPoint.Y = HeightPart * 2;
                }
                //если необходимо урезать вторую точку
                if (CodeSecond != 0)
                {
                    if (IsBordersOnTheBoard(CodeSecond, Board.Верхняя))
                        secondPoint.Y = HeightPart;
                    else
                        secondPoint.Y = HeightPart * 2;
                }
                GraphicsDraw.DrawLine(Pen, firstPoint, secondPoint);
                Picture.Image = PicturePage;
                Picture.Refresh();
                return;
            }
            //проверка возможности отсечения линии, если она попадает в зону отрисовки
            //если первая точка лежит в области то точки меняются местами
            if (CodeFirst == 0)
                SwapPoint(ref firstPoint, ref secondPoint, ref CodeFirst, ref CodeSecond);
            //отсечение с проверками на вхождение
            while (CodeFirst != 0)
            {
                //проверка кода с помощью коньюнкции маски со сдвигом
                //маска, проверка кода начинается левой граници и сдвигается на 1 разряд вправа
                int mask = 0b1000;
                bool flag = true;
                for (int i = 0; i < 4 && flag; i++)
                {
                    //переменная T указывает на пересечение
                    float T;
                    //тестовая точка для проверки принадлежности к области вхождения
                    Point testPoint = new Point();
                    //проверка совпадения с границей
                    if ((CodeFirst & mask) > 0)
                        switch (i)
                        {
                            //левая
                            case 0:
                                {
                                    T = CalculateT(true, firstPoint, secondPoint, WidthPart);
                                    //если переменная T находится в диапозоне от 0 до 1, то линия пересекает данную границу
                                    if (0 <= T && T <= 1)
                                    {
                                        //вычисление точек
                                        testPoint.X = WidthPart;
                                        testPoint.Y = (int)(firstPoint.Y + (secondPoint.Y - firstPoint.Y) * T);
                                        //проверка полученной точки на вхождение в область отрисовки
                                        if (GetCodePoint(testPoint) == 0)
                                        {
                                            firstPoint = testPoint;
                                            CodeFirst = 0;
                                            //прерывание цикла т.к. найден вариант вхождения
                                            flag = false;
                                        }
                                    }
                                }
                                break;
                            //правая
                            case 1:
                                {
                                    T = CalculateT(true, firstPoint, secondPoint, WidthPart * 2);
                                    //если переменная T находится в диапозоне от 0 до 1, то линия пересекает данную границу
                                    if (0 <= T && T <= 1)
                                    {
                                        //вычисление точек
                                        testPoint.X = WidthPart * 2;
                                        testPoint.Y = (int)(firstPoint.Y + (secondPoint.Y - firstPoint.Y) * T);
                                        //проверка полученной точки на вхождение в область отрисовки
                                        if (GetCodePoint(testPoint) == 0)
                                        {
                                            firstPoint = testPoint;
                                            CodeFirst = 0;
                                            //прерывание цикла т.к. найден вариант вхождения
                                            flag = false;
                                        }
                                    }
                                }
                                break;
                            //нижняя
                            case 2:
                                {
                                    T = CalculateT(false, firstPoint, secondPoint, HeightPart);
                                    //если переменная T находится в диапозоне от 0 до 1, то линия пересекает данную границу
                                    if (0 <= T && T <= 1)
                                    {
                                        //вычисление точек
                                        testPoint.Y = HeightPart;
                                        testPoint.X = (int)(firstPoint.X + (secondPoint.X - firstPoint.X) * T);
                                        //проверка полученной точки на вхождение в область отрисовки
                                        if (GetCodePoint(testPoint) == 0)
                                        {
                                            firstPoint = testPoint;
                                            CodeFirst = 0;
                                            //прерывание цикла т.к. найден вариант вхождения
                                            flag = false;
                                        }
                                    }
                                }
                                break;
                            //верхняя
                            case 3:
                                {
                                    T = CalculateT(false, firstPoint, secondPoint, HeightPart * 2);
                                    //если переменная T находится в диапозоне от 0 до 1, то линия пересекает данную границу
                                    if (0 <= T && T <= 1)
                                    {
                                        //вычисление точек
                                        testPoint.Y = HeightPart * 2;
                                        testPoint.X = (int)(firstPoint.X + (secondPoint.X - firstPoint.X) * T);
                                        //проверка полученной точки на вхождение в область отрисовки
                                        if (GetCodePoint(testPoint) == 0)
                                        {
                                            firstPoint = testPoint;
                                            CodeFirst = 0;
                                            //прерывание цикла т.к. найден вариант вхождения
                                            flag = false;
                                        }
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    //сдвиг маски на 1 разряд вправа
                    mask = mask >> 1;
                }
                //если исследуемая точка так и не вошла в область отрисовки => данный отрезон не пересекает область отрисовки и линия игнорируется
                if (CodeFirst != 0)
                    return;
                //если flag == false => точка была изменена на входящую в область видимости и нужно их поменять местами
                if (!flag && CodeSecond != 0)
                    SwapPoint(ref firstPoint, ref secondPoint, ref CodeFirst, ref CodeSecond);
            }
            //отрисовка полученного отрезка
            GraphicsDraw.DrawLine(Pen,firstPoint, secondPoint);
            Picture.Image = PicturePage;
            Picture.Refresh();
        }
        /// <summary>
        /// Проверяет с какой из сторон граничит точка для урезания
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        private bool IsBordersOnTheBoard(int Code, Board board)
        {
            switch (board)
            {
                case Board.Левая:
                    if ((Code & 0b1000) > 0)
                        return true;
                    break;
                case Board.Правая:
                    if ((Code & 0b0100) > 0)
                        return true;
                    break;
                case Board.Нижняя:
                    if ((Code & 0b0010) > 0)
                        return true;
                    break;
                case Board.Верхняя:
                    if ((Code & 0b0001) > 0)
                        return true;
                    break;
                default:
                    break;

            }
            return false;
        }
        /// <summary>
        /// Возвращает код точки, определяющей её вхождение в одну из 9 областей
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private int GetCodePoint(Point point)
        {
            //Код формата ЛПНВ
            int code = 00;
            //1 бит кода
            //Левый
            if (point.X < WidthPart)
                code = code | 0b1000;
            //2 бит кода
            //Правый
            if (point.X > WidthPart * 2)
                code = code | 0b0100;
            //3 бит кода
            //Нижний
            if (point.Y < HeightPart)
                code = code | 0b0010;
            //4 бит кода
            //Верхний
            if (point.Y > HeightPart*2)
                code = code | 0b0001;
            return code;
        }
        /// <summary>
        /// Обмен точками местами
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="secondPoint"></param>
        private void SwapPoint(ref Point firstPoint, ref Point secondPoint, ref int CodeFirst, ref int CodeSecord)
        {
            int tmpcode = CodeFirst;
            CodeFirst = CodeSecord;
            CodeSecord = tmpcode;
            Point tmp = firstPoint;
            firstPoint = secondPoint;
            secondPoint = tmp;
        }
        /// <summary>
        /// Вычисление переменной T для определения пересечения
        /// </summary>
        /// <param name="xy">true - координаты X, false - координаты Y</param>
        /// <param name="P1">первая точка</param>
        /// <param name="P2">вторая точка</param>
        /// <param name="Board">координата границы</param>
        /// <returns></returns>
        private float CalculateT(bool xy, Point P1, Point P2, int Board)
        {
            //если по X
            if (xy)
                return ((float)(Board - P1.X)) / (float)((P2.X - P1.X));
            //если по Y
            else
                return ((float)(Board - P1.Y)) / (float)((P2.Y - P1.Y));
        }

    }
}
