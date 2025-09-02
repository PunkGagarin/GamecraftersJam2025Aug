using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

namespace Jam.Scripts.Utils.Editor
{
    public class CSVToXMLParser : EditorWindow
    {
        private string _csvFilePath = "";
        private Object _csvFileObject;

        [MenuItem("Tools/CSV to XML Parser")]
        public static void ShowWindow()
        {
            GetWindow<CSVToXMLParser>("CSV to XML Parser");
        }

        private void OnGUI()
        {
            GUILayout.Label("CSV to XML Converter", EditorStyles.boldLabel);

            GUILayout.Label("Drag and Drop CSV File Here:");
            _csvFileObject = EditorGUILayout.ObjectField("CSV File", _csvFileObject, typeof(Object), false);

            if (_csvFileObject != null)
            {
                _csvFilePath = AssetDatabase.GetAssetPath(_csvFileObject);
            }

            if (GUILayout.Button("Parse CSV"))
            {
                if (!string.IsNullOrEmpty(_csvFilePath) && File.Exists(_csvFilePath))
                {
                    ParseCSV(_csvFilePath);
                    EditorUtility.DisplayDialog("Success", "XML files have been created in Resources/XML/", "OK");
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "CSV file not found!", "OK");
                }
            }
        }

        private void ParseCSV(string path)
        {
            List<string[]> lines = new List<string[]>();

            using (var reader = new StreamReader(path, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = ParseCSVLine(line);
                    lines.Add(values);
                }
            }

            if (lines.Count < 2) return;

            string[] headers = lines[0]; // Первая строка - языки
            Directory.CreateDirectory("Assets/Resources/XML");

            for (int langIndex = 1; langIndex < headers.Length; langIndex++)
            {
                string lang = headers[langIndex].Trim();
                XDocument xmlDoc = new XDocument(new XElement("Language", new XAttribute("LANG", lang), new XAttribute("ID", langIndex - 1)));

                for (int i = 1; i < lines.Count; i++)
                {
                    string[] values = lines[i];
                    if (values.Length <= langIndex) continue;

                    string key = values[0].Trim();
                    string value = values[langIndex].Trim();

                    xmlDoc.Root.Add(new XElement("text", new XAttribute("key", key), value));
                }

                string savePath = $"Assets/Resources/XML/{lang}.xml";
                xmlDoc.Save(savePath);
            }

            AssetDatabase.Refresh();
        }

        private string[] ParseCSVLine(string line)
        {
            List<string> values = new List<string>();
            StringBuilder currentValue = new StringBuilder();
            bool inQuotes = false;

            foreach (char c in line)
            {
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    values.Add(currentValue.ToString());
                    currentValue.Clear();
                }
                else
                {
                    currentValue.Append(c);
                }
            }

            values.Add(currentValue.ToString());
            return values.ToArray();
        }
    }
}