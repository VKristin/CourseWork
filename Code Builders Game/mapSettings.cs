using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace builders
{
    public partial class mapSettings : Form
    {
        mainForm frm;
        public mapSettings(int nCellsX, int numCellsY)
        {
            InitializeComponent();
            numericUpDownX.Value = nCellsX - 1;
            numericUpDownY.Value = numCellsY;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            frm = (mainForm)this.Owner;

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show("Применить изменения?", "", buttons);
            if (result == System.Windows.Forms.DialogResult.No)
            {
                this.Close();
                return;
            }

            frm.gridSize((int)numericUpDownX.Value, (int)numericUpDownY.Value);
            this.Close();
        }
        
        private void buttonHelp_Click(object sender, EventArgs e) 
        {
            MessageBox.Show("Минимальное значение параметров: 1" + "\n" + "Максимальное значение параметров: 20");
        }
    }
}
