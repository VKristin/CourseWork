namespace builders
{
    partial class mainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьАлгоритмToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьАлгоритмToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.проверитьЗаданиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьЗаданиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.очиститьЗаданиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изменитьПолеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.распечататьАлгоритмToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выполнитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.шагToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.наНачалоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playingFieldBox = new System.Windows.Forms.PictureBox();
            this.labelSize = new System.Windows.Forms.Label();
            this.buttonGo = new System.Windows.Forms.Button();
            this.trackBarSpeed = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonStep = new System.Windows.Forms.Button();
            this.lbPath = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbPathTask = new System.Windows.Forms.TextBox();
            this.tbPathAlg = new System.Windows.Forms.TextBox();
            this.trackBarSize = new System.Windows.Forms.TrackBar();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.playingFieldBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpeed)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSize)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.SystemColors.Window;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.выполнитьToolStripMenuItem,
            this.шагToolStripMenuItem,
            this.наНачалоToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(741, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.загрузитьАлгоритмToolStripMenuItem,
            this.сохранитьАлгоритмToolStripMenuItem,
            this.проверитьЗаданиеToolStripMenuItem,
            this.загрузитьЗаданиеToolStripMenuItem,
            this.очиститьЗаданиеToolStripMenuItem,
            this.изменитьПолеToolStripMenuItem,
            this.распечататьАлгоритмToolStripMenuItem,
            this.выходToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // загрузитьАлгоритмToolStripMenuItem
            // 
            this.загрузитьАлгоритмToolStripMenuItem.Name = "загрузитьАлгоритмToolStripMenuItem";
            this.загрузитьАлгоритмToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.загрузитьАлгоритмToolStripMenuItem.Text = "Загрузить алгоритм";
            this.загрузитьАлгоритмToolStripMenuItem.Click += new System.EventHandler(this.загрузитьАлгоритмToolStripMenuItem_Click);
            // 
            // сохранитьАлгоритмToolStripMenuItem
            // 
            this.сохранитьАлгоритмToolStripMenuItem.Name = "сохранитьАлгоритмToolStripMenuItem";
            this.сохранитьАлгоритмToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.сохранитьАлгоритмToolStripMenuItem.Text = "Сохранить алгоритм";
            this.сохранитьАлгоритмToolStripMenuItem.Click += new System.EventHandler(this.сохранитьАлгоритмToolStripMenuItem_Click);
            // 
            // проверитьЗаданиеToolStripMenuItem
            // 
            this.проверитьЗаданиеToolStripMenuItem.Name = "проверитьЗаданиеToolStripMenuItem";
            this.проверитьЗаданиеToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.проверитьЗаданиеToolStripMenuItem.Text = "Проверить задание";
            this.проверитьЗаданиеToolStripMenuItem.Click += new System.EventHandler(this.проверитьЗаданиеToolStripMenuItem_Click);
            // 
            // загрузитьЗаданиеToolStripMenuItem
            // 
            this.загрузитьЗаданиеToolStripMenuItem.Name = "загрузитьЗаданиеToolStripMenuItem";
            this.загрузитьЗаданиеToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.загрузитьЗаданиеToolStripMenuItem.Text = "Загрузить задание";
            this.загрузитьЗаданиеToolStripMenuItem.Click += new System.EventHandler(this.загрузитьЗаданиеToolStripMenuItem_Click);
            // 
            // очиститьЗаданиеToolStripMenuItem
            // 
            this.очиститьЗаданиеToolStripMenuItem.Name = "очиститьЗаданиеToolStripMenuItem";
            this.очиститьЗаданиеToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.очиститьЗаданиеToolStripMenuItem.Text = "Очистить задание";
            this.очиститьЗаданиеToolStripMenuItem.Click += new System.EventHandler(this.очиститьЗаданиеToolStripMenuItem_Click);
            // 
            // изменитьПолеToolStripMenuItem
            // 
            this.изменитьПолеToolStripMenuItem.Name = "изменитьПолеToolStripMenuItem";
            this.изменитьПолеToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.изменитьПолеToolStripMenuItem.Text = "Изменить поле";
            this.изменитьПолеToolStripMenuItem.Click += new System.EventHandler(this.изменитьПолеToolStripMenuItem_Click);
            // 
            // распечататьАлгоритмToolStripMenuItem
            // 
            this.распечататьАлгоритмToolStripMenuItem.Name = "распечататьАлгоритмToolStripMenuItem";
            this.распечататьАлгоритмToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.распечататьАлгоритмToolStripMenuItem.Text = "Сохранить текст алгоритма";
            this.распечататьАлгоритмToolStripMenuItem.Click += new System.EventHandler(this.распечататьАлгоритмToolStripMenuItem_Click);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
            // 
            // выполнитьToolStripMenuItem
            // 
            this.выполнитьToolStripMenuItem.Name = "выполнитьToolStripMenuItem";
            this.выполнитьToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.выполнитьToolStripMenuItem.Text = "Выполнить";
            this.выполнитьToolStripMenuItem.Click += new System.EventHandler(this.выполнитьToolStripMenuItem_Click);
            // 
            // шагToolStripMenuItem
            // 
            this.шагToolStripMenuItem.Name = "шагToolStripMenuItem";
            this.шагToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.шагToolStripMenuItem.Text = "Шаг";
            this.шагToolStripMenuItem.Click += new System.EventHandler(this.шагToolStripMenuItem_Click);
            // 
            // наНачалоToolStripMenuItem
            // 
            this.наНачалоToolStripMenuItem.Name = "наНачалоToolStripMenuItem";
            this.наНачалоToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.наНачалоToolStripMenuItem.Text = "На начало";
            this.наНачалоToolStripMenuItem.Click += new System.EventHandler(this.наНачалоToolStripMenuItem_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.справкаToolStripMenuItem.Text = "Справка";
            this.справкаToolStripMenuItem.Click += new System.EventHandler(this.справкаToolStripMenuItem_Click);
            // 
            // playingFieldBox
            // 
            this.playingFieldBox.Location = new System.Drawing.Point(0, 210);
            this.playingFieldBox.Name = "playingFieldBox";
            this.playingFieldBox.Size = new System.Drawing.Size(589, 227);
            this.playingFieldBox.TabIndex = 2;
            this.playingFieldBox.TabStop = false;
            this.playingFieldBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // labelSize
            // 
            this.labelSize.AutoSize = true;
            this.labelSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelSize.Location = new System.Drawing.Point(94, 151);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(145, 15);
            this.labelSize.TabIndex = 4;
            this.labelSize.Text = "Размер стройплощадки";
            // 
            // buttonGo
            // 
            this.buttonGo.BackColor = System.Drawing.Color.Azure;
            this.buttonGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonGo.Location = new System.Drawing.Point(12, 36);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(177, 25);
            this.buttonGo.TabIndex = 5;
            this.buttonGo.Text = "Выполнить алгоритм";
            this.buttonGo.UseVisualStyleBackColor = false;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // trackBarSpeed
            // 
            this.trackBarSpeed.AutoSize = false;
            this.trackBarSpeed.LargeChange = 10;
            this.trackBarSpeed.Location = new System.Drawing.Point(323, 169);
            this.trackBarSpeed.Maximum = 2000;
            this.trackBarSpeed.Minimum = 10;
            this.trackBarSpeed.Name = "trackBarSpeed";
            this.trackBarSpeed.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.trackBarSpeed.Size = new System.Drawing.Size(276, 21);
            this.trackBarSpeed.SmallChange = 10;
            this.trackBarSpeed.TabIndex = 6;
            this.trackBarSpeed.TabStop = false;
            this.trackBarSpeed.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarSpeed.Value = 500;
            this.trackBarSpeed.Scroll += new System.EventHandler(this.trackBarSpeed_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(401, 151);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Скорость анимации";
            // 
            // buttonStep
            // 
            this.buttonStep.BackColor = System.Drawing.Color.Azure;
            this.buttonStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonStep.Location = new System.Drawing.Point(237, 36);
            this.buttonStep.Name = "buttonStep";
            this.buttonStep.Size = new System.Drawing.Size(177, 25);
            this.buttonStep.TabIndex = 8;
            this.buttonStep.Text = "Выполнить шаг";
            this.buttonStep.UseVisualStyleBackColor = false;
            this.buttonStep.Click += new System.EventHandler(this.buttonStep_Click);
            // 
            // lbPath
            // 
            this.lbPath.AutoSize = true;
            this.lbPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbPath.Location = new System.Drawing.Point(6, 11);
            this.lbPath.Name = "lbPath";
            this.lbPath.Size = new System.Drawing.Size(147, 15);
            this.lbPath.TabIndex = 9;
            this.lbPath.Text = "Загруженный алгоритм:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(16, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "Загруженное задание:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbPath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbPathTask);
            this.groupBox1.Controls.Add(this.tbPathAlg);
            this.groupBox1.Location = new System.Drawing.Point(12, 67);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(705, 69);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // tbPathTask
            // 
            this.tbPathTask.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbPathTask.Location = new System.Drawing.Point(162, 39);
            this.tbPathTask.Name = "tbPathTask";
            this.tbPathTask.ReadOnly = true;
            this.tbPathTask.Size = new System.Drawing.Size(537, 21);
            this.tbPathTask.TabIndex = 12;
            this.tbPathTask.Text = "Не загружено";
            // 
            // tbPathAlg
            // 
            this.tbPathAlg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbPathAlg.Location = new System.Drawing.Point(162, 11);
            this.tbPathAlg.Name = "tbPathAlg";
            this.tbPathAlg.ReadOnly = true;
            this.tbPathAlg.Size = new System.Drawing.Size(537, 21);
            this.tbPathAlg.TabIndex = 0;
            this.tbPathAlg.Text = "Не загружен";
            // 
            // trackBarSize
            // 
            this.trackBarSize.AutoSize = false;
            this.trackBarSize.LargeChange = 8;
            this.trackBarSize.Location = new System.Drawing.Point(12, 169);
            this.trackBarSize.Maximum = 512;
            this.trackBarSize.Minimum = 30;
            this.trackBarSize.Name = "trackBarSize";
            this.trackBarSize.Size = new System.Drawing.Size(277, 21);
            this.trackBarSize.SmallChange = 8;
            this.trackBarSize.TabIndex = 3;
            this.trackBarSize.TabStop = false;
            this.trackBarSize.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarSize.Value = 48;
            this.trackBarSize.Scroll += new System.EventHandler(this.trackBarSize_Scroll);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(741, 469);
            this.Controls.Add(this.buttonStep);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonGo);
            this.Controls.Add(this.labelSize);
            this.Controls.Add(this.playingFieldBox);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.trackBarSize);
            this.Controls.Add(this.trackBarSpeed);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "mainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Стройплощадка";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.playingFieldBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpeed)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.PictureBox playingFieldBox;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.TrackBar trackBarSpeed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonStep;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьАлгоритмToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьАлгоритмToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem проверитьЗаданиеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьЗаданиеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem очиститьЗаданиеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem изменитьПолеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem наНачалоToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выполнитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem шагToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.Label lbPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbPathAlg;
        private System.Windows.Forms.TextBox tbPathTask;
        private System.Windows.Forms.ToolStripMenuItem распечататьАлгоритмToolStripMenuItem;
        private System.Windows.Forms.TrackBar trackBarSize;
    }
}

