using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using static System.Console;

namespace MyGame
{
    /// <summary>
    /// Основная программа
    /// </summary>
    class Program
    {
        public static Button bOne, bTwo, bThree;
        public static Label text;
        static void Main(string[] args)
        {
            Form form = new Form
            {
                Width = Screen.PrimaryScreen.Bounds.Width,
                Height = Screen.PrimaryScreen.Bounds.Height
            };
            ReadKey();
            //InitForm1(form);
            Game.Init(form);
            form.Show();
            Game.Draw();
            Application.Run(form);
        }

        #region SPLASH
        /// <summary>
        /// Инициализация объектов для формы Splash Screen приложения
        /// </summary>
        /// <param name="form"></param>
        static void InitForm2(Form form)
        {
            text = new Label();
            text.Parent = form;
            text.Left = 20;
            text.Top = 500;
            text.Text = "Турчин Михаил";
            text.Height = 100;
            text.Width = 300;
            text.BackColor = Color.Orange;
            text.Font = new System.Drawing.Font("Tobota", 25, System.Drawing.FontStyle.Italic);

            // start
            bOne = new Button();
            bOne.Parent = form;
            bOne.Left = 20;
            bOne.Top = 40;
            bOne.Text = "Start Game";
            bOne.Click += new EventHandler(bOne_Click);
            void bOne_Click(object sender, EventArgs e)
            {
                MessageBox.Show("Start");
            }

            // records
            bTwo = new Button();
            bTwo.Parent = form;
            bTwo.Left = 20;
            bTwo.Top = 70;
            bTwo.Text = "Records";
            bTwo.Click += new EventHandler(bTwo_Click);
            void bTwo_Click(object sender, EventArgs e)
            {
                MessageBox.Show("Records");
            }

            // quit
            bThree = new Button();
            bThree.Parent = form;
            bThree.Left = 20;
            bThree.Top = 100;
            bThree.Text = "Quit";
            bThree.Click += new EventHandler(bThree_Click);
            void bThree_Click(object sender, EventArgs e)
            {
                Application.Exit();
            }
        }
        #endregion
    }
}