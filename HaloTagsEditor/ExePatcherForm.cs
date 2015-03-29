using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace HaloTagsEditor
{
    public partial class ExePatcherForm : Form
    {
        Stream exeFile;
        string fileName;
        public ExePatcherForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            exeFile = null;
            OpenFileDialog openExeDialog = new OpenFileDialog();
            openExeDialog.Filter = "Exe Files (.exe)|*.exe";
            openExeDialog.FilterIndex = 1;
            if (openExeDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openExeDialog.FileName;
                try
                {
                    fileName = openExeDialog.FileName;
                    if ((exeFile = openExeDialog.OpenFile()) != null)
                    {
                        using (exeFile)
                        {
                            checkBox1.Enabled = true;
                            checkBox2.Enabled = true;
                            checkBox3.Enabled = true;
                            checkBox5.Enabled = true;
                            button2.Enabled = true;
                            byte[] chkForceEnglish = new byte[1];
                            byte[] chkAllowMods = new byte[1];
                            byte[] chkDisableCountdown = new byte[1];
                            byte[] chkDisableAccount = new byte[2];
                            exeFile.Position = 0x2327FD;
                            exeFile.Read(chkForceEnglish, 0, 1);
                            exeFile.Position = 0x100E5B;
                            exeFile.Read(chkAllowMods, 0, 1);
                            exeFile.Position = 0x3117AB;
                            exeFile.Read(chkDisableCountdown, 0, 1);
                            exeFile.Position = 0x43671A;
                            exeFile.Read(chkDisableAccount, 0, 2);
                            if(BitConverter.ToString(chkForceEnglish) == "00")
                            {
                                checkBox1.Checked = true;
                            }
                            if(BitConverter.ToString(chkAllowMods) == "EB")
                            {
                                checkBox2.Checked = true;
                            }
                            if(BitConverter.ToString(chkDisableAccount) == "EB-0E")
                            {
                                checkBox5.Checked = true;
                            }
                            if(BitConverter.ToString(chkDisableCountdown) == "EB")
                            {
                                checkBox3.Checked = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            exeFile = null;
            exeFile = File.OpenWrite(fileName);
            byte[] newForceEnglish = new byte[1];
            byte[] newAllowMods0 = new byte[1];
            byte[] newAllowMods1 = new byte[2];
            byte[] newDisableCountdown = new byte[1];
            byte[] newDisableAccount0 = new byte[2];
            byte[] newDisableAccount1 = new byte[2];
            // Set Force English
            exeFile.Position = 0x2327FD;
            if (checkBox1.Checked == true)
            {
                newForceEnglish = MainForm.StringToByteArray("00");
            }
            else
            {
                newForceEnglish = MainForm.StringToByteArray("0B");
            }
            exeFile.Write(newForceEnglish, 0, 1);
            // Set Allow Mods
            if (checkBox2.Checked == true)
            {
                newAllowMods0 = MainForm.StringToByteArray("EB");
                newAllowMods1 = MainForm.StringToByteArray("9090");
                exeFile.Position = 0x100E5B;
                exeFile.Write(newAllowMods0, 0, 1);
                exeFile.Position = 0x101C74;
                exeFile.Write(newAllowMods1, 0, 1);
            }
            else
            {
                newAllowMods0 = MainForm.StringToByteArray("75");
                newAllowMods1 = MainForm.StringToByteArray("753E");
                exeFile.Position = 0x100E5B;
                exeFile.Write(newAllowMods0, 0, 1);
                exeFile.Position = 0x101C74;
                exeFile.Write(newAllowMods1, 0, 1);
            }
            // Set Disable Countdown
            if (checkBox3.Checked == true)
            {
                newDisableCountdown = MainForm.StringToByteArray("EB");
                exeFile.Position = 0x3117AB;
                exeFile.Write(newDisableCountdown, 0, 1);
            }
            else
            {
                newDisableCountdown = MainForm.StringToByteArray("74");
                exeFile.Position = 0x3117AB;
                exeFile.Write(newDisableCountdown, 0, 1);
            }
            // Set Disable Account
            if (checkBox2.Checked == true)
            {
                newDisableAccount0 = MainForm.StringToByteArray("EB0E");
                newDisableAccount1 = MainForm.StringToByteArray("EB03");
                exeFile.Position = 0x43671A;
                exeFile.Write(newDisableAccount0, 0, 2);
                exeFile.Position = 0x4367AD;
                exeFile.Write(newDisableAccount1, 0, 2);
            }
            else
            {
                newAllowMods0 = MainForm.StringToByteArray("68A4");
                newAllowMods1 = MainForm.StringToByteArray("E81E");
                exeFile.Position = 0x43671A;
                exeFile.Write(newDisableAccount0, 0, 2);
                exeFile.Position = 0x4367AD;
                exeFile.Write(newDisableAccount1, 0, 2);
            }
            exeFile.Dispose();
            MessageBox.Show("Patch Completed!");
        }
    }
}
