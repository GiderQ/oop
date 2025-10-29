using LabCalculator;

namespace CalculatorExcel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var res = Calculator.Evaluate(textBox1.Text);
            label1.Text = res.ToString();
        }

    }
}
