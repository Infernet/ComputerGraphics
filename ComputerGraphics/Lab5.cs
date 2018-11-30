using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputerGraphics
{
    class Lab5
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
        /// Отсекающий полигон
        /// </summary>
        public Polygon PolygonWindow { get; set; }
        /// <summary>
        /// Список отсекаемых полигонов
        /// </summary>
        public List<Polygon>PolygonsSplit { get; set; }

        private List<Polygon> ResultAlgorithm = new List<Polygon>();

        public Lab5(ref PictureBox picture)
        {
            Picture = picture;
            Pen = new Pen(Color.Red, 1);
            PicturePage = new Bitmap(Picture.Width, Picture.Height);
            GraphicsDraw = Graphics.FromImage(PicturePage);
            PolygonWindow = new Polygon();
            PolygonsSplit = new List<Polygon>();
        }

        //Методы рисования
        /// <summary>
        /// Рисование границы с областью видимости
        /// </summary>
        public void DrawWindow()
        {
            GraphicsDraw.Clear(BackgroundColor);
            Picture.BackColor = BackgroundColor;
            //Установка цвета линий полигона
            Pen.Color = PolygonWindow.Color;
            //Рисование полигона
            GraphicsDraw.DrawPolygon(Pen, PolygonWindow.Points.ToArray());
            //Отображение на форме
            Picture.Image = PicturePage;
            Picture.Refresh();
        }
        /// <summary>
        /// Рисование всех полигонов
        /// </summary>
        public void DrawAllPolygon()
        {
            GraphicsDraw.Clear(BackgroundColor);
            Picture.BackColor = BackgroundColor;
            //рисование отсекаемых полигонов
            foreach (Polygon polygon in PolygonsSplit)
            {
                Pen.Color = polygon.Color;
                GraphicsDraw.DrawPolygon(Pen, polygon.Points.ToArray());
            }
            //рисование отсекающего полигона
            Pen.Color = PolygonWindow.Color;
            GraphicsDraw.DrawPolygon(Pen, PolygonWindow.Points.ToArray());
            //Отображение на форме
            Picture.Image = PicturePage;
            Picture.Refresh();
        }
        /// <summary>
        /// Рисование полигонов с учетом алгоритма
        /// </summary>
        public void DrawPolygonsWithAlgorithm()
        {
            DrawWindow();
            foreach (Polygon item in PolygonsSplit)
                StartAlgorithm(item.Points, PolygonWindow.Points,item.Color);
            Picture.Image = PicturePage;
            Picture.Refresh();
        }
    /// <summary>
    /// Определяет факт пересечения ребра полигона с ребром окна
    /// </summary>
    /// <param name="p1">Начальная точка ребра полигона</param>
    /// <param name="p2">Конечная точка ребра полигона</param>
    /// <param name="w1">Начальная точка ребра окна</param>
    /// <param name="w2">Конечная точка ребра окна</param>
    /// <returns></returns>
    private bool CrossingFactor(Point p1,Point p2,Point w1,Point w2)
        {
            float S1 = GetMatrix(p1, w1, w2).GetDeterminant();
            float S2 = GetMatrix(p2, w1, w2).GetDeterminant();
            float S3 = GetMatrix(w1, p1, p2).GetDeterminant();
            float S4 = GetMatrix(w2, p1, p2).GetDeterminant();

            if ((Math.Sign(S1) != Math.Sign(S2)) && (Math.Sign(S3) != Math.Sign(S4)))
                return true;
            else
                return false;
        }

        private Matrix GetMatrix(Point v0,Point v1,Point v2)
        {
            Matrix matrix = new Matrix(3, 3);
            matrix.MatrixList[0][0]= v0.X;
            matrix.MatrixList[0][1]= v1.X;
            matrix.MatrixList[0][2]= v2.X;

            matrix.MatrixList[1][0] = v0.Y;
            matrix.MatrixList[1][1] = v1.Y;
            matrix.MatrixList[1][2] = v2.Y;

            matrix.MatrixList[2][0] = 1;
            matrix.MatrixList[2][1] = 1;
            matrix.MatrixList[2][2] = 1;
            return matrix;
        }

        /// <summary>
        /// Определение точки пересечения ребра окна и ребра полигона
        /// </summary>
        /// <param name="p1">Точка начала ребра полигона</param>
        /// <param name="p2">Точка конца ребра полигона</param>
        /// <param name="w1">Точка начала ребра окна</param>
        /// <param name="w2">Точка конца ребра окна</param>
        /// <returns></returns>
        public Point IntersectionPoint(Point p1,Point p2,Point w1,Point w2)
        {
            //итоговая точка
            Point pointResult = new Point();
            //матрица коэфициентов
            Matrix matrixCoefficient = new Matrix(2, 2);
            //матрица параметров описания отреков
            Matrix matrixParam = new Matrix(1, 2);
            //матрица значений правой части уравнений
            Matrix matrixRight = new Matrix(1, 2);

            //Формирование матрицы коэфициентов
            matrixCoefficient.MatrixList[0][0] = p2.X-p1.X;
            matrixCoefficient.MatrixList[0][1] = w1.X-w2.X;
            matrixCoefficient.MatrixList[1][0] = p2.Y-p1.Y;
            matrixCoefficient.MatrixList[1][1] = w1.Y-w2.Y;
            //Формирование матрицы правых частей
            matrixRight.MatrixList[0][0] = w1.X - p1.X;
            matrixRight.MatrixList[1][0] = w1.Y - p1.Y;
            //Обращение матрицы и умножение матрицы коэфициентов на матрицу правых частей
            matrixCoefficient.MatrixList= matrixCoefficient.GetReverseMatrix(matrixCoefficient.MatrixList);
            matrixParam.MatrixList = matrixParam.MultiplicationMatrix(matrixCoefficient.MatrixList, matrixRight.MatrixList);
            //определение точки пересечения
            pointResult.X =(int)( p1.X + (p2.X - p1.X) * matrixParam.MatrixList[0][0]);
            pointResult.Y = (int)(p1.Y + (p2.Y - p1.Y) * matrixParam.MatrixList[0][0]);



            PointF tmp = new PointF((float)(p1.X + (p2.X - p1.X) * matrixParam.MatrixList[0][0]), (float)(p1.Y + (p2.Y - p1.Y) * matrixParam.MatrixList[0][0]));
            return pointResult;
        }



        /// <summary>
        /// Главный метод отсечения по алгоритму Сазерленда-Ходжмана
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="window"></param>
        /// <param name="polygonColor"></param>
        public void StartAlgorithm(List<Point> polygon, List<Point> window,Color polygonColor)
        {
            List<Point[]> result = new List<Point[]>();
            List<bool> includePoint = new List<bool>();
            foreach (Point item in polygon)
            {
                includePoint.Add(isIncludePoint(item, window));
            }

            for (int p = 0; p < polygon.Count; p++)
            {
                int pS = (p + 1 == polygon.Count ? 0 : p + 1);
                int pF = (p == polygon.Count ? 0 : p);
                //Случай полной видимости
                //если обе точки внутри многоугольника
                if (includePoint[pF] && includePoint[pS])
                {
                    result.Add(new Point[] { polygon[pF], polygon[pS] });
                    continue;
                }
                //временная переменная для хранения точек на случай полной невидимости
                List<Point> testPointList = new List<Point>();
                //Обработка оставшихся 3 случаев
                for (int w = 0; w <= window.Count; w++)
                {
                    int wS = (w + 1 >= window.Count ? 0 : w + 1);
                    int wF = (w >= window.Count ? 0 : w);

                    //случай вхождения или выхода из окна
                    if( (includePoint[pF]&&!includePoint[pS]) || (!includePoint[pF]&&includePoint[pS]) )
                        //если обнаружено ребро, с которым есть пересечение
                        if (CrossingFactor(polygon[pF], polygon[pS], window[wF], window[wS]))
                        {
                            Point intersectionPoint=IntersectionPoint(polygon[pF], polygon[pS], window[wF], window[wS]);
                            //проверка точки пересечение на вхождение в многоугольник
                                result.Add(new Point[] { (includePoint[pF]?polygon[pF]:polygon[pS]) ,intersectionPoint });
                                break;
                        }
                    //случай полной невидимости (возможно пересечение ребер на сквозь)
                    //необходимо проверить факт пересечения с каждым ребром
                    if(CrossingFactor(polygon[pF],polygon[pS],window[wF],window[wS]))
                    {
                        Point tmp = IntersectionPoint(polygon[pF], polygon[pS], window[wF], window[wS]);
                            testPointList.Add(tmp);
                    }

                }
                if (testPointList.Count == 2)
                {
                    result.Add(new Point[] { testPointList[0], testPointList[1] });
                    testPointList.Clear();
                }
                else
                    testPointList.Clear();
            }

            Pen.Color = polygonColor;
            foreach (Point[] item in result)
            {
                GraphicsDraw.DrawLine(Pen,item[0],item[1]);
            }
        }

        /// <summary>
        /// Проверка на вклочение точки в многоугольник
        /// </summary>
        /// <param name="point"></param>
        /// <param name="Polygon"></param>
        /// <returns></returns>
        private bool isIncludePoint(Point point, List<Point> Polygon)
        {
            double Result = 0.0;
            for (int i = 0; i < Polygon.Count; i++)
            {
                Point vectorA = GetVector(point, Polygon[(i == Polygon.Count ? 0 : i)]);
                Point vectorB = GetVector(point, Polygon[((i + 1) == Polygon.Count ? 0 : (i + 1))]);

                double arcVectors = GetArgVerctors(vectorA, vectorB);
                Result += Math.Sign(GetZCoord(vectorA, vectorB)) * arcVectors;
            }

            Result = Math.Round(Result / 2.0);
            if (Result.Equals(double.NaN) || Result != 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Возвращает угол между векторами
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        private double GetArgVerctors(Point A, Point B)
        {
            double alpha = 0.0;
            double vecMul = A.X * B.X + A.Y * B.Y;
            double modulVecMul = Math.Sqrt((Math.Pow(A.X, 2) + Math.Pow(A.Y, 2)) * (Math.Pow(B.X, 2) + Math.Pow(B.Y, 2)));
            alpha = Math.Acos(vecMul / modulVecMul);
            return alpha;
        }
        /// <summary>
        /// Получение вектора из двух точек
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private Point GetVector(Point start, Point end)
        {
            return new Point(end.X - start.X, end.Y - start.Y);
        }
        /// <summary>
        /// Возвращает Z-компоненту векторного произведения двух векторов
        /// </summary>
        /// <param name="vectorA">Вектор А</param>
        /// <param name="vectorB">Вектор B</param>
        /// <returns></returns>
        private int GetZCoord(Point vectorA, Point vectorB)
        {
            return vectorA.X * vectorB.Y - vectorA.Y * vectorB.X;
        }
    }
}
