using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Typist.Appearance
{
    public static class FontNames
    {
        public static class FixedPitch
        {
            public static string AndaleMono { get { return "Andale Mono"; } }
            public static string Anonymous { get { return "Anonymous"; } }
            public static string AnonymousPro { get { return "Anonymous Pro"; } }
            public static string BitstreamVeraSansMono { get { return "Bitstream Vera Sans Mono"; } }
            public static string Consolas { get { return "Consolas"; } }
            public static string CourierNew { get { return "Courier New"; } }
            public static string DejaVuSansMono { get { return "DejaVu Sans Mono"; } }
            public static string EnvyCodeR { get { return "Envy Code R"; } }
            public static string LuxiMono { get { return "Luxi Mono"; } }
        }

        public static class FixedPitchRomanian
        {
            public static string AnonymousPro { get { return FixedPitch.AnonymousPro; } }
            public static string Consolas { get { return FixedPitch.Consolas; } }
            public static string DejaVuSansMono { get { return FixedPitch.DejaVuSansMono; } }
            public static string EnvyCodeR { get { return FixedPitch.EnvyCodeR; } }
            public static string LuxiMono { get { return FixedPitch.LuxiMono; } }
        }

        public static class VariablePitch
        {
            public static string Arial { get { return "Arial"; } }
            public static string Georgia { get { return "Georgia"; } }
            public static string Tahoma { get { return "Tahoma"; } }
            public static string Verdana { get { return "Verdana"; } }
        }
    }
}
