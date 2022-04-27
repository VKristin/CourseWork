using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Drawing.Imaging;
using System.Configuration;

namespace taskConstructor
{
    public partial class taskConstructor : Form
    {
        //string directory = Directory.GetCurrentDirectory();
        string directory;

        public taskConstructor()
        {
            this.Left = 0;
            this.Top = 0;
            InitializeComponent();
            var appSettings = ConfigurationManager.AppSettings;
            directory = ConfigurationManager.AppSettings["path"];
            if (directory == null || directory == "" || !File.Exists(directory))
                directory = Directory.GetCurrentDirectory();
        }
        int numOfCellsX = -1;
        int numOfCellsY = -1;
        int cellSize;
        List<string> actions = new List<string>();
        List<Beam> beam = new List<Beam>(); //балки
        List<Beam> beamForBuild = new List<Beam>(); //балки
        bool draw = false;
        bool clear = false;
        bool check = false;
        bool save = false;
        bool fly = true;
        Image bmp;
        Graphics g;
        Beam wrongBeam = null;
        
        public void gridSize(int _x, int _y)
        {
            numOfCellsX = _x + 1;
            numOfCellsY = _y;
            draw = false;
            playingFieldBox.Refresh();
            playingFieldBox.Invalidate();
            draw = true;
            playingFieldBox.Invalidate();
        }

        private void trackBarSize_Scroll(object sender, EventArgs e)
        {
            playingFieldBox.Invalidate();
        }

        private void playingFieldBox_Paint(object sender, PaintEventArgs e)
        {
            playingFieldBox.Image = new Bitmap(playingFieldBox.Width, playingFieldBox.Height);
            bmp = playingFieldBox.Image;
            g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
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
                g.DrawLine(pen, 0, y * cellSize, numOfCellsX * cellSize, y * cellSize);
            }

            for (int x = 0; x <= numOfCellsX * 2; x++)
            {
                g.DrawLine(pen, x * cellSize / 2, 0, x * cellSize / 2, numOfCellsY * cellSize);
            }

            Font drawFont = new Font("Arial", 16);
            StringFormat drawFormat = new StringFormat();
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            char letter = 'A';
            for (int l = 1; l < numOfCellsX; l++)
            {
                g.FillRectangle(Brushes.Black, cellSize * l - 1, numOfCellsY * cellSize - 1, 3, 3);
                g.DrawString(letter.ToString(), drawFont, drawBrush, cellSize * l - 12, begLetterY, drawFormat);
                letter++;
            }
            Pen penBuild = new Pen(Color.Black, 3);
            Pen penWrong = new Pen(Color.Red, 3);
            SolidBrush brushBuild = new SolidBrush(Color.Red);
            Font fontBuild = new Font("Arial", 14);

            if (draw && !clear)
            {
                for (int i = 0; i < beam.Count; i++)
                {
                    Point point1 = new Point((int)(beam[i].x1 * cellSize / 2), (int)((-beam[i].y1 + numOfCellsY) * cellSize));
                    Point point2 = new Point((int)(beam[i].x2 * cellSize / 2), (int)((-beam[i].y2 + numOfCellsY) * cellSize));
                    g.DrawLine(penBuild, point1, point2);
                }
                if (wrongBeam != null && beam.FindAll(x => x.x2 == wrongBeam.x2 && x.x1 == wrongBeam.x1 && x.y1 == wrongBeam.y1 && x.y2 == wrongBeam.y2).Count() != 0)
                {
                    Point point1 = new Point((int)(wrongBeam.x1 * cellSize / 2), (int)((-wrongBeam.y1 + numOfCellsY) * cellSize));
                    Point point2 = new Point((int)(wrongBeam.x2 * cellSize / 2), (int)((-wrongBeam.y2 + numOfCellsY) * cellSize));
                    g.DrawLine(penWrong, point1, point2);
                }
                else
                {
                    wrongBeam = null;
                }
            }
        }
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save = true;
                beam.RemoveAll(x => x.x1 < 0 || x.x2 < 0 || x.y1 < 0 || x.y2 < 0);
                if (beam.Count != 0)
                {
                    AddInTask();
                }
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = directory;
                saveFileDialog.Filter = "building task files (*.buildingtask)|*.buildingtask|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = saveFileDialog.FileName;

                List<Beam> overview = new List<Beam>();
                overview = beamForBuild;
                System.Xml.Serialization.XmlSerializer writer =
                    new System.Xml.Serialization.XmlSerializer(typeof(List<Beam>));
                if (filename == "")
                    return;
                directory = filename;
                saveNewSettings(filename);
                System.IO.FileStream file = System.IO.File.Create(filename);

                writer.Serialize(file, overview);
                file.Close();

                save = false;
            }
        }
        private void buttonCheck_Click(object sender, EventArgs e)
        {
            CheckBeam();
            playingFieldBox.Refresh();
            check = false;
        }

        public void CheckBeam()
        {
            beam.RemoveAll(x => x.x1 < 0 || x.x2 < 0 || x.y1 < 0 || x.y2 < 0);
            check = true;
            beamForBuild.Clear();
            int count = 0;
            for (int i = 0; i < beam.Count(); i++)
            {
                if (beam[i].x1 != beam[i].x2)
                    count++;
                if (beam[i].y1 != beam[i].y2 && beam[i].y2 != 0)
                {
                    if (beam.FindAll(x => x.x1 <= beam[i].x2 && x.x2 >= beam[i].x2 && x.y1 == beam[i].y2 && x.x1 != x.x2).Count() == 0)
                    {
                        MessageBox.Show("Обнаружена парящая вертикальная балка", "Проверка закончена", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error,
                                        MessageBoxDefaultButton.Button1,
                                        MessageBoxOptions.ServiceNotification);
                        wrongBeam = new Beam(beam[i].x1, beam[i].y1, beam[i].x2, beam[i].y2);
                        return;
                    }
                }
            }

            for (int i = 0; i < beam.Count(); i++)
            {
                if (beam[i].y1 == beam[i].y2) //если балка горизонтальная
                {
                    if (beam.FindAll(x => x.x2 == beam[i].x1 && x.x1 != x.x2 || x.x1 == beam[i].x2 && x.x1 != x.x2).Count() == 0)
                    {
                        MessageBox.Show("Обнаружена балка которая не имеет пары", "Проверка закончена", MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.ServiceNotification);
                        wrongBeam = new Beam(beam[i].x1, beam[i].y1, beam[i].x2, beam[i].y2);
                        check = false;
                        return;
                    }
                    else
                    {
                        check = true;
                    }
                    if (check)
                    {
                        if (beam.FindAll(x => x.x1 == x.x2 && x.x1 >= beam[i].x1 && x.x2 <= beam[i].x2).Count() == 0)
                        {
                            fly = true;
                            MessageBox.Show("Обнаружена парящая горизонтальная балка", "Проверка закончена", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.ServiceNotification);
                            wrongBeam = new Beam(beam[i].x1, beam[i].y1, beam[i].x2, beam[i].y2);
                            return;
                        }
                        else
                            fly = false;
                    }
                }
            }
            if (check)
            {
                MessageBox.Show("Потенциально верная постройка.", "Проверка закончена", MessageBoxButtons.OK,
                                MessageBoxIcon.Information,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.ServiceNotification);
                wrongBeam = null;
                return;
            }
            if (beam.Count() == 0)
            {
                check = true;
                if (!save)
                    MessageBox.Show("Строительная площадка пустая.", "Проверка закончена", MessageBoxButtons.OK,
                                        MessageBoxIcon.Information,
                                        MessageBoxDefaultButton.Button1,
                                        MessageBoxOptions.ServiceNotification);

            }
        }
        public void AddInTask()
        {
            beamForBuild.Clear();
            for (int i = 0; i < beam.Count(); i++)
            {
                beamForBuild.Add(new Beam(beam[i].x1, beam[i].y1, beam[i].x2, beam[i].y2));
            }
        }
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = directory;
            openFileDialog.Filter = "building task files (*.buildingtask)|*.buildingtask|All files (*.*)|*.*";
            openFileDialog.ShowDialog();
            string filename = openFileDialog.FileName;
            if (filename == "")
                return;
            directory = filename;
            saveNewSettings(filename);
            beam.Clear();
            draw = false;
            playingFieldBox.Invalidate();
            System.Xml.Serialization.XmlSerializer reader =
            new System.Xml.Serialization.XmlSerializer(typeof(List<Beam>));

            System.IO.StreamReader file = new System.IO.StreamReader(filename);
            beam = (List<Beam>)reader.Deserialize(file);
            numOfCellsX = Convert.ToInt32(Math.Ceiling(beam.Max(x => x.x2)/2));
            numOfCellsY = Convert.ToInt32(Math.Ceiling(beam.Max(x => x.y1))) + 1;
            file.Close();
            draw = true;
            clear = false;
            playingFieldBox.Invalidate();
        }

        private void playingFieldBox_MouseClick(object sender, MouseEventArgs e)
        {
            Point pos = e.Location;
            pos.Y = playingFieldBox.Size.Height - pos.Y - 40;
            int posX1 = -1, posY1 = -1;
            int posX2 = -1, posY2 = -1;

            for (int y = 0; y <= numOfCellsY; y++)
            {
                    if (pos.Y < y * cellSize - 5 && pos.Y > (y - 1) * cellSize + 5)
                    {
                        posY1 = y;
                        posY2 = y - 1;
                        break;
                    }
                    if (pos.Y < y * cellSize + 5 && pos.Y > y * cellSize - 5)
                    {
                        posY1 = y;
                        posY2 = y;
                        break;
                    }
            }

            for (int x = 0; x <= numOfCellsX * 2; x++)
            {
                    if (pos.X < x * cellSize / 2 - 5 && pos.X > (x - 1) * cellSize / 2 + 5)
                    {
                        posX1 = x - 1;
                        posX2 = x;
                        break;
                    }
                    if (pos.X < x * cellSize / 2 + 5 && pos.X > x * cellSize / 2 - 5)
                    {
                        posX1 = x;
                        posX2 = x;
                        break;
                    }
            }
            if ((posX1 == posX2 || posY1 == posY2) && (posX1 != posX2 || posY1 != posY2)) //в случае, если хоть одни из координат равны и наоборот
            {
                Beam b = new Beam(posX1, posY1, posX2, posY2); //добавляем её в список
                    if ((beam.FindAll(x => x.x1 == b.x1 && x.x2 == b.x2 && x.y1 == b.y1 && x.y2 == b.y2).Count == 0))
                        beam.Add(b);
                    else
                        beam.Remove(beam.Find(x => x.x1 == b.x1 && x.x2 == b.x2 && x.y1 == b.y1 && x.y2 == b.y2));
            }
            clear = false;
            draw = true;
            playingFieldBox.Invalidate();
        }

        private void сохранитьИзображениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = directory;
            saveFileDialog.Filter = "bitmap (*.bmp)|*.bmp|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = saveFileDialog.FileName;
                directory = filename;
                bmp.Save(filename);
                saveNewSettings(filename);
            }
        }

        private void очиститьПолеToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            beam.Clear();
            draw = false;
            clear = true;
            playingFieldBox.Invalidate();
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapSettings frm = new mapSettings(numOfCellsX, numOfCellsY);
            frm.Owner = this; //Передаём вновь созданной форме её владельца.
            frm.Show();
        }

        private void taskConstructor_FormClosing(object sender, FormClosingEventArgs e)
        {
            
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
            catch (Exception ex)
            { }
        }
    }
}
    
