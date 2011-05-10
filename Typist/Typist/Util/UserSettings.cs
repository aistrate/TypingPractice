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
        public bool WordWrap { get; set; }
        public bool VisibleNewlines { get; set; }
        public bool RemoveEndOfLineSpaces { get; set; }
        public bool RemoveMultipleWhitespace { get; set; }
        public bool RememberLastImportedFile { get; set; }
        public bool HideStatisticsWhileTyping { get; set; }
        public bool CountWhitespaceAsWordChars { get; set; }
        public bool CountErrorsAsWordChars { get; set; }
        public bool AskBeforeCloseDuringPractice { get; set; }
        public bool ShowCursorWhenPaused { get; set; }
        public int PauseAfterElapsed { get; set; }
    }
}
