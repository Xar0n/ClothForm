using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClothForm.Form
{

    public partial class FMesh : System.Windows.Forms.Form
    {
        public Cloth cloth;

        public FMesh()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            cloth = new Cloth(float.Parse(textBoxScaleMesh.Text), int.Parse(textBoxSizeMesh.Text), 
                float.Parse(textBoxSS.Text), float.Parse(textBoxBS.Text));
            this.Close();
        }

        private void checkDigitInt(System.Windows.Forms.KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) | e.KeyChar == '\b') return;
            else
                e.Handled = true;
        }

        private void checkDigitFloat(System.Windows.Forms.KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) | (e.KeyChar == Convert.ToChar(",")) | e.KeyChar == '\b') return;
            else
                e.Handled = true;
        }

        private void textBoxSizeMesh_KeyPress(object sender, KeyPressEventArgs e)
        {
            checkDigitInt(e);
        }

        private void textBoxScaleMesh_KeyPress(object sender, KeyPressEventArgs e)
        {
            checkDigitInt(e);
        }

        private void textBoxSS_KeyPress(object sender, KeyPressEventArgs e)
        {
            checkDigitFloat(e);
        }

        private void textBoxBS_KeyPress(object sender, KeyPressEventArgs e)
        {
            checkDigitFloat(e);
        }

        private void FMesh_Load(object sender, EventArgs e)
        {
            textBoxSizeMesh.Text = "13";
            textBoxScaleMesh.Text = "20";
            textBoxSS.Text = "2,5";
            textBoxBS.Text = "1,5";
        }
    }
}
