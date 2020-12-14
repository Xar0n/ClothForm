
namespace ClothForm.Form
{
    partial class FMesh
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxSizeMesh = new System.Windows.Forms.TextBox();
            this.textBoxScaleMesh = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxBS = new System.Windows.Forms.TextBox();
            this.textBoxSS = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 50);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Размер";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(669, 50);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "Масштаб ";
            // 
            // textBoxSizeMesh
            // 
            this.textBoxSizeMesh.Location = new System.Drawing.Point(101, 47);
            this.textBoxSizeMesh.Name = "textBoxSizeMesh";
            this.textBoxSizeMesh.Size = new System.Drawing.Size(200, 29);
            this.textBoxSizeMesh.TabIndex = 2;
            this.textBoxSizeMesh.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxSizeMesh_KeyPress);
            // 
            // textBoxScaleMesh
            // 
            this.textBoxScaleMesh.Location = new System.Drawing.Point(774, 47);
            this.textBoxScaleMesh.Name = "textBoxScaleMesh";
            this.textBoxScaleMesh.Size = new System.Drawing.Size(172, 29);
            this.textBoxScaleMesh.TabIndex = 3;
            this.textBoxScaleMesh.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            this.textBoxScaleMesh.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxScaleMesh_KeyPress);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBoxSizeMesh);
            this.panel1.Controls.Add(this.textBoxScaleMesh);
            this.panel1.Location = new System.Drawing.Point(0, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(959, 112);
            this.panel1.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(391, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(168, 24);
            this.label3.TabIndex = 4;
            this.label3.Text = "Параметры сетки";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textBoxBS);
            this.panel2.Controls.Add(this.textBoxSS);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(0, 120);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(959, 139);
            this.panel2.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(375, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(285, 24);
            this.label4.TabIndex = 0;
            this.label4.Text = "Параметры жесткости пружин";
            // 
            // textBoxBS
            // 
            this.textBoxBS.Location = new System.Drawing.Point(760, 61);
            this.textBoxBS.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxBS.Name = "textBoxBS";
            this.textBoxBS.Size = new System.Drawing.Size(180, 29);
            this.textBoxBS.TabIndex = 8;
            this.textBoxBS.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxBS_KeyPress);
            // 
            // textBoxSS
            // 
            this.textBoxSS.Location = new System.Drawing.Point(147, 64);
            this.textBoxSS.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxSS.Name = "textBoxSS";
            this.textBoxSS.Size = new System.Drawing.Size(180, 29);
            this.textBoxSS.TabIndex = 7;
            this.textBoxSS.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxSS_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(684, 64);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 24);
            this.label5.TabIndex = 6;
            this.label5.Text = "Изгиб";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 66);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 24);
            this.label6.TabIndex = 5;
            this.label6.Text = "Растяжение";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(433, 265);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 41);
            this.button1.TabIndex = 6;
            this.button1.Text = "Создать";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FMesh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 318);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "FMesh";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Создание ткани";
            this.Load += new System.EventHandler(this.FMesh_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSizeMesh;
        private System.Windows.Forms.TextBox textBoxScaleMesh;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxBS;
        private System.Windows.Forms.TextBox textBoxSS;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
    }
}