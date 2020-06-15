using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace ContinuumBitmapReader
{
    public class XmlInputConfiguration
    {
        public string SelectedField { get; private set; }
        public string FieldNames { get; private set; }

        // Note that the constructor is private.  Instances are created through the LoadFromConfigration method.
        private XmlInputConfiguration(
            string selectedField, 
            string fieldNames)
        {
            SelectedField = selectedField;
            FieldNames = fieldNames;
        }

        public static XmlInputConfiguration LoadFromConfiguration(XmlElement eConfig)
        {
            string selectedField = getStringFromConfig(eConfig, Constants.SELECTEDFIELDKEY, Constants.DEFAULTSELECTEDFIELD);
            string fieldNames = getStringFromConfig(eConfig, Constants.FIELDNAMESKEY, Constants.DEFAULTFIELDNAMES);

            return new XmlInputConfiguration(
                selectedField, 
                fieldNames);
        }

        public static string getStringFromConfig(XmlElement eConfig, string key, string valueDefault)
        {
            string sReturn = valueDefault;

            XmlElement xe = eConfig.SelectSingleNode(key) as XmlElement;
            if (xe != null)
            {
                if (!string.IsNullOrEmpty(xe.InnerText))
                    sReturn = xe.InnerText;
            }

            return sReturn;
        }

        // Property Name Accessor
        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }

    }
}
