using System;
using System.Windows.Forms;

namespace ClothForm
{
    public partial class FSprings : System.Windows.Forms.Form
    {
        public FSprings()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkDigit(System.Windows.Forms.KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) | (e.KeyChar == Convert.ToChar(",")) | e.KeyChar == '\b') return;
            else
                e.Handled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FMain.cloth.setStretchStiffness(float.Parse(textBox1.Text));
            FMain.cloth.setBendStiffness(float.Parse(textBox2.Text));
            FMain.cloth.reset();
            this.Close();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            checkDigit(e);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            checkDigit(e);
        }

        private void FSprings_Load(object sender, EventArgs e)
        {
            textBox1.Text = FMain.cloth.getStretchStiffness().ToString();
            textBox2.Text = FMain.cloth.getBendStiffness().ToString();
        }
    }
}
