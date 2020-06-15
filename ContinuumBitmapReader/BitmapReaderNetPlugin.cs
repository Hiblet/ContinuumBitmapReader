using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Linq;
using AlteryxRecordInfoNet;
using System.Net;
using System.Drawing;

namespace ContinuumBitmapReader
{
    public class BitmapReaderNetPlugin : INetPlugin, IIncomingConnectionInterface
    {

        private int _toolID; // Integer identifier provided by Alteryx, this tools tool number.
        private EngineInterface _engineInterface; // Reference provided by Alteryx so that we can talk to the engine.
        private XmlElement _xmlProperties; // Xml configuration for this custom tool

        private PluginOutputConnectionHelper _outputHelper;

        private RecordInfo _recordInfoIn;
        private RecordInfo _recordInfoOut;

        private RecordCopier _recordCopier;

        private string _selectedField = Constants.DEFAULTSELECTEDFIELD;




        public void PI_Init(int nToolID, EngineInterface engineInterface, XmlElement pXmlProperties)
        {
            DebugMessage($"PI_Init() Entering; ToolID={_toolID}");

            _toolID = nToolID;
            _engineInterface = engineInterface;
            _xmlProperties = pXmlProperties;

            // Use the information in the pXmlProperties parameter to get the input xml field name
            XmlElement configElement = XmlHelpers.GetFirstChildByName(_xmlProperties, "Configuration", true);
            if (configElement != null)
            {
                getConfigSetting(configElement, Constants.SELECTEDFIELDKEY, ref _selectedField);
            }

            _outputHelper = new PluginOutputConnectionHelper(_toolID, _engineInterface);

            DebugMessage($"PI_Init() Exiting; ToolID={_toolID}");
        }

        public IIncomingConnectionInterface PI_AddIncomingConnection(string pIncomingConnectionType, string pIncomingConnectionName)
        {
            DebugMessage($"PI_AddIncomingConnection() has been called; Name={pIncomingConnectionName}");
            return this;
        }

        public bool PI_AddOutgoingConnection(string pOutgoingConnectionName, OutgoingConnection outgoingConnection)
        {
            // Add the outgoing connection to our PluginOutputConnectionHelper so it can manage it.
            _outputHelper.AddOutgoingConnection(outgoingConnection);

            DebugMessage($"PI_AddOutgoingConnection() has been called; Name={pOutgoingConnectionName}");
            return true;
        }

        public bool PI_PushAllRecords(long nRecordLimit)
        {
            // Should not be called
            DebugMessage($"PI_PushAllRecords() has been called; THIS SHOULD NOT BE CALLED FOR A TOOL WITH AN INPUT CONNECTION!");
            return true;
        }

        public void PI_Close(bool bHasErrors)
        {
            // Release any resources used by the control
            _outputHelper.Close();
            DebugMessage("PI_Close() has been called; OutputHelper has been closed.");
        }

        public bool ShowDebugMessages()
        {
            // Return true to help us debug our tool. This should be set to false for general distribution.
            #if DEBUG
                return true;
            #else
                return false;      
            #endif
        }

        public XmlElement II_GetPresortXml(XmlElement pXmlProperties)
        {
            DebugMessage($"II_GetPresortXml() has been called");

            return null;
        }

        public bool II_Init(RecordInfo recordInfo)
        {
            DebugMessage($"II_Init() Entering; ToolID={_toolID}");

            _recordInfoIn = recordInfo;

            prep();

            DebugMessage($"II_Init() Exiting; ToolID={_toolID}; prep() should have completed.");
            return true;
        }


