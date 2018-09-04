using System.Windows.Forms;

namespace Manipulation
{
    public class AntiFlickerForm : Form
    {
        public AntiFlickerForm()
        {
            //Включает механизм двойной буферизации вывода, который предотвращает мерцание формы при перерисовке
            DoubleBuffered = true;
        }
    }
}