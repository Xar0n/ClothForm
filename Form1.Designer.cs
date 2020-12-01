
namespace ClothForm
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
            this.components = new System.ComponentModel.Container();
            this.glControl1 = new OpenTK.GLControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.пункт1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.создатьТканьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отчиститьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.пункт2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.начатьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.перезапуститьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.видToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.текстураToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сеткаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(0, 27);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(893, 534);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = true;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            this.glControl1.Click += new System.EventHandler(this.glControl1_Click);
            this.glControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.glControl1_KeyDown);
            this.glControl1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.glControl1_KeyUp);
            this.glControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseDown);
            this.glControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseMove);
            this.glControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseUp);
            this.glControl1.Resize += new System.EventHandler(this.glControl1_Resize);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.пункт1ToolStripMenuItem,
            this.пункт2ToolStripMenuItem,
            this.видToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(893, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // пункт1ToolStripMenuItem
            // 
            this.пункт1ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.создатьТканьToolStripMenuItem,
            this.отчиститьToolStripMenuItem});
            this.пункт1ToolStripMenuItem.Name = "пункт1ToolStripMenuItem";
            this.пункт1ToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.пункт1ToolStripMenuItem.Text = "Система";
            // 
            // создатьТканьToolStripMenuItem
            // 
            this.создатьТканьToolStripMenuItem.Name = "создатьТканьToolStripMenuItem";
            this.создатьТканьToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.создатьТканьToolStripMenuItem.Text = "Создать ткань";
            this.создатьТканьToolStripMenuItem.Click += new System.EventHandler(this.создатьТканьToolStripMenuItem_Click);
            // 
            // отчиститьToolStripMenuItem
            // 
            this.отчиститьToolStripMenuItem.Name = "отчиститьToolStripMenuItem";
            this.отчиститьToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.отчиститьToolStripMenuItem.Text = "Отчистить";
            this.отчиститьToolStripMenuItem.Click += new System.EventHandler(this.отчиститьToolStripMenuItem_Click);
            // 
            // пункт2ToolStripMenuItem
            // 
            this.пункт2ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.начатьToolStripMenuItem,
            this.перезапуститьToolStripMenuItem});
            this.пункт2ToolStripMenuItem.Name = "пункт2ToolStripMenuItem";
            this.пункт2ToolStripMenuItem.Size = new System.Drawing.Size(82, 20);
            this.пункт2ToolStripMenuItem.Text = "Симуляция";
            this.пункт2ToolStripMenuItem.Click += new System.EventHandler(this.пункт2ToolStripMenuItem_Click);
            // 
            // начатьToolStripMenuItem
            // 
            this.начатьToolStripMenuItem.Name = "начатьToolStripMenuItem";
            this.начатьToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.начатьToolStripMenuItem.Text = "Запустить";
            this.начатьToolStripMenuItem.Click += new System.EventHandler(this.начатьToolStripMenuItem_Click);
            // 
            // перезапуститьToolStripMenuItem
            // 
            this.перезапуститьToolStripMenuItem.Name = "перезапуститьToolStripMenuItem";
            this.перезапуститьToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.перезапуститьToolStripMenuItem.Text = "Перезапустить";
            this.перезапуститьToolStripMenuItem.Click += new System.EventHandler(this.перезапуститьToolStripMenuItem_Click);
            // 
            // видToolStripMenuItem
            // 
            this.видToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.текстураToolStripMenuItem,
            this.сеткаToolStripMenuItem});
            this.видToolStripMenuItem.Name = "видToolStripMenuItem";
            this.видToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.видToolStripMenuItem.Text = "Вид";
            // 
            // текстураToolStripMenuItem
            // 
            this.текстураToolStripMenuItem.Name = "текстураToolStripMenuItem";
            this.текстураToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.текстураToolStripMenuItem.Text = "Текстура";
            // 
            // сеткаToolStripMenuItem
            // 
            this.сеткаToolStripMenuItem.Name = "сеткаToolStripMenuItem";
            this.сеткаToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.сеткаToolStripMenuItem.Text = "Сетка";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 619);
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Симулирование динамики пружинной системы для моделирования ткани";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem пункт1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem пункт2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem создатьТканьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отчиститьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem начатьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem перезапуститьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem видToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem текстураToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сеткаToolStripMenuItem;
    }
}