        // Receive an inbound record and process it.
        // Information about the record is held in the recordInfo object
        // that was passed in to II_Init(), and (hopefully) cached.
        public bool II_PushRecord(AlteryxRecordInfoNet.RecordData recordDataIn)
        {
            DebugMessage($"II_PushRecord() Entering; ToolID={_toolID}");

            // If we have no selected field, we can't continue.
            if (string.IsNullOrWhiteSpace(_selectedField))
            {
                Message($"Selected input field was blank.");
                return false;
            }

            // Get the FieldBase for the "Input" data the contains the bitmap filename to process.
            FieldBase filenameFieldBase = null;
            try { filenameFieldBase = _recordInfoIn.GetFieldByName(_selectedField, false); }
            catch {
                Message($"Input column requires a path to a bitmap file.");
                return false;
            }

            // If that field doesn't exist, we can't continue.
            if (filenameFieldBase == null) return false;
            

            // Get the filename from the current record.
            string filenameText = "";
            try { filenameText = filenameFieldBase.GetAsString(recordDataIn); } catch { return false; }

            if (string.IsNullOrEmpty(filenameText))
                return false;

            DebugMessage($"II_PushRecord() Processing [{filenameText}]; ToolID={_toolID}");


            // Load Image

            Bitmap image = null;
            try { image = (Bitmap)Image.FromFile(filenameText, true); }
            catch(OutOfMemoryException)
            {
                Message($"Failed to load [{filenameText}] - OutOfMemory exception; Check file exists and is an image file of type BMP/DIB/JPEG/GIF/TIFF/PNG format.", MessageStatus.STATUS_Error);
                return false;
            }
            catch(Exception ex)
            {
                string exMessage = ex.Message;
                string exBaseMessage = ex.GetBaseException()?.Message ?? "";
                Message($"Failed to load [{filenameText}]; Exception:[{ex.Message}]; BaseException:[{exBaseMessage}]", MessageStatus.STATUS_Error);
                return false;
            }

            if (image == null)
            {
                Message($"Image object loaded, but null, for file:[{filenameText}]; ToolID={_toolID}", MessageStatus.STATUS_Error);
                return false;
            }



            // Walk bitmap sequentially, retrieving data
            int height = image.Height;
            int width = image.Width;
            DebugMessage($"II_PushRecord() Processing [{filenameText}]; Total Pixels={height * width}; ToolID={_toolID}");

            double dTotalPixelsOnePercent = height * width / 100;
            double dTotalPixelsTenPercent = height * width / 10;

            Int64 totalPixelsOnePercent = Math.Max((Int64)dTotalPixelsOnePercent,1);
            Int64 totalPixelsTenPercent = Math.Max((Int64)dTotalPixelsTenPercent,1);


            Int32 x = 0;
            Int32 y = 0;
            Int64 countPixels = 0;

            for (y = 0; y < height; ++y)
            {                
                for (x = 0; x < width; ++x)
                {
                    ++countPixels;
                    Int64 progress = Convert.ToInt64(countPixels / totalPixelsOnePercent);

                    if (countPixels % totalPixelsTenPercent == 0)
                        Message($"Bitmap Reader Progress: {progress}%; ToolID={_toolID}");

                    Color color = image.GetPixel(x, y);
                    pushRecord(
                        x,
                        y,
                        color.R,
                        color.G,
                        color.B,
                        color.A,
                        recordDataIn);
                }
            }

            // Drop the bitmap
            image.Dispose();

            // DIAG
            // Call to pushRecord() to simulate activity once.
            //pushRecord("XCoord","YCoord","RValue","GValue","BValue","AlphaValue",recordDataIn);
	        // END DIAG


            DebugMessage($"II_PushRecord() Exiting; ToolID={_toolID}");
            return true;
        }

        private void pushRecord(
            Int32 xCoord, Int32 yCoord,
            Int32 rValue, Int32 gValue, Int32 bValue,
            Int32 alphaValue, RecordData recordDataIn)
        {
            Record recordOut = _recordInfoOut.CreateRecord();
            recordOut.Reset();

            _recordCopier.Copy(recordOut, recordDataIn);

            var outFieldCount = _recordInfoOut.NumFields();
            int indexXCoordField = (int)outFieldCount - 6;
            int indexYCoordField = (int)outFieldCount - 5;
            int indexRValueField = (int)outFieldCount - 4;
            int indexGValueField = (int)outFieldCount - 3;
            int indexBValueField = (int)outFieldCount - 2;
            int indexAlphaValueField = (int)outFieldCount - 1;

            // RValue Field
            FieldBase fieldBase = _recordInfoOut[indexXCoordField];            
            fieldBase.SetFromInt32(recordOut, xCoord);
            // RValue Field
            fieldBase = _recordInfoOut[indexYCoordField];
            fieldBase.SetFromInt32(recordOut, yCoord);
            // RValue Field
            fieldBase = _recordInfoOut[indexRValueField];
            fieldBase.SetFromInt32(recordOut, rValue);
            // GValue Field
            fieldBase = _recordInfoOut[indexGValueField];
            fieldBase.SetFromInt32(recordOut, gValue);
            // RValue Field
            fieldBase = _recordInfoOut[indexBValueField];
            fieldBase.SetFromInt32(recordOut, bValue);
            // AlphaValue Field
            fieldBase = _recordInfoOut[indexAlphaValueField];
            fieldBase.SetFromInt32(recordOut, alphaValue);

            _outputHelper.PushRecord(recordOut.GetRecord());
        }



