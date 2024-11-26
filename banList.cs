using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Factorio_Headless_Server_Tool
{
    public partial class banList : Form
    {
        public banList()
        {
            InitializeComponent();
        }

        private void banList_Load(object sender, EventArgs e)
        {
            string banJsonFile = Properties.Settings.Default.factorioDataPath + "\\server-banlist.json";

            if (File.Exists(banJsonFile))
            {
                Debug.WriteLine("Loading ban File: " + banJsonFile);
                dynamic banListFile = JsonConvert.DeserializeObject(File.ReadAllText(banJsonFile));
                
                Debug.WriteLine("Banned User List:");

                for (int i = 0; i < banListFile.Count; i++)
                {
                    if (banListFile[i] is JObject obj && obj.ContainsKey("username"))
                    {
                        listBox.AppendText((string)obj["username"]);
                        //Debug.WriteLine((string)obj["username"]);
                    } else
                    {
                        listBox.Text += banListFile[i] + "\r\n";
                        //Debug.WriteLine(banListFile[i]);
                    }                    
                }
            }
        }

        private void saveBTN_Click(object sender, EventArgs e)
        {
            string dataFolder = Properties.Settings.Default.factorioDataPath;
            string banJsonFile = dataFolder + "\\server-banlist.json";

            if (Directory.Exists(dataFolder))
            {
                Debug.WriteLine("Saving ban File: " + banJsonFile);

                string[] banUsernames = new string[listBox.Lines.Length];

                for (int i = 0; i < listBox.Lines.Length; i++)
                {
                    banUsernames[i] = listBox.Lines[i];
                }

                string banListFile = JsonConvert.SerializeObject(banUsernames);

                File.WriteAllText(banJsonFile, banListFile);

                Debug.WriteLine("Saved ban File: " + banJsonFile);

                Debug.WriteLine("Saved Admin File: " + banJsonFile);
                MessageBox.Show("Saved Ban List");
            }
        }

        private void clearBTN_Click(object sender, EventArgs e)
        {
            listBox.Text = "";
        }
    }
}
