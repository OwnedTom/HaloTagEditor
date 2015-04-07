using DarkPluginLib;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace PluginExample
{

    public class HelloToolkit : DarkPlugin
    {
        #region DarkPlugin Members

        public string Title
        {
            get
            {
                return "Simple Halo Tags Editor";
            }
        }
        public string Description
        {
            get
            {
                return "A Simple tag editor for Halo: Online";
            }
        }
        public string Author
        {
            get
            {
                return "OwnedTom";
            }
        }
        public string Url
        {
            get
            {
                return "https://github.com/OwnedTom/HaloTagEditor";
            }
        }
        public string Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
        public string Built
        {
            get
            {
                System.IO.FileInfo fi = new System.IO.FileInfo("Plugins/" + Assembly.GetExecutingAssembly().GetName().Name + ".dll");
                return fi.LastWriteTime.ToString();
            }
        }
        public Form MainForm
        {
            get
            {
                return helloForm;
            }
        }

        public HaloTagsEditor.MainForm helloForm;
        public static bool runningForm = false;
        public void DarkPluginMain()
        {

            DarkLog.WriteLine("DarkPluginMain() has been hit in HelloToolkit Plugin Example.");
            DarkLog.WriteLine("Halo Online Folder: " + DarkSettings.HaloOnlineFolder);
            if (!runningForm)
            {
                helloForm = new HaloTagsEditor.MainForm();
                helloForm.Show();
                runningForm = true;
            }
            else
            {
                DarkLog.WriteLine("Plugin already running!");

                helloForm.Show();
            }
        }

        #endregion
    }

}