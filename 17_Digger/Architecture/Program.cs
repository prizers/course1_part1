using System;
using System.Windows.Forms;

namespace Digger
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Game.CreateMap();
            Application.Run(new DiggerWindow());
        }
    }
}