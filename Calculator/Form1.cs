using NCalc;
using System;
using System.Windows.Forms;

namespace Calculator
{
    public partial class MainForm : Form
    {
        bool pressAmount = false;
        double result = 0.0;
        

        public MainForm()
        {
            InitializeComponent();
        }
     
        private void btnNum_Click(object sender, EventArgs e)
        { 
            try
            {
                //介面輸入數字
                if (sender is Button)
                {
                    Button btnNum = (Button)sender;
                    txtInput.Text += btnNum.Text;
                }
                //鍵盤輸入數字
                if (sender is MainForm)
                {
                    KeyEventArgs keyCode = (KeyEventArgs)e;
                    string keyNum = keyCode.KeyCode.ToString();
                    if (keyNum == "Decimal")
                        keyNum = ".";
                    txtInput.Text += keyNum.Substring(keyNum.Length - 1, 1);   
                }
                    
                checkStartNotZero();
            }
            catch (Exception ex) {

                Console.WriteLine("btnNum_Click_" + ex.Message);
            }
           
        }

        private void checkStartNotZero()
        {
            if (txtInput.Text.StartsWith("00"))
                txtInput.Text = "0";
           
            if (txtInput.Text.StartsWith("0") && !txtInput.Text.Contains(".") && txtInput.Text.Length >= 2)
                txtInput.Text = txtInput.Text.Remove(0, 1);
        }

        private void btnAC_Click(object sender, EventArgs e)
        {
            txtInput.Text = "0";
            txtResult.Text = "";
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (Double.Parse(txtInput.Text) != 0.0)
            {
                if (txtInput.Text.Length == 1)
                    txtInput.Text = "0";
                else
                    txtInput.Text = txtInput.Text.Remove(txtInput.Text.Length - 1, 1);
            }
        }

        private void btnCalculator_Click(object sender, EventArgs e)
        {
            string strCal = "";

            //介面輸入運算子
            if (sender is Button)
            {
                Button btnCal = (Button)sender;
                strCal = btnCal.Text;
            }
            //鍵盤輸入運算子
            if (sender is MainForm)
            {
                KeyEventArgs keyCode = (KeyEventArgs)e;
                string keyNum = keyCode.KeyCode.ToString();
                switch (keyNum)
                {
                    case "Add":
                        strCal = "+";
                        break;
                    case "Subtract":
                        strCal = "-";
                        break;
                    case "Multiply":
                        strCal = "*";
                        break;
                    case "Divide":
                        strCal = "/";
                        break;
                }
            }

            if (pressAmount)
                txtResult.Text = txtInput.Text + strCal;
            else
                txtResult.Text += txtInput.Text + strCal;

                txtInput.Text = "";
                pressAmount = false;
        }

        private void btnAmont_Click(object sender, EventArgs e)
        {
            pressAmount = true;
            string strOperation = txtResult.Text + txtInput.Text;
            Expression ep = new Expression(strOperation);
            result = Convert.ToDouble(ep.Evaluate());
         
            txtResult.Text = strOperation;
            txtInput.Text = result.ToString();
            //去除小數點後為0的數字
            if (result.ToString().Contains(".") && !(double.Parse(result.ToString().Split(".")[1]) > 0))
                txtInput.Text = result.ToString().Split(".")[0];
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D0:
                case Keys.NumPad0:
                case Keys.D1:
                case Keys.NumPad1:
                case Keys.D2:
                case Keys.NumPad2:
                case Keys.D3:
                case Keys.NumPad3:
                case Keys.D4:
                case Keys.NumPad4:
                case Keys.D5:
                case Keys.NumPad5:
                case Keys.D6:
                case Keys.NumPad6:
                case Keys.D7:
                case Keys.NumPad7:
                case Keys.D8:
                case Keys.NumPad8:
                case Keys.D9:
                case Keys.NumPad9:
                case Keys.Decimal:
                    btnNum_Click(sender, e);
                    break;
                case Keys.Add:
                case Keys.Subtract:
                case Keys.Multiply:
                case Keys.Divide:
                    btnCalculator_Click(sender, e);
                    break;
                case Keys.Back:
                    btnBack_Click(sender, e);
                    break;
                case Keys.Oemplus:
                case Keys.Enter:
                    btnAmont_Click(sender, e);
                    break;
                case Keys.Delete:
                case Keys.Escape:
                    btnAC_Click(sender, e);
                    break;
                default: break;
            }
        }

        //針對鍵盤輸入enter, 特殊處理
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            KeyEventArgs e = new KeyEventArgs(keyData);
            if (keyData == Keys.Enter)
            {
                Form1_KeyDown(btnAmont, e);
                return true;
            }
            //要調用keydown, 一定要返回false
            return false;

        }
    }
}
