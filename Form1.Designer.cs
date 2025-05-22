namespace TTN_Auto
{
    partial class Form1
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
            this.TTN_data_grid = new System.Windows.Forms.DataGridView();
            this.TTN_accepting_title_label = new System.Windows.Forms.Label();
            this.TTN_accepting_start = new System.Windows.Forms.Button();
            this.TTN_accepting_stop = new System.Windows.Forms.Button();
            this.EXIT_button = new System.Windows.Forms.Button();
            this.STATUS_label = new System.Windows.Forms.Label();
            this.TTN_Table_switcher_1_rb = new System.Windows.Forms.RadioButton();
            this.TTN_Table_switcher_2_rb = new System.Windows.Forms.RadioButton();
            this.TTN_Table_switcher_3_rb = new System.Windows.Forms.RadioButton();
            this.GOST_key_title_lable = new System.Windows.Forms.Label();
            this.GOST_key_date_lable = new System.Windows.Forms.Label();
            this.FSRARID_label = new System.Windows.Forms.Label();
            this.FSRARID_title_label = new System.Windows.Forms.Label();
            this.FSRARID_get_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.TTN_data_grid)).BeginInit();
            this.SuspendLayout();
            // 
            // TTN_data_grid
            // 
            this.TTN_data_grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.TTN_data_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TTN_data_grid.GridColor = System.Drawing.SystemColors.Control;
            this.TTN_data_grid.Location = new System.Drawing.Point(11, 144);
            this.TTN_data_grid.Margin = new System.Windows.Forms.Padding(2);
            this.TTN_data_grid.Name = "TTN_data_grid";
            this.TTN_data_grid.RowHeadersVisible = false;
            this.TTN_data_grid.RowHeadersWidth = 51;
            this.TTN_data_grid.RowTemplate.Height = 24;
            this.TTN_data_grid.Size = new System.Drawing.Size(337, 162);
            this.TTN_data_grid.TabIndex = 0;
            // 
            // TTN_accepting_title_label
            // 
            this.TTN_accepting_title_label.AutoSize = true;
            this.TTN_accepting_title_label.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TTN_accepting_title_label.Location = new System.Drawing.Point(11, 88);
            this.TTN_accepting_title_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TTN_accepting_title_label.Name = "TTN_accepting_title_label";
            this.TTN_accepting_title_label.Size = new System.Drawing.Size(189, 14);
            this.TTN_accepting_title_label.TabIndex = 4;
            this.TTN_accepting_title_label.Text = "Автоподтверждение накладных";
            // 
            // TTN_accepting_start
            // 
            this.TTN_accepting_start.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TTN_accepting_start.Location = new System.Drawing.Point(11, 104);
            this.TTN_accepting_start.Margin = new System.Windows.Forms.Padding(2);
            this.TTN_accepting_start.Name = "TTN_accepting_start";
            this.TTN_accepting_start.Size = new System.Drawing.Size(86, 36);
            this.TTN_accepting_start.TabIndex = 5;
            this.TTN_accepting_start.Text = "Начать";
            this.TTN_accepting_start.UseVisualStyleBackColor = true;
            this.TTN_accepting_start.Click += new System.EventHandler(this.TTN_accepting_start_Click);
            // 
            // TTN_accepting_stop
            // 
            this.TTN_accepting_stop.Enabled = false;
            this.TTN_accepting_stop.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TTN_accepting_stop.Location = new System.Drawing.Point(114, 104);
            this.TTN_accepting_stop.Margin = new System.Windows.Forms.Padding(2);
            this.TTN_accepting_stop.Name = "TTN_accepting_stop";
            this.TTN_accepting_stop.Size = new System.Drawing.Size(86, 36);
            this.TTN_accepting_stop.TabIndex = 6;
            this.TTN_accepting_stop.Text = "Остановить";
            this.TTN_accepting_stop.UseVisualStyleBackColor = true;
            this.TTN_accepting_stop.Click += new System.EventHandler(this.TTN_accepting_stop_Click);
            // 
            // EXIT_button
            // 
            this.EXIT_button.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.EXIT_button.Location = new System.Drawing.Point(266, 336);
            this.EXIT_button.Margin = new System.Windows.Forms.Padding(2);
            this.EXIT_button.Name = "EXIT_button";
            this.EXIT_button.Size = new System.Drawing.Size(83, 40);
            this.EXIT_button.TabIndex = 7;
            this.EXIT_button.Text = "Выход";
            this.EXIT_button.UseVisualStyleBackColor = true;
            this.EXIT_button.Click += new System.EventHandler(this.EXIT_button_Click);
            // 
            // STATUS_label
            // 
            this.STATUS_label.AutoSize = true;
            this.STATUS_label.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.STATUS_label.Location = new System.Drawing.Point(11, 362);
            this.STATUS_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.STATUS_label.Name = "STATUS_label";
            this.STATUS_label.Size = new System.Drawing.Size(43, 14);
            this.STATUS_label.TabIndex = 10;
            this.STATUS_label.Text = "Статус";
            // 
            // TTN_Table_switcher_1_rb
            // 
            this.TTN_Table_switcher_1_rb.AutoSize = true;
            this.TTN_Table_switcher_1_rb.Checked = true;
            this.TTN_Table_switcher_1_rb.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.TTN_Table_switcher_1_rb.Location = new System.Drawing.Point(19, 311);
            this.TTN_Table_switcher_1_rb.Name = "TTN_Table_switcher_1_rb";
            this.TTN_Table_switcher_1_rb.Size = new System.Drawing.Size(45, 18);
            this.TTN_Table_switcher_1_rb.TabIndex = 11;
            this.TTN_Table_switcher_1_rb.TabStop = true;
            this.TTN_Table_switcher_1_rb.Text = "Все";
            this.TTN_Table_switcher_1_rb.UseVisualStyleBackColor = true;
            // 
            // TTN_Table_switcher_2_rb
            // 
            this.TTN_Table_switcher_2_rb.AutoSize = true;
            this.TTN_Table_switcher_2_rb.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.TTN_Table_switcher_2_rb.Location = new System.Drawing.Point(81, 311);
            this.TTN_Table_switcher_2_rb.Name = "TTN_Table_switcher_2_rb";
            this.TTN_Table_switcher_2_rb.Size = new System.Drawing.Size(94, 18);
            this.TTN_Table_switcher_2_rb.TabIndex = 12;
            this.TTN_Table_switcher_2_rb.Text = "Непринятые";
            this.TTN_Table_switcher_2_rb.UseVisualStyleBackColor = true;
            // 
            // TTN_Table_switcher_3_rb
            // 
            this.TTN_Table_switcher_3_rb.AutoSize = true;
            this.TTN_Table_switcher_3_rb.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.TTN_Table_switcher_3_rb.Location = new System.Drawing.Point(181, 311);
            this.TTN_Table_switcher_3_rb.Name = "TTN_Table_switcher_3_rb";
            this.TTN_Table_switcher_3_rb.Size = new System.Drawing.Size(81, 18);
            this.TTN_Table_switcher_3_rb.TabIndex = 13;
            this.TTN_Table_switcher_3_rb.Text = "Принятые";
            this.TTN_Table_switcher_3_rb.UseVisualStyleBackColor = true;
            // 
            // GOST_key_title_lable
            // 
            this.GOST_key_title_lable.AutoSize = true;
            this.GOST_key_title_lable.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GOST_key_title_lable.Location = new System.Drawing.Point(190, 9);
            this.GOST_key_title_lable.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.GOST_key_title_lable.Name = "GOST_key_title_lable";
            this.GOST_key_title_lable.Size = new System.Drawing.Size(130, 14);
            this.GOST_key_title_lable.TabIndex = 8;
            this.GOST_key_title_lable.Text = "Срок действия ключа:";
            // 
            // GOST_key_date_lable
            // 
            this.GOST_key_date_lable.AutoSize = true;
            this.GOST_key_date_lable.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GOST_key_date_lable.Location = new System.Drawing.Point(190, 23);
            this.GOST_key_date_lable.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.GOST_key_date_lable.Name = "GOST_key_date_lable";
            this.GOST_key_date_lable.Size = new System.Drawing.Size(72, 14);
            this.GOST_key_date_lable.TabIndex = 9;
            this.GOST_key_date_lable.Text = "неизвестен";
            // 
            // FSRARID_label
            // 
            this.FSRARID_label.AutoSize = true;
            this.FSRARID_label.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FSRARID_label.Location = new System.Drawing.Point(78, 9);
            this.FSRARID_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FSRARID_label.Name = "FSRARID_label";
            this.FSRARID_label.Size = new System.Drawing.Size(71, 14);
            this.FSRARID_label.TabIndex = 2;
            this.FSRARID_label.Text = "не получен";
            // 
            // FSRARID_title_label
            // 
            this.FSRARID_title_label.AutoSize = true;
            this.FSRARID_title_label.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FSRARID_title_label.Location = new System.Drawing.Point(16, 9);
            this.FSRARID_title_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FSRARID_title_label.Name = "FSRARID_title_label";
            this.FSRARID_title_label.Size = new System.Drawing.Size(58, 14);
            this.FSRARID_title_label.TabIndex = 1;
            this.FSRARID_title_label.Text = "FSRAR ID:";
            // 
            // FSRARID_get_button
            // 
            this.FSRARID_get_button.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FSRARID_get_button.Location = new System.Drawing.Point(11, 25);
            this.FSRARID_get_button.Margin = new System.Windows.Forms.Padding(2);
            this.FSRARID_get_button.Name = "FSRARID_get_button";
            this.FSRARID_get_button.Size = new System.Drawing.Size(118, 35);
            this.FSRARID_get_button.TabIndex = 3;
            this.FSRARID_get_button.Text = "Получить FSRAR ID";
            this.FSRARID_get_button.UseVisualStyleBackColor = true;
            this.FSRARID_get_button.Click += new System.EventHandler(this.FSRARID_get_button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(360, 387);
            this.Controls.Add(this.STATUS_label);
            this.Controls.Add(this.TTN_accepting_title_label);
            this.Controls.Add(this.EXIT_button);
            this.Controls.Add(this.FSRARID_get_button);
            this.Controls.Add(this.TTN_accepting_start);
            this.Controls.Add(this.TTN_accepting_stop);
            this.Controls.Add(this.FSRARID_title_label);
            this.Controls.Add(this.TTN_Table_switcher_3_rb);
            this.Controls.Add(this.FSRARID_label);
            this.Controls.Add(this.GOST_key_date_lable);
            this.Controls.Add(this.TTN_Table_switcher_2_rb);
            this.Controls.Add(this.GOST_key_title_lable);
            this.Controls.Add(this.TTN_Table_switcher_1_rb);
            this.Controls.Add(this.TTN_data_grid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximumSize = new System.Drawing.Size(380, 430);
            this.MinimumSize = new System.Drawing.Size(380, 430);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Подтверждение ТТН";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TTN_data_grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView TTN_data_grid;
        private System.Windows.Forms.Label TTN_accepting_title_label;
        private System.Windows.Forms.Button TTN_accepting_start;
        private System.Windows.Forms.Button TTN_accepting_stop;
        private System.Windows.Forms.Button EXIT_button;
        private System.Windows.Forms.Label STATUS_label;
        private System.Windows.Forms.RadioButton TTN_Table_switcher_1_rb;
        private System.Windows.Forms.RadioButton TTN_Table_switcher_2_rb;
        private System.Windows.Forms.RadioButton TTN_Table_switcher_3_rb;
        private System.Windows.Forms.Label GOST_key_title_lable;
        private System.Windows.Forms.Label GOST_key_date_lable;
        private System.Windows.Forms.Label FSRARID_label;
        private System.Windows.Forms.Label FSRARID_title_label;
        private System.Windows.Forms.Button FSRARID_get_button;
    }
}

