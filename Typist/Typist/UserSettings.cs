using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Typist
{
    public struct UserSettings
    {
        public bool BeepOnError { get; set; }
        public bool AllowBackspace { get; set; }
        public bool VisibleNewlines { get; set; }
        public bool CountWhitespaceAsWordChars { get; set; }
    }
}
