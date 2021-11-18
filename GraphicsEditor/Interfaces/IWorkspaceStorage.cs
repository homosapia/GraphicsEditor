using GraphicsEditor.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEditor.Interfaces
{
    interface IWorkspaceStorage
    {
        public void Save(WorkspaceDataToSave workspaceData);
        
        public WorkspaceDataToSave Load();
    }
}
