using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System;
using System.Linq;


namespace SPACEINVADERS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int riktning = 0;
        PictureBox bullet = new PictureBox();
        List<PictureBox> enemyBullets = new List<PictureBox>();
        Random random = new Random();
        int enemyShootTimer = 0;
        int enemyShootInterval = 60; //"enemy" shoot interval

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Moving mechanism
            pictureBox1.Left += riktning * 10;

            // user bullet movement and collision detection
            if (bullet != null && this.Controls.Contains(bullet))
            {
                bullet.Top -= 15;
                if (bullet.Top < 0)
                {
                    this.Controls.Remove(bullet);
                    bullet.Dispose();
                }
                else
                {
                    foreach (Control ctrl in this.Controls.OfType<PictureBox>().ToList())
                    {
                        if (ctrl == bullet || ctrl == pictureBox1) continue;
                        if (enemyBullets.Contains((PictureBox)ctrl)) continue;
                        if (ctrl.Tag?.ToString() != "enemy") continue;
                        if (bullet.Bounds.IntersectsWith(ctrl.Bounds))
                        {
                            this.Controls.Remove(ctrl);
                            ctrl.Dispose();
                            this.Controls.Remove(bullet);
                            bullet.Dispose();
                            break;
                        }
                    }
                }
            }

            // Enemy bullet movement and collision detection
            foreach (PictureBox eb in enemyBullets.ToList())
            {
                eb.Top += 8;

                // remove enemy bullet from the user (hinders it from passing through the user and hitting multiple times)
                if (eb.Bounds.IntersectsWith(pictureBox1.Bounds))
                {
                    timer1.Stop();

                    this.Controls.Remove(eb);
                    eb.Dispose();
                    enemyBullets.Remove(eb);

                    DialogResult result = MessageBox.Show(
                        "Du blev träffad! Vill du försöka igen?",
                        "GAME OVER",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes) Application.Restart();
                    else Application.Exit();
                    return;
                }

                // removes bullet that goes of "SCREEN" aka play area
                if (eb.Top > this.Height)
                {
                    this.Controls.Remove(eb);
                    eb.Dispose();
                    enemyBullets.Remove(eb);
                }
            }

            // random enemy shooting sequence
            enemyShootTimer++;
            if (enemyShootTimer >= enemyShootInterval)
            {
                enemyShootTimer = 0;
                enemyShootInterval = random.Next(30, 90); // random intervalls between shots

                List<PictureBox> activeEnemies = this.Controls
                    .OfType<PictureBox>()
                    .Where(p => p != pictureBox1 && p != bullet && !enemyBullets.Contains(p))
                    .ToList();

                if (activeEnemies.Count > 0)
                {
                    // chooses from 25 enemies and uses one random "alive one" to send out "ENEMY BULLETS" (red colour)
                    PictureBox shooter = activeEnemies[random.Next(activeEnemies.Count)];

                    PictureBox eb = new PictureBox();
                    eb.Size = new Size(5, 15);
                    eb.BackColor = Color.Red;
                    eb.Location = new Point(
                        shooter.Left + shooter.Width / 2,
                        shooter.Bottom + 2
                    );
                    this.Controls.Add(eb);
                    eb.BringToFront();
                    enemyBullets.Add(eb);
                }
            }

            gamewon();
        }

        private void form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) riktning = -1;
            else if (e.KeyCode == Keys.Right) riktning = 1;
            else if (e.KeyCode == Keys.Space) shoot();
        }

        private void form1_keyup(object sender, KeyEventArgs e)
        {
            riktning = 0;
        }
        bool gameStarted = false;

        private void button1_Click(object sender, EventArgs e)
        {
            if (gameStarted) return; 
            gameStarted = true;

            button1.Visible = false;
            timer1.Start();

            for (int i = 0; i < 25; i++)
            {
                PictureBox enemy = new PictureBox();
                enemy.Image = Properties.Resources.CocaColaLogo;
                enemy.Size = new Size(40, 30);
                enemy.SizeMode = PictureBoxSizeMode.StretchImage;
                enemy.Tag = "enemy"; 

                int col = i % 5;
                int row = i / 5;
                enemy.Location = new Point(col * 80 + 40, row * 30 + 40);
                this.Controls.Add(enemy);
            }
        }

        private void shoot()
        {
            // shooting, (built to only shoot one at a time)
            if (bullet != null && this.Controls.Contains(bullet)) return;
            bullet = new PictureBox();
            bullet.Size = new Size(5, 15);
            bullet.BackColor = Color.Blue;
            bullet.Location = new Point(
                pictureBox1.Left + pictureBox1.Width / 2,
                pictureBox1.Top - 20
            );
            this.Controls.Add(bullet);
            bullet.BringToFront();
        }
        private void gamewon()
        {
            bool enemiesLeft = this.Controls
                .OfType<PictureBox>()
                .Any(p => p.Tag?.ToString() == "enemy");

            if (!enemiesLeft)
            {
                timer1.Stop();

                DialogResult result = MessageBox.Show(
                    "Du vann! Vill du fortsätta spela?",
                    "Grattis!",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                   
                    foreach (PictureBox eb in enemyBullets.ToList())
                    {
                        this.Controls.Remove(eb);
                        eb.Dispose();
                    }
                    enemyBullets.Clear();

                    for (int i = 0; i < 25; i++)
                    {
                        PictureBox enemy = new PictureBox();
                        enemy.Image = Properties.Resources.CocaColaLogo;
                        enemy.Size = new Size(40, 30);
                        enemy.SizeMode = PictureBoxSizeMode.StretchImage;
                        enemy.Tag = "enemy";

                        int col = i % 5;
                        int row = i / 5;
                        enemy.Location = new Point(col * 80 + 40, row * 30 + 40);
                        this.Controls.Add(enemy);
                    }

                    enemyShootTimer = 0;
                    timer1.Start();
                }
                else
                {
                    Application.Exit();
                }
            }
        }
    }
    }


