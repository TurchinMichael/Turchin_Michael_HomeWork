using System.IO;
using System.Windows.Forms;

namespace MyGame
{
    # region 2.a Добавить ведение журнала в консоль с помощью делегатов;
    public delegate void Message();
    public delegate void EnergyMessage(int energy);
    //public delegate void JournalCollisionMessage(object sender, object receiver); // был создан обобщенный делегат
    #endregion

    /// <summary>
    /// Основная программа
    /// </summary>
    class Program
    {
        #region 2.b Добавить ведение журнала в файл
        public static StreamWriter streamWriter = new StreamWriter("test.txt");
        #endregion

        static void Main(string[] args)
        {
            Form form = new Form
            {
                Width = Screen.PrimaryScreen.Bounds.Width,
                Height = Screen.PrimaryScreen.Bounds.Height
            };

            //InitForm1(form);
            Game.Init(form);
            form.Show();
            Game.Draw();
            Application.Run(form);
            if (Program.streamWriter.BaseStream != null)
                Program.streamWriter?.Close();
        }
    }
}