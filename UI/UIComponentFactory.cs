using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PomodoroApp.Constants;

namespace PomodoroApp.UI
{
    /// <summary>
    /// Factory class for creating modern UI components with contemporary styling
    /// </summary>
    public static class UIComponentFactory
    {
        /// <summary>
        /// Creates a modern rounded panel with gradient and shadow effect
        /// </summary>
        public static Panel CreateRoundedPanel(int width, int height, int x, int y)
        {
            var panel = new Panel
            {
                Size = new Size(width, height),
                Location = new Point(x, y),
                BackColor = Color.FromArgb(
                    AppConstants.Colors.PanelColor[0],
                    AppConstants.Colors.PanelColor[1],
                    AppConstants.Colors.PanelColor[2]
                ),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            panel.Paint += (s, e) =>
            {
                if (s is Panel p)
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    
                    // Draw subtle shadow
                    using (GraphicsPath shadowPath = CreateRoundedRectanglePath(
                        new Rectangle(4, 4, p.Width - 8, p.Height - 8),
                        AppConstants.PanelCornerRadius))
                    {
                        using (PathGradientBrush shadowBrush = new PathGradientBrush(shadowPath))
                        {
                            shadowBrush.CenterColor = Color.FromArgb(40, 0, 0, 0);
                            shadowBrush.SurroundColors = new[] { Color.FromArgb(0, 0, 0, 0) };
                            e.Graphics.FillPath(shadowBrush, shadowPath);
                        }
                    }
                    
                    // Draw panel with rounded corners
                    using (GraphicsPath path = CreateRoundedRectanglePath(
                        new Rectangle(0, 0, p.Width, p.Height),
                        AppConstants.PanelCornerRadius))
                    {
                        p.Region = new Region(path);
                        
                        // Subtle gradient overlay
                        using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                            new Rectangle(0, 0, p.Width, p.Height),
                            Color.FromArgb(5, 255, 255, 255),
                            Color.FromArgb(0, 255, 255, 255),
                            LinearGradientMode.Vertical))
                        {
                            e.Graphics.FillPath(gradientBrush, path);
                        }
                    }
                }
            };

            return panel;
        }

        /// <summary>
        /// Creates a modern styled label with custom color
        /// </summary>
        public static Label CreateLabel(string text, float fontSize, int x, int y, bool autoSize = true, bool isSecondary = false)
        {
            var colorArray = isSecondary ? AppConstants.Colors.SecondaryText : AppConstants.Colors.PrimaryText;
            
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", fontSize, FontStyle.Regular),
                ForeColor = Color.FromArgb(colorArray[0], colorArray[1], colorArray[2]),
                AutoSize = autoSize,
                Location = new Point(x, y),
                BackColor = Color.Transparent
            };
        }

        /// <summary>
        /// Creates a section title label
        /// </summary>
        public static Label CreateSectionTitle(string text, int x, int y)
        {
            return new Label
            {
                Text = text.ToUpper(),
                Font = new Font("Segoe UI Semibold", AppConstants.SectionTitleFontSize, FontStyle.Bold),
                ForeColor = Color.FromArgb(
                    AppConstants.Colors.PrimaryText[0],
                    AppConstants.Colors.PrimaryText[1],
                    AppConstants.Colors.PrimaryText[2]
                ),
                AutoSize = true,
                Location = new Point(x, y),
                BackColor = Color.Transparent
            };
        }

        /// <summary>
        /// Creates a modern timer display label with glow effect
        /// </summary>
        public static Label CreateTimerLabel(string text, int x, int y, string name)
        {
            var label = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", AppConstants.TimerLabelFontSize, FontStyle.Bold),
                ForeColor = Color.FromArgb(
                    AppConstants.Colors.PrimaryAccent[0],
                    AppConstants.Colors.PrimaryAccent[1],
                    AppConstants.Colors.PrimaryAccent[2]
                ),
                AutoSize = true,
                Location = new Point(x, y),
                Name = name,
                BackColor = Color.Transparent
            };

            // Add glow effect on paint
            label.Paint += (s, e) =>
            {
                if (s is Label lbl)
                {
                    e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                }
            };

            return label;
        }

        /// <summary>
        /// Creates a modern numeric input control with styling
        /// </summary>
        public static NumericUpDown CreateNumericInput(decimal value, decimal min, decimal max, int width, int x, int y)
        {
            var input = new NumericUpDown
            {
                Value = value,
                Minimum = min,
                Maximum = max,
                Width = width,
                Height = 45,
                Location = new Point(x, y),
                BackColor = Color.FromArgb(
                    AppConstants.Colors.PanelAccent[0],
                    AppConstants.Colors.PanelAccent[1],
                    AppConstants.Colors.PanelAccent[2]
                ),
                ForeColor = Color.FromArgb(
                    AppConstants.Colors.PrimaryText[0],
                    AppConstants.Colors.PrimaryText[1],
                    AppConstants.Colors.PrimaryText[2]
                ),
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                BorderStyle = BorderStyle.None,
                TextAlign = HorizontalAlignment.Center
            };

            return input;
        }

        /// <summary>
        /// Creates a modern styled button with hover effects
        /// </summary>
        public static Button CreateButton(string text, int width, int height, int x, int y, Color backgroundColor)
        {
            var button = new Button
            {
                Text = text,
                Font = new Font("Segoe UI Semibold", AppConstants.ButtonFontSize, FontStyle.Bold),
                Size = new Size(width, height),
                Location = new Point(x, y),
                FlatStyle = FlatStyle.Flat,
                BackColor = backgroundColor,
                ForeColor = Color.White,
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand,
                TabStop = false
            };

            // Round the corners
            button.Region = new Region(CreateRoundedRectanglePath(
                new Rectangle(0, 0, button.Width, button.Height), 
                15));

            // Hover effects
            Color originalColor = backgroundColor;
            Color hoverColor = Color.FromArgb(
                Math.Min(255, backgroundColor.R + 30),
                Math.Min(255, backgroundColor.G + 30),
                Math.Min(255, backgroundColor.B + 30)
            );

            button.MouseEnter += (s, e) =>
            {
                button.BackColor = hoverColor;
            };

            button.MouseLeave += (s, e) =>
            {
                button.BackColor = originalColor;
            };

            button.MouseDown += (s, e) =>
            {
                button.BackColor = Color.FromArgb(
                    Math.Max(0, backgroundColor.R - 20),
                    Math.Max(0, backgroundColor.G - 20),
                    Math.Max(0, backgroundColor.B - 20)
                );
            };

            button.MouseUp += (s, e) =>
            {
                button.BackColor = button.ClientRectangle.Contains(button.PointToClient(Cursor.Position)) 
                    ? hoverColor 
                    : originalColor;
            };

            return button;
        }

        /// <summary>
        /// Creates the modern start/stop button
        /// </summary>
        public static Button CreateStartButton(int width, int height, int x, int y)
        {
            return CreateButton(
                AppConstants.StartButtonText,
                width,
                height,
                x,
                y,
                Color.FromArgb(
                    AppConstants.Colors.StartButtonColor[0],
                    AppConstants.Colors.StartButtonColor[1],
                    AppConstants.Colors.StartButtonColor[2]
                )
            );
        }

        /// <summary>
        /// Updates button appearance for start/stop states with modern styling
        /// </summary>
        public static void UpdateButtonForState(Button button, bool isRunning)
        {
            if (isRunning)
            {
                button.Text = AppConstants.StopButtonText;
                button.BackColor = Color.FromArgb(
                    AppConstants.Colors.StopButtonColor[0],
                    AppConstants.Colors.StopButtonColor[1],
                    AppConstants.Colors.StopButtonColor[2]
                );
            }
            else
            {
                button.Text = AppConstants.StartButtonText;
                button.BackColor = Color.FromArgb(
                    AppConstants.Colors.StartButtonColor[0],
                    AppConstants.Colors.StartButtonColor[1],
                    AppConstants.Colors.StartButtonColor[2]
                );
            }
        }

        /// <summary>
        /// Creates a modern card-like panel for grouping controls
        /// </summary>
        public static Panel CreateCard(int width, int height, int x, int y)
        {
            var card = new Panel
            {
                Size = new Size(width, height),
                Location = new Point(x, y),
                BackColor = Color.FromArgb(
                    AppConstants.Colors.PanelAccent[0],
                    AppConstants.Colors.PanelAccent[1],
                    AppConstants.Colors.PanelAccent[2]
                )
            };

            card.Paint += (s, e) =>
            {
                if (s is Panel p)
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (GraphicsPath path = CreateRoundedRectanglePath(
                        new Rectangle(0, 0, p.Width, p.Height), 16))
                    {
                        p.Region = new Region(path);
                    }
                }
            };

            return card;
        }

        /// <summary>
        /// Creates a progress bar visualization
        /// </summary>
        public static ProgressBar CreateProgressBar(int width, int height, int x, int y)
        {
            var progress = new ProgressBar
            {
                Size = new Size(width, height),
                Location = new Point(x, y),
                Style = ProgressBarStyle.Continuous,
                ForeColor = Color.FromArgb(
                    AppConstants.Colors.PrimaryAccent[0],
                    AppConstants.Colors.PrimaryAccent[1],
                    AppConstants.Colors.PrimaryAccent[2]
                )
            };

            return progress;
        }

        private static GraphicsPath CreateRoundedRectanglePath(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();
            int diameter = radius * 2;

            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
