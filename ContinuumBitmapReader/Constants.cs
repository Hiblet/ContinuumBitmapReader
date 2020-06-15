using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContinuumBitmapReader
{
    public class Constants
    {
        // NOTE: THESE STRINGS SHOULD MATCH THE ACCESSORS IN XmlInputConfiguration.cs
        public static string SELECTEDFIELDKEY = "ContinuumBitmapReaderSelectedField";
        public static string FIELDNAMESKEY = "ContinuumBitmapReaderFieldNames"; // Last seen field

        // Default Values
        public static string DEFAULTSELECTEDFIELD = "Field_1";
        public static string DEFAULTFIELDNAMES = "Field_1";

        public static int LARGEOUTPUTFIELDSIZE = 127;
        public static int INT64_FIELDSIZE = 8;
        public static int INT32_FIELDSIZE = 4;
    }
}
