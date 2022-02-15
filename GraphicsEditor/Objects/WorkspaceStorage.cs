using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GraphicsEditor.Data;
using GraphicsEditor.Interfaces;
using Newtonsoft.Json;

namespace GraphicsEditor.Objects
{
    class WorkspaceStorage : IWorkspaceStorage
    {
        public void Save(WorkspaceData workspaceData)
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.ShowDialog();
            try
            {
                using (StreamWriter sw = new(saveFileDialog.FileName, false, Encoding.Default))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(workspaceData));
                }
            }
            catch { }
        }

        public WorkspaceData Load()
        {
            WorkspaceData workspace = new();
            
            OpenFileDialog openFileDialog = new();
            openFileDialog.ShowDialog();

            try
            {
                using (StreamReader sr = new(openFileDialog.FileName, Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        workspace = JsonConvert.DeserializeObject<WorkspaceData>(line);
                    }
                }
            }
            catch {}

            return workspace;
        }
    }
}
