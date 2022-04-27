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
    public partial class procedure : Form
    {
        List<string> actions;
        public mainForm form;

        public procedure(List<string> _actions, mainForm main)
        {
            form = main;
            InitializeComponent();
            this.ControlBox = false;
            this.Left = 0;
            this.Top = form.Height;
            actions = _actions;
            InitializationDgvFirst();
        }

        public void InitializationDgvFirst()
        {
        }

        private char getEnglLetter(char rus)
        {
            char en = 'a';
            switch (rus)
            {
                case 'А': en = 'F'; break;
                case 'В': en = 'D'; break;
                case 'Г': en = 'U'; break;
                case 'Д': en = 'L'; break;
                case 'Е': en = 'T'; break;
                case 'З': en = 'P'; break;
                case 'И': en = 'B'; break;
                case 'Й': en = 'Q'; break;
                case 'К': en = 'R'; break;
                case 'Л': en = 'K'; break;
                case 'М': en = 'V'; break;
                case 'Н': en = 'Y'; break;
                case 'О': en = 'J'; break;
                case 'П': en = 'G'; break;
                case 'Р': en = 'H'; break;
                case 'С': en = 'C'; break;
                case 'Т': en = 'N'; break;
                case 'У': en = 'E'; break;
                case 'Ф': en = 'A'; break;
                case 'Ц': en = 'W'; break;
                case 'Ч': en = 'X'; break;
                case 'Ш': en = 'I'; break;
                case 'Щ': en = 'O'; break;
                case 'Ы': en = 'S'; break;
                case 'Ь': en = 'M'; break;
                case 'Я': en = 'Z'; break;
                default: en = rus; break;
            }
            return en;
        }

        private void dgvFirst_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                if ((string)dgvFirst.Rows[e.RowIndex].Cells[1].Value == actions[4] || (string)dgvFirst.Rows[e.RowIndex].Cells[1].Value == actions[6])
                {
                }
                else
                {
                    if ((string)dgvFirst.Rows[e.RowIndex].Cells[1].Value == actions[0] || (string)dgvFirst.Rows[e.RowIndex].Cells[1].Value == actions[1]
                        || (string)dgvFirst.Rows[e.RowIndex].Cells[1].Value == actions[2] || (string)dgvFirst.Rows[e.RowIndex].Cells[1].Value == actions[3]
                        || (string)dgvFirst.Rows[e.RowIndex].Cells[1].Value == actions[5])
                    {
                    }
                    dgvFirst.Rows[e.RowIndex].Cells[3].Value = "";
                    if ((string)dgvFirst.Rows[e.RowIndex].Cells[1].Value == actions[7])
                    {
                        dgvFirst.Rows[e.RowIndex].Cells[0].Value = "";
                        dgvFirst.Rows[e.RowIndex].Cells[2].Value = "";
                    }
                }
            }
            if (dgvFirst.Rows[e.RowIndex].Cells[2].Value != null && dgvFirst.Rows[e.RowIndex].Cells[2].Value.ToString().Length == 1 && dgvFirst.Rows[e.RowIndex].Cells[2].Value.ToString() != "" && (Convert.ToChar(dgvFirst.Rows[e.RowIndex].Cells[2].Value) >= 'a' && Convert.ToChar(dgvFirst.Rows[e.RowIndex].Cells[2].Value) <= 'z'
                || Convert.ToChar(dgvFirst.Rows[e.RowIndex].Cells[2].Value) >= 'а' && Convert.ToChar(dgvFirst.Rows[e.RowIndex].Cells[2].Value) <= 'я'))
            {
                dgvFirst.Rows[e.RowIndex].Cells[2].Value = Convert.ToChar(dgvFirst.Rows[e.RowIndex].Cells[2].Value).ToString().ToUpper();

            }
            if (dgvFirst.Rows[e.RowIndex].Cells[2].Value != null && dgvFirst.Rows[e.RowIndex].Cells[2].Value.ToString().Length == 1)
            {
                char letter = Convert.ToChar(dgvFirst.Rows[e.RowIndex].Cells[2].Value);
                letter = getEnglLetter(letter);
                dgvFirst.Rows[e.RowIndex].Cells[2].Value = letter;
            }

            if (dgvFirst.Rows[e.RowIndex].Cells[3].Value != null && dgvFirst.Rows[e.RowIndex].Cells[3].Value.ToString().Length == 1 && dgvFirst.Rows[e.RowIndex].Cells[3].Value.ToString() != "" && (Convert.ToChar(dgvFirst.Rows[e.RowIndex].Cells[3].Value) >= 'a' && Convert.ToChar(dgvFirst.Rows[e.RowIndex].Cells[2].Value) <= 'z'
                || Convert.ToChar(dgvFirst.Rows[e.RowIndex].Cells[3].Value) >= 'а' && Convert.ToChar(dgvFirst.Rows[e.RowIndex].Cells[2].Value) <= 'я'))
            {
                dgvFirst.Rows[e.RowIndex].Cells[3].Value = Convert.ToChar(dgvFirst.Rows[e.RowIndex].Cells[3].Value).ToString().ToUpper();

            }
            if (dgvFirst.Rows[e.RowIndex].Cells[3].Value != null && dgvFirst.Rows[e.RowIndex].Cells[3].Value.ToString().Length == 1)
            {
                char letter = Convert.ToChar(dgvFirst.Rows[e.RowIndex].Cells[3].Value);
                letter = getEnglLetter(letter);
                dgvFirst.Rows[e.RowIndex].Cells[3].Value = letter;
            }
            if (e.RowIndex >= 0)
            {
                form.step = -1;
                form.deleteColor();
            }
            /*if (dgvFirst.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText != "")
                dgvFirst.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "";*/
        }


        private void очиститьАлгоритмДляПервойБригадыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvFirst.Rows.Clear();
        }

        private void очиститьАлгоритмДляВторойБригадыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvSecond.Rows.Clear();
        }

        private void очиститьАлгоритмДляТретьейБригадыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvThird.Rows.Clear();
        }

        private void dgvFirst_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = this.dgvFirst.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                this.dgvFirst.Rows[index].HeaderCell.Value = indexStr;
        }

        private void dgvSecond_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = this.dgvSecond.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                this.dgvSecond.Rows[index].HeaderCell.Value = indexStr;
        }

        private void dgvThird_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = this.dgvThird.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                this.dgvThird.Rows[index].HeaderCell.Value = indexStr;
        }

        private void dgvFirst_MouseDown(object sender, MouseEventArgs e)
        {
            dgvFirst.DoDragDrop(this.dgvFirst, DragDropEffects.Move);
        }

        private void dgvSecond_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                if ((string)dgvSecond.Rows[e.RowIndex].Cells[1].Value == actions[4] || (string)dgvSecond.Rows[e.RowIndex].Cells[1].Value == actions[6])
                {
                }
                else
                {
                    if ((string)dgvSecond.Rows[e.RowIndex].Cells[1].Value == actions[0] || (string)dgvSecond.Rows[e.RowIndex].Cells[1].Value == actions[1]
                        || (string)dgvSecond.Rows[e.RowIndex].Cells[1].Value == actions[2] || (string)dgvSecond.Rows[e.RowIndex].Cells[1].Value == actions[3]
                        || (string)dgvSecond.Rows[e.RowIndex].Cells[1].Value == actions[5])
                    {
                    }
                    dgvSecond.Rows[e.RowIndex].Cells[3].Value = "";
                    if ((string)dgvSecond.Rows[e.RowIndex].Cells[1].Value == actions[7])
                    {
                        dgvSecond.Rows[e.RowIndex].Cells[0].Value = "";
                        dgvSecond.Rows[e.RowIndex].Cells[2].Value = "";
                    }
                }
            }
            if (dgvSecond.Rows[e.RowIndex].Cells[2].Value != null && dgvSecond.Rows[e.RowIndex].Cells[2].Value.ToString().Length == 1 && dgvSecond.Rows[e.RowIndex].Cells[2].Value.ToString() != ""
                && (Convert.ToChar(dgvSecond.Rows[e.RowIndex].Cells[2].Value) >= 'a' && Convert.ToChar(dgvSecond.Rows[e.RowIndex].Cells[2].Value) <= 'z' || Convert.ToChar(dgvSecond.Rows[e.RowIndex].Cells[2].Value) <= 'я' && Convert.ToChar(dgvSecond.Rows[e.RowIndex].Cells[2].Value) >= 'а'))
            {
                dgvSecond.Rows[e.RowIndex].Cells[2].Value = Convert.ToChar(dgvSecond.Rows[e.RowIndex].Cells[2].Value).ToString().ToUpper();

            }
            if (dgvSecond.Rows[e.RowIndex].Cells[2].Value != null && dgvSecond.Rows[e.RowIndex].Cells[2].Value.ToString().Length == 1)
            {
                char letter = Convert.ToChar(dgvSecond.Rows[e.RowIndex].Cells[2].Value);
                letter = getEnglLetter(letter);
                dgvSecond.Rows[e.RowIndex].Cells[2].Value = letter;
            }

            if (dgvSecond.Rows[e.RowIndex].Cells[3].Value != null && dgvSecond.Rows[e.RowIndex].Cells[3].Value.ToString().Length == 1 && dgvSecond.Rows[e.RowIndex].Cells[3].Value.ToString() != "" && (Convert.ToChar(dgvSecond.Rows[e.RowIndex].Cells[3].Value) >= 'a' && Convert.ToChar(dgvSecond.Rows[e.RowIndex].Cells[3].Value) <= 'z'
                || Convert.ToChar(dgvSecond.Rows[e.RowIndex].Cells[3].Value) >= 'а' && Convert.ToChar(dgvSecond.Rows[e.RowIndex].Cells[3].Value) <= 'я'))
            {
                dgvSecond.Rows[e.RowIndex].Cells[3].Value = Convert.ToChar(dgvSecond.Rows[e.RowIndex].Cells[3].Value).ToString().ToUpper();

            }
            if (dgvSecond.Rows[e.RowIndex].Cells[3].Value != null && dgvSecond.Rows[e.RowIndex].Cells[3].Value.ToString().Length == 1)
            {
                char letter = Convert.ToChar(dgvSecond.Rows[e.RowIndex].Cells[3].Value);
                letter = getEnglLetter(letter);
                dgvSecond.Rows[e.RowIndex].Cells[3].Value = letter;
            }
            if (e.RowIndex >= 0)
            {
                form.step = -1;
                form.deleteColor();
            }
            /*if (dgvSecond.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText != "")
                dgvSecond.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "";*/
        }

        private void dgvThird_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                if ((string)dgvThird.Rows[e.RowIndex].Cells[1].Value == actions[4] || (string)dgvThird.Rows[e.RowIndex].Cells[1].Value == actions[6])
                {
                    ;
                }
                else
                {
                    dgvThird.Rows[e.RowIndex].Cells[3].Value = "";
                    if ((string)dgvThird.Rows[e.RowIndex].Cells[1].Value == actions[7])
                    {
                        dgvThird.Rows[e.RowIndex].Cells[0].Value = "";
                        dgvThird.Rows[e.RowIndex].Cells[2].Value = "";
                    }
                }
            }
            if (dgvThird.Rows[e.RowIndex].Cells[2].Value != null && dgvThird.Rows[e.RowIndex].Cells[2].Value.ToString().Length == 1 && dgvThird.Rows[e.RowIndex].Cells[2].Value.ToString() != "" && (Convert.ToChar(dgvThird.Rows[e.RowIndex].Cells[2].Value) >= 'a' && Convert.ToChar(dgvThird.Rows[e.RowIndex].Cells[2].Value) <= 'z'
                || Convert.ToChar(dgvThird.Rows[e.RowIndex].Cells[2].Value) >= 'а' && Convert.ToChar(dgvThird.Rows[e.RowIndex].Cells[2].Value) <= 'я'))
            {
                dgvThird.Rows[e.RowIndex].Cells[2].Value = Convert.ToChar(dgvThird.Rows[e.RowIndex].Cells[2].Value).ToString().ToUpper();
            }
            if (dgvThird.Rows[e.RowIndex].Cells[2].Value != null && dgvThird.Rows[e.RowIndex].Cells[2].Value.ToString().Length == 1)
            {
                char letter = Convert.ToChar(dgvThird.Rows[e.RowIndex].Cells[2].Value);
                letter = getEnglLetter(letter);
                dgvThird.Rows[e.RowIndex].Cells[2].Value = letter;
            }
            if (dgvThird.Rows[e.RowIndex].Cells[3].Value != null && dgvThird.Rows[e.RowIndex].Cells[3].Value.ToString().Length == 1 && dgvThird.Rows[e.RowIndex].Cells[3].Value.ToString() != "" && (Convert.ToChar(dgvThird.Rows[e.RowIndex].Cells[3].Value) >= 'a' && Convert.ToChar(dgvThird.Rows[e.RowIndex].Cells[3].Value) <= 'z'
                || Convert.ToChar(dgvThird.Rows[e.RowIndex].Cells[3].Value) >= 'а' && Convert.ToChar(dgvThird.Rows[e.RowIndex].Cells[3].Value) <= 'я'))
            {
                dgvThird.Rows[e.RowIndex].Cells[3].Value = Convert.ToChar(dgvThird.Rows[e.RowIndex].Cells[3].Value).ToString().ToUpper();
            }
            if (dgvThird.Rows[e.RowIndex].Cells[3].Value != null && dgvThird.Rows[e.RowIndex].Cells[3].Value.ToString().Length == 1)
            {
                char letter = Convert.ToChar(dgvThird.Rows[e.RowIndex].Cells[3].Value);
                letter = getEnglLetter(letter);
                dgvThird.Rows[e.RowIndex].Cells[3].Value = letter;
            }
            if (e.RowIndex >= 0)
            {
                form.step = -1;
                form.deleteColor();
            }
        }

        private void dgvFirst_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (this.Visible)
                dgvFirst.Width = dgvFirst.Columns[0].Width + dgvFirst.Columns[1].Width + dgvFirst.Columns[2].Width + dgvFirst.Columns[3].Width + dgvFirst.RowHeadersWidth;
            groupBox1.Width = dgvFirst.Columns[0].Width + dgvFirst.Columns[1].Width + dgvFirst.Columns[2].Width + dgvFirst.Columns[3].Width + dgvFirst.RowHeadersWidth + 13;
            groupBox4.Width = dgvFirst.Width + dgvSecond.Width + dgvThird.Width+20;
        }

        private void dgvSecond_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (this.Visible)
                dgvSecond.Width = dgvSecond.Columns[0].Width + dgvSecond.Columns[1].Width + dgvSecond.Columns[2].Width + dgvSecond.Columns[3].Width + dgvSecond.RowHeadersWidth;
            groupBox2.Width = dgvSecond.Columns[0].Width + dgvSecond.Columns[1].Width + dgvSecond.Columns[2].Width + dgvSecond.Columns[3].Width + dgvSecond.RowHeadersWidth + 13;
            groupBox4.Width = dgvFirst.Width + dgvSecond.Width + dgvThird.Width + 20;
        }

        private void dgvThird_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (this.Visible)
                dgvThird.Width = dgvThird.Columns[0].Width + dgvThird.Columns[1].Width + dgvThird.Columns[2].Width + dgvThird.Columns[3].Width + dgvThird.RowHeadersWidth ;
            groupBox3.Width = dgvThird.Columns[0].Width + dgvThird.Columns[1].Width + dgvThird.Columns[2].Width + dgvThird.Columns[3].Width + dgvThird.RowHeadersWidth + 13;
            groupBox4.Width = dgvFirst.Width + dgvSecond.Width + dgvThird.Width + 20;
        }


        private void btnAddFirst_Click(object sender, EventArgs e)
        {
            dgvFirst.Rows.Add();
            int k = dgvFirst.SelectedCells[0].RowIndex;
            for (int i = dgvFirst.Rows.Count - 1; i > k; i--)
            {
                for (int j = 0; j < dgvFirst.Columns.Count; j++)
                    dgvFirst.Rows[i].Cells[j].Value = dgvFirst.Rows[i - 1].Cells[j].Value;
            }
            for (int j = 0; j < dgvFirst.Columns.Count; j++)
                dgvFirst.Rows[k].Cells[j].Value = "";
        }

        private void btnAddSecond_Click(object sender, EventArgs e)
        {
            dgvSecond.Rows.Add();
            int k = dgvSecond.SelectedCells[0].RowIndex;
            for (int i = dgvSecond.Rows.Count - 1; i > k; i--)
            {
                for (int j = 0; j < dgvSecond.Columns.Count; j++)
                    dgvSecond.Rows[i].Cells[j].Value = dgvSecond.Rows[i - 1].Cells[j].Value;
            }
            for (int j = 0; j < dgvSecond.Columns.Count; j++)
                dgvSecond.Rows[k].Cells[j].Value = "";
        }

        private void btnAddThird_Click(object sender, EventArgs e)
        {
            dgvThird.Rows.Add();
            int k = dgvThird.SelectedCells[0].RowIndex;
            for (int i = dgvThird.Rows.Count - 1; i > k; i--)
            {
                for (int j = 0; j < dgvThird.Columns.Count; j++)
                    dgvThird.Rows[i].Cells[j].Value = dgvThird.Rows[i - 1].Cells[j].Value;
            }
            for (int j = 0; j < dgvThird.Columns.Count; j++)
                dgvThird.Rows[k].Cells[j].Value = "";
        }

        private void btnDeleteFirst_Click(object sender, EventArgs e)
        {
            if (dgvFirst.SelectedCells.Count != 0 && dgvFirst.RowCount - 1 != dgvFirst.SelectedCells[0].RowIndex)
                dgvFirst.Rows.RemoveAt(dgvFirst.SelectedCells[0].RowIndex);
        }

        private void btnDeleteSecond_Click(object sender, EventArgs e)
        {
            if (dgvSecond.SelectedCells.Count != 0 && dgvSecond.RowCount - 1 != dgvSecond.SelectedCells[0].RowIndex)
                dgvSecond.Rows.RemoveAt(dgvSecond.SelectedCells[0].RowIndex);
        }

        private void btnDeleteThird_Click(object sender, EventArgs e)
        {
            if (dgvThird.SelectedCells.Count != 0 && dgvThird.RowCount - 1 != dgvThird.SelectedCells[0].RowIndex)
                dgvThird.Rows.RemoveAt(dgvThird.SelectedCells[0].RowIndex);
        }

        private void btnClearFirst_Click(object sender, EventArgs e)
        {
            dgvFirst.Rows.Clear();
        }

        private void btnClearSecond_Click(object sender, EventArgs e)
        {
            dgvSecond.Rows.Clear();
        }

        private void btnClearThird_Click(object sender, EventArgs e)
        {
            dgvThird.Rows.Clear();
        }

        private void procedure_Load(object sender, EventArgs e)
        {
            labelFirst.ForeColor = Color.Black;
            labelSecond.ForeColor = Color.CornflowerBlue;
            labelThird.ForeColor = Color.MediumSeaGreen;
            ToolTip toolTipAdd = new ToolTip();
            ToolTip toolTipDelete = new ToolTip();
            ToolTip toolTipClear = new ToolTip();
            toolTipAdd.SetToolTip(btnAddFirst, "Добавить строку");
            toolTipAdd.SetToolTip(btnAddSecond, "Добавить строку");
            toolTipAdd.SetToolTip(btnAddThird, "Добавить строку");
            toolTipDelete.SetToolTip(btnDeleteFirst, "Удалить строку");
            toolTipDelete.SetToolTip(btnDeleteSecond, "Удалить строку");
            toolTipDelete.SetToolTip(btnDeleteThird, "Удалить строку");
            toolTipClear.SetToolTip(btnClearFirst, "Очистить алгоритм");
            toolTipClear.SetToolTip(btnClearSecond, "Очистить алгоритм");
            toolTipClear.SetToolTip(btnClearThird, "Очистить алгоритм");
        }
    }

}