        public void II_UpdateProgress(double dPercent)
        {
            // Since our progress is directly proportional to he progress of the
            // upstream tool, we can simply output it's percentage as our own.
            if (_engineInterface.OutputToolProgress(_toolID, dPercent) != 0)
            {
                // If this returns anything but 0, then the user has canceled the operation.
                throw new AlteryxRecordInfoNet.UserCanceledException();
            }

            // Have the PluginOutputConnectionHelper ask the downstream tools to update their progress.
            _outputHelper.UpdateProgress(dPercent);
            DebugMessage($"II_UpdateProgress() has been called; dPercent={dPercent}; ToolID={_toolID}");
        }

        public void II_Close()
        {
            DebugMessage($"II_Close() has been called; ToolID={_toolID}");
        }

        private void prep()
        {
            // Exit if already done (safety)
            if (_recordInfoOut != null)
                return;

            _recordInfoOut = new AlteryxRecordInfoNet.RecordInfo();

            populateRecordInfoOut();

            _recordCopier = new RecordCopier(_recordInfoOut, _recordInfoIn, true);

            uint countFields = _recordInfoIn.NumFields();
            for (int i = 0; i < countFields; ++i)
            {
                var fieldName = _recordInfoIn[i].GetFieldName();

                var newFieldNum = _recordInfoOut.GetFieldNum(fieldName, false);
                if (newFieldNum == -1)
                    continue;

                _recordCopier.Add(newFieldNum, i);
            }

            _recordCopier.DoneAdding();

            _outputHelper.Init(_recordInfoOut, "Output", null, _xmlProperties);
        }

        private void populateRecordInfoOut()
        {
            _recordInfoOut = new AlteryxRecordInfoNet.RecordInfo();

            // Copy the fieldbase structure of the incoming record
            uint countFields = _recordInfoIn.NumFields();
            for (int i = 0; i < countFields; ++i)
            {
                FieldBase fbIn = _recordInfoIn[i];
                var currentFieldName = fbIn.GetFieldName();

                _recordInfoOut.AddField(currentFieldName, fbIn.FieldType, (int)fbIn.Size, fbIn.Scale, fbIn.GetSource(), fbIn.GetDescription());
            }



            // Add the output columns at the end
            _recordInfoOut.AddField("XCoord", FieldType.E_FT_Int32, Constants.INT32_FIELDSIZE, 0, "", "");
            _recordInfoOut.AddField("YCoord", FieldType.E_FT_Int32, Constants.INT32_FIELDSIZE, 0, "", "");
            _recordInfoOut.AddField("RValue", FieldType.E_FT_Int32, Constants.INT32_FIELDSIZE, 0, "", "");
            _recordInfoOut.AddField("GValue", FieldType.E_FT_Int32, Constants.INT32_FIELDSIZE, 0, "", "");
            _recordInfoOut.AddField("BValue", FieldType.E_FT_Int32, Constants.INT32_FIELDSIZE, 0, "", "");
            _recordInfoOut.AddField("AlphaValue", FieldType.E_FT_Int32, Constants.INT32_FIELDSIZE, 0, "", "");

        }



        //////////// 
        // HELPERS


        private void getConfigSetting(XmlElement configElement, string key, ref string memberToSet)
        {
            XmlElement xe = XmlHelpers.GetFirstChildByName(configElement, key, false);
            if (xe != null)
            {
                if (!string.IsNullOrWhiteSpace(xe.InnerText))
                    memberToSet = xe.InnerText;
            }
        }

        public static bool isTrueString(string target)
        {
            string cleanTarget = target.Trim().ToUpper();
            switch (cleanTarget)
            {
                case "Y":
                case "TRUE":
                case "1":
                    return true;
                default:
                    break;
            }
            return false;
        }



        //////////////////////// 
        // Message Boilerplate

        public void Message(string message, MessageStatus messageStatus = MessageStatus.STATUS_Info)
        {
            this._engineInterface?.OutputMessage(this._toolID, messageStatus, message);
        }

        public void DebugMessage(string message)
        {
            if (ShowDebugMessages()) this.Message(message);
        }
    }
}
