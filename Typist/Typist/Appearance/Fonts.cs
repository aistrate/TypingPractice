using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Typist.Appearance
{
    public static class Fonts
    {
        public static class Small
        {
            public static Font CourierNew { get { return new Font(FontNames.FixedPitch.CourierNew, 9.75f); } }
            public static Font AnonymousPro { get { return new Font(FontNames.FixedPitch.AnonymousPro, 11.25f); } }

            public static Font[] All { get { return typeof(Small).AllStaticProperties().GetValues<Font>(); } }
        }

        public static class Large
        {
            public static Font AndaleMono { get { return new Font(FontNames.FixedPitch.AndaleMono, 21.75f); } }
            public static Font Anonymous { get { return new Font(FontNames.FixedPitch.Anonymous, 20); } }
            public static Font AnonymousPro { get { return new Font(FontNames.FixedPitch.AnonymousPro, 24); } }
            public static Font AudimatMono { get { return new Font(FontNames.FixedPitch.AudimatMono, 23); } }
            public static Font BitstreamVeraSansMono { get { return new Font(FontNames.FixedPitch.BitstreamVeraSansMono, 21); } }
            public static Font BPmono { get { return new Font(FontNames.FixedPitch.BPmono, 24); } }
            public static Font CodenameCoderFree4F { get { return new Font(FontNames.FixedPitch.CodenameCoderFree4F, 24.75f, FontStyle.Bold); } }
            public static Font Consolas { get { return new Font(FontNames.FixedPitch.Consolas, 21.75f); } }
            public static Font CourierNew { get { return new Font(FontNames.FixedPitch.CourierNew, 21, FontStyle.Bold); } }
            public static Font DejaVuSansMono { get { return new Font(FontNames.FixedPitch.DejaVuSansMono, 21); } }
            public static Font DroidSansMono { get { return new Font(FontNames.FixedPitch.DroidSansMono, 21); } }
            public static Font EnvyCodeR { get { return new Font(FontNames.FixedPitch.EnvyCodeR, 21.75f); } }
            public static Font LucidaConsole { get { return new Font(FontNames.FixedPitch.LucidaConsole, 22); } }
            public static Font LuxiMono { get { return new Font(FontNames.FixedPitch.LuxiMono, 20.25f); } }
            public static Font MPlus1M { get { return new Font(FontNames.FixedPitch.MPlus1M, 22); } }
            public static Font MonospaceTypewriter { get { return new Font(FontNames.FixedPitch.MonospaceTypewriter, 19.25f); } }
            public static Font MSGothic { get { return new Font(FontNames.FixedPitch.MSGothic, 24.75f); } }
            public static Font ProFontWindows { get { return new Font(FontNames.FixedPitch.ProFontWindows, 24.75f); } }
            public static Font SaxMono { get { return new Font(FontNames.FixedPitch.SaxMono, 20.25f); } }

            public static Font[] All { get { return typeof(Large).AllStaticProperties().GetValues<Font>(); } }
        }
    }
}
