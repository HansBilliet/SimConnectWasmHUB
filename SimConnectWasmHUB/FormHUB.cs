using System;
using System.Windows.Forms;

namespace SimConnectWasmHUB
{
    public partial class FormHUB : Form
    {
        SimConnectHUB _SimConnectHUB = new SimConnectHUB();

        public FormHUB()
        {
            InitializeComponent();

            _SimConnectHUB.LogResult += OnAddResult;
            _SimConnectHUB.ExeResult += OnExeResult;
        }

        protected override void WndProc(ref Message m)
        {
            _SimConnectHUB?.HandleWndProc(ref m);

            base.WndProc(ref m);
        }

        private void FormHUB_Shown(object sender, EventArgs e)
        {
            _SimConnectHUB.SetHandle(this.Handle);

            comboBoxVariables.Items.Add("A:AUTOPILOT ALTITUDE LOCK VAR:3,feet,FLOAT64");
            comboBoxVariables.Items.Add("A:KOHLSMAN SETTING HG:1,inHg,FLOAT64");
            comboBoxVariables.Items.Add("A:LIGHT POTENTIOMETER:84,percent,INT32");
            comboBoxVariables.Items.Add("L:A32NX_EFIS_L_OPTION,enum");
            comboBoxVariables.Items.Add("L:A32NX_EFIS_R_OPTION,enum");
            comboBoxVariables.Items.Add("K:FUELSYSTEM_PUMP_TOGGLE");
            comboBoxVariables.Items.Add("K:A32NX.FCU_HDG_INC");

            if (comboBoxVariables.Items.Count > 0)
                comboBoxVariables.SelectedIndex = 0;
        }

        private void OnAddResult(object sender, string sResult)
        {
            if (textResult.Text != "")
                textResult.AppendText(Environment.NewLine);
            textResult.AppendText(sResult);
        }

        private void OnExeResult(object sender, SimConnectHUB.Result ExeResult)
        {
            textBoxExecCalcCodeFloat.Text = ExeResult.exeF.ToString("0.000");
            textBoxExecCalcCodeInt.Text = ExeResult.exeI.ToString();
            textBoxExecCalcCodeString.Text = ExeResult.exeS;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            _SimConnectHUB?.Connect();
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            _SimConnectHUB?.Disconnect();
        }

        private void buttonAddVariable_Click(object sender, EventArgs e)
        {
            string sVar = comboBoxVariables.Text;

            if (sVar == "")
                return;

            if ((bool)_SimConnectHUB?.AddVariable(sVar))
            {
                if (!comboBoxVariables.Items.Contains(sVar))
                    comboBoxVariables.Items.Add(sVar);
            }
        }

        private void buttonRemoveVariable_Click(object sender, EventArgs e)
        {
            string sVar = comboBoxVariables.Text;

            if (sVar == "")
                return;

            if((bool)_SimConnectHUB?.RemoveVariable(sVar))
            {
                if (comboBoxVariables.Items.Contains(sVar))
                {
                    comboBoxVariables.Items.Remove(sVar);
                    if (comboBoxVariables.Items.Count > 0)
                        comboBoxVariables.SelectedIndex = 0;
                    else
                        comboBoxVariables.Text = "";
                }
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textResult.Text = "";
        }

        private void buttonSetValue_Click(object sender, EventArgs e)
        {
            string sVar = comboBoxVariables.Text;
            string sVal = textBoxValue.Text;

            if (sVar == "")
                return;

            _SimConnectHUB?.SetVariable(sVar, sVal);
        }

        private void buttonExecCalcCodeSend_Click(object sender, EventArgs e)
        {
            string sExe = textBoxExecCalcCode.Text;

            if (sExe == "")
                return;

            _SimConnectHUB?.ExecuteCalculatorCode(sExe);
        }
    }
}
