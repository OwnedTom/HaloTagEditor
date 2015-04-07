using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HaloTagsEditor
{
    public partial class MainForm : Form
    {
        public Dictionary<string, string> weapons, projectiles;
        public Stream tagsFile;
        public string fileName;
        public MainForm()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tagsFile = null;
            checkBox1.Enabled = true;
            checkBox2.Enabled = true;
            comboBox1.Enabled = true;
            OpenFileDialog openTagsDialog = new OpenFileDialog();
            openTagsDialog.Filter = "Dat Files (.dat)|*.dat|All Files (*.*)|*.*";
            openTagsDialog.FilterIndex = 1;
            if(openTagsDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    fileName = openTagsDialog.FileName;
                    if ((tagsFile = openTagsDialog.OpenFile()) != null)
                    {
                        using (tagsFile)
                        {
                            byte[] startingWeapon = new byte[4];   
                            byte[] barrierCheck = new byte[4];
                            // Get Deathless Player Status
                            tagsFile.Position = 0x1F952F0;
                            if (tagsFile.ReadByte().ToString() == "131")
                            {
                                checkBox1.Checked = true;
                            }
                            else
                            {
                                checkBox1.Checked = false;
                            }
                            // Get Remove Barriers Status
                            tagsFile.Position = 0x3D4FA3C;
                            tagsFile.Read(barrierCheck, 0, 4);
                            string barrierHex= BitConverter.ToString(barrierCheck).Replace("-","");
                            if(barrierHex == "FFFFFFFF")
                            {
                                checkBox2.Checked = true;
                            }
                            else
                            {
                                checkBox2.Checked = false;
                            }
                            // Get Starting Weapon Status
                            tagsFile.Position = 0x1214880;
                            tagsFile.Read(startingWeapon, 0, 4);
                            string weapHex = BitConverter.ToString(startingWeapon).Replace("-","");
                            foreach (KeyValuePair<string, string> weap in weapons)
                            {
                                if(weap.Value == weapHex)
                                {
                                    for (int i = 0; i < comboBox1.Items.Count; i++)
                                    {
                                        if(weap.Key == comboBox1.GetItemText(comboBox1.Items[i]))
                                        {
                                            comboBox1.SelectedIndex = i;
                                        }
                                    }
                                }
                            }
                            tagsFile.Dispose();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MainMenuStrip = STEMenu;
            weapons = new Dictionary<string, string> {
                {"Spiker", "00150000"},
                {"Beam Rifle", "09150000"},
                {"Gravity Hammer", "0C150000"},
                {"Assault Rifle (Default)", "1e150000"},
                {"Assault Rifle (Red)", "81150000"},
                {"Assault Rifle (Yellow)", "82150000"},
                {"Assault Rifle (Green)", "83150000"},
                {"Assault Rifle (Gold)", "84150000"},
                {"SMG (Default)", "7D150000"},
                {"SMG (Red)", "8E150000"},
                {"SMG (Yellow)", "8C150000"},
                {"SMG (Green)", "8D150000"},
                {"SMG (Gold)", "8F150000"},
                {"Battle Rifle (Default)", "7C150000"},
                {"Battle Rifle (Red)", "86150000"},
                {"Battle Rifle (Yellow)", "89150000"},
                {"Battle Rifle (Green)", "88150000"},
                {"Battle Rifle (Blue)", "85150000"},
                {"Battle Rifle (Gold)", "87150000"},
                {"DMR (Default)", "80150000"},
                {"DMR (Red)", "8A150000"},
                {"DMR (Gold)", "8B150000"},
                {"DMR (Yellow)", "8C150000"},
                {"DMR (Green)", "8D150000"},
                {"Magnum (Default)", "7E150000"},
                {"Magnum (Red)", "93150000"},
                {"Magnum (Gold)", "94150000"},
                {"Carbine (Default)", "FE140000"},
                {"Carbine (Gold)", "91150000"},
                {"Mauler (Default)", "04150000"},
                {"Mauler (Gold)", "92150000"},
                {"Energy Sword", "9E150000"},
                {"Useless Energy Sword", "7F150000"},
                {"Plasma Rifle", "25150000"},
                {"Plasma Pistol", "F7140000"},
                {"Flag", "A2150000"},
                {"Skull", "A3150000"},
                {"Bomb", "A4150000"},
                {"Sniper Rifle", "B1150000"},
                {"Rocket Launcher", "B3150000"},
                {"Needler", "F8140000"},
                {"Fuel Rod", "F9140000"},
                {"Brute Shot", "FF140000"}
            };
            projectiles = new Dictionary<string, string> {
                {"Frag Grenade", "AD010000"},
                {"Plasma Grenade", "AD010000"},
                {"Spike Grenade", "AD010000"},
                {"Incidiary Grenade", "AD010000"},
                {"Spike Round (Spike Grenade)", "0000"},
                {"Rocket", "C9150000"},
                {"Missile Pod Rocket", "CA150000"},
                {"Wraith Shot", "CB150000"},
                {"Brute Shot Round", "CD150000"},
                {"Hornet Rocket", "CE150000"},
                {"Assault Rifle Round", "901B0000"},
                {"Battle Rifle Round", "881D0000"},
                {"Carbine Round", "BC150000"},
                {"Shoutgun Pellet", "001F0000"},
                {"Sniper Round", "921F0000"},
                {"Spiker Round", "58200000"},
                {"Mauler Round", "8F200000"},
                {"Needler Round", "EF200000"},
                {"Plasma Pistol", "5C220000"},
                {"Plasma Pistol (Charged)", "5E220000"},
            };
            /*foreach(KeyValuePair<string,string> proj in projectiles)
            {
                comboBox2.Items.Add(proj.Key);
            }*/
            foreach(KeyValuePair<string,string> weap in weapons)
            {
                comboBox1.Items.Add(weap.Key);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tagsFile = null;
            tagsFile = File.OpenWrite(fileName);
            byte[] deathlessPlayer = new byte[1];
            byte[] barrierCheck = new byte[4];
            byte[] startingWeap = new byte[4];
            // Set Deathless Player
            tagsFile.Position = 0x1F952F0;
            if (checkBox1.Checked == true)
            {
                deathlessPlayer = StringToByteArray("83");
            }
            else
            {
                deathlessPlayer = StringToByteArray("03");
            }
            tagsFile.Write(deathlessPlayer, 0, 1);
            // Set Remove Barriers
            
            if(checkBox2.Checked == true)
            {
                barrierCheck = StringToByteArray("FFFFFFFF");
                tagsFile.Position = 0x3D4FA3C;
                tagsFile.Write(barrierCheck, 0, 4);
                tagsFile.Position = 0x447006C;
                tagsFile.Write(barrierCheck, 0, 4);
                tagsFile.Position = 0x4C0BB2C;
                tagsFile.Write(barrierCheck, 0, 4);
                tagsFile.Position = 0x513C20C;
                tagsFile.Write(barrierCheck, 0, 4);
                tagsFile.Position = 0x556E8EC;
                tagsFile.Write(barrierCheck, 0, 4);
                tagsFile.Position = 0x59D288C;
                tagsFile.Write(barrierCheck, 0, 4);
            }
            tagsFile.Position = 0x1214880;
            foreach(KeyValuePair<string,string> weap in weapons)
            {
                if(weap.Key == comboBox1.SelectedText)
                {
                    startingWeap = StringToByteArray(weap.Value);
                }
            }
            tagsFile.Write(startingWeap, 0, 4);
            tagsFile.Dispose();
            MessageBox.Show("Save Completed!");
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void automaticExePatcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExePatcherForm patch = new ExePatcherForm();
            patch.Show();
        }
    }

    public class tagsSettings
    {
        private bool deathlessPlayer;
        private bool barriersRemoved;

        public tagsSettings()
        {

        }

        public void setDeathlessPlayer(bool value)
        {
            this.deathlessPlayer = value;
        }

        public void setBarriersRemoved(bool value)
        {
            this.barriersRemoved = value;
        }

        public bool getDeathlessPlayer()
        {
            return this.deathlessPlayer;
        }

        public bool getBarriersRemoved()
        {
            return this.barriersRemoved;
        }
    }

}
