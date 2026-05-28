using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace SPACEINVADERS
{


    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int riktning = 0;

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Left += riktning * 10;
        }
        private void form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) riktning = -1;
            else if (e.KeyCode == Keys.Right) riktning = 1;
        }

        private void form1_keyup(object sender, KeyEventArgs e)
        {
            riktning = 0;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            timer1.Start();


            PictureBox[] enemies = new PictureBox[25];
            for (int i = 0; i < 25; i++)
            {
                enemies[i] = new PictureBox();
                enemies[i].Image = Properties.Resources.CocaColaLogo;
                enemies[i].Size = new Size(40, 30);
                enemies[i].SizeMode = PictureBoxSizeMode.StretchImage;
                enemies[i].BackColor = Color.Transparent;

                // 5 columns, 5 rows
                int col = i % 5;
                int row = i / 5;

                enemies[i].Location = new Point(col * 80 + 40, row * 30 + 40);

                this.Controls.Add(enemies[i]);
            }

            Rectangle bullet = new Rectangle
            {
                Tag = "bullet",
                Height = 20,
                Width = 5,
                fill = Brushes.Red
                Stroke = Brushes.Red

            };
    }
}










}

