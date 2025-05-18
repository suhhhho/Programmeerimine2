namespace KooliProjekt.WinFormsApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TodoListsGrid = new DataGridView();
            IdLabel = new Label();
            IdField = new TextBox();
            TitleLabel = new Label();
            TitleField = new TextBox();
            NewButton = new Button();
            SaveButton = new Button();
            DeleteButton = new Button();
            RatePerMinuteLabel = new Label();
            RatePerMinuteField = new NumericUpDown();
            RatePerKmLabel = new Label();
            RatePerKmField = new NumericUpDown();
            IsAvailableCheckBox = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)TodoListsGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RatePerMinuteField).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RatePerKmField).BeginInit();
            SuspendLayout();
            // 
            // TodoListsGrid
            // 
            TodoListsGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            TodoListsGrid.Location = new Point(5, 6);
            TodoListsGrid.MultiSelect = false;
            TodoListsGrid.Name = "TodoListsGrid";
            TodoListsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            TodoListsGrid.Size = new Size(419, 432);
            TodoListsGrid.TabIndex = 0;
            // 
            // IdLabel
            // 
            IdLabel.AutoSize = true;
            IdLabel.Location = new Point(460, 16);
            IdLabel.Name = "IdLabel";
            IdLabel.Size = new Size(21, 15);
            IdLabel.TabIndex = 1;
            IdLabel.Text = "ID:";
            // 
            // IdField
            // 
            IdField.Location = new Point(507, 13);
            IdField.Name = "IdField";
            IdField.ReadOnly = true;
            IdField.Size = new Size(281, 23);
            IdField.TabIndex = 2;
            // 
            // TitleLabel
            // 
            TitleLabel.AutoSize = true;
            TitleLabel.Location = new Point(460, 56);
            TitleLabel.Name = "TitleLabel";
            TitleLabel.Size = new Size(33, 15);
            TitleLabel.TabIndex = 3;
            TitleLabel.Text = "Title:";
            // 
            // TitleField
            // 
            TitleField.Location = new Point(507, 53);
            TitleField.Name = "TitleField";
            TitleField.Size = new Size(281, 23);
            TitleField.TabIndex = 4;
            // 
            // RatePerMinuteLabel
            // 
            RatePerMinuteLabel.AutoSize = true;
            RatePerMinuteLabel.Location = new Point(430, 92);
            RatePerMinuteLabel.Name = "RatePerMinuteLabel";
            RatePerMinuteLabel.Size = new Size(64, 15);
            RatePerMinuteLabel.TabIndex = 8;
            RatePerMinuteLabel.Text = "Rate/Min:";
            // 
            // RatePerMinuteField
            // 
            RatePerMinuteField.DecimalPlaces = 2;
            RatePerMinuteField.Increment = new decimal(new int[] { 10, 0, 0, 131072 });
            RatePerMinuteField.Location = new Point(507, 90);
            RatePerMinuteField.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            RatePerMinuteField.Name = "RatePerMinuteField";
            RatePerMinuteField.Size = new Size(120, 23);
            RatePerMinuteField.TabIndex = 9;
            // 
            // RatePerKmLabel
            // 
            RatePerKmLabel.AutoSize = true;
            RatePerKmLabel.Location = new Point(437, 131);
            RatePerKmLabel.Name = "RatePerKmLabel";
            RatePerKmLabel.Size = new Size(57, 15);
            RatePerKmLabel.TabIndex = 10;
            RatePerKmLabel.Text = "Rate/KM:";
            // 
            // RatePerKmField
            // 
            RatePerKmField.DecimalPlaces = 2;
            RatePerKmField.Increment = new decimal(new int[] { 10, 0, 0, 131072 });
            RatePerKmField.Location = new Point(507, 129);
            RatePerKmField.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            RatePerKmField.Name = "RatePerKmField";
            RatePerKmField.Size = new Size(120, 23);
            RatePerKmField.TabIndex = 11;
            // 
            // IsAvailableCheckBox
            // 
            IsAvailableCheckBox.AutoSize = true;
            IsAvailableCheckBox.Location = new Point(507, 168);
            IsAvailableCheckBox.Name = "IsAvailableCheckBox";
            IsAvailableCheckBox.Size = new Size(75, 19);
            IsAvailableCheckBox.TabIndex = 12;
            IsAvailableCheckBox.Text = "Available";
            IsAvailableCheckBox.UseVisualStyleBackColor = true;
            // 
            // NewButton
            // 
            NewButton.Location = new Point(522, 203);
            NewButton.Name = "NewButton";
            NewButton.Size = new Size(75, 23);
            NewButton.TabIndex = 5;
            NewButton.Text = "New";
            NewButton.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            SaveButton.Location = new Point(603, 203);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(75, 23);
            SaveButton.TabIndex = 6;
            SaveButton.Text = "Save";
            SaveButton.UseVisualStyleBackColor = true;
            // 
            // DeleteButton
            // 
            DeleteButton.Location = new Point(684, 203);
            DeleteButton.Name = "DeleteButton";
            DeleteButton.Size = new Size(75, 23);
            DeleteButton.TabIndex = 7;
            DeleteButton.Text = "Delete";
            DeleteButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(IsAvailableCheckBox);
            Controls.Add(RatePerKmField);
            Controls.Add(RatePerKmLabel);
            Controls.Add(RatePerMinuteField);
            Controls.Add(RatePerMinuteLabel);
            Controls.Add(DeleteButton);
            Controls.Add(SaveButton);
            Controls.Add(NewButton);
            Controls.Add(TitleField);
            Controls.Add(TitleLabel);
            Controls.Add(IdField);
            Controls.Add(IdLabel);
            Controls.Add(TodoListsGrid);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)TodoListsGrid).EndInit();
            ((System.ComponentModel.ISupportInitialize)RatePerMinuteField).EndInit();
            ((System.ComponentModel.ISupportInitialize)RatePerKmField).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView TodoListsGrid;
        private Label IdLabel;
        private TextBox IdField;
        private Label TitleLabel;
        private TextBox TitleField;
        private Button NewButton;
        private Button SaveButton;
        private Button DeleteButton;
        private Label RatePerMinuteLabel;
        private NumericUpDown RatePerMinuteField;
        private Label RatePerKmLabel;
        private NumericUpDown RatePerKmField;
        private CheckBox IsAvailableCheckBox;
    }
}