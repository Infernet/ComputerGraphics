using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace ComputerGraphics
{
    class Lab1
    {
        //Поля для рисования
        /// <summary>
        /// Карандаш для рисования
        /// </summary>
        private Pen Pen;
        /// <summary>
        /// Видеостраница 1
        /// </summary>
        private Bitmap PicturePage1;
        /// <summary>
        /// Видеостраница 2
        /// </summary>
        private Bitmap PicturePage2;
        /// <summary>
        /// Объект Graphics для первой видеостраницы
        /// </summary>
        private Graphics GraphicsDraw1;
        /// <summary>
        /// Объект Graphics для второй видеостраницы
        /// </summary>
        private Graphics GraphicsDraw2;
        /// <summary>
        /// Конечная форма для отображения рисунка
        /// </summary>
        private PictureBox Picture;
        /// <summary>
        /// Чередование видеостраниц
        /// </summary>
        public bool SelectedPage = false;
        //Поля и свойства рисуемой фигуры
        /// <summary>
        /// Параметр масштаба фигуры
        /// </summary>
        private int Scale;
        /// <summary>
        /// Массив вершин фигуры
        /// </summary>
        private PointF[] Points = new PointF[4];


        //Поля и свойства отвечающие за перемещение
        /// <summary>
        /// Движение по X 
        /// </summary>
        private float Move_X;
        /// <summary>
        /// Движение по Y
        /// </summary>
        private float Move_Y;


        /// <summary>
        /// Коснтруктор
        /// </summary>
        /// <param name="pictureBox">Ссылка на объект PictureBox для отображения рисунка</param>
        public Lab1(ref PictureBox pictureBox)
        {
            Scale = 0;
            Move_X = 0;
            Move_Y = 0;

            Picture = pictureBox;
            Pen = new Pen(Color.Red,1);
            PicturePage1 = new Bitmap(Picture.Width, Picture.Height);
            PicturePage2 = new Bitmap(Picture.Width, Picture.Height);
            GraphicsDraw1 = Graphics.FromImage(PicturePage1);
            GraphicsDraw2 = Graphics.FromImage(PicturePage2);

            Points[0] = new PointF() { X = 20, Y = 100 };
            Points[1] = new PointF() { X = 40, Y = 80 };
            Points[2] = new PointF() { X = 80, Y = 80 };
            Points[3] = new PointF() { X = 100, Y = 100 };
        }


        //Типы отрисовок
        /// <summary>
        /// Стандартный вывод на экран без перемещения
        /// </summary>
        public void Draw()
        {
            DrawFigure();
            PictureOutput();
            Picture.Refresh();
        }
        /// <summary>
        /// Очистка окна
        /// </summary>
        public void DrawClearWindow()
        {
            Move();
            GraphicsDraw1.Clear(Picture.BackColor);
            DrawFigure();
            PictureOutput();
            Picture.Refresh();
            
        }
        /// <summary>
        /// Видеостраницами
        /// </summary>
        public void DrawVideoPages()
        {
            //Перемещение фигуры
            Move();
            //Рисование фигуры на активной видеостранице
            DrawFigure(SelectedPage);
            //Её вывод на экран
            PictureOutput(SelectedPage);
            Picture.Refresh();
            
            //Смена активной видеостраницы
            SelectedPage = !SelectedPage;
        }
        /// <summary>
        /// XOR стирание предыдущей позиции
        /// </summary>
        public void DrawClearXOR()
        {
            //установка карандаша цветом фона, для стирания предыдущей позиции
            Pen.Color = Picture.BackColor;
            //стирание предыдущей позиции
            GraphicsDraw1.DrawLine(Pen, Points[0], Points[1]);
            GraphicsDraw1.DrawLine(Pen, Points[1], Points[2]);
            GraphicsDraw1.DrawLine(Pen, Points[2], Points[3]);
            GraphicsDraw1.DrawLine(Pen, Points[3], Points[0]);

            //Изменение позиции
            Move();
            //Новая отрисовка
            Pen.Color = Color.Red;
            //отрисовка
            GraphicsDraw1.DrawLine(Pen, Points[0], Points[1]);
            GraphicsDraw1.DrawLine(Pen, Points[1], Points[2]);
            GraphicsDraw1.DrawLine(Pen, Points[2], Points[3]);
            GraphicsDraw1.DrawLine(Pen, Points[3], Points[0]);
            //вывод
            PictureOutput();
            Picture.Refresh();
            

        }


        //Отображение фигуры
        /// <summary>
        /// Подготовка фигуры к выводу на экран
        /// </summary>
        /// <param name="selectedPage">Параметр выбора видеостраницы</param>
        private void DrawFigure(bool selectedPage=true)
        {
            //Выбрана первая видеостраница
            if (selectedPage)
            {
                Pen.Color = Color.Red;
                GraphicsDraw1.Clear(Picture.BackColor);
                GraphicsDraw1.DrawLine(Pen, Points[0], Points[1]);
                GraphicsDraw1.DrawLine(Pen, Points[1], Points[2]);
                GraphicsDraw1.DrawLine(Pen, Points[2], Points[3]);
                GraphicsDraw1.DrawLine(Pen, Points[3], Points[0]);
            }
            //Выбрана вторая видеостраница
            else
            {
                Pen.Color = Color.Blue;
                GraphicsDraw2.Clear(Picture.BackColor);
                GraphicsDraw2.DrawLine(Pen, Points[0], Points[1]);
                GraphicsDraw2.DrawLine(Pen, Points[1], Points[2]);
                GraphicsDraw2.DrawLine(Pen, Points[2], Points[3]);
                GraphicsDraw2.DrawLine(Pen, Points[3], Points[0]);
            }
            
        }
        /// <summary>
        /// Вывод видеостраницы на экран
        /// </summary>
        /// <param name="selectedPage"></param>
        private void PictureOutput(bool selectedPage=true)
        {
            if (selectedPage)
                Picture.Image = PicturePage1;
            else
                Picture.Image = PicturePage2;
            Picture.Refresh();
        }
        /// <summary>
        /// Передвижение фигуры на новую позицию
        /// </summary>
        private void Move()
        {
            if (IsBoardWidth()) Move_X *= -1;
            if (IsBoardHeight()) Move_Y *= -1;

            for (int i = 0; i < Points.Length; i++)
            {
                Points[i].X += Move_X;
                Points[i].Y += Move_Y;
            }

        }


        //Изменение размеров фигуры
        /// <summary>
        /// Увеличение размера фигуры
        /// </summary>
        public void ScaleUP()
        {
            //если увеличенный размер фигуры не выйдет за границы
            if ((Points[0].Y + 2) < PicturePage1.Height &&
                (Points[0].X-2)>0 &&
                (Points[3].Y + 2) < PicturePage1.Height &&
                (Points[3].X + 2) < PicturePage1.Width &&
                (Points[2].X + 2) < PicturePage1.Width)
            {
                Points[0].Y += 2;
                Points[0].X-=2;
                Points[2].X += 2;
                Points[3].X +=4;
                Points[3].Y += 2;
                Scale += 2;
                Draw();
            }
        }
        /// <summary>
        /// Уменьшение размера фигуры
        /// </summary>
        public void ScaleDown()
        {
            if(Scale>=2)
            {
                Points[0].Y -= 2;
                Points[0].X+=2;
                Points[2].X -= 2;
                Points[3].X-=2;
                Points[3].Y -= 2;
                Scale -= 2;
                Draw();
            }
        }
        //Изменение угла вдижения
        /// <summary>
        /// Установка движения под новым углом
        /// </summary>
        /// <param name="degree"></param>
        public void SetDegree(int degree)
        {
            Move_X =(Math.Sign(Move_X)!=0?Math.Sign(Move_X):1)* (float)Math.Cos(((double)degree * Math.PI) / 180.0);
            Move_Y =(Math.Sign(Move_Y)!=0?Math.Sign(Move_Y):1)* (float)Math.Sin(((double)degree * Math.PI) / 180.0);
        }
        //Проверка на достижение границы
        /// <summary>
        /// Определение столкновения фигуры с боковыми границами
        /// </summary>
        /// <returns></returns>
        private bool IsBoardWidth()
        {
            for (int i = 0; i < Points.Length; i++)
                if ((Points[i].X+Move_X) <= 0 || (Points[i].X+Move_X) >= PicturePage1.Width)
                    return true;
            return false;
        }
        /// <summary>
        /// Определение столкновения фигуры с вертикальными границами
        /// </summary>
        /// <returns></returns>
        private bool IsBoardHeight()
        {
            for (int i = 0; i < Points.Length; i++)
                if ((Points[i].Y+Move_Y) <= 0 || (Points[i].Y+Move_Y) >= PicturePage1.Height)
                    return true;
            return false;
        }
    }
}
