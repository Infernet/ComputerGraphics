using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace ComputerGraphics
{
    public partial class Form1 : Form
    {
        //Классы лабораторных работ
        /// <summary>
        /// Лабораторная работа №1
        /// </summary>
        private Lab1 Lab1;
        /// <summary>
        /// Лабораторная работа №2
        /// </summary>
        private Lab2 Lab2;
        /// <summary>
        /// Лабораторная работа №3
        /// </summary>
        private Lab3 Lab3;
        /// <summary>
        /// Лабораторная работа №4
        /// </summary>
        private Lab4 Lab4;
        /// <summary>
        /// Лабораторная работа №5
        /// </summary>
        private Lab5 Lab5;

        //Вспомогательные поля
        private Stopwatch stopwatch = new Stopwatch();
        private List<LineAndColor> Lines = new List<LineAndColor>();
        private int PointCount = 0;
        private Polygon Polygon = new Polygon();
        //Стиль Paint
        Graphics graphics;
        Bitmap bitmap;
        Pen pen;

        public Form1()
        {
            InitializeComponent();
            //Лабораторная №1
            radioClear.Checked = true;
            Lab1 = new Lab1(ref pictureBox1);
            Lab1.Draw();
            //Лабораторная №2
            Lab2 = new Lab2(ref pictureBox1);
            radioLab2Line.Checked = true;
            //Лабораторная №3
            Lab3 = new Lab3(ref pictureBox1);
            //Лабораторная №4
            Lab4 = new Lab4(ref pictureBox1);
            //Лабораторная №5
            Lab5 = new Lab5(ref pictureBox1);

            bitmap= new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(bitmap);
            pen=new Pen(Color.Red,1);

        }
        /// <summary>
        /// При движении по PictureBox отображает координаты точки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            switch (tabControlLabForms.SelectedIndex)
            {
                case 3:
                    lab4XcoordMouse.Text = "Координаты по X - " + e.X;
                    lab4YcoordMouse.Text = "Координаты по Y - " + e.Y;
                    break;
                case 4:
                    {
                        lab5XcoordMouse.Text = "Координаты по X - " + e.X;
                        lab5YcoordMouse.Text = "Координаты по Y - " + e.Y;
                        //if (Polygon.Points.Count != 0)
                        //{
                        //    Point point = new Point(e.X, e.Y);
                        //    graphics.Clear(pictureBox1.BackColor);

                        //    pen.Color = Polygon.Color;
                        //    for (int i = 0; i < Polygon.Points.Count - 1; i++)
                        //    {
                        //        graphics.DrawLine(pen, Polygon.Points[i], Polygon.Points[i + 1]);

                        //    }
                        //    graphics.DrawLine(pen, Polygon.Points[Polygon.Points.Count - 1], point);
                        //    pictureBox1.Image = bitmap;
                        //    pictureBox1.Refresh();
                        //}
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Фиксация точки при клике по PictureBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (tabControlLabForms.SelectedIndex)
            {
                case 3:
                    {
                        lab4LineCountLabel.Text = "Количество рисуемых линий - " + Lines.Count;
                        if (Lines.Count == 0)
                            Lines.Add(new LineAndColor());
                        PointCount++;
                        //если эта точка первая
                        if (PointCount % 2 != 0)
                            Lines[Lines.Count - 1].P1 = new Point(e.X, e.Y);
                        //вторая точка и установка цвета для линии
                        else
                        {
                            Lines[Lines.Count - 1].P2 = new Point(e.X, e.Y);
                            while (colorDialog1.ShowDialog() != DialogResult.OK)
                            {
                                MessageBox.Show("Пожалуйста выберете цвет для линии.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            Lines[Lines.Count - 1].Color = colorDialog1.Color;
                            //резер для новых линий
                            Lines.Add(new LineAndColor());
                        }
                    }
                    break;
                case 4:
                    {
                        Polygon.Points.Add(new Point(e.X, e.Y));
                        PointCount++;
                        lab5LineCountLabel.Text = "Количество вершин многоугольника - " + Polygon.Points.Count;
                    }
                    break;
                default:
                    break;
            }
        }
        private void DrawLine()
        {
            if (Polygon.Points.Count >= 2)
            {
                graphics.Clear(pictureBox1.BackColor);
                pen.Color = Polygon.Color;
                for (int i = 0; i < Polygon.Points.Count - 1; i++)
                {
                    graphics.DrawLine(pen, Polygon.Points[i], Polygon.Points[i + 1]);

                }
                pictureBox1.Image = bitmap;
                pictureBox1.Refresh();
            }
        }

        /// <summary>
        /// Очистка окна при переключении
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlLabForms_Selected(object sender, TabControlEventArgs e)
        {
            Polygon.Points.Clear();
            Lines.Clear();
            PointCount = 0;
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.BackColor = Color.FromArgb(192, 192, 255);
            pictureBox1.Refresh();
        }
        //
        //
        //
        //Лабораторная №1
        /// <summary>
        /// Запуск анимации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStartAnimation_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)numericTimeMove.Value; i++)
            {
                if (radioClear.Checked) Lab1.DrawClearWindow();
                if (radioVideoPage.Checked) Lab1.DrawVideoPages();
                if (radioXor.Checked) Lab1.DrawClearXOR();

                if ((int)numericPagePause.Value != 0)
                    Thread.Sleep((int)numericPagePause.Value);
            }
        }
        /// <summary>
        /// Изменение угла движения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericDegree_ValueChanged(object sender, EventArgs e)
        {
            Lab1.SetDegree((int)numericDegree.Value);
        }
        /// <summary>
        /// Увеличение размера фигуры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonScaleUp_Click(object sender, EventArgs e)
        {
            Lab1.ScaleUP();
        }
        /// <summary>
        /// Уменьшение размера фигуры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonScaleDown_Click(object sender, EventArgs e)
        {
            Lab1.ScaleDown();
        }
        //
        //
        //
        //Лабораторная №2
        /// <summary>
        /// Рисование с выбранного объекта с выбранными параметрами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLab2Draw_Click(object sender, EventArgs e)
        {
            Lab2.DrawCircleBresenham((int)numericLab2Length.Value);
            if (radioLab2Line.Checked)
                Lab2.DrawLineBresenham((int)numericLab2Arc.Value);
            else if (radioLab2Circle.Checked)
                Lab2.DrawCircleBresenham((int)numericLab2Length.Value);
            else
                Lab2.DrawArcBresenham((int)numericLab2Arc.Value, (int)numericLab2Length.Value);
        }
        /// <summary>
        /// Изменение описания параметров
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioLab2Line_CheckedChanged(object sender, EventArgs e)
        {
            labelLab2Length.Visible = false;
            numericLab2Length.Visible = false;
            labelLab2Arc.Visible = true;
            numericLab2Arc.Visible = true;
            labelLab2Arc.Text = "Октант";
        }
        /// <summary>
        /// Изменение описания параметров
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioLab2Circle_CheckedChanged(object sender, EventArgs e)
        {
            labelLab2Length.Text = "Радиус окружности";
            labelLab2Arc.Text = "Октант";
            labelLab2Length.Visible = true;
            numericLab2Length.Visible = true;
            labelLab2Arc.Visible = false;
            numericLab2Arc.Visible = false;
        }
        /// <summary>
        /// Изменение описания параметров
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioLab2Arc_CheckedChanged(object sender, EventArgs e)
        {
            labelLab2Length.Text = "Расстояние от центра окружности";
            labelLab2Arc.Text = "Октанты (выбранный +1)";
            labelLab2Length.Visible = true;
            numericLab2Length.Visible = true;
            labelLab2Arc.Visible = true;
            numericLab2Arc.Visible = true;
        }
        /// <summary>
        /// Правка изменений в поле октанта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericLab2Arc_ValueChanged(object sender, EventArgs e)
        {
            if (radioLab2Arc.Checked)
            {
                if ((int)numericLab2Arc.Value % 2 == 0)
                    numericLab2Arc.Value--;
            }
        }
        //
        //
        //
        //Лабораторная №3
        /// <summary>
        /// Показать фигуру до затравки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLab3Print_Click(object sender, EventArgs e)
        {
            Lab3.DrawFigure();
        }
        /// <summary>
        /// Запуск первого алгоритма затравки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLab3Fill1_Click(object sender, EventArgs e)
        {
            Lab3.DrawFigure();
            stopwatch.Restart();
            Lab3.StartFillingOne();
            stopwatch.Stop();
            textBoxLab3Result1.Text = stopwatch.ElapsedMilliseconds + "  мс";
        }
        /// <summary>
        /// Запуск второго алгоритма затравки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLab3Fill2_Click(object sender, EventArgs e)
        {
            Lab3.DrawFigure();
            stopwatch.Restart();
            Lab3.StartFillingTwo();
            stopwatch.Stop();
            textBoxLab3Result2.Text = stopwatch.ElapsedMilliseconds + "  мс";
        }
        //
        //
        //
        //Лабораторная №4
        /// <summary>
        /// Отобразить отсекающее окно
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lab4DrawGrid_Click(object sender, EventArgs e)
        {
            Lab4.DrawWindowBoard();
        }
        /// <summary>
        /// Выбор цвета фона
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lab4BackgroundColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Lab4.BackgroundColor = colorDialog1.Color;
                pictureBox1.BackColor = colorDialog1.Color;
                pictureBox1.Refresh();
            }
        }
        /// <summary>
        /// Выбор цвета границ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Lab4BoardColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                Lab4.BoardColor = colorDialog1.Color;
        }
        /// <summary>
        /// Выбор цвета отображаемого окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lab4PlaceColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                Lab4.PlaceColor = colorDialog1.Color;
        }
        /// <summary>
        /// Отобразить все линии
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lab4DrawAllLines_Click(object sender, EventArgs e)
        {

            if (Lines.Count!=0 && PointCount % 2 == 0)
                for (int i = 0; i < PointCount / 2; i++)
                    Lab4.DrawLine(Lines[i].P1, Lines[i].P2, Lines[i].Color);
            else
                MessageBox.Show("Не выбрана вторая точка для отрезка, либо нет линий для отображения", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        /// <summary>
        /// Вывод линий с учетом алгоритма отсечения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lab4DrawWithAlgorithm_Click(object sender, EventArgs e)
        {
            Lab4.DrawWindowBoard();
            if (Lines.Count != 0 && PointCount % 2 == 0)
                for (int i = 0; i < PointCount / 2; i++)
                    Lab4.DrawLineWithAlgorithm(Lines[i].P1, Lines[i].P2, Lines[i].Color);
            else
                MessageBox.Show("Не выбрана вторая точка для отрезка, либо нет линий для отображения", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        /// <summary>
        /// Сброс нарисованных линий
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lab4DrawReset_Click(object sender, EventArgs e)
        {
            PointCount = 0;
            Lines.Clear();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Refresh();
            lab4LineCountLabel.Text = "Количество рисуемых линий - " + Lines.Count;
        }
        //
        //
        //
        //Лабораторная №5
        /// <summary>
        /// Выбор цвета фона
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lab5BackgroundColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Lab5.BackgroundColor = colorDialog1.Color;
                pictureBox1.BackColor = colorDialog1.Color;
                pictureBox1.Refresh();
            }
        }
        /// <summary>
        /// Фиксация окна отсечения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lab5ButtonSaveWindow_Click(object sender, EventArgs e)
        {
            while (colorDialog1.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("Пожалуйста выберете цвет полигона.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Polygon.Color = colorDialog1.Color;
            Lab5.PolygonWindow.Points = new List<Point>(Polygon.Points);
            Lab5.PolygonWindow.Color = Polygon.Color;
            Polygon.Points.Clear();
            PointCount = 0;
        }
        /// <summary>
        /// Фиксация отсекаемого полигона
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lab5ButtonSavePolygon_Click(object sender, EventArgs e)
        {
            if (Polygon.Points.Count != 0)
            {
                while (colorDialog1.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("Пожалуйста выберете цвет полигона.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                Polygon.Color = colorDialog1.Color;
                Lab5.PolygonsSplit.Add(new Polygon());
                Lab5.PolygonsSplit[Lab5.PolygonsSplit.Count - 1].Points =new List<Point>(Polygon.Points);
                Lab5.PolygonsSplit[Lab5.PolygonsSplit.Count - 1].Color = Polygon.Color;
                Polygon.Points.Clear();
                pen.Dispose();
                PointCount = 0;
            }
            else
                MessageBox.Show("Для фиксации полигона, его следует нарисовать.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        /// <summary>
        /// Отобразить отсекающий полигон
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lab5DrawWindow_Click(object sender, EventArgs e)
        {
            if (Lab5.PolygonWindow.Points.Count != 0)
                Lab5.DrawWindow();
            else
                MessageBox.Show("Для отображения отсекающего полигона, его следует нарисовать.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        /// <summary>
        /// Отобразить все полигоны без отсечения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lab5DrawAllPolygon_Click(object sender, EventArgs e)
        {
            if (Lab5.PolygonWindow.Points.Count != 0 && Lab5.PolygonsSplit.Count != 0)
                Lab5.DrawAllPolygon();
            else
                MessageBox.Show("Для отображения полигонов, их следует нарисовать.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        /// <summary>
        /// Отобразить полигоны с учетом алгоритма
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lab5DrawPolygonsWithAlgorithm_Click(object sender, EventArgs e)
        {
            if (Lab5.PolygonWindow.Points.Count != 0 && Lab5.PolygonsSplit.Count != 0)
                Lab5.DrawPolygonsWithAlgorithm();
            else
                MessageBox.Show("Для отображения полигонов, их следует нарисовать.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void lab5ResetWindow_Click(object sender, EventArgs e)
        {
            Lab5.PolygonWindow.Points.Clear();
            MessageBox.Show("Окно удалено");
            Polygon.Points.Clear();
            pictureBox1.BackColor = Lab5.BackgroundColor;
            pictureBox1.Refresh();
        }

        private void lab5ResetDrawingPolygons_Click(object sender, EventArgs e)
        {
            Lab5.PolygonsSplit.Clear();
            MessageBox.Show("Отсекаемые многоугольники удалены");
            Polygon.Points.Clear();
            pictureBox1.BackColor = Lab5.BackgroundColor;
            pictureBox1.Refresh();
        }


    }
}
