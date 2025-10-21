namespace PomodoroApp.Constants
{
    /// <summary>
    /// Application-wide constants
    /// </summary>
    public static class AppConstants
    {
        // Timer constants
        public const int DefaultPomodoroDurationMinutes = 25;
        public const int DefaultBreakDurationMinutes = 5;
        public const int MinPomodoroDurationMinutes = 1;
        public const int MaxPomodoroDurationMinutes = 60;
        public const int MinBreakDurationMinutes = 1;
        public const int MaxBreakDurationMinutes = 30;
        public const int TimerIntervalMilliseconds = 1000;
        public const int SecondsPerMinute = 60;

        // UI constants
        public const int MainFormWidth = 600;
        public const int MainFormHeight = 750;
        public const int MainPanelWidth = 540;
        public const int MainPanelHeight = 640;
        public const int PanelMargin = 30;
        public const int PanelCornerRadius = 30;
        
        // UI Colors (RGB values)
        public static class Colors
        {
            // Dark mode with subtle gradients
            public static readonly int[] BackgroundColor = { 18, 18, 24 };
            public static readonly int[] PanelColor = { 28, 28, 36 };
            public static readonly int[] PanelAccent = { 45, 45, 58 };
            
            // Modern accent colors
            public static readonly int[] PrimaryAccent = { 99, 102, 241 };      // Indigo
            public static readonly int[] SecondaryAccent = { 139, 92, 246 };    // Purple
            public static readonly int[] SuccessColor = { 34, 197, 94 };        // Green
            public static readonly int[] WarningColor = { 251, 146, 60 };       // Orange
            public static readonly int[] DangerColor = { 239, 68, 68 };         // Red
            
            // Button colors
            public static readonly int[] StartButtonColor = { 99, 102, 241 };   // Indigo
            public static readonly int[] StopButtonColor = { 239, 68, 68 };     // Red
            public static readonly int[] HoverAccent = { 129, 140, 248 };       // Light Indigo
            
            // Text colors
            public static readonly int[] PrimaryText = { 248, 250, 252 };       // Almost white
            public static readonly int[] SecondaryText = { 148, 163, 184 };     // Gray
            public static readonly int[] MutedText = { 100, 116, 139 };         // Dark gray
            
            // Overlay
            public static readonly int[] OverlayColor = { 0, 0, 0 };
            public static readonly int[] OverlayGradient = { 18, 18, 24 };
        }

        // UI Font sizes
        public const float TimerLabelFontSize = 72f;
        public const float TimerLabelFontSizeSmall = 56f;
        public const float SectionTitleFontSize = 16f;
        public const float StandardLabelFontSize = 14f;
        public const float ButtonFontSize = 16f;
        public const float OverlayFontSize = 84f;
        public const float SubtitleFontSize = 12f;

        // Overlay constants
        public const double OverlayOpacity = 0.95;
        
        // Animation and interaction
        public const int ButtonHoverAnimationMs = 150;
        public const int PanelShadowSize = 20;
        public const int ControlSpacing = 24;
        public const int SectionSpacing = 40;

        // Text constants
        public const string AppTitle = "FocusSwift";
        public const string AppSubtitle = "Pomodoro Timer";
        public const string StartButtonText = "Start Focus";
        public const string StopButtonText = "Stop";
        public const string PauseButtonText = "Pause";
        public const string BreakMessageFormat = "Take a break\n{0}";
        public const string PomodoroLabelText = "Focus Duration";
        public const string BreakLabelText = "Break Duration";
        public const string MinutesText = "minutes";
        public const string SessionCompleteText = "Session Complete!";
        public const string BreakCompleteText = "Break Over!";
        public const string FocusTimeText = "Focus Time";
        public const string BreakTimeText = "Break Time";
        
        // Asset paths
        public const string IconPath = "assets\\clock.ico";
    }
}
