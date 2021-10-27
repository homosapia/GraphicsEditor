
using GraphicsEditor.Data;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GraphicsEditor
{
    class SaveLoad
    {
        private const string Brkenline = "FigureBrokenLine$";
        private const string square = "FigureRectangle$";


        List<IFigure> figures;

        public SaveLoad(){}

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
                        if (figure.ToString() == "GraphicsEditor.FigureQuadrilateral")
                        {
                            string json = square;

                            ListOfDataToSave data = figure.SerializeFigure();
                            json += JsonConvert.SerializeObject(data);

                            sw.WriteLine(json);
                        }

                        if (figure.ToString() == "GraphicsEditor.FigureBrokenLine")
                        {
                            string json = Brkenline;

                            ListOfDataToSave data = figure.SerializeFigure();
                            json += JsonConvert.SerializeObject(data);

                            sw.WriteLine(json);
                        }
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

            try
            {
                using (StreamReader sr = new StreamReader(openFileDialog.FileName, Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains(Brkenline))
                        {
                            string FBL = line.Remove(0, Brkenline.IndexOf("$") + 1);
                            ListOfDataToSave data = JsonConvert.DeserializeObject<ListOfDataToSave>(FBL);
                            FigureBrokenLine brokenLine = new();
                            brokenLine.DeserializeFigure(data);
                            figures.Add(brokenLine);
                        }
                        if (line.Contains(square))
                        {
                            string FR = line.Remove(0, square.IndexOf("$") + 1);
                            ListOfDataToSave data = JsonConvert.DeserializeObject<ListOfDataToSave>(FR);
                            FigureQuadrilateral figureRectangle = new();
                            figureRectangle.DeserializeFigure(data);
                            figures.Add(figureRectangle);
                        }
                    }
                }
            }
            catch { }

            return figures;
        }
    }
}
