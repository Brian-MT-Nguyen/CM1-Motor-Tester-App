using SimpleTCP;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CM1_Motor_Test_Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SimpleTcpClient client;
        bool positionChanged = false;
        string currentPosition = "none";
        List<string> commandList = new List<string>();
        int commandCounter = 1;

        private void btnConnect_Click(object sender, EventArgs e)
        {
            txtStatus.Text += "Connecting to Motor...\r\n";
            client.Connect(txtHost.Text, Convert.ToInt32(txtPort.Text));
            btnConnect.Enabled = false;
            btnMSConnect.Enabled = false;
            btnSPConnect.Enabled = false;
            txtStatus.Text += "Connected.\r\n";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new SimpleTcpClient();
            client.StringEncoder = Encoding.UTF8;
            client.DataReceived += Client_DataReceived;
            cmdHistoryCB.SelectedIndex = 0;
        }

        private void Client_DataReceived(object sender, SimpleTCP.Message e)
        {
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                txtStatus.Text += e.MessageString;
            });
        }

        private void btnEnableMotor_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                client.Write("(.1\r");
                commandList.Add("(.1");
                txtBoxCmdHistory.Text += commandCounter.ToString() + ": " + "Enabled Motor\r\n";
                commandCounter++;
            }
        }

        private void btnDisableMotor_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                client.Write(").1\r");
                commandList.Add(").1");
                txtBoxCmdHistory.Text += commandCounter.ToString() + ": " + "Disabled Motor\r\n";
                commandCounter++;
            }
        }

        private void btnHomeMotor_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                client.Write("|.1\r");
                commandList.Add("|.1");
                txtBoxCmdHistory.Text += commandCounter.ToString() + ": " + "Homing Motor\r\n";
                commandCounter++;
            }
        }

        private void btnOrigin_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                client.Write("|2.1\r");
                commandList.Add("|2.1");
                txtBoxCmdHistory.Text += commandCounter.ToString() + ": " + "Set Origin of Motor\r\n";
                commandCounter++;
            }
        }

        private void btnReadK_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                client.Write("?90.1\r");
                commandList.Add("?90.1");
                txtBoxCmdHistory.Text += commandCounter.ToString() + ": " + "Read K Parameters Motor\r\n";
                commandCounter++;
            }
        }

        private void btnReadHome_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                client.Write("?70.1\r");
                commandList.Add("?70.1");
                txtBoxCmdHistory.Text += commandCounter.ToString() + ": " + "Read Home Sensor of Motor\r\n";
                commandCounter++;
            }
        }

        private void btnCPos_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                client.Write("?96.1\r");
                commandList.Add("?96.1");
                txtBoxCmdHistory.Text += commandCounter.ToString() + ": " + "Read Current Position of Motor\r\n";
                commandCounter++;
            }
        }

        private void btnCSpeed_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                client.Write("?97.1\r");
                commandList.Add("?97.1");
                txtBoxCmdHistory.Text += commandCounter.ToString() + ": " + "Read Current Speed of Motor\r\n";
                commandCounter++;
            }
        }

        private void btnChangeMP_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                client.Write("S.1=" + txtSpeed.Text + ", A.1=" + txtAccel.Text + ", K44.1=" + txtDeccel.Text + "\r");
                commandList.Add("S.1=" + txtSpeed.Text + ", A.1=" + txtAccel.Text + ", K44.1=" + txtDeccel.Text);
                txtBoxCmdHistory.Text += commandCounter.ToString() + ": " + "Changed Motor Motion Parameters\r\n";
                commandCounter++;
            }
        }

        private void btnChangePos_Click(object sender, EventArgs e)
        {
            if ((currentPosition != txtPosition.Text) && !(String.IsNullOrEmpty(txtPosition.Text)) && (btnConnect.Enabled == false))
            {
                client.Write("P.1=" + txtPosition.Text + "\r");
                positionChanged = true;
                currentPosition = txtPosition.Text;
                commandList.Add("P.1=" + txtPosition.Text);
                txtBoxCmdHistory.Text += commandCounter.ToString() + ": " + "Changed Position Motor will Move to\r\n";
                commandCounter++;
            }
        }

        private void btnMovePos_Click(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(txtPosition.Text)) && (btnConnect.Enabled == false))
            {

                if (positionChanged == true)
                {
                    client.Write("^.1\r");
                    commandList.Add("^.1");
                    positionChanged = false;
                    txtBoxCmdHistory.Text += commandCounter.ToString() + ": " + "Moved Motor to Position\r\n";
                    commandCounter++;
                }
                else
                {
                    if (currentPosition != txtPosition.Text)
                    {
                        client.Write("P.1=" + txtPosition.Text + ", ^.1\r");
                        currentPosition = txtPosition.Text;
                        commandList.Add("P.1=" + txtPosition.Text + ", ^.1");
                        txtBoxCmdHistory.Text += commandCounter.ToString() + ": " + "Moved Motor to Position\r\n";
                        commandCounter++;
                    }
                }
            }
        }

        private void btnKOptical_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                client.Write("K27.1=0200, K46.1=2, K37.1=6, K42.1=50, K45.1=0, K48.1=-2, K55.1=10\r");
                commandList.Add("K27.1=0200, K46.1=2, K37.1=6, K42.1=50, K45.1=0, K48.1=-2, K55.1=10");
                txtBoxCmdHistory.Text += commandCounter.ToString() + ": " + "Set K Parameters to Optical Home Sensor\r\n";
                commandCounter++;
            }
        }

        private void btnKHardStop_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                client.Write("K27.1=0200, K46.1=0, K47.1=10, K37.1=5, K42.1=10, K45.1=0, K48.1=0, K55.1=10\r");
                commandList.Add("K27.1=0200, K46.1=0, K47.1=10, K37.1=5, K42.1=10, K45.1=0, K48.1=0, K55.1=10");
                txtBoxCmdHistory.Text += commandCounter.ToString() + ": " + "Set K Parameters to Hard Stop as Home\r\n";
                commandCounter++;
            }
        }

        private void btnSetK_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                client.Write("K" + txtKNumber.Text + ".1=" + txtKValue.Text + "\r");
                commandList.Add("K" + txtKNumber.Text + ".1=" + txtKValue.Text);
                txtBoxCmdHistory.Text += commandCounter.ToString() + ": " + "Set K" + txtKNumber.Text + " to " + txtKValue.Text + "\r\n";
                commandCounter++;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                client.Write(txtMessage.Text + "\r");
                commandList.Add(txtMessage.Text);
                txtBoxCmdHistory.Text += commandCounter.ToString() + ": " + "Manually Sent Command to Motor\r\n";
                commandCounter++;
                if (txtMessage.Text.Contains("P.1="))
                {
                    currentPosition = "brian";
                    positionChanged = false;
                }
            }
        }

        private void btnSaveCmd_Click(object sender, EventArgs e)
        {
            string fullPath = "";
            bool dialogOK = false;
            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                fullPath = @"" + saveFileDialog.FileName;
                saveFileDialog.FileName = "";
                dialogOK = true;
            }

            if(cmdHistoryCB.Text == "Last" && (dialogOK == true))
            {
                string[] commandArray = commandList.GetRange(commandList.Count - Convert.ToInt32(txtCmdHistory.Text), Convert.ToInt32(txtCmdHistory.Text)).ToArray();
                File.WriteAllLines(fullPath, commandArray);
            }
            else if(cmdHistoryCB.Text == "First" && (dialogOK == true))
            {
                string[] commandArray = commandList.GetRange(0, Convert.ToInt32(txtCmdHistory.Text)).ToArray();
            }
            else if (cmdHistoryCB.Text == "All" && (dialogOK == true))
            {
                string[] commandArray = commandList.ToArray();
                File.WriteAllLines(fullPath, commandArray);
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                txtStatus.Clear();
            }
        }

        private void btnMSConnect_Click(object sender, EventArgs e)
        {
            txtStatus.Text += "Connecting to Motor...\r\n";
            client.Connect(txtHost.Text, Convert.ToInt32(txtPort.Text));
            btnConnect.Enabled = false;
            btnMSConnect.Enabled = false;
            btnSPConnect.Enabled = false;
            txtStatus.Text += "Connected.\r\n";
        }

        private void btnMSEnable_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                client.Write("(.1\r");
            }
        }

        private void btnMSDisable_Click(object sender, EventArgs e)
        {
            if(btnConnect.Enabled == false)
            {
                client.Write(").1\r");
            }
        }

        private void btnMSHome_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                client.Write("S.1=" + txtMSSpeed.Text + ", A.1=" + txtMSAccel.Text + ", K44.1=" + txtMSDeccel.Text + "\r");
                client.Write("|.1\r");
            }
        }

        private void btnMSOrigin_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                client.Write("|2.1\r");
            }
        }

        private void btnMSMove_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                client.Write("S.1=" + txtMSSpeed.Text + ", A.1=" + txtMSAccel.Text + ", K44.1=" + txtMSDeccel.Text + "\r");
                client.Write("P.1=" + txtMSPositionT.Text + ", ^.1\r");
            }
        }
        bool started = false;
        bool paused = false;
        private void btnMSStart_Click(object sender, EventArgs e)
        {
            if(btnConnect.Enabled == false)
            {
                client.Write("P1.1=" + txtMSPosition1.Text + "\r");
                client.Write("P2.1=" + txtMSPosition2.Text + "\r");
                client.Write("S1.1=" + txtMSSpeed.Text + "\r");
                client.Write("A1.1=" + txtMSAccel.Text + "\r");
                client.Write("K44.1=" + txtMSDeccel.Text + "\r");
                client.Write("T1.1=" + txtMSPosDelay.Text + "\r");
                client.Write("T2.1=" + txtMSRptDelay.Text + "\r");
                client.Write("T3.1=" + txtMSCntDelay.Text + "\r");

                string command = "B1.1";
                for(int i = 0; i < Convert.ToInt32(txtMSCount.Text); i++)
                {
                    command += "\rX" + txtMSRepeat.Text + ".1\rA1.1,S1.1,P1.1";
                    if(Convert.ToInt32(txtMSPosDelay.Text) > 0)
                    {
                        command += ",T1.1";
                    }
                    command += "\rP2.1";
                    if(Convert.ToInt32(txtMSRptDelay.Text) > 0)
                    {
                        command += ", T2.1";
                    }
                    command += "\rX-";
                    if(Convert.ToInt32(txtMSCntDelay.Text) > 0)
                    {
                        command += "\rT3.1";
                    }
                }
                command += "\rEND.1\r";
                client.Write(command);
                client.Write("], ]\r[1.1\r");
                started = true;
            }
        }

        private void btnMSPause_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false && started == true)
            {
                client.Write("]1.1\r");
                paused = true;
            }
        }

        private void btnMSContinue_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false && paused == true)
            {
                client.Write("[1.1\r");
                paused = false;
            }
        }

        private void btnMSAbort_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                client.Write("], ]\r");
                started = false;
            }
        }

        private void btnMSSave_Click(object sender, EventArgs e)
        {
            string fullPath = "";
            bool dialogOK = false;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                fullPath = @"" + saveFileDialog.FileName;
                saveFileDialog.FileName = "";
                dialogOK = true;
            }
            if (dialogOK == true)
            {
                string parameters = txtMSPosition1.Text + "\r\n" + txtMSPosDelay.Text + "\r\n" + txtMSPosition2.Text + "\r\n" + txtMSRepeat.Text + "\r\n" + txtMSRptDelay.Text + "\r\n" + txtMSCount.Text + "\r\n" + txtMSCntDelay.Text + "\r\n";
                File.WriteAllText(fullPath, parameters);
            }
        }

        string[] msParameters = new string[7];
        private void btnMSLoad_Click(object sender, EventArgs e)
        {
            string fullPath = "";
            bool dialogOK = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fullPath = @"" + openFileDialog.FileName;
                openFileDialog.FileName = "";
                dialogOK = true;
            }
            if (dialogOK == true)
            {
                int index = 0;
                foreach (string line in File.ReadLines(fullPath))
                {
                    msParameters[index] = line;
                    index += 1;
                }
                index = 0;
                txtMSPosition1.Text = msParameters[0];
                txtMSPosDelay.Text = msParameters[1];
                txtMSPosition2.Text = msParameters[2];
                txtMSRepeat.Text = msParameters[3];
                txtMSRptDelay.Text = msParameters[4];
                txtMSCount.Text = msParameters[5];
                txtMSCntDelay.Text = msParameters[6];
            }
        }

        private void btnMSDPP_Click(object sender, EventArgs e)
        {
            if (msParameters[0] == null)
            {
                txtMSPosition1.Text = "0";
                txtMSPosDelay.Text = "1000";
                txtMSPosition2.Text = "5000";
                txtMSRepeat.Text = "5";
                txtMSRptDelay.Text = "2000";
                txtMSCount.Text = "3";
                txtMSCntDelay.Text = "4000";
            }
            else
            {
                txtMSPosition1.Text = msParameters[0];
                txtMSPosDelay.Text = msParameters[1];
                txtMSPosition2.Text = msParameters[2];
                txtMSRepeat.Text = msParameters[3];
                txtMSRptDelay.Text = msParameters[4];
                txtMSCount.Text = msParameters[5];
                txtMSCntDelay.Text = msParameters[6];
            }
        }

        private void btnSPConnect_Click(object sender, EventArgs e)
        {
            txtStatus.Text += "Connecting to Motor...\r\n";
            client.Connect(txtHost.Text, Convert.ToInt32(txtPort.Text));
            btnConnect.Enabled = false;
            btnMSConnect.Enabled = false;
            btnSPConnect.Enabled = false;
            txtStatus.Text += "Connected.\r\n";
        }

        private void btnSPKOptical_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                client.Write("K27.1=0200, K46.1=2, K37.1=6, K42.1=50, K45.1=0, K48.1=-2, K55.1=10\r");
                commandList.Add("K27.1=0200, K46.1=2, K37.1=6, K42.1=50, K45.1=0, K48.1=-2, K55.1=10");
            }
        }

        private void btnSPHard_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                client.Write("K27.1=0200, K46.1=0, K47.1=10, K37.1=5, K42.1=10, K45.1=0, K48.1=0, K55.1=10\r");
                commandList.Add("K27.1=0200, K46.1=0, K47.1=10, K37.1=5, K42.1=10, K45.1=0, K48.1=0, K55.1=10");
            }
        }

        private void btnSPSet_Click(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                client.Write("K" + txtSPKNumber.Text + ".1=" + txtSPKValue.Text + "\r");
                commandList.Add("K" + txtSPKNumber.Text + ".1=" + txtSPKValue.Text);
            }
        }
    }
}
