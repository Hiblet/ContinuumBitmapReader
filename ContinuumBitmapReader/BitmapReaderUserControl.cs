using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AlteryxGuiToolkit.Plugins;
using System.Xml;

namespace ContinuumBitmapReader
{
    public partial class BitmapReaderUserControl : UserControl, IPluginConfiguration
    {
        public BitmapReaderUserControl()
        {
            InitializeComponent();
        }

        public Control GetConfigurationControl(
            AlteryxGuiToolkit.Document.Properties docProperties,
            XmlElement eConfig,
            XmlElement[] eIncomingMetaInfo,
            int nToolId,
            string strToolName)
        {
            // Call LoadFromConfiguration to get the xml file name and field information from eConfig.
            XmlInputConfiguration xmlConfig = XmlInputConfiguration.LoadFromConfiguration(eConfig);

            if (xmlConfig == null)
                return this;

            // Populate the ComboBox with field names
            // If there is no incoming connection, use what is stored

            string selectedField = xmlConfig.SelectedField;

            if (eIncomingMetaInfo == null || eIncomingMetaInfo[0] == null)
            {
                string fieldNames = xmlConfig.FieldNames;
                string[] arrFieldNames = fieldNames.Split(',');

                comboboxFilenameField.Items.Clear();
                foreach (string fieldName in arrFieldNames)
                    comboboxFilenameField.Items.Add(fieldName);

                // Select the saved field                
                if (!string.IsNullOrWhiteSpace(selectedField))
                {
                    int selectedIndex = comboboxFilenameField.FindStringExact(selectedField);
                    if (selectedIndex > 0)
                        comboboxFilenameField.SelectedIndex = selectedIndex;
                }
            }
            else
            {
                comboboxFilenameField.Items.Clear();

                var xmlElementMetaInfo = eIncomingMetaInfo[0];
                var xmlElementRecordInfo = xmlElementMetaInfo.FirstChild;

                foreach (XmlElement elementChild in xmlElementRecordInfo)
                {
                    string fieldName = elementChild.GetAttribute("name");
                    string fieldType = elementChild.GetAttribute("type");

                    if (isStringType(fieldType))
                    {
                        comboboxFilenameField.Items.Add(fieldName);
                    }
                }

                // If the selectedField matches a possible field in the combo box,
                // make it the selected field.
                // If the selectedField does not match, do not select anything and 
                // blank the selectedField.
                if (!string.IsNullOrWhiteSpace(selectedField))
                {
                    int selectedIndex = comboboxFilenameField.FindStringExact(selectedField);
                    if (selectedIndex == -1)
                    {
                        // Not Found
                        XmlElement xmlElementSelectedField = XmlHelpers.GetOrCreateChildNode(eConfig, Constants.SELECTEDFIELDKEY);
                        xmlElementSelectedField.InnerText = "";
                    }
                    else
                    {
                        // Found
                        comboboxFilenameField.SelectedIndex = selectedIndex;
                    }
                }
            } // end of "if (eIncomingMetaInfo == null || eIncomingMetaInfo[0] == null)"

            return this;
        }

        // Helper
        private static bool isStringType(string fieldType)
        {
            return string.Equals(fieldType, "string", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(fieldType, "v_string", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(fieldType, "wstring", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(fieldType, "v_wstring", StringComparison.OrdinalIgnoreCase);
        }

        public void SaveResultsToXml(XmlElement eConfig, out string strDefaultAnnotation)
        {
            XmlElement xmlElementFieldNames = XmlHelpers.GetOrCreateChildNode(eConfig, Constants.FIELDNAMESKEY);
            List<string> fieldNames = new List<string>();
            foreach (var item in comboboxFilenameField.Items)
                fieldNames.Add(item.ToString());

            xmlElementFieldNames.InnerText = string.Join(",", fieldNames);

            XmlElement xmlElementSelectedField = XmlHelpers.GetOrCreateChildNode(eConfig, Constants.SELECTEDFIELDKEY);
            var selectedItem = comboboxFilenameField.SelectedItem;
            string selectedField = "";
            if (selectedItem != null) selectedField = comboboxFilenameField.SelectedItem.ToString();
            xmlElementSelectedField.InnerText = selectedField;

            // Set the default annotation
            strDefaultAnnotation = "Bitmap Reader";
        }
    }
}
