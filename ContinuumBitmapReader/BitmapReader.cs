using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AlteryxGuiToolkit.Plugins;

namespace ContinuumBitmapReader
{
    public class BitmapReader : IPlugin
    {
        private System.Drawing.Bitmap _icon;
        private string _iconResource = "ContinuumBitmapReader.Resources.BitmapReader_171.png";


        public IPluginConfiguration GetConfigurationGui()
        {
            return new BitmapReaderUserControl();
        }

        public EntryPoint GetEngineEntryPoint()
        {
            return new AlteryxGuiToolkit.Plugins.EntryPoint("ContinuumBitmapReader.dll", "ContinuumBitmapReader.BitmapReaderNetPlugin", true);
        }

        public Image GetIcon()
        {
            // DIAG
            // To see the actual name of the embedded resource, as the assembly sees it.
            var arrResources = typeof(BitmapReader).Assembly.GetManifestResourceNames();
            // END DIAG

            if (_icon == null)
            {
                System.IO.Stream s = typeof(BitmapReader).Assembly.GetManifestResourceStream(_iconResource);
                if (s == null)
                {
                    throw new ArgumentNullException("Could not find local resource [" + _iconResource + "]");
                }

                _icon = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromStream(s);
                _icon.MakeTransparent();
            }

            return _icon;
        }

        public Connection[] GetInputConnections()
        {
            return new Connection[] { new Connection("Input") };
        }

        public Connection[] GetOutputConnections()
        {
            return new Connection[] { new Connection("Output") };

        }
    }
}
