using System;
using System.Windows.Forms;
using System.Drawing;

namespace MyGame
{
    class Program
    {
        public static Button bOne, bTwo, bThree;
        public static Label text;
        static void Main(string[] args)
        {
            Form form = new Form();
            form.Width = 800;
            form.Height = 600;

            if (MessageBox.Show("Yes = Splash | No = Asteroids", "Start", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                /*
                 * Разработать собственный класс-заставку SplashScreen, 
                 * аналогичный классу Game. Создать в нем собственную иерархию объектов и задать их движение. 
                 * Предусмотреть кнопки «Начало игры», «Рекорды», «Выход». Добавить на заставку имя автора.
                 */
                InitForm2(form);
                splash.Init(form);
                form.Show();
                splash.Draw();
                Application.Run(form);
            }
            else
            {
                InitForm1(form);
                Game.Init(form);
                form.Show();
                Game.Draw();
                Application.Run(form);
            }
        }

        #region SPLASH
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

        #region GAME
        static void InitForm1(Form form)
        {
            bOne = new Button();
            bOne.Parent = form;
            bOne.Left = 20;
            bOne.Top = 40;
            bOne.Text = "star image";
            bOne.Click += new EventHandler(bOne_Click);
            void bOne_Click(object sender, EventArgs e)
            {
                BaseObject.Change();//NewBase.Change();
            }
        }
        #endregion
    }
}