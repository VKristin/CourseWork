using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace builders
{
    public partial class infoForm : Form
    {
        public infoForm()
        {
            InitializeComponent();
        }

        private void infoForm_Load(object sender, EventArgs e)
        {
            try
            {
                string filename = Directory.GetCurrentDirectory() + "\\Справка по игре Стройка.txt";
                StreamReader file = new StreamReader(filename, Encoding.GetEncoding(1251));
                string line = file.ReadLine();
                label1.Text = line;
                while (line != null)
                {
                    line = file.ReadLine();
                    label1.Text += "\n" + line;
                }
                file.Close();
            }
            catch
            {
                label1.Text = "";
            }
        }
    }
}
