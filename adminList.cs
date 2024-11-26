using System.Diagnostics;
using Newtonsoft.Json;


namespace Factorio_Headless_Server_Tool
{
    public partial class adminList : Form
    {
        public adminList()
        {
            InitializeComponent();
        }

        private void saveBTN_Click(object sender, EventArgs e)
        {
            string dataFolder = Properties.Settings.Default.factorioDataPath;
            string adminJsonFile = dataFolder + "\\server-adminlist.json";

            if (Directory.Exists(dataFolder))
            {
                Debug.WriteLine("Saving Admin File: " + adminJsonFile);

                string[] adminUsernames = new string[listBox.Lines.Length];

                for (int i = 0; i < listBox.Lines.Length; i++)
                {
                    adminUsernames[i] = listBox.Lines[i];
                }

                string adminListFile = JsonConvert.SerializeObject(adminUsernames);

                File.WriteAllText(adminJsonFile, adminListFile);

                Debug.WriteLine("Saved Admin File: " + adminJsonFile);
                MessageBox.Show("Saved Admin List");
            }
        }

        private void adminList_Load(object sender, EventArgs e)
        {            
            string adminJsonFile = Properties.Settings.Default.factorioDataPath + "\\server-adminlist.json";
            
            if (File.Exists(adminJsonFile)) { 
                Debug.WriteLine("Loading Admin File: " + adminJsonFile);                        
                dynamic adminListFile = JsonConvert.DeserializeObject(File.ReadAllText(adminJsonFile));            
                Debug.WriteLine("\r User List:");
                foreach (string adminUsername in adminListFile)
                {
                    listBox.Text += adminUsername;                
                    Debug.WriteLine(adminUsername);
                }  
            }
        }

        private void ClearBTN_Click(object sender, EventArgs e)
        {
            listBox.Text = "";
        }
    }
}
