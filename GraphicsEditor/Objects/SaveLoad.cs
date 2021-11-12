using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GraphicsEditor.Objects
{
    class SaveLoad
    {
        private List<IFigure> figures;

        public SaveLoad() { }

        public SaveLoad(List<IFigure> figures)
        {
            this.figures = figures;
        }

        public void Save()
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.ShowDialog();
            try
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName, false, Encoding.Default))
                {
                    foreach (IFigure figure in figures)
                    {
                        sw.WriteLine(Factory.Serialize(figure));
                    }
                }
            }
            catch { }
        }

        public List<IFigure> Load()
        {
            List<IFigure> figures = new();

            OpenFileDialog openFileDialog = new();
            openFileDialog.ShowDialog();

            StreamReader sr = new StreamReader(openFileDialog.FileName, Encoding.Default);
            try
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    figures.Add(Factory.Deserialize(line));
                }
            }
            catch { }

            return figures;
        }
    }
}
