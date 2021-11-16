using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GraphicsEditor.Data;
using Newtonsoft.Json;

namespace GraphicsEditor.Objects
{
    class SaveLoad
    {

        public void Save(WorkspaceDataToSave workspaceData)
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.ShowDialog();
            try
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName, false, Encoding.Default))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(workspaceData));
                }
            }
            catch { }
        }

        public WorkspaceDataToSave Load()
        {
            WorkspaceDataToSave workspace = new();

            OpenFileDialog openFileDialog = new();
            openFileDialog.ShowDialog();

            StreamReader sr = new StreamReader(openFileDialog.FileName, Encoding.Default);
            try
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    workspace = JsonConvert.DeserializeObject<WorkspaceDataToSave>(line);
                }
            }
            catch { }

            return workspace;
        }
    }
}
