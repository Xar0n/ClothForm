using System;
using System.Windows.Forms;
using OpenTK;
namespace ClothForm
{
    public partial class FGravity : Form
    {
        public FGravity()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            checkDigit(e);
            
        }

        private void textBox2_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            checkDigit(e);
        }

        private void textBox3_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            checkDigit(e);
        }

        private void FGravity_Load(object sender, EventArgs e)
        {
            textBox1.Text = FMain.cloth.gravity.X.ToString();
            textBox2.Text = FMain.cloth.gravity.Y.ToString();
            textBox3.Text = FMain.cloth.gravity.Z.ToString();
        }

        private void checkDigit(System.Windows.Forms.KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) | (e.KeyChar == Convert.ToChar(",")) | e.KeyChar == '\b') return;
            else
                e.Handled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FMain.cloth.gravity = new Vector3();
        }
    }
}
