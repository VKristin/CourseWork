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
using System.IO;
using System.Configuration;

namespace builders
{
    public partial class mainForm : Form
    {
        string errorStr = "";
        public int numOfCellsX = -1;
        public int numOfCellsY = -1;
        int cellSize;
        procedure procWindow;
        List<Points> points = new List<Points>();
        List<string> actions = new List<string>();
        List<Beam> beam1 = new List<Beam>(); //балки от 1 бригады
        List<Beam> beam2 = new List<Beam>(); //балки от 2 бригады
        List<Beam> beam3 = new List<Beam>(); //балки от 3 бригады
        List<Beam> taskBeam = new List<Beam>();
        List<Beam> lb = new List<Beam>(); //балки из задания
        List<Beam> bb = new List<Beam>(); //лишние балки
        Graphics ev;
        List<Action> actionList = new List<Action>();
        bool draw = false;
        bool clear = false;
        int speed;
        bool task = false;
        bool check = false;
        Color c;
        public int step = -1;
        string directory;

        public mainForm()
        {
            InitializeComponent();
            speed = trackBarSpeed.Value;
            this.Left = 0;
            this.Top = 0;
            AddWindow();
            fillLists();
            c = procWindow.dgvFirst.Rows[0].HeaderCell.Style.BackColor;
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                directory = ConfigurationManager.AppSettings["path"];
                if (directory == null || directory == "" || !File.Exists(directory))
                    directory = Directory.GetCurrentDirectory();
            }
            catch
            {
                directory = Directory.GetCurrentDirectory();
            }
        }

        public void AddWindow()
        {
            procWindow = new procedure(actions, this);
            procWindow.Owner = this; //Передаём вновь созданной форме её владельца.
            procWindow.Show();
        }

        public void fillLists()
        {
            actions.Add("поставить в точку");
            actions.Add("поставить на левый край балки");
            actions.Add("поставить на правый край балки");
            actions.Add("поставить на средину балки");
            actions.Add("положить между балками");
            actions.Add("положить серединой на балку");
            actions.Add("положить на землю между точками");
            actions.Add("пауза"); //пустая команда
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            if (numOfCellsX == -1 && numOfCellsY == -1)
            {
                numOfCellsY = 4;
                numOfCellsX = 4;
            }
            Pen pen = new Pen(Color.LightGray, 1);

            cellSize = trackBarSize.Value;
            int begLetterY = numOfCellsY * cellSize + 10; //откуда начинать писать буквы по Y
            int begLetterX = cellSize; //откуда начинать писать буквы по X
            playingFieldBox.Width = numOfCellsX * cellSize + 1;
            playingFieldBox.Height = numOfCellsY * cellSize + 40;

            for (int y = 0; y <= numOfCellsY; y++)
            {
                e.Graphics.DrawLine(pen, 0, y * cellSize, numOfCellsX * cellSize, y * cellSize);
            }

            for (int x = 0; x <= numOfCellsX; x++)
            {
                e.Graphics.DrawLine(pen, x * cellSize, 0, x * cellSize, numOfCellsY * cellSize);
            }

            Font drawFont = new Font("Arial", 12);
            StringFormat drawFormat = new StringFormat();
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            char letter = 'A';
            points.Clear();
            for (int l = 1; l < numOfCellsX; l++)
            {
                Points p = new Points(l, 0, letter);
                points.Add(p); //добавляем точку с именем в список
                e.Graphics.FillRectangle(Brushes.Black, cellSize * l - 1, numOfCellsY * cellSize - 1, 3, 3);
                e.Graphics.DrawString(letter.ToString(), drawFont, drawBrush, cellSize * l - 8, begLetterY, drawFormat);
                letter++;
            }

            SolidBrush brushBuild = new SolidBrush(Color.Gray);
            Font fontBuild = new Font("Arial", 12, FontStyle.Bold);
            int max = beam1.Count() > beam2.Count() ? beam1.Count() : beam2.Count();
            int _max = max > beam3.Count() ? max : beam3.Count();
            Pen penTask = new Pen(Color.YellowGreen, 3);
            float[] dashValues = { 3, 3 };
            if (task)
            {
                penTask.DashPattern = dashValues;
                for (int i = 0; i < taskBeam.Count(); i++)
                {
                    Point point1 = new Point((int)(taskBeam[i].x1 * cellSize), (int)((-taskBeam[i].y1 + numOfCellsY) * cellSize));
                    Point point2 = new Point((int)(taskBeam[i].x2 * cellSize), (int)((-taskBeam[i].y2 + numOfCellsY) * cellSize));
                    e.Graphics.DrawLine(penTask, point1, point2);
                }
            }
            Pen penWrong = new Pen(Color.FromArgb(248, 0, 0), 3);
            Pen penBuildFirst = new Pen(Color.Black, 3);
            Pen penBuildSecond = new Pen(Color.CornflowerBlue, 3);
            Pen penBuildThird = new Pen(Color.MediumSeaGreen, 3);
            if (draw && !clear)
            {
                for (int i = 0; i < _max; i++)
                {
                    if (beam1.Count > i)
                    {
                        if (beam1[i].location == 1)
                        {
                            e.Graphics.DrawString(beam1[i].number.ToString(), fontBuild, brushBuild, (float)(beam1[i].x2 * (double)cellSize + 1), (float)((-beam1[i].y2 + numOfCellsY) * cellSize - cellSize / 2 - cellSize / 10), drawFormat);
                        }
                        if (beam1[i].location == 2)
                        {
                            e.Graphics.DrawString(beam1[i].number.ToString(), fontBuild, brushBuild, (float)(beam1[i].x2 * cellSize - cellSize / 2 + cellSize / 10), (float)((-beam1[i].y2 + numOfCellsY) * cellSize), drawFormat);
                        }
                        if (beam1[i].location != -1)
                        {
                            Point point1 = new Point((int)(beam1[i].x1 * cellSize), (int)((-beam1[i].y1 + numOfCellsY) * cellSize));
                            Point point2 = new Point((int)(beam1[i].x2 * cellSize), (int)((-beam1[i].y2 + numOfCellsY) * cellSize));
                            if (check && bb.FindAll(x => x.x1 == beam1[i].x1 && x.x2 == beam1[i].x2 && x.y1 == beam1[i].y1 && x.y2 == beam1[i].y2).Count() != 0)
                                e.Graphics.DrawLine(penWrong, point1, point2);
                            else
                                e.Graphics.DrawLine(penBuildFirst, point1, point2);
                        }
                    }
                    if (beam2.Count > i)
                    {
                        if (beam2[i].location == 1)
                        {
                            e.Graphics.DrawString(beam2[i].number.ToString(), fontBuild, brushBuild, (float)(beam2[i].x2 * (double)cellSize + 1), (float)((-beam2[i].y2 + numOfCellsY) * cellSize - cellSize / 2 - cellSize / 10), drawFormat);
                        }
                        if (beam2[i].location == 2)
                        {
                            e.Graphics.DrawString(beam2[i].number.ToString(), fontBuild, brushBuild, (float)(beam2[i].x2 * cellSize - cellSize / 2 + cellSize / 10), (float)((-beam2[i].y2 + numOfCellsY) * cellSize), drawFormat);
                        }
                        if (beam2[i].location != -1)
                        {
                            Point point1 = new Point((int)(beam2[i].x1 * cellSize), (int)((-beam2[i].y1 + numOfCellsY) * cellSize));
                            Point point2 = new Point((int)(beam2[i].x2 * cellSize), (int)((-beam2[i].y2 + numOfCellsY) * cellSize));
                            if (check && bb.FindAll(x => x.x1 == beam2[i].x1 && x.x2 == beam2[i].x2 && x.y1 == beam2[i].y1 && x.y2 == beam2[i].y2).Count() != 0)
                                e.Graphics.DrawLine(penWrong, point1, point2);
                            else
                                e.Graphics.DrawLine(penBuildSecond, point1, point2);
                        }
                    }
                    if (beam3.Count > i)
                    {
                        if (beam3[i].location == 1)
                        {
                            e.Graphics.DrawString(beam3[i].number.ToString(), fontBuild, brushBuild, (float)(beam3[i].x2 * (double)cellSize + 1), (float)((-beam3[i].y2 + numOfCellsY) * cellSize - cellSize / 2 - cellSize / 10), drawFormat);
                        }
                        if (beam3[i].location == 2)
                        {
                            e.Graphics.DrawString(beam3[i].number.ToString(), fontBuild, brushBuild, (float)(beam3[i].x2 * cellSize - cellSize / 2 + cellSize / 10), (float)((-beam3[i].y2 + numOfCellsY) * cellSize), drawFormat);
                        }
                        if (beam3[i].location != -1)
                        {
                            Point point1 = new Point((int)(beam3[i].x1 * cellSize), (int)((-beam3[i].y1 + numOfCellsY) * cellSize));
                            Point point2 = new Point((int)(beam3[i].x2 * cellSize), (int)((-beam3[i].y2 + numOfCellsY) * cellSize));
                            if (check && bb.FindAll(x => x.x1 == beam3[i].x1 && x.x2 == beam3[i].x2 && x.y1 == beam3[i].y1 && x.y2 == beam3[i].y2).Count() != 0)
                                e.Graphics.DrawLine(penWrong, point1, point2);
                            else
                                e.Graphics.DrawLine(penBuildThird, point1, point2);
                        }
                    }
                }
            }
            SolidBrush brushCheck = new SolidBrush(Color.IndianRed);
            if (lb.Count() > 0)
            {
                for (int i = 0; i < lb.Count(); i++)
                {
                    if (lb[i].location == 1)
                    {
                        e.Graphics.DrawString("#" + (i + 1).ToString(), fontBuild, brushCheck, (float)(lb[i].x2 * (double)cellSize + 1), (float)((-lb[i].y2 + numOfCellsY) * cellSize - cellSize / 2 - cellSize / 10), drawFormat);
                    }
                    if (lb[i].location == 2)
                    {
                        e.Graphics.DrawString("#" + (i + 1).ToString(), fontBuild, brushCheck, (float)(lb[i].x2 * cellSize - cellSize / 2 + cellSize / 10), (float)((-lb[i].y2 + numOfCellsY) * cellSize), drawFormat);
                    }
                }
            }
            for (int l = 1; l < numOfCellsX; l++)
            {
                Points p = new Points(l, 0, letter);
                e.Graphics.FillRectangle(Brushes.Black, cellSize * l - 1, numOfCellsY * cellSize - 1, 3, 3);
            }
        }
        public void gridSize(int x, int y)
        {
            clearErrors();
            numOfCellsX = x + 1;
            numOfCellsY = y;
            draw = false;
            playingFieldBox.Refresh();
            playingFieldBox.Invalidate();
            draw = true;
            playingFieldBox.Invalidate();
        }
        public void deleteColor()
        {
            for (int i = 0; i < procWindow.dgvFirst.Rows.Count; i++)
                procWindow.dgvFirst.Rows[i].HeaderCell.Style.BackColor = c;
            for (int i = 0; i < procWindow.dgvSecond.Rows.Count; i++)
                procWindow.dgvSecond.Rows[i].HeaderCell.Style.BackColor = c;
            for (int i = 0; i < procWindow.dgvThird.Rows.Count; i++)
                procWindow.dgvThird.Rows[i].HeaderCell.Style.BackColor = c;
            procWindow.dgvFirst.Update();
            procWindow.dgvSecond.Update();
            procWindow.dgvThird.Update();
            playingFieldBox.Refresh();
            playingFieldBox.Invalidate();
        }
        private void drawPic(int i)
        {
            draw = true;
            speed = trackBarSpeed.Value;
            Pen penBuildFirst = new Pen(Color.Black, 3);
            Pen penBuildSecond = new Pen(Color.CornflowerBlue, 3);
            Pen penBuildThird = new Pen(Color.MediumSeaGreen, 3);
            SolidBrush brushBuild = new SolidBrush(Color.Gray);
            Font fontBuild = new Font("Arial", 12, FontStyle.Bold);
            StringFormat drawFormat = new StringFormat();
            ev = playingFieldBox.CreateGraphics();
            Point point1 = new Point(), point2 = new Point();
            if (beam1.Count > i)
            {
                procWindow.dgvFirst.Rows[i].HeaderCell.Style.BackColor = Color.LightGreen;
                procWindow.dgvFirst.Update();
                if (i > 0)
                    procWindow.dgvFirst.Rows[i - 1].HeaderCell.Style.BackColor = c;
                procWindow.dgvFirst.Update();

                if (beam1[i].location == -1)
                    ;
                else
                {
                    if (beam1[i].location == 1)
                    {
                        ev.DrawString(beam1[i].number.ToString(), fontBuild, brushBuild, (float)(beam1[i].x2 * (double)cellSize + 1), (float)((-beam1[i].y2 + numOfCellsY) * cellSize - cellSize / 2 - cellSize / 10), drawFormat);
                    }
                    if (beam1[i].location == 2)
                    {
                        ev.DrawString(beam1[i].number.ToString(), fontBuild, brushBuild, (float)(beam1[i].x2 * cellSize - cellSize / 2 + cellSize / 10), (float)((-beam1[i].y2 + numOfCellsY) * cellSize), drawFormat);
                    }
                    point1 = new Point((int)(beam1[i].x1 * cellSize), (int)((-beam1[i].y1 + numOfCellsY) * cellSize));
                    point2 = new Point((int)(beam1[i].x2 * cellSize), (int)((-beam1[i].y2 + numOfCellsY) * cellSize));
                    ev.DrawLine(penBuildFirst, point1, point2);
                }
            }
            else
            {
                if (beam1.Count - 1 == i)
                    procWindow.dgvFirst.Rows[i].HeaderCell.Style.BackColor = c;
                procWindow.dgvFirst.Update();
            }
            if (beam2.Count > i)
            {
                procWindow.dgvSecond.Rows[i].HeaderCell.Style.BackColor = Color.LightGreen;
                procWindow.dgvSecond.Update();
                if (i > 0)
                    procWindow.dgvSecond.Rows[i - 1].HeaderCell.Style.BackColor = c;
                procWindow.dgvSecond.Update();
                if (beam2[i].location == -1)
                    ;
                else
                {
                    if (beam2[i].location == 1)
                    {
                        ev.DrawString(beam2[i].number.ToString(), fontBuild, brushBuild, (float)(beam2[i].x2 * (double)cellSize + 1), (float)((-beam2[i].y2 + numOfCellsY) * cellSize - cellSize / 2 - cellSize / 10), drawFormat);
                    }
                    if (beam2[i].location == 2)
                    {
                        ev.DrawString(beam2[i].number.ToString(), fontBuild, brushBuild, (float)(beam2[i].x2 * cellSize - cellSize / 2 + cellSize / 10), (float)((-beam2[i].y2 + numOfCellsY) * cellSize), drawFormat);
                    }
                    point1 = new Point((int)(beam2[i].x1 * cellSize), (int)((-beam2[i].y1 + numOfCellsY) * cellSize));
                    point2 = new Point((int)(beam2[i].x2 * cellSize), (int)((-beam2[i].y2 + numOfCellsY) * cellSize));
                    ev.DrawLine(penBuildSecond, point1, point2);
                }
            }
            else
            {
                if (beam2.Count - 1 == i)
                    procWindow.dgvSecond.Rows[i].HeaderCell.Style.BackColor = c;
                procWindow.dgvSecond.Update();
            }
            if (beam3.Count > i)
            {
                procWindow.dgvThird.Rows[i].HeaderCell.Style.BackColor = Color.LightGreen;
                procWindow.dgvThird.Update();
                if (i > 0)
                    procWindow.dgvThird.Rows[i - 1].HeaderCell.Style.BackColor = c;
                procWindow.dgvThird.Update();
                if (beam3[i].location == -1)
                    ;
                else
                {
                    if (beam3[i].location == 1)
                    {
                        ev.DrawString(beam3[i].number.ToString(), fontBuild, brushBuild, (float)(beam3[i].x2 * (double)cellSize + 1), (float)((-beam3[i].y2 + numOfCellsY) * cellSize - cellSize / 2 - cellSize / 10), drawFormat);
                    }
                    if (beam3[i].location == 2)
                    {
                        ev.DrawString(beam3[i].number.ToString(), fontBuild, brushBuild, (float)(beam3[i].x2 * cellSize - cellSize / 2 + cellSize / 10), (float)((-beam3[i].y2 + numOfCellsY) * cellSize), drawFormat);
                    }
                    point1 = new Point((int)(beam3[i].x1 * cellSize), (int)((-beam3[i].y1 + numOfCellsY) * cellSize));
                    point2 = new Point((int)(beam3[i].x2 * cellSize), (int)((-beam3[i].y2 + numOfCellsY) * cellSize));
                    ev.DrawLine(penBuildThird, point1, point2);
                }
            }
            else
            {
                if (beam3.Count - 1 == i)
                    procWindow.dgvThird.Rows[i].HeaderCell.Style.BackColor = c;
                procWindow.dgvThird.Update();
            }
            Thread.Sleep(speed);
        }
        private void buttonGo_Click(object sender, EventArgs e)
        {
            procWindow.richTb.Text = "";
            clearErrors();
            check = false;
            draw = true;
            clear = false;
            lb.Clear();
            bb.Clear();
            beam1.Clear();
            beam2.Clear();
            beam3.Clear();
            procWindow.dgvFirst.EndEdit();
            procWindow.dgvSecond.EndEdit();
            procWindow.dgvThird.EndEdit();
            playingFieldBox.Refresh();
            playingFieldBox.Invalidate();
            preparationForDrawing();
        }

        /*actions.Add("в точку");
        actions.Add("на левый край балки");
        actions.Add("на правый край балки");
        actions.Add("на средину балки");
        actions.Add("между балками");
        actions.Add("серединой на балку");
        actions.Add("на землю между точками");*/
        private void addBeams(int row)
        {
            bool go = true;
            Beam beamHelp;
            errorStr = "";
            if (procWindow.dgvFirst.Rows.Count - 1 > row)
            {
                string command = (string)procWindow.dgvFirst.Rows[row].Cells[1].Value;
                int name = -1;
                if (command == null || command == "")
                {
                    errorStr += "Бригада 1. Строка " + (row + 1) + ". Отсутствует команда.";
                    showExeption("Отсутствует команда.", 1, row, 1);
                }
                if (command != actions[7])
                {
                    if (procWindow.dgvFirst.Rows[row].Cells[0].Value == null)
                    {
                        errorStr += "Бригада 1. Строка " + (row + 1) + ". Отсутствует номер балки.";
                        showExeption("Отсутствует номер балки.", 1, row, 0);
                    }
                    if (!int.TryParse(procWindow.dgvFirst.Rows[row].Cells[0].Value.ToString(), out name) || name < 1)
                    {
                        errorStr += "Бригада 1. Строка " + (row + 1) + $". {procWindow.dgvFirst.Rows[row].Cells[0].Value.ToString()} - недопустимый номер балки.";
                        showExeption("Недопустимый номер.", 1, row, 0);
                    }
                }
                else
                {
                    beam1.Add(new Beam(-1, -1, -1, -1, -1, -1));
                }
                if (name != -1 && command != actions[7])
                {
                    Beam help = beam1.Find(x => x.number == name);
                    if (beam1.Find(x => x.number == name) != null || beam2.Find(x => x.number == name) != null || beam1.Find(x => x.number == name) != null)
                    {
                        errorStr += "Бригада 1. Строка " + (row + 1) + $". Номер балки {procWindow.dgvFirst.Rows[row].Cells[0].Value.ToString()} занят.";
                        showExeption("Номер занят.", 1, row, 0);
                    }
                    if (command == actions[1] || command == actions[2] || command == actions[3] || command == actions[5])
                    {
                        int pos1;
                        if (!int.TryParse(procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString(), out pos1))
                        {
                            errorStr += "Бригада 1. Строка " + (row + 1) + $". {procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString()} - недопустимый номер для балки.";
                            showExeption("Недопустимый номер.", 1, row, 2);
                        }
                        Beam b = beam1.Find(x => x.number == pos1);
                        int index = beam1.FindIndex(x => x.number == pos1);
                        if (b == null)
                        {
                            b = beam2.Find(x => x.number == pos1);
                            index = beam2.FindIndex(x => x.number == pos1);
                        }
                        if (b == null)
                        {
                            b = beam3.Find(x => x.number == pos1);
                            index = beam3.FindIndex(x => x.number == pos1);
                        }
                        if (b == null || index == row)
                        {
                            errorStr += "Бригада 1. Строка " + (row + 1) + $". Балка с номером {procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString()} не существует.";
                            showExeption("Несуществующая балка.", 1, row, 2);
                        }
                        switch (command)
                        {
                            case "поставить на левый край балки":
                                if (b.location == 1)
                                {
                                    errorStr += "Бригада 1. Строка " + (row + 1) + $". Поставить балку можно только на горизонтальную балку.";
                                    showExeption("Поставить балку можно только на горизонтальную балку.", 1, row, 1);
                                }
                                beamHelp = new Beam(b.x1, b.y1 + 1, b.x1, b.y1, 1, name);
                                List<Beam> unionOfBeams = beam1.Union(beam2).Union(beam3).ToList();
                                if (unionOfBeams.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                                {
                                    errorStr += "Бригада 1. Строка " + (row + 1) + $". Балка {procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString()} занята.";
                                    showExeption("Место занято.", 1, row, 2);
                                }
                                beam1.Add(beamHelp);
                                break;
                            case "поставить на правый край балки":
                                if (b.location == 1)
                                {
                                    errorStr += "Бригада 1. Строка " + (row + 1) + $". Поставить балку можно только на горизонтальную балку.";
                                    showExeption("Поставить балку можно только на горизонтальную балку.", 1, row, 1);
                                }
                                beamHelp = new Beam(b.x2, b.y2 + 1, b.x2, b.y2, 1, name);
                                unionOfBeams = beam1.Union(beam2).Union(beam3).ToList();
                                if (unionOfBeams.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                                {
                                    errorStr += "Бригада 1. Строка " + (row + 1) + $". Балка {procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString()} занята.";
                                    showExeption("Место занято.", 1, row, 2);
                                }
                                beam1.Add(beamHelp);
                                break;
                            case "поставить на средину балки":
                                if (b.location == 1)
                                {
                                    errorStr += "Бригада 1. Строка " + (row + 1) + $". Поставить балку можно только на горизонтальную балку.";
                                    showExeption("Поставить балку можно только на горизонтальную балку.", 1, row, 1);
                                }
                                beamHelp = new Beam((b.x1 + b.x2) / 2, b.y1 + 1, (b.x1 + b.x2) / 2, b.y1, 1, name);
                                unionOfBeams = beam1.Union(beam2).Union(beam3).ToList();
                                if (unionOfBeams.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                                {
                                    errorStr += "Бригада 1. Строка " + (row + 1) + $". Балка {procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString()} занята.";
                                    showExeption("Место занято.", 1, row, 2);
                                }
                                beam1.Add(beamHelp);
                                break;
                            case "положить серединой на балку":
                                if (b.location == 2)
                                {
                                    errorStr += "Бригада 1. Строка " + (row + 1) + $". Положить балку можно только на вертикальную балку.";
                                    showExeption("Положить балку можно только на вертикальную балку.", 1, row, 1);
                                }
                                beamHelp = new Beam(b.x1 - 0.5, b.y1, b.x1 + 0.5, b.y1, 2, name);
                                unionOfBeams = beam1.Union(beam2).Union(beam3).ToList();
                                if (unionOfBeams.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                                {
                                    errorStr += "Бригада 1. Строка " + (row + 1) + $". Балка {procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString()} занята.";
                                    showExeption("Место занято.", 1, row, 2);
                                }
                                if (beam1.Find(x => (x.x1 == (beamHelp.x1 + beamHelp.x2) / 2 || x.x2 == (beamHelp.x1 + beamHelp.x2) / 2) && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                    beam2.Find(x => (x.x1 == (beamHelp.x1 + beamHelp.x2) / 2 || x.x2 == (beamHelp.x1 + beamHelp.x2) / 2) && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                    beam3.Find(x => (x.x1 == (beamHelp.x1 + beamHelp.x2) / 2 || x.x2 == (beamHelp.x1 + beamHelp.x2) / 2) && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                                {
                                    errorStr += "Бригада 1. Строка " + (row + 1) + $". Балка {procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString()} занята.";
                                    showExeption("Место занято.", 1, row, 2);
                                }
                                beam1.Add(beamHelp);
                                break;
                        }
                    }
                    if (command == actions[0])
                    {
                        char pos1;
                        if (procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString().Count() != 1 || Convert.ToChar(procWindow.dgvFirst.Rows[row].Cells[2].Value) < 'A' || Convert.ToChar(procWindow.dgvFirst.Rows[row].Cells[2].Value) > 'Z')
                        {
                            errorStr += "Бригада 1. Строка " + (row + 1) + $". Точка {procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString()} имеет недопустимое имя.";
                            showExeption("Недопустимое имя точки.", 1, row, 2);
                        }
                        char.TryParse(procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString(), out pos1);
                        Points p = points.Find(x => x.pointName == pos1);
                        if (p == null)
                        {
                            errorStr += "Бригада 1. Строка " + (row + 1) + $". Точка с именем '{procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString()}' не существует.";
                            showExeption("Несуществующая точка.", 1, row, 2);
                        }
                        beamHelp = new Beam(p.x, p.y + 1, p.x, p.y, 1, name);
                        List<Beam> unionOfBeams = beam1.Union(beam2).Union(beam3).ToList();
                        if (unionOfBeams.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                        {
                            errorStr += "Бригада 1. Строка " + (row + 1) + $". Точка {procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString()} занята.";
                            showExeption("Место занято.", 1, row, 2);
                        }
                        beam1.Add(beamHelp);
                    }

                    if (command == actions[4])
                    {
                        int pos1 = 0;
                        int pos2 = 0;
                        if (!int.TryParse(procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString(), out pos1) || !int.TryParse(procWindow.dgvFirst.Rows[row].Cells[3].Value.ToString(), out pos2))
                        {
                            if (!int.TryParse(procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString(), out pos1))
                            {
                                errorStr += "Бригада 1. Строка " + (row + 1) + $". {procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString()} - недопустимый номер для балки.";
                                showExeption("Недопустимый номер.", 1, row, 2);
                            }
                            else
                            {
                                errorStr += "Бригада 1. Строка " + (row + 1) + $". {procWindow.dgvFirst.Rows[row].Cells[3].Value.ToString()} - недопустимый номер для балки.";
                                showExeption("Недопустимый номер.", 1, row, 3);
                            }
                        }
                        Beam b1 = beam1.Find(x => x.number == pos1);
                        int index1 = beam1.FindIndex(x => x.number == pos1);
                        if (b1 == null)
                        {
                            b1 = beam2.Find(x => x.number == pos1);
                            index1 = beam2.FindIndex(x => x.number == pos1);
                        }
                        if (b1 == null)
                        {
                            b1 = beam3.Find(x => x.number == pos1);
                            index1 = beam3.FindIndex(x => x.number == pos1);
                        }
                        Beam b2 = beam1.Find(x => x.number == pos2);
                        int index2 = beam1.FindIndex(x => x.number == pos2);
                        if (b2 == null)
                        {
                            b2 = beam2.Find(x => x.number == pos2);
                            index2 = beam2.FindIndex(x => x.number == pos2);
                        }
                        if (b2 == null)
                        {
                            b2 = beam3.Find(x => x.number == pos2);
                            index2 = beam3.FindIndex(x => x.number == pos2);
                        }
                        if (b1 == null || b2 == null || index1 == row || index2 == row)
                        {
                            if (b1 == null || index1 == row)
                            {
                                errorStr += "Бригада 1. Строка " + (row + 1) + $". Балка с номером {procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString()} не существует.";
                                showExeption("Несуществующая балка.", 1, row, 2);
                            }
                            else
                            {
                                    errorStr += "Бригада 1. Строка " + (row + 1) + $". Балка с номером {procWindow.dgvFirst.Rows[row].Cells[3].Value.ToString()} не существует.";
                                showExeption("Несуществующая балка.", 1, row, 3);
                            }
                        }

                        if (b1.y1 != b2.y1 || b1.y2 != b2.y2 || Math.Abs(b1.x1 - b2.x1) != 1)
                        {
                            errorStr += "Бригада 1. Строка " + (row + 1) + $". Недопустимое расстояние между балками {procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString()} и {procWindow.dgvFirst.Rows[row].Cells[3].Value.ToString()}.";
                            showExeption("Недопустимое расстояние между балками.", 1, row, 2, 3);
                        }
                        if (b1.x1 > b2.x1) //если первая указанная балка правее второй указанной балки
                        {
                            if (beam1.FindAll(x => (x.x1 < b1.x1 && x.x1 > b2.x1 || x.x2 < b1.x1 && x.x2 > b2.x1) && x.y1 == x.y2 && x.y2 == b1.y1).Count() != 0 ||
                                beam2.FindAll(x => (x.x1 < b1.x1 && x.x1 > b2.x1 || x.x2 < b1.x1 && x.x2 > b2.x1) && x.y1 == x.y2 && x.y2 == b1.y1).Count() != 0 ||
                                beam3.FindAll(x => (x.x1 < b1.x1 && x.x1 > b2.x1 || x.x2 < b1.x1 && x.x2 > b2.x1) && x.y1 == x.y2 && x.y2 == b1.y1).Count() != 0)
                            {
                                    errorStr += "Бригада 1. Строка " + (row + 1) + $". Балка {procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString()} занята.";
                                showExeption("Балка занята.", 1, row, 2);
                            }
                        }
                        else
                        {
                            if (beam1.FindAll(x => (x.x1 < b2.x1 && x.x1 > b1.x1 || x.x2 < b2.x1 && x.x2 > b1.x1) && x.y1 == x.y2 && x.y2 == b2.y1).Count() != 0 ||
                                beam2.FindAll(x => (x.x1 < b2.x1 && x.x1 > b1.x1 || x.x2 < b2.x1 && x.x2 > b1.x1) && x.y1 == x.y2 && x.y2 == b2.y1).Count() != 0 ||
                                beam3.FindAll(x => (x.x1 < b2.x1 && x.x1 > b1.x1 || x.x2 < b2.x1 && x.x2 > b1.x1) && x.y1 == x.y2 && x.y2 == b2.y1).Count() != 0)
                            {
                                    errorStr += "Бригада 1. Строка " + (row + 1) + $". Балка {procWindow.dgvFirst.Rows[row].Cells[3].Value.ToString()} занята.";
                                showExeption("Балка занята.", 1, row, 3);
                            }
                        }
                        if (b1.x1 < b2.x1)
                            beamHelp = new Beam(b1.x1, b1.y1, b2.x1, b2.y1, 2, name);
                        else
                            beamHelp = new Beam(b2.x1, b1.y1, b1.x1, b2.y1, 2, name);
                        if (beam1.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                            beam2.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                            beam3.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                        {
                            errorStr += "Бригада 1. Строка " + (row + 1) + $". Место между балками {procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString()} и {procWindow.dgvFirst.Rows[row].Cells[3].Value.ToString()} занято.";
                            showExeption("Место занято.", 1, row, 2, 3);
                        }
                        beam1.Add(beamHelp);
                    }
                    if (command == actions[6])
                    {
                        if (procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString().Count() != 1 || Convert.ToChar(procWindow.dgvFirst.Rows[row].Cells[2].Value) < 'A' || Convert.ToChar(procWindow.dgvFirst.Rows[row].Cells[2].Value) > 'Z')
                        {
                            errorStr += "Бригада 1. Строка " + (row + 1) + $". Точка {procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString()} имеет недопустимое имя.";
                            showExeption("Недопустимое имя точки.", 1, row, 2);
                        }
                        if (procWindow.dgvFirst.Rows[row].Cells[3].Value.ToString().Count() != 1 || Convert.ToChar(procWindow.dgvFirst.Rows[row].Cells[3].Value) < 'A' || Convert.ToChar(procWindow.dgvFirst.Rows[row].Cells[3].Value) > 'Z')
                        {
                            errorStr += "Бригада 1. Строка " + (row + 1) + $". Точка {procWindow.dgvFirst.Rows[row].Cells[3].Value.ToString()} имеет недопустимое имя.";
                            showExeption("Недопустимое имя точки.", 1, row, 3);
                        }
                        char pos1 = Convert.ToChar(procWindow.dgvFirst.Rows[row].Cells[2].Value);
                        char pos2 = Convert.ToChar(procWindow.dgvFirst.Rows[row].Cells[3].Value);
                        Points p1 = points.Find(x => x.pointName == pos1);
                        Points p2 = points.Find(x => x.pointName == pos2);
                        if (p1 == null || p2 == null)
                        {
                            if (p1 == null)
                            {
                                errorStr += "Бригада 1. Строка " + (row + 1) + $". Точка с именем '{procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString()}' не существует.";
                                showExeption("Несуществующая точка.", 1, row, 2);
                            }
                            else
                            {
                                errorStr += "Бригада 1. Строка " + (row + 1) + $". Точка с именем '{procWindow.dgvFirst.Rows[row].Cells[3].Value.ToString()}' не существует.";
                                showExeption("Несуществующая точка.", 1, row, 3);
                            }
                        }
                        if (Math.Abs(p1.x - p2.x) != 1)
                        {
                            errorStr += "Бригада 1. Строка " + (row + 1) + $". Недопустимое расстояние между точками {procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString()} и {procWindow.dgvFirst.Rows[row].Cells[3].Value.ToString()}.";
                            showExeption("Недопустимое расстояние между точками.", 1, row, 2, 3);
                        }
                        if (p1.x < p2.x)
                            beamHelp = new Beam(p1.x, p1.y, p2.x, p2.y, 2, name);
                        else
                            beamHelp = new Beam(p2.x, p1.y, p1.x, p2.y, 2, name);
                        if (beam1.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                            beam2.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                            beam3.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                        {
                             errorStr += "Бригада 1. Строка " + (row + 1) + $". Место между точками {procWindow.dgvFirst.Rows[row].Cells[2].Value.ToString()} и {procWindow.dgvFirst.Rows[row].Cells[3].Value.ToString()} занято.";
                            showExeption("Место занято.", 1, row, 2, 3);
                        }
                        beam1.Add(beamHelp);
                    }
                }
            }
            if (procWindow.dgvSecond.Rows.Count - 1 > row)
            {
                go = true;
                string command = (string)procWindow.dgvSecond.Rows[row].Cells[1].Value;
                int name = -1;
                if (command == null || command == "")
                    if (command == null || command == "")
                    {
                        errorStr += "Бригада 2. Строка " + (row + 1) + ". Отсутствует команда.";
                        showExeption("Отсутствует команда.", 2, row, 1);
                    }
                if (command != actions[7])
                {
                    if (procWindow.dgvSecond.Rows[row].Cells[0].Value == null)
                    {
                        errorStr += "Бригада 2. Строка " + (row + 1) + ". Отсутствует номер балки.";
                        showExeption("Отсутствует номер балки.", 2, row, 0);
                    }
                    if (!int.TryParse(procWindow.dgvSecond.Rows[row].Cells[0].Value.ToString(), out name) || name < 1)
                    {
                        errorStr += "Бригада 2. Строка " + (row + 1) + $". {procWindow.dgvSecond.Rows[row].Cells[0].Value.ToString()} - недопустимый номер балки.";

                        showExeption("Недопустимый номер.", 2, row, 0);
                    }
                }
                else
                {
                    beam2.Add(new Beam(-1, -1, -1, -1, -1, -1));
                    go = false;
                }
                if (go)
                {
                    if (name != -1)
                    {
                        if (beam1.Find(x => x.number == name) != null || beam2.Find(x => x.number == name) != null || beam1.Find(x => x.number == name) != null)
                        {
                            errorStr += "Бригада 2. Строка " + (row + 1) + $". Номер балки {procWindow.dgvSecond.Rows[row].Cells[0].Value.ToString()} занят.";
                            showExeption("Номер занят.", 2, row, 0);
                        }
                        if (command == actions[1] || command == actions[2] || command == actions[3] || command == actions[5])
                        {
                            int pos1;
                            if (!int.TryParse(procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString(), out pos1))
                            {
                                errorStr += "Бригада 2. Строка " + (row + 1) + $". {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} - недопустимый номер для балки.";

                                showExeption("Недопустимый номер.", 2, row, 2);
                            }
                            Beam b = beam1.Find(x => x.number == pos1);
                            int index = beam1.FindIndex(x => x.number == pos1);
                            if (b == null)
                            {
                                b = beam2.Find(x => x.number == pos1);
                                index = beam2.FindIndex(x => x.number == pos1);
                            }
                            if (b == null)
                            {
                                b = beam3.Find(x => x.number == pos1);
                                index = beam3.FindIndex(x => x.number == pos1);
                            }
                            if (b == null || index == row)
                            {
                                errorStr += "Бригада 2. Строка " + (row + 1) + $". Балка с номером {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} не существует.";
                                showExeption("Несуществующая балка.", 2, row, 2);
                            }
                            switch (command)
                            {
                                case "поставить на левый край балки":
                                    if (b.location == 1)
                                    {
                                        errorStr += "Бригада 2. Строка " + (row + 1) + $". Поставить балку можно только на горизонтальную балку.";
                                        showExeption("Поставить балку можно только на горизонтальную балку.", 2, row, 1);
                                    }
                                    beamHelp = new Beam(b.x1, b.y1 + 1, b.x1, b.y1, 1, name);
                                    if (beam1.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam2.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam3.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                                    {
                                        errorStr += "Бригада 2. Строка " + (row + 1) + $". Балка {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} занята.";
                                        showExeption("Место занято.", 2, row, 2);
                                    }
                                    beam2.Add(beamHelp);
                                    break;
                                case "поставить на правый край балки":
                                    if (b.location == 1)
                                    {
                                        errorStr += "Бригада 2. Строка " + (row + 1) + $". Поставить балку можно только на горизонтальную балку.";
                                        showExeption("Поставить балку можно только на горизонтальную балку.", 2, row, 1);
                                    }
                                    beamHelp = new Beam(b.x2, b.y2 + 1, b.x2, b.y2, 1, name);
                                    if (beam1.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam2.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam3.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                                    {
                                        errorStr += "Бригада 2. Строка " + (row + 1) + $". Балка {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} занята.";
                                        showExeption("Место занято.", 2, row, 2);
                                    }
                                    beam2.Add(beamHelp);
                                    break;
                                case "поставить на средину балки":
                                    if (b.location == 1)
                                    {
                                        errorStr += "Бригада 2. Строка " + (row + 1) + $". Поставить балку можно только на горизонтальную балку.";
                                        showExeption("Поставить балку можно только на горизонтальную балку.", 2, row, 1);
                                    }
                                    beamHelp = new Beam((b.x1 + b.x2) / 2, b.y1 + 1, (b.x1 + b.x2) / 2, b.y1, 1, name);
                                    if (beam1.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam2.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam3.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                                    {
                                        errorStr += "Бригада 2. Строка " + (row + 1) + $". Балка {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} занята.";
                                        showExeption("Место занято.", 2, row, 2);
                                    }
                                    beam2.Add(beamHelp);
                                    break;
                                case "положить серединой на балку":
                                    if (b.location == 2)
                                    {
                                        errorStr += "Бригада 2. Строка " + (row + 1) + $". Положить балку можно только на вертикальную балку.";
                                        showExeption("Положить балку можно только на вертикальную балку.", 2, row, 1);
                                    }
                                    beamHelp = new Beam(b.x1 - 0.5, b.y1, b.x1 + 0.5, b.y1, 2, name);
                                    if (beam1.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam2.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam3.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                                    {
                                       errorStr += "Бригада 2. Строка " + (row + 1) + $". Балка {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} занята.";
                                        
                                        showExeption("Место занято.", 2, row, 2);
                                    }
                                    if (beam1.Find(x => (x.x1 == (beamHelp.x1 + beamHelp.x2) / 2 || x.x2 == (beamHelp.x1 + beamHelp.x2) / 2) && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                            beam2.Find(x => (x.x1 == (beamHelp.x1 + beamHelp.x2) / 2 || x.x2 == (beamHelp.x1 + beamHelp.x2) / 2) && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                            beam3.Find(x => (x.x1 == (beamHelp.x1 + beamHelp.x2) / 2 || x.x2 == (beamHelp.x1 + beamHelp.x2) / 2) && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                                    {
                                        errorStr += "Бригада 2. Строка " + (row + 1) + $". Балка {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} занята.";
                                        showExeption("Место занято.", 2, row, 2);
                                    }
                                    beam2.Add(beamHelp);
                                    break;
                            }
                        }
                        if (command == actions[0])
                        {
                            char pos1;
                            if (char.TryParse(procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString(), out pos1))
                                if (Convert.ToChar(procWindow.dgvSecond.Rows[row].Cells[2].Value) < 'A' || Convert.ToChar(procWindow.dgvSecond.Rows[row].Cells[2].Value) > 'Z')
                                {
                                    if (errorStr == "")
                                        errorStr += "Бригада 2. Строка " + (row + 1) + $". Точка {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} имеет недопустимое имя.";
                                    else
                                        errorStr += "\nБригада 2. Строка " + (row + 1) + $". Точка {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} имеет недопустимое имя.";
                                    showExeption("Недопустимое имя точки.", 2, row, 2);
                                }
                            Points p = points.Find(x => x.pointName == pos1);
                            if (p == null)
                            {
                                if (errorStr == "")
                                    errorStr += "Бригада 2. Строка " + (row + 1) + $". Точка с именем '{procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()}' не существует.";
                                else
                                    errorStr += "\nБригада 2. Строка " + (row + 1) + $". Точка с именем '{procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()}' не существует.";
                                showExeption("Несуществующая точка.", 2, row, 2);
                            }
                            beamHelp = new Beam(p.x, p.y + 1, p.x, p.y, 1, name);
                            if (beam1.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam2.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam3.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                            {
                                if (errorStr == "")
                                    errorStr += "Бригада 2. Строка " + (row + 1) + $". Точка {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} занята.";
                                else
                                    errorStr += "\nБригада 2. Строка " + (row + 1) + $". Точка{procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} занята.";
                                showExeption("Место занято.", 2, row, 2);
                            }
                            beam2.Add(beamHelp);
                        }
                        if (command == actions[4])
                        {
                            int pos1 = 0;
                            int pos2 = 0;
                            if (!int.TryParse(procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString(), out pos1) || !int.TryParse(procWindow.dgvSecond.Rows[row].Cells[3].Value.ToString(), out pos2))
                            {
                                if (!int.TryParse(procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString(), out pos1))
                                {
                                    if (errorStr == "")
                                        errorStr += "Бригада 2. Строка " + (row + 1) + $". {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} - недопустимый номер для балки.";
                                    else
                                        errorStr += "\nБригада 2. Строка " + (row + 1) + $". {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} - недопустимый номер для балки.";
                                    showExeption("Недопустимый номер.", 2, row, 2);
                                }
                                else
                                {
                                    if (errorStr == "")
                                        errorStr += "Бригада 2. Строка " + (row + 1) + $". {procWindow.dgvSecond.Rows[row].Cells[3].Value.ToString()} - недопустимый номер для балки.";
                                    else
                                        errorStr += "\nБригада 2. Строка " + (row + 1) + $". {procWindow.dgvSecond.Rows[row].Cells[3].Value.ToString()} - недопустимый номер для балки.";
                                    showExeption("Недопустимый номер.", 2, row, 3);
                                }
                            }
                            Beam b1 = beam1.Find(x => x.number == pos1);
                            int index1 = beam1.FindIndex(x => x.number == pos1);
                            if (b1 == null)
                            {
                                b1 = beam2.Find(x => x.number == pos1);
                                index1 = beam2.FindIndex(x => x.number == pos1);
                            }
                            if (b1 == null)
                            {
                                b1 = beam3.Find(x => x.number == pos1);
                                index1 = beam3.FindIndex(x => x.number == pos1);
                            }

                            Beam b2 = beam1.Find(x => x.number == pos2);
                            int index2 = beam1.FindIndex(x => x.number == pos2);
                            if (b2 == null)
                            {
                                b2 = beam2.Find(x => x.number == pos2);
                                index2 = beam2.FindIndex(x => x.number == pos2);
                            }
                            if (b2 == null)
                            {
                                b2 = beam3.Find(x => x.number == pos2);
                                index2 = beam3.FindIndex(x => x.number == pos2);
                            }
                            if (b1 == null || b2 == null || index1 == row || index2 == row)
                            {
                                if (b1 == null || index1 == row)
                                {
                                    if (errorStr == "")
                                        errorStr += "Бригада 2. Строка " + (row + 1) + $". Балка с номером {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} не существует.";
                                    else
                                        errorStr += "\nБригада 2. Строка " + (row + 1) + $". Балка с номером {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} не существует.";
                                    showExeption("Несуществующая балка.", 2, row, 2);
                                }
                                else
                                {
                                    if (errorStr == "")
                                        errorStr += "Бригада 2. Строка " + (row + 1) + $". Балка с номером {procWindow.dgvSecond.Rows[row].Cells[3].Value.ToString()} не существует.";
                                    else
                                        errorStr += "\nБригада 2. Строка " + (row + 1) + $". Балка с номером {procWindow.dgvSecond.Rows[row].Cells[3].Value.ToString()} не существует.";

                                    showExeption("Несуществующая балка.", 2, row, 3);
                                }
                            }
                            if (b1.y1 != b2.y1 || b1.y2 != b2.y2 || Math.Abs(b1.x1 - b2.x1) != 1)
                            {
                                if (errorStr == "")
                                    errorStr += "Бригада 2. Строка " + (row + 1) + $". Недопустимое расстояние между балками {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} и {procWindow.dgvSecond.Rows[row].Cells[3].Value.ToString()}.";
                                else
                                    errorStr += "\nБригада 2. Строка " + (row + 1) + $". Недопустимое расстояние между балками {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} и {procWindow.dgvSecond.Rows[row].Cells[3].Value.ToString()}.";
                                showExeption("Недопустимое расстояние между балками.", 2, row, 2, 3);
                            }
                            if (b1.x1 > b2.x1) //если первая указанная балка правее второй указанной балки
                            {
                                if (beam1.FindAll(x => (x.x1 < b1.x1 && x.x1 > b2.x1 || x.x2 < b1.x1 && x.x2 > b2.x1) && x.y1 == x.y2 && x.y2 == b1.y1).Count() != 0 ||
                                    beam2.FindAll(x => (x.x1 < b1.x1 && x.x1 > b2.x1 || x.x2 < b1.x1 && x.x2 > b2.x1) && x.y1 == x.y2 && x.y2 == b1.y1).Count() != 0 ||
                                    beam3.FindAll(x => (x.x1 < b1.x1 && x.x1 > b2.x1 || x.x2 < b1.x1 && x.x2 > b2.x1) && x.y1 == x.y2 && x.y2 == b1.y1).Count() != 0)
                                {
                                    if (errorStr == "")
                                        errorStr += "Бригада 2. Строка " + (row + 1) + $". Балка {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} занята.";
                                    else
                                        errorStr += "\nБригада 2. Строка " + (row + 1) + $". Балка {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} занята.";
                                    showExeption("Балка занята.", 2, row, 2);
                                }
                            }
                            else
                            {
                                if (beam1.FindAll(x => (x.x1 < b2.x1 && x.x1 > b1.x1 || x.x2 < b2.x1 && x.x2 > b1.x1) && x.y1 == x.y2 && x.y2 == b2.y1).Count() != 0 ||
                                    beam2.FindAll(x => (x.x1 < b2.x1 && x.x1 > b1.x1 || x.x2 < b2.x1 && x.x2 > b1.x1) && x.y1 == x.y2 && x.y2 == b2.y1).Count() != 0 ||
                                    beam3.FindAll(x => (x.x1 < b2.x1 && x.x1 > b1.x1 || x.x2 < b2.x1 && x.x2 > b1.x1) && x.y1 == x.y2 && x.y2 == b2.y1).Count() != 0)
                                {
                                    if (errorStr == "")
                                        errorStr += "Бригада 2. Строка " + (row + 1) + $". Балка {procWindow.dgvSecond.Rows[row].Cells[3].Value.ToString()} занята.";
                                    else
                                        errorStr += "\nБригада 2. Строка " + (row + 1) + $". Балка {procWindow.dgvSecond.Rows[row].Cells[3].Value.ToString()} занята.";
                                    showExeption("Балка занята.", 2, row, 3);
                                }
                            }
                            if (b1.x1 < b2.x1)
                                beamHelp = new Beam(b1.x1, b1.y1, b2.x1, b2.y1, 2, name);
                            else
                                beamHelp = new Beam(b2.x1, b1.y1, b1.x1, b2.y1, 2, name);
                            if (beam1.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam2.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam3.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                            {
                                if (errorStr == "")
                                    errorStr += "Бригада 2. Строка " + (row + 1) + $". Место между балками {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} и {procWindow.dgvSecond.Rows[row].Cells[3].Value.ToString()} занято.";
                                else
                                    errorStr += "\nБригада 2. Строка " + (row + 1) + $". Место между балками {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} и {procWindow.dgvSecond.Rows[row].Cells[3].Value.ToString()} занято.";
                                showExeption("Место занято.", 2, row, 2, 3);
                            }
                            beam2.Add(beamHelp);
                        }
                        if (command == actions[6])
                        {
                            char pos1 = Convert.ToChar(procWindow.dgvSecond.Rows[row].Cells[2].Value);
                            char pos2 = Convert.ToChar(procWindow.dgvSecond.Rows[row].Cells[3].Value);
                            Points p1 = points.Find(x => x.pointName == pos1);
                            Points p2 = points.Find(x => x.pointName == pos2);
                            if (p1 == null || p2 == null)
                            {
                                if (p1 == null)
                                {
                                    if (errorStr == "")
                                        errorStr += "Бригада 2. Строка " + (row + 1) + $". Точка с именем '{procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()}' не существует.";
                                    else
                                        errorStr += "\nБригада 2. Строка " + (row + 1) + $". Точка с именем '{procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()}' не существует.";
                                    showExeption("Несуществующая точка.", 2, row, 2);
                                }
                                else
                                {
                                    if (errorStr == "")
                                        errorStr += "Бригада 2. Строка " + (row + 1) + $". Точка с именем '{procWindow.dgvSecond.Rows[row].Cells[3].Value.ToString()}' не существует.";
                                    else
                                        errorStr += "\nБригада 2. Строка " + (row + 1) + $". Точка с именем '{procWindow.dgvSecond.Rows[row].Cells[3].Value.ToString()}' не существует.";
                                    showExeption("Несуществующая точка.", 2, row, 3);
                                }
                            }
                            if (Math.Abs(p1.x - p2.x) != 1)
                            {
                                if (errorStr == "")
                                    errorStr += "Бригада 2. Строка " + (row + 1) + $". Недопустимое расстояние между точками {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} и {procWindow.dgvSecond.Rows[row].Cells[3].Value.ToString()}.";
                                else
                                    errorStr += "\nБригада 2. Строка " + (row + 1) + $". Недопустимое расстояние между точками {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} и {procWindow.dgvSecond.Rows[row].Cells[3].Value.ToString()}.";
                                showExeption("Недопустимое расстояние между точками.", 2, row, 2, 3);
                            }
                            if (p1.x < p2.x)
                                beamHelp = new Beam(p1.x, p1.y, p2.x, p2.y, 2, name);
                            else
                                beamHelp = new Beam(p2.x, p1.y, p1.x, p2.y, 2, name);
                            if (beam1.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam2.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam3.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                            {
                                if (errorStr == "")
                                    errorStr += "Бригада 2. Строка " + (row + 1) + $". Место между точками {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} и {procWindow.dgvSecond.Rows[row].Cells[3].Value.ToString()} занято.";
                                else
                                    errorStr += "\nБригада 2. Строка " + (row + 1) + $". Место между точками {procWindow.dgvSecond.Rows[row].Cells[2].Value.ToString()} и {procWindow.dgvSecond.Rows[row].Cells[3].Value.ToString()} занято.";
                                showExeption("Место занято.", 2, row, 2, 3);
                            }
                            beam2.Add(beamHelp);
                        }
                    }
                }

            }
            if (procWindow.dgvThird.Rows.Count - 1 > row)
            {
                go = true;
                string command = (string)procWindow.dgvThird.Rows[row].Cells[1].Value;
                int name = -1;
                if (command == null || command == "")
                {
                    errorStr += "Бригада 3. Строка " + (row + 1) + ". Отсутствует команда.";
                    showExeption("Отсутствует команда.", 3, row, 1);
                }
                if (command != actions[7])
                {
                    if (procWindow.dgvThird.Rows[row].Cells[0].Value == null)
                    {
                        if (errorStr == "")
                            errorStr += "Бригада 3. Строка " + (row + 1) + ". Отсутствует номер балки.";
                        else
                            errorStr += "\nБригада 3. Строка " + (row + 1) + ". Отсутствует номер балки.";

                        showExeption("Отсутствует номер балки.", 3, row, 0);
                    }
                    if (!int.TryParse(procWindow.dgvThird.Rows[row].Cells[0].Value.ToString(), out name) || name < 1)
                    {
                        if (errorStr == "")
                            errorStr += "Бригада 3. Строка " + (row + 1) + $". {procWindow.dgvThird.Rows[row].Cells[0].Value.ToString()} - недопустимый номер балки.";
                        else
                            errorStr += "\nБригада 3. Строка " + (row + 1) + $". {procWindow.dgvThird.Rows[row].Cells[0].Value.ToString()} - недопустимый номер балки.";
                        showExeption("Недопустимый номер.", 3, row, 0);
                    }

                }
                else
                {
                    beam3.Add(new Beam(-1, -1, -1, -1, -1, -1));
                    go = false;
                }
                if (go)
                {
                    if (name != -1)
                    {
                        if (beam1.Find(x => x.number == name) != null || beam2.Find(x => x.number == name) != null || beam1.Find(x => x.number == name) != null)
                        {
                            if (errorStr == "")
                                errorStr += "Бригада 3. Строка " + (row + 1) + $". Номер балки {procWindow.dgvThird.Rows[row].Cells[0].Value.ToString()} занят.";
                            else
                                errorStr += "\nБригада 3. Строка " + (row + 1) + $". Номер балки {procWindow.dgvThird.Rows[row].Cells[0].Value.ToString()} занят.";
                            showExeption("Номер занят.", 3, row, 0);
                        }
                        if (command == actions[1] || command == actions[2] || command == actions[3] || command == actions[5])
                        {
                            int pos1;
                            if (!int.TryParse(procWindow.dgvThird.Rows[row].Cells[2].Value.ToString(), out pos1))
                            {
                                if (errorStr == "")
                                    errorStr += "Бригада 3. Строка " + (row + 1) + $". {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} - недопустимый номер для балки.";
                                else
                                    errorStr += "\nБригада 3. Строка " + (row + 1) + $". {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} - недопустимый номер для балки.";
                                showExeption("Недопустимый номер.", 3, row, 2);
                            }
                            Beam b = beam1.Find(x => x.number == pos1);
                            int index = beam1.FindIndex(x => x.number == pos1);
                            if (b == null)
                            {
                                b = beam2.Find(x => x.number == pos1);
                                index = beam2.FindIndex(x => x.number == pos1);
                            }
                            if (b == null)
                            {
                                b = beam3.Find(x => x.number == pos1);
                                index = beam3.FindIndex(x => x.number == pos1);
                            }
                            if (b == null || index == row)
                            {
                                if (errorStr == "")
                                    errorStr += "Бригада 3. Строка " + (row + 1) + $". Балка с номером {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} не существует.";
                                else
                                    errorStr += "\nБригада 3. Строка " + (row + 1) + $". Балка с номером {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} не существует.";
                                showExeption("Несуществующая балка.", 3, row, 2);
                            }
                            switch (command)
                            {
                                case "поставить на левый край балки":
                                    if (b.location == 1)
                                    {
                                        if (errorStr == "")
                                            errorStr += "Бригада 3. Строка " + (row + 1) + $". Поставить балку можно только на горизонтальную балку.";
                                        showExeption("Поставить балку можно только на горизонтальную балку.", 3, row, 1);
                                    }
                                    beamHelp = new Beam(b.x1, b.y1 + 1, b.x1, b.y1, 1, name);
                                    if (beam1.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam2.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam3.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                                    {
                                        if (errorStr == "")
                                            errorStr += "Бригада 3. Строка " + (row + 1) + $". Балка {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} занята.";
                                        else
                                            errorStr += "\nБригада 3. Строка " + (row + 1) + $". Балка {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} занята.";
                                        showExeption("Место занято.", 3, row, 2);
                                    }
                                    beam3.Add(beamHelp);
                                    break;
                                case "поставить на правый край балки":
                                    if (b.location == 1)
                                    {
                                        if (errorStr == "")
                                            errorStr += "Бригада 3. Строка " + (row + 1) + $". Поставить балку можно только на горизонтальную балку.";
                                        showExeption("Поставить балку можно только на горизонтальную балку.", 3, row, 1);
                                    }
                                    beamHelp = new Beam(b.x2, b.y2 + 1, b.x2, b.y2, 1, name);
                                    if (beam1.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam2.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam3.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                                    {
                                        if (errorStr == "")
                                            errorStr += "Бригада 3. Строка " + (row + 1) + $". Балка {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} занята.";
                                        else
                                            errorStr += "\nБригада 3. Строка " + (row + 1) + $". Балка {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} занята.";
                                        showExeption("Место занято.", 3, row, 2);
                                    }
                                    beam3.Add(beamHelp);
                                    break;
                                case "поставить на средину балки":
                                    if (b.location == 1)
                                    {
                                        if (errorStr == "")
                                            errorStr += "Бригада 3. Строка " + (row + 1) + $". Поставить балку можно только на горизонтальную балку.";
                                        showExeption("Поставить балку можно только на горизонтальную балку.", 3, row, 1);

                                    }
                                    beamHelp = new Beam((b.x1 + b.x2) / 2, b.y1 + 1, (b.x1 + b.x2) / 2, b.y1, 1, name);
                                    if (beam1.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam2.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam3.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                                    {
                                        if (errorStr == "")
                                            errorStr += "Бригада 3. Строка " + (row + 1) + $". Балка {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} занята.";
                                        else
                                            errorStr += "\nБригада 3. Строка " + (row + 1) + $". Балка {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} занята.";
                                        showExeption("Место занято.", 3, row, 2);
                                    }
                                    beam3.Add(beamHelp);
                                    break;
                                case "положить серединой на балку":
                                    if (b.location == 2)
                                    {
                                        if (errorStr == "")
                                            errorStr += "Бригада 3. Строка " + (row + 1) + $". Положить балку можно только на вертикальную балку.";
                                        showExeption("Положить балку можно только на вертикальную балку.", 3, row, 1);
                                    }
                                    beamHelp = new Beam(b.x1 - 0.5, b.y1, b.x1 + 0.5, b.y1, 2, name);
                                    if (beam1.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam2.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam3.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                                    {
                                        if (errorStr == "")
                                            errorStr += "Бригада 3. Строка " + (row + 1) + $". Балка {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} занята.";
                                        else
                                            errorStr += "\nБригада 3. Строка " + (row + 1) + $". Балка {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} занята.";
                                        showExeption("Место занято.", 3, row, 2);
                                    }
                                    if (beam1.Find(x => (x.x1 == (beamHelp.x1 + beamHelp.x2) / 2 || x.x2 == (beamHelp.x1 + beamHelp.x2) / 2) && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                    beam2.Find(x => (x.x1 == (beamHelp.x1 + beamHelp.x2) / 2 || x.x2 == (beamHelp.x1 + beamHelp.x2) / 2) && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                    beam3.Find(x => (x.x1 == (beamHelp.x1 + beamHelp.x2) / 2 || x.x2 == (beamHelp.x1 + beamHelp.x2) / 2) && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                                    {
                                        if (errorStr == "")
                                            errorStr += "Бригада 3. Строка " + (row + 1) + $". Балка {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} занята.";
                                        else
                                            errorStr += "\nБригада 3. Строка " + (row + 1) + $". Балка {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} занята.";
                                        showExeption("Место занято.", 3, row, 2);
                                    }
                                    beam3.Add(beamHelp);
                                    break;
                            }
                        }
                        if (command == actions[0])
                        {
                            char pos1;
                            if (char.TryParse(procWindow.dgvThird.Rows[row].Cells[2].Value.ToString(), out pos1))
                                if (Convert.ToChar(procWindow.dgvThird.Rows[row].Cells[2].Value) < 'A' || Convert.ToChar(procWindow.dgvThird.Rows[row].Cells[2].Value) > 'Z')
                                {
                                    if (errorStr == "")
                                        errorStr += "Бригада 3. Строка " + (row + 1) + $". Точка {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} имеет недопустимое имя.";
                                    else
                                        errorStr += "\nБригада 3. Строка " + (row + 1) + $". Точка {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} имеет недопустимое имя.";
                                    showExeption("Недопустимое имя точки.", 3, row, 2);
                                }
                            Points p = points.Find(x => x.pointName == pos1);
                            if (p == null)
                            {
                                if (errorStr == "")
                                    errorStr += "Бригада 3. Строка " + (row + 1) + $". Точка с именем '{procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()}' не существует.";
                                else
                                    errorStr += "\nБригада 3. Строка " + (row + 1) + $". Точка с именем '{procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()}' не существует.";
                                showExeption("Несуществующая точка.", 3, row, 2);
                            }
                            beamHelp = new Beam(p.x, p.y + 1, p.x, p.y, 1, name);
                            if (beam1.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam2.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam3.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                            {
                                if (errorStr == "")
                                    errorStr += "Бригада 3. Строка " + (row + 1) + $". Точка {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} занята.";
                                else
                                    errorStr += "\nБригада 3. Строка " + (row + 1) + $". Точка{procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} занята.";
                                showExeption("Место занято.", 3, row, 2);
                            }
                            beam3.Add(beamHelp);
                        }
                        if (command == actions[4])
                        {
                            int pos1 = 0;
                            int pos2 = 0;
                            if (!int.TryParse(procWindow.dgvThird.Rows[row].Cells[2].Value.ToString(), out pos1) || !int.TryParse(procWindow.dgvThird.Rows[row].Cells[3].Value.ToString(), out pos2))
                            {
                                if (!int.TryParse(procWindow.dgvThird.Rows[row].Cells[2].Value.ToString(), out pos1))
                                {
                                    if (errorStr == "")
                                        errorStr += "Бригада 3. Строка " + (row + 1) + $". {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} - недопустимый номер для балки.";
                                    else
                                        errorStr += "\nБригада 3. Строка " + (row + 1) + $". {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} - недопустимый номер для балки.";
                                    showExeption("Недопустимый номер.", 3, row, 2);
                                }
                                else
                                    showExeption("Недопустимый номер.", 3, row, 3);
                            }
                            Beam b1 = beam1.Find(x => x.number == pos1);
                            int index1 = beam1.FindIndex(x => x.number == pos1);
                            if (b1 == null)
                            {
                                b1 = beam2.Find(x => x.number == pos1);
                                index1 = beam2.FindIndex(x => x.number == pos1);
                            }
                            if (b1 == null)
                            {
                                b1 = beam3.Find(x => x.number == pos1);
                                index1 = beam3.FindIndex(x => x.number == pos1);
                            }

                            Beam b2 = beam1.Find(x => x.number == pos2);
                            int index2 = beam1.FindIndex(x => x.number == pos2);
                            if (b2 == null)
                            {
                                b2 = beam2.Find(x => x.number == pos2);
                                index2 = beam2.FindIndex(x => x.number == pos2);
                            }
                            if (b2 == null)
                            {
                                b2 = beam3.Find(x => x.number == pos2);
                                index2 = beam3.FindIndex(x => x.number == pos2);
                            }
                            if (b1 == null || b2 == null || index1 == row || index2 == row)
                            {
                                if (b1 == null || index1 == row)
                                {
                                    if (errorStr == "")
                                        errorStr += "Бригада 3. Строка " + (row + 1) + $". Балка с номером {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} не существует.";
                                    else
                                        errorStr += "\nБригада 3. Строка " + (row + 1) + $". Балка с номером {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} не существует.";
                                    showExeption("Несуществующая балка.", 3, row, 2);
                                }
                                else
                                {
                                    if (errorStr == "")
                                        errorStr += "Бригада 3. Строка " + (row + 1) + $". Балка с номером {procWindow.dgvThird.Rows[row].Cells[3].Value.ToString()} не существует.";
                                    else
                                        errorStr += "\nБригада 3. Строка " + (row + 1) + $". Балка с номером {procWindow.dgvThird.Rows[row].Cells[3].Value.ToString()} не существует.";
                                    showExeption("Несуществующая балка.", 3, row, 3);
                                }
                            }
                            if (b1.y1 != b2.y1 || b1.y2 != b2.y2 || Math.Abs(b1.x1 - b2.x1) != 1)
                            {
                                if (errorStr == "")
                                    errorStr += "Бригада 3. Строка " + (row + 1) + $". Недопустимое расстояние между балками {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} и {procWindow.dgvThird.Rows[row].Cells[3].Value.ToString()}.";
                                else
                                    errorStr += "\nБригада 3. Строка " + (row + 1) + $". Недопустимое расстояние между балками {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} и {procWindow.dgvThird.Rows[row].Cells[3].Value.ToString()}.";
                                showExeption("Недопустимое расстояние между балками.", 3, row, 2, 3);
                            }
                            if (b1.x1 > b2.x1) //если первая указанная балка правее второй указанной балки
                            {
                                if (beam1.FindAll(x => (x.x1 < b1.x1 && x.x1 > b2.x1 || x.x2 < b1.x1 && x.x2 > b2.x1) && x.y1 == x.y2 && x.y2 == b1.y1).Count() != 0 ||
                                    beam2.FindAll(x => (x.x1 < b1.x1 && x.x1 > b2.x1 || x.x2 < b1.x1 && x.x2 > b2.x1) && x.y1 == x.y2 && x.y2 == b1.y1).Count() != 0 ||
                                    beam3.FindAll(x => (x.x1 < b1.x1 && x.x1 > b2.x1 || x.x2 < b1.x1 && x.x2 > b2.x1) && x.y1 == x.y2 && x.y2 == b1.y1).Count() != 0)
                                {
                                    if (errorStr == "")
                                        errorStr += "Бригада 3. Строка " + (row + 1) + $". Балка {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} занята.";
                                    else
                                        errorStr += "\nБригада 3. Строка " + (row + 1) + $". Балка {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} занята.";
                                    showExeption("Балка занята.", 3, row, 2);
                                }
                            }
                            else
                            {
                                if (beam1.FindAll(x => (x.x1 < b2.x1 && x.x1 > b1.x1 || x.x2 < b2.x1 && x.x2 > b1.x1) && x.y1 == x.y2 && x.y2 == b2.y1).Count() != 0 ||
                                    beam2.FindAll(x => (x.x1 < b2.x1 && x.x1 > b1.x1 || x.x2 < b2.x1 && x.x2 > b1.x1) && x.y1 == x.y2 && x.y2 == b2.y1).Count() != 0 ||
                                    beam3.FindAll(x => (x.x1 < b2.x1 && x.x1 > b1.x1 || x.x2 < b2.x1 && x.x2 > b1.x1) && x.y1 == x.y2 && x.y2 == b2.y1).Count() != 0)
                                {
                                    if (errorStr == "")
                                        errorStr += "Бригада 3. Строка " + (row + 1) + $". Балка {procWindow.dgvThird.Rows[row].Cells[3].Value.ToString()} занята.";
                                    else
                                        errorStr += "\nБригада 3. Строка " + (row + 1) + $". Балка {procWindow.dgvThird.Rows[row].Cells[3].Value.ToString()} занята.";
                                    showExeption("Балка занята.", 3, row, 3);
                                }
                            }
                            if (b1.x1 < b2.x1)
                                beamHelp = new Beam(b1.x1, b1.y1, b2.x1, b2.y1, 2, name);
                            else
                                beamHelp = new Beam(b2.x1, b1.y1, b1.x1, b2.y1, 2, name);
                            if (beam1.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam2.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam3.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                            {
                                if (errorStr == "")
                                    errorStr += "Бригада 3. Строка " + (row + 1) + $". Место между балками {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} и {procWindow.dgvThird.Rows[row].Cells[3].Value.ToString()} занято.";
                                else
                                    errorStr += "\nБригада 3. Строка " + (row + 1) + $". Место между балками {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} и {procWindow.dgvThird.Rows[row].Cells[3].Value.ToString()} занято.";
                                showExeption("Место занято.", 3, row, 2, 3);
                            }
                            beam3.Add(beamHelp);
                        }
                        if (command == actions[6])
                        {
                            if (procWindow.dgvThird.Rows[row].Cells[2].Value.ToString().Count() != 1 || Convert.ToChar(procWindow.dgvThird.Rows[row].Cells[2].Value) < 'A' || Convert.ToChar(procWindow.dgvThird.Rows[row].Cells[2].Value) > 'Z')
                            {
                                if (errorStr == "")
                                    errorStr += "Бригада 3. Строка " + (row + 1) + $". Точка {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} имеет недопустимое имя.";
                                else
                                    errorStr += "\nБригада 3. Строка " + (row + 1) + $". Точка {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} имеет недопустимое имя.";
                                showExeption("Недопустимое имя точки.", 3, row, 2);
                            }
                            if (procWindow.dgvThird.Rows[row].Cells[3].Value.ToString().Count() != 1 || Convert.ToChar(procWindow.dgvThird.Rows[row].Cells[3].Value) < 'A' || Convert.ToChar(procWindow.dgvThird.Rows[row].Cells[3].Value) > 'Z')
                            {
                                if (errorStr == "")
                                    errorStr += "Бригада 3. Строка " + (row + 1) + $". Точка {procWindow.dgvThird.Rows[row].Cells[3].Value.ToString()} имеет недопустимое имя.";
                                else
                                    errorStr += "\nБригада 3. Строка " + (row + 1) + $". Точка {procWindow.dgvThird.Rows[row].Cells[3].Value.ToString()} имеет недопустимое имя.";
                                showExeption("Недопустимое имя точки.", 3, row, 2);
                            }
                            char pos1 = Convert.ToChar(procWindow.dgvThird.Rows[row].Cells[2].Value);
                            char pos2 = Convert.ToChar(procWindow.dgvThird.Rows[row].Cells[3].Value);
                            Points p1 = points.Find(x => x.pointName == pos1);
                            Points p2 = points.Find(x => x.pointName == pos2);
                            if (p1 == null || p2 == null)
                            {
                                if (p1 == null)
                                {
                                    if (errorStr == "")
                                        errorStr += "Бригада 3. Строка " + (row + 1) + $". Точка с именем '{procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()}' не существует.";
                                    else
                                        errorStr += "\nБригада 3. Строка " + (row + 1) + $". Точка с именем '{procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()}' не существует.";
                                    showExeption("Несуществующая точка.", 3, row, 2);
                                }
                                else
                                {
                                    if (errorStr == "")
                                        errorStr += "Бригада 3. Строка " + (row + 1) + $". Точка с именем '{procWindow.dgvThird.Rows[row].Cells[3].Value.ToString()}' не существует.";
                                    else
                                        errorStr += "\nБригада 3. Строка " + (row + 1) + $". Точка с именем '{procWindow.dgvThird.Rows[row].Cells[3].Value.ToString()}' не существует.";
                                    showExeption("Несуществующая точка.", 3, row, 3);
                                }
                            }
                            if (Math.Abs(p1.x - p2.x) != 1)
                            {
                                if (errorStr == "")
                                    errorStr += "Бригада 3. Строка " + (row + 1) + $". Недопустимое расстояние между точками {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} и {procWindow.dgvThird.Rows[row].Cells[3].Value.ToString()}.";
                                else
                                    errorStr += "\nБригада 3. Строка " + (row + 1) + $". Недопустимое расстояние между точками {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} и {procWindow.dgvThird.Rows[row].Cells[3].Value.ToString()}.";
                                showExeption("Недопустимое расстояние между точками.", 3, row, 2, 3);
                            }
                            if (p1.x < p2.x)
                                beamHelp = new Beam(p1.x, p1.y, p2.x, p2.y, 2, name);
                            else
                                beamHelp = new Beam(p2.x, p1.y, p1.x, p2.y, 2, name);
                            if (beam1.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam2.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null ||
                                beam3.Find(x => x.x1 == beamHelp.x1 && x.x2 == beamHelp.x2 && x.y1 == beamHelp.y1 && x.y2 == beamHelp.y2) != null)
                            {
                                if (errorStr == "")
                                    errorStr += "Бригада 3. Строка " + (row + 1) + $". Место между точками {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} и {procWindow.dgvThird.Rows[row].Cells[3].Value.ToString()} занято.";
                                else
                                    errorStr += "\nБригада 3. Строка " + (row + 1) + $". Место между точками {procWindow.dgvThird.Rows[row].Cells[2].Value.ToString()} и {procWindow.dgvThird.Rows[row].Cells[3].Value.ToString()} занято.";
                                showExeption("Место занято.", 3, row, 2, 3);
                            }
                            beam3.Add(beamHelp);
                        }
                    }
                }
            }
        }
        private void showExeption(string ex, int brigade, int str, int cell)
        {
            if (brigade == 1)
                procWindow.dgvFirst.Rows[str].Cells[cell].ErrorText = ex;
            if (brigade == 2)
            {
                procWindow.dgvSecond.Rows[str].Cells[cell].ErrorText = ex;
                if (beam1.Count > str)
                    beam1.RemoveAt(str);
            }
            if (brigade == 3)
            {
                procWindow.dgvThird.Rows[str].Cells[cell].ErrorText = ex;
                if (beam1.Count > str)
                    beam1.RemoveAt(str);
                if (beam2.Count > str)
                    beam2.RemoveAt(str);
            }
            procWindow.richTb.Text = "Ошибка!\n" + errorStr;
            throw new Exception("Возникла ошибка!\n" + errorStr);
        }
        private void showExeption(string ex, int brigade, int str, int cell1, int cell2)
        {
            if (brigade == 1)
            {
                procWindow.dgvFirst.Rows[str].Cells[cell1].ErrorText = ex;
                procWindow.dgvFirst.Rows[str].Cells[cell2].ErrorText = ex;
            }
            if (brigade == 2)
            {
                procWindow.dgvSecond.Rows[str].Cells[cell1].ErrorText = ex;
                procWindow.dgvSecond.Rows[str].Cells[cell2].ErrorText = ex;
                if (beam1.Count > str)
                    beam1.RemoveAt(str);
            }
            if (brigade == 3)
            {
                procWindow.dgvThird.Rows[str].Cells[cell1].ErrorText = ex;
                procWindow.dgvThird.Rows[str].Cells[cell2].ErrorText = ex;
                if (beam1.Count > str)
                    beam1.RemoveAt(str);
                if (beam2.Count > str)
                    beam2.RemoveAt(str);
            }
            procWindow.richTb.Text = "Ошибка!\n" + errorStr;
            throw new Exception("Возникла ошибка!\n" + errorStr);
        }
        private void preparationForDrawing()
        {
            bb.Clear();
            lb.Clear();
            beam1.Clear();
            beam2.Clear();
            beam3.Clear();
            beam1 = new List<Beam>();
            beam2 = new List<Beam>();
            beam3 = new List<Beam>();
            procWindow.richTb.Text = "";
            int _max = procWindow.dgvFirst.Rows.Count > procWindow.dgvSecond.Rows.Count ? procWindow.dgvFirst.Rows.Count : procWindow.dgvSecond.Rows.Count;
            int max = _max > procWindow.dgvThird.Rows.Count ? _max : procWindow.dgvThird.Rows.Count;
            for (int row = 0; row < max - 1; row++)
            {
                try
                {
                    addBeams(row);
                    drawPic(row);
                    deleteColor();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    procWindow.Activate();
                    return;
                }
            }
            if (taskBeam.Count != 0)
            {
                if (procWindow.dgvFirst.Rows.Count > 1 || procWindow.dgvSecond.Rows.Count > 1 || procWindow.dgvThird.Rows.Count > 1)
                {
                    string res = getCheckResult();
                    check = true;
                    playingFieldBox.Refresh();
                    int num1 = procWindow.dgvFirst.Rows.Count - 1;
                    int num2 = procWindow.dgvSecond.Rows.Count - 1;
                    int num3 = procWindow.dgvThird.Rows.Count - 1;
                    MessageBox.Show("Алгоритм выполнен." + "\n" + res + $"\nВыполнено шагов: {num1} +  {num2} + {num3} = {num1+num2+num3}");
                }
            }
            else
            {
                if (procWindow.dgvFirst.Rows.Count > 1 || procWindow.dgvSecond.Rows.Count > 1 || procWindow.dgvThird.Rows.Count > 1)
                    MessageBox.Show("Алгоритм выполнен.");
            }
            step = -1;
        }
        private void trackBarSpeed_Scroll(object sender, EventArgs e)
        {
            playingFieldBox.Invalidate();
        }

        private void splitBeams(List<Beam> bm)
        {
            bm.Sort((a1, a2) => a1.x1.CompareTo(a2.x1)); //сортируем, чтобы пройтись по горизонтальным балкам строго слева направо
            int count = 0;
            bool cicle = true;
            while (cicle)
            {

                if (bm[count].y1 == bm[count].y2)
                {
                    if (bm.FindAll(x => x.x1 == bm[count].x2 && x.x1 != x.x2).Count != 0)
                    {
                        Beam _bb = new Beam(bm[count].x1 / 2, bm[count].y1, bm[count].x1 / 2 + 1, bm[count].y2, 2, 0);
                        taskBeam.Add(_bb);
                        Beam b1 = bm.Find(x => x.x1 == bm[count].x2 && x.x1 != x.x2 && x.y1 == bm[count].y1 && x.y1 == bm[count].y2);
                        Beam b2 = bm.Find(x => x.x2 == bm[count].x2 && x.x1 != x.x2 && x.y1 == bm[count].y1 && x.y1 == bm[count].y2);
                        bm.Remove(b1);
                        bm.Remove(b2);
                        count--;
                    }
                }
                else
                {
                    Beam _bb = new Beam(bm[count].x1 / 2, bm[count].y1, bm[count].x2 / 2, bm[count].y2, 1, 0);//добавляем эту балку в список для сохранения
                    taskBeam.Add(_bb);
                }
                count++;
                if (count > bm.Count() - 1)
                    cicle = false;
            }
            task = true;
        }

        private void preporationForSaveActions()
        {
            actionList.Clear();
                for (int i = 0; i < procWindow.dgvFirst.RowCount - 1; i++)
                {
                    Action action = new Action();
                    if (procWindow.dgvFirst.Rows[i].Cells[1].Value == null || procWindow.dgvFirst.Rows[i].Cells[1].Value.ToString() == "")
                    {
                        throw new Exception("Невозможно сохранить алгоритм! Присутствует пустая команда в алгоритме 1 бригады.");
                    }
                    if (procWindow.dgvFirst.Rows[i].Cells[1].Value.ToString() != actions[7])
                        action = new Action(Convert.ToInt32(procWindow.dgvFirst.Rows[i].Cells[0].Value), actions.FindIndex(x => x == procWindow.dgvFirst.Rows[i].Cells[1].Value.ToString()),
                                            procWindow.dgvFirst.Rows[i].Cells[2].Value.ToString(), procWindow.dgvFirst.Rows[i].Cells[3].Value.ToString(), 1);
                    else
                        action = new Action(0, 7, "", "", 1);
                    actionList.Add(action);
                }
                for (int i = 0; i < procWindow.dgvSecond.RowCount - 1; i++)
                {
                    Action action = new Action();
                    if (procWindow.dgvSecond.Rows[i].Cells[1].Value == null || procWindow.dgvSecond.Rows[i].Cells[1].Value.ToString() == "")
                    {
                        throw new Exception("Невозможно сохранить алгоритм! Присутствует пустая команда в алгоритме 2 бригады.");
                    }
                    if (procWindow.dgvSecond.Rows[i].Cells[1].Value.ToString() != actions[7])
                        action = new Action(Convert.ToInt32(procWindow.dgvSecond.Rows[i].Cells[0].Value), actions.FindIndex(x => x == procWindow.dgvSecond.Rows[i].Cells[1].Value.ToString()),
                                                    procWindow.dgvSecond.Rows[i].Cells[2].Value.ToString(), procWindow.dgvSecond.Rows[i].Cells[3].Value.ToString(), 2);
                    else
                        action = new Action(0, 7, "", "", 2);
                    actionList.Add(action);
                }
                for (int i = 0; i < procWindow.dgvThird.RowCount - 1; i++)
                {
                    Action action = new Action();
                    if (procWindow.dgvThird.Rows[i].Cells[1].Value == null || procWindow.dgvThird.Rows[i].Cells[1].Value.ToString() == "")
                    {
                        throw new Exception("Невозможно сохранить алгоритм! Присутствует пустая команда в алгоритме 3 бригады.");
                    }
                    if (procWindow.dgvThird.Rows[i].Cells[1].Value.ToString() != actions[7])
                        action = new Action(Convert.ToInt32(procWindow.dgvThird.Rows[i].Cells[0].Value), actions.FindIndex(x => x == procWindow.dgvThird.Rows[i].Cells[1].Value.ToString()),
                                            procWindow.dgvThird.Rows[i].Cells[2].Value.ToString(), procWindow.dgvThird.Rows[i].Cells[3].Value.ToString(), 3);
                    else
                        action = new Action(0, 7, "", "", 3);
                    actionList.Add(action);
                }
        }

        private void clearErrors()
        {
            for (int i = 0; i < procWindow.dgvFirst.RowCount - 1; i++)
            {
                for (int j = 0; j < 4; j++)
                    procWindow.dgvFirst.Rows[i].Cells[j].ErrorText = "";
            }
            for (int i = 0; i < procWindow.dgvSecond.RowCount - 1; i++)
            {
                for (int j = 0; j < 4; j++)
                    procWindow.dgvSecond.Rows[i].Cells[j].ErrorText = "";
            }
            for (int i = 0; i < procWindow.dgvThird.RowCount - 1; i++)
            {
                for (int j = 0; j < 4; j++)
                    procWindow.dgvThird.Rows[i].Cells[j].ErrorText = "";
            }
        }
        private void buttonStep_Click(object sender, EventArgs e)
        {
            draw = true;
            clear = false;
            check = false;
            procWindow.dgvFirst.EndEdit();
            procWindow.dgvSecond.EndEdit();
            procWindow.dgvThird.EndEdit();
            if (step == -1)
            {
                check = false;
                procWindow.richTb.Text = "";
                lb.Clear();
                bb.Clear();
                clearErrors();
                beam1.Clear();
                beam2.Clear();
                beam3.Clear();
                beam1 = new List<Beam>();
                beam2 = new List<Beam>();
                beam3 = new List<Beam>();
                playingFieldBox.Refresh();
            }
            step++;
            speed = 0;
            int _max = procWindow.dgvFirst.Rows.Count > procWindow.dgvSecond.Rows.Count ? procWindow.dgvFirst.Rows.Count : procWindow.dgvSecond.Rows.Count;
            int max = _max > procWindow.dgvThird.Rows.Count ? _max : procWindow.dgvThird.Rows.Count;
            if (step < _max - 1)
            {
                try
                {
                    addBeams(step);
                    drawPic(step);
                }
                catch (Exception ex)
                {
                    step = -1;
                    procWindow.Activate();
                    MessageBox.Show(ex.Message);
                    deleteColor();
                    return;
                }
                if (step == _max - 2)
                {
                    if (taskBeam.Count != 1)
                    {
                        string res = getCheckResult();
                        check = true;
                        playingFieldBox.Refresh();
                        int num1 = procWindow.dgvFirst.Rows.Count - 1;
                        int num2 = procWindow.dgvSecond.Rows.Count - 1;
                        int num3 = procWindow.dgvThird.Rows.Count - 1;
                        MessageBox.Show("Алгоритм выполнен." + "\n" + res + $"\nВыполнено шагов: {num1} +  {num2} + {num3} = {num1 + num2 + num3}");
                    }
                    else
                        MessageBox.Show("Алгоритм выполнен.");
                    step = -1;
                    deleteColor();
                }
            }
        }

        private void загрузитьАлгоритмToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = directory;
            openFileDialog.Filter = "building algorithm files (*.buildingalgorithm)|*.buildingalgorithm|All files (*.*)|*.*";
            openFileDialog.ShowDialog();
            string filename = openFileDialog.FileName;
            if (filename == "")
                return;
            directory = filename;
            saveNewSettings(filename);
            actionList.Clear();
            procWindow.dgvFirst.Rows.Clear();
            procWindow.dgvSecond.Rows.Clear();
            procWindow.dgvThird.Rows.Clear();
            tbPathAlg.Text = filename;
            try
            {
                System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(List<Action>));

                System.IO.StreamReader file = new System.IO.StreamReader(filename);
                actionList = (List<Action>)reader.Deserialize(file);
                file.Close();
            }
            catch
            {
                tbPathAlg.Text = "Не загружен";
                MessageBox.Show("Невозможно загрузить алгоритм.");
                return;
            }

            for (int i = 0; i < actionList.Count(); i++)
            {
                DataGridViewRow row = null;
                if (actionList[i].brigade == 1)
                    row = (DataGridViewRow)procWindow.dgvFirst.Rows[0].Clone();

                if (actionList[i].brigade == 2)
                    row = (DataGridViewRow)procWindow.dgvSecond.Rows[0].Clone();

                if (actionList[i].brigade == 3)
                    row = (DataGridViewRow)procWindow.dgvThird.Rows[0].Clone();
                if (!(actionList[i].action < 8 && actionList[i].action > 0))
                {
                    actionList.Clear();
                    procWindow.dgvFirst.Rows.Clear();
                    procWindow.dgvSecond.Rows.Clear();
                    procWindow.dgvThird.Rows.Clear();
                    MessageBox.Show("Невозможно загрузить алгоритм.");
                    tbPathAlg.Text = "";
                    return;
                }
                if (actionList[i].action != 7)
                {
                    row.Cells[0].Value = actionList[i].name;
                    row.Cells[1].Value = actions[actionList[i].action];
                    row.Cells[2].Value = actionList[i].firstCoord;
                    row.Cells[3].Value = actionList[i].secondCoord;
                }
                else
                {
                    row.Cells[0].Value = "";
                    row.Cells[1].Value = actions[actionList[i].action];
                    row.Cells[2].Value = "";
                    row.Cells[3].Value = "";
                }
                if (actionList[i].brigade == 1)
                    procWindow.dgvFirst.Rows.Add(row);

                if (actionList[i].brigade == 2)
                    procWindow.dgvSecond.Rows.Add(row);

                if (actionList[i].brigade == 3)
                    procWindow.dgvThird.Rows.Add(row);
            }
    
        }
        private string getCheckResult()
        {
            lb.Clear();
            bb.Clear();
            for (int j = 0; j < taskBeam.Count(); j++)
            {
                lb.Add(taskBeam[j]);
            }
            bb = beam1.Union(beam2).Union(beam3).ToList();
            int i = 0;
            bool cicle = true;
            bb.RemoveAll(x => x.location == -1);

            while (cicle)
            {
                if (bb.Find(x => x.x1 == lb[i].x1 && lb[i].x2 == x.x2 && x.y1 == lb[i].y1 && lb[i].y2 == x.y2) != null)
                {
                    bb.Remove(bb.Find(x => x.x1 == lb[i].x1 && lb[i].x2 == x.x2 && x.y1 == lb[i].y1 && lb[i].y2 == x.y2));
                    lb.Remove(lb.Find(x => x.x1 == lb[i].x1 && lb[i].x2 == x.x2 && x.y1 == lb[i].y1 && lb[i].y2 == x.y2));
                    i--;
                }
                i++;
                if (i > lb.Count() - 1)
                    cicle = false;
            }
            string result = "";
            if (lb.Count() == 0 && bb.Count == 0)
            {
                result = "Построение выполнено правильно.";
            }
            else
            {
                string t = ""; //строка для вывода непостроенных для задания балок (task)
                string p = ""; //строка для балок, которые были поставлены неверно (picture)
                string r = ""; //строка для балок, которые были поставлены верно (right)
                if (lb.Count() != 0) //не все балки, указанные в задании, были построены
                {
                    t += "Количество непоставленных балок: ";
                    t += lb.Count();
                }
                if (bb.Count() != 0)
                {
                    p += "Количество лишних балок: ";
                    p += bb.Count();
                }
                    r += "Количество верных балок: ";
                    r += taskBeam.Count - lb.Count;
                playingFieldBox.Refresh();
                playingFieldBox.Invalidate();
                if (t != "")
                    result += t + " (помечены знаком #)";
                if (p != "")
                {
                    if (result != "")
                        result += "\n" + p + " (помечены красным)";
                    else
                        result += p;
                }
                if (r != "")
                    result += "\n" + r;
            }
            return result;
        }
        private void сохранитьАлгоритмToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                preporationForSaveActions();
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                saveFileDialog.Filter = "building algorithm files (*.buildingalgorithm)|*.buildingalgorithm|All files (*.*)|*.*";
                saveFileDialog.ShowDialog();
                string filename = saveFileDialog.FileName;
                if (filename == "")
                    return;
                tbPathAlg.Text = filename;
                directory = filename;
                saveNewSettings(filename);
                List<Action> overview = new List<Action>();
                overview = actionList;
                System.Xml.Serialization.XmlSerializer writer =
                    new System.Xml.Serialization.XmlSerializer(typeof(List<Action>));

                System.IO.FileStream file = System.IO.File.Create(filename);

                writer.Serialize(file, overview);
                file.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void проверитьЗаданиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string result = "";
            if (taskBeam.Count() == 0)
            {
                MessageBox.Show("Задание не было загружено.");
                return;
            }
            result = getCheckResult();
            check = true;
            playingFieldBox.Refresh();
            MessageBox.Show(result);
        }

        private void загрузитьЗаданиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
                List<Beam> bm = new List<Beam>();
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = directory;
                openFileDialog.Filter = "building task files (*.buildingtask)|*.buildingtask|All files (*.*)|*.*";
                openFileDialog.ShowDialog();
                string filename = openFileDialog.FileName;
                if (filename == "")
                    return;
            directory = filename;
            saveNewSettings(filename);
                lb.Clear();
                bb.Clear();
                taskBeam.Clear();
                check = false;
                beam1.Clear();
                beam2.Clear();
                beam3.Clear();
                tbPathTask.Text = filename;
            try
            {
                System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(List<Beam>));
                System.IO.StreamReader file = new System.IO.StreamReader(filename);
                bm = (List<Beam>)reader.Deserialize(file);
                file.Close();
                splitBeams(bm);
                numOfCellsX = Convert.ToInt32(Math.Ceiling(taskBeam.Max(x => x.x2)));
                numOfCellsY = Convert.ToInt32(Math.Ceiling(taskBeam.Max(x => x.y1))) + 1;
                playingFieldBox.Invalidate();
            }
            catch
            {
                tbPathTask.Text = "Не загружено";
                MessageBox.Show("Невозможно загрузить задание.");
                return;
            }
            
        }

        private void очиститьЗаданиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lb.Clear();
            bb.Clear();
            beam1.Clear();
            beam2.Clear();
            beam3.Clear();
            taskBeam.Clear();
            tbPathTask.Text = "Не загружено";
            task = false;
            playingFieldBox.Invalidate();
        }

        private void изменитьПолеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapSettings frm = new mapSettings(numOfCellsX, numOfCellsY);
            frm.Owner = this; //Передаём вновь созданной форме её владельца.
            frm.Show();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void наНачалоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearErrors();
            lb.Clear();
            bb.Clear();
            procWindow.richTb.Text = "";
            check = false;
            step = -1;
            deleteColor();
            draw = false;
            clear = true;
            beam1.Clear();
            beam2.Clear();
            beam3.Clear();

            playingFieldBox.Invalidate();
        }
        private void выполнитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            check = false;
            draw = true;
            clear = false;
            lb.Clear();
            bb.Clear();
            beam1.Clear();
            beam2.Clear();
            beam3.Clear();
            procWindow.dgvFirst.EndEdit();
            procWindow.dgvSecond.EndEdit();
            procWindow.dgvThird.EndEdit();
            playingFieldBox.Refresh();
            playingFieldBox.Invalidate();
            preparationForDrawing();
        }

        private void шагToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (step == -1)
            {
                beam1.Clear();
                beam2.Clear();
                beam3.Clear();
                beam1 = new List<Beam>();
                beam2 = new List<Beam>();
                beam3 = new List<Beam>();
                playingFieldBox.Refresh();
            }
            step++;
            speed = 0;
            int _max = procWindow.dgvFirst.Rows.Count > procWindow.dgvSecond.Rows.Count ? procWindow.dgvFirst.Rows.Count : procWindow.dgvSecond.Rows.Count;
            int max = _max > procWindow.dgvThird.Rows.Count ? _max : procWindow.dgvThird.Rows.Count;
            if (step < _max - 1)
            {
                try
                {
                    addBeams(step);
                    drawPic(step);
                }
                catch (Exception ex)
                {
                    step = -1;
                    MessageBox.Show(ex.Message);
                    deleteColor();
                    return;
                }
            }
            else
            {
                if (procWindow.dgvFirst.Rows.Count > 1 || procWindow.dgvSecond.Rows.Count > 1 || procWindow.dgvThird.Rows.Count > 1)
                {
                    MessageBox.Show("Алгоритм успешно выполнен!");
                    step = -1;
                    deleteColor();
                }
            }
        }
        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            procWindow.dgvFirst.EndEdit();
            procWindow.dgvSecond.EndEdit();
            procWindow.dgvThird.EndEdit();
            if ((procWindow.dgvFirst.Rows.Count != 1 || procWindow.dgvSecond.Rows.Count != 1 || procWindow.dgvThird.Rows.Count != 1) && tbPathAlg.Text == "Не загружен")
            {
                const string message =
                "Алгоритм не был сохранён, Вы уверены что хотите выйти?";
                const string caption = "Выход из программы";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                    e.Cancel = true;
            }
            if ((procWindow.dgvFirst.Rows.Count != 1 || procWindow.dgvSecond.Rows.Count != 1 || procWindow.dgvThird.Rows.Count != 1) && tbPathAlg.Text != "Не загружен" && checkAlgorithm())
            {
                const string message =
                "Алгоритм был изменён и не был сохранён, Вы уверены что хотите выйти?";
                const string caption = "Выход из программы";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                    e.Cancel = true;
            }

        }
        private bool checkAlgorithm()
        {
            List<Action> _actions = new List<Action>();
            check = true;
            string filename = tbPathAlg.Text;
            System.Xml.Serialization.XmlSerializer reader =
            new System.Xml.Serialization.XmlSerializer(typeof(List<Action>));
            System.IO.StreamReader file = new System.IO.StreamReader(filename);
            _actions = (List<Action>)reader.Deserialize(file);
            file.Close();

            List<Action> helpAction = new List<Action>();
            for (int i = 0; i < procWindow.dgvFirst.RowCount - 1; i++)
            {
                Action action = new Action();
                if (procWindow.dgvFirst.Rows[i].Cells[1].Value == null || procWindow.dgvFirst.Rows[i].Cells[1].Value.ToString() == "")
                    return false;
                if (procWindow.dgvFirst.Rows[i].Cells[1].Value.ToString() != actions[7])
                    action = new Action(Convert.ToInt32(procWindow.dgvFirst.Rows[i].Cells[0].Value), actions.FindIndex(x => x == procWindow.dgvFirst.Rows[i].Cells[1].Value.ToString()),
                                        procWindow.dgvFirst.Rows[i].Cells[2].Value.ToString(), procWindow.dgvFirst.Rows[i].Cells[3].Value.ToString(), 1);
                else
                    action = new Action(0, 7, "", "", 1);
                helpAction.Add(action);
            }
            for (int i = 0; i < procWindow.dgvSecond.RowCount - 1; i++)
            {
                Action action = new Action();
                if (procWindow.dgvSecond.Rows[i].Cells[1].Value == null || procWindow.dgvSecond.Rows[i].Cells[1].Value.ToString() == "")
                    return false;
                if (procWindow.dgvSecond.Rows[i].Cells[1].Value.ToString() != actions[7])
                    action = new Action(Convert.ToInt32(procWindow.dgvSecond.Rows[i].Cells[0].Value), actions.FindIndex(x => x == procWindow.dgvSecond.Rows[i].Cells[1].Value.ToString()),
                                                procWindow.dgvSecond.Rows[i].Cells[2].Value.ToString(), procWindow.dgvSecond.Rows[i].Cells[3].Value.ToString(), 2);
                else
                    action = new Action(0, 7, "", "", 2);
                helpAction.Add(action);
            }
            for (int i = 0; i < procWindow.dgvThird.RowCount - 1; i++)
            {
                Action action = new Action();
                if (procWindow.dgvThird.Rows[i].Cells[1].Value == null || procWindow.dgvThird.Rows[i].Cells[1].Value.ToString() == "")
                    return false;
                if (procWindow.dgvThird.Rows[i].Cells[1].Value.ToString() != actions[7])
                    action = new Action(Convert.ToInt32(procWindow.dgvThird.Rows[i].Cells[0].Value), actions.FindIndex(x => x == procWindow.dgvThird.Rows[i].Cells[1].Value.ToString()),
                                        procWindow.dgvThird.Rows[i].Cells[2].Value.ToString(), procWindow.dgvThird.Rows[i].Cells[3].Value.ToString(), 3);
                else
                    action = new Action(0, 7, "", "", 3);
                helpAction.Add(action);
            }
            if (_actions.Count() != helpAction.Count())
                return true;
            for (int i = 0; i < actionList.Count(); i++)
                if (_actions[i].action != helpAction[i].action || _actions[i].name != helpAction[i].name || _actions[i].firstCoord != helpAction[i].firstCoord || _actions[i].brigade != helpAction[i].brigade || _actions[i].secondCoord != helpAction[i].secondCoord)
                    return true;
            return false;
        }

        private void распечататьАлгоритмToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string b1 = "", b2 = "", b3 = "";
            string b = "";
            preporationForSave(ref b1, ref b2, ref b3);
            b += "Бригада 1\n" + b1;
            b += "Бригада 2\n" + b2;
            b += "Бригада 3\n" + b3;

            try
            {
                preporationForSaveActions();
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = directory;
                saveFileDialog.Filter = "text file (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.ShowDialog();
                string filename = saveFileDialog.FileName;
                if (filename == "")
                    return;
                saveNewSettings(filename);
                directory = filename;
                StreamWriter file = new StreamWriter(new FileStream(filename, FileMode.Create, FileAccess.Write));
                file.Write(b);
                file.Close();
            }
            catch (Exception ex)
            {
                return;
            }
        }
        private void saveNewSettings(string path)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                settings["path"].Value = path;
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch
            {
                return;
            }
        }
        private void preporationForSave(ref string b1, ref string b2, ref string b3)
        {
            b1 += "----------------------------------------------------------------------------\n";
            for (int i = 0; i < procWindow.dgvFirst.RowCount - 1; i++)
            {
                string help = "";
                if (procWindow.dgvFirst.Rows[i].Cells[3].Value.ToString() != "")
                    help += " и" + " " + procWindow.dgvFirst.Rows[i].Cells[3].Value.ToString();
                b1 += " " + procWindow.dgvFirst.Rows[i].Cells[0].Value + "-ю балку " + procWindow.dgvFirst.Rows[i].Cells[1].Value + " " + procWindow.dgvFirst.Rows[i].Cells[2].Value + help + " \n\n";
            }
            b1 += "----------------------------------------------------------------------------\n";
            b2 += "----------------------------------------------------------------------------\n";
            for (int i = 0; i < procWindow.dgvSecond.RowCount - 1; i++)
            {
                string help = "";
                if (procWindow.dgvSecond.Rows[i].Cells[3].Value.ToString() != "")
                    help += " и" + " " + procWindow.dgvSecond.Rows[i].Cells[3].Value.ToString();
                b2 += " " + procWindow.dgvSecond.Rows[i].Cells[0].Value + "-ю балку " + procWindow.dgvSecond.Rows[i].Cells[1].Value + " " + procWindow.dgvSecond.Rows[i].Cells[2].Value + help + " \n\n";
            }
            b2 += "----------------------------------------------------------------------------\n";
            b3 += "----------------------------------------------------------------------------\n";
            for (int i = 0; i < procWindow.dgvThird.RowCount - 1; i++)
            {
                string help = "";
                if (procWindow.dgvThird.Rows[i].Cells[3].Value.ToString() != "")
                    help += " и" + " " + procWindow.dgvThird.Rows[i].Cells[3].Value.ToString();
                b3 += " " + procWindow.dgvThird.Rows[i].Cells[0].Value + "-ю балку " + procWindow.dgvThird.Rows[i].Cells[1].Value + " " + procWindow.dgvThird.Rows[i].Cells[2].Value + help + " \n\n";
            }
            b3 += "----------------------------------------------------------------------------\n";
        }
        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            infoForm frm = new infoForm();
            frm.Owner = this; //Передаём вновь созданной форме её владельца.
            frm.Show();
        }

        private void trackBarSize_Scroll(object sender, EventArgs e)
        {
            playingFieldBox.Invalidate();
        }

    }
}
