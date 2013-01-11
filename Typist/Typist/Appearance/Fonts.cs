using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Typist.Appearance
{
    public static class Fonts
    {
        public static FontInfo GenericMonospace =
            new FontInfo(FontFamily.GenericMonospace.Name, 9.75f);

        public static FontInfo[] Small = new FontInfo[]
        {
            new FontInfo(FontNames.FixedPitch.CourierNew, 9.75f),
            new FontInfo(FontNames.FixedPitch.AnonymousPro, 11.25f),
            new FontInfo(FontNames.FixedPitch.Consolas, 11.25f),
            new FontInfo(FontNames.FixedPitch.PixelCarnageMonoTT, 12),
        };

        public static FontInfo[] Large = new FontInfo[]
        {
            new FontInfo(FontNames.FixedPitch.AndaleMono, 21.75f),
            new FontInfo(FontNames.FixedPitch.Anonymous, 20),
            new FontInfo(FontNames.FixedPitch.AnonymousPro, 24),
            new FontInfo(FontNames.FixedPitch.AudimatMono, 24),
            new FontInfo(FontNames.FixedPitch.BitstreamVeraSansMono, 21),
            new FontInfo(FontNames.FixedPitch.BPmono, 18.75f),
            new FontInfo(FontNames.FixedPitch.CodenameCoderFree4F, 24.75f, FontStyle.Bold),
            new FontInfo(FontNames.FixedPitch.Consolas, 21.75f),
            new FontInfo(FontNames.FixedPitch.CourierNew, 21, FontStyle.Bold),
            new FontInfo(FontNames.FixedPitch.DejaVuSansMono, 21),
            new FontInfo(FontNames.FixedPitch.DroidSansMono, 21),
            new FontInfo(FontNames.FixedPitch.LucidaConsole, 22),
            new FontInfo(FontNames.FixedPitch.LuxiMono, 20.25f),
            new FontInfo(FontNames.FixedPitch.MPlus1M, 22),
            new FontInfo(FontNames.FixedPitch.MonospaceTypewriter, 19.25f),
            new FontInfo(FontNames.FixedPitch.MSGothic, 24.75f),
            new FontInfo(FontNames.FixedPitch.ProFontWindows, 24.75f),
            new FontInfo(FontNames.FixedPitch.SaxMono, 20.25f),
            new FontInfo(FontNames.FixedPitch.Telegrama, 20.25f),
        };

        public static FontInfo[] All = Small.Concat(Large).ToArray();
    }
}
