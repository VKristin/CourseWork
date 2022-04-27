using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskConstructor
{
    public partial class mapSettings : Form
    {
        public mapSettings(int numCellsX, int numCellsY)
        {
            InitializeComponent();
            numericUpDownX.Value = numCellsX - 1;
            numericUpDownY.Value = numCellsY;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            taskConstructor frm = (taskConstructor)this.Owner;

            frm.gridSize((int)numericUpDownX.Value, (int)numericUpDownY.Value);
            this.Close();
        }
    }
}
