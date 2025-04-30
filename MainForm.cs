using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PomodoroApp
{
    public partial class MainForm : Form
    {
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Timer breakTimer;
        private int remainingSeconds;
        private int breakRemainingSeconds;
        private int pomodoroDuration = 25 * 60; // Default 25 minutes
        private int breakDuration = 5 * 60; // Default 5 minutes
        private bool isInputBlocked = false;
        private Form darkOverlayForm;
        private Label overlayTimerLabel;

        [DllImport("user32.dll")]
        private static extern bool BlockInput(bool fBlockIt);

        public MainForm()
        {
            InitializeComponent();
            InitializeTimers();
            InitializeUI();
        }

        private void InitializeComponent()
        {
            // This method is required by the Windows Forms designer
            // but we're creating controls programmatically in InitializeUI
        }

        private void InitializeTimers()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1 second
            timer.Tick += Timer_Tick;
            remainingSeconds = pomodoroDuration;

            breakTimer = new System.Windows.Forms.Timer();
            breakTimer.Interval = 1000; // 1 second
            breakTimer.Tick += BreakTimer_Tick;
            breakRemainingSeconds = breakDuration;
        }

        private void InitializeUI()
        {
            this.Text = "FocusSwift - Pomodoro Timer";
            this.Size = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ForeColor = Color.White;

            // Create a rounded panel for the main content
            Panel mainPanel = new Panel();
            mainPanel.Size = new Size(450, 450);
            mainPanel.Location = new Point(25, 25);
            mainPanel.BackColor = Color.FromArgb(45, 45, 45);
            mainPanel.Paint += (s, e) =>
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(0, 0, 20, 20, 180, 90);
                    path.AddArc(mainPanel.Width - 20, 0, 20, 20, 270, 90);
                    path.AddArc(mainPanel.Width - 20, mainPanel.Height - 20, 20, 20, 0, 90);
                    path.AddArc(0, mainPanel.Height - 20, 20, 20, 90, 90);
                    path.CloseFigure();
                    mainPanel.Region = new Region(path);
                }
            };
            this.Controls.Add(mainPanel);

            // Timer label
            Label timeLabel = new Label();
            timeLabel.Text = FormatTime(remainingSeconds);
            timeLabel.Font = new Font("Segoe UI", 48, FontStyle.Bold);
            timeLabel.AutoSize = true;
            timeLabel.Location = new Point(150, 50);
            timeLabel.Name = "timeLabel";
            mainPanel.Controls.Add(timeLabel);

            // Pomodoro duration controls
            Label pomodoroLabel = new Label();
            pomodoroLabel.Text = "Pomodoro Duration (minutes):";
            pomodoroLabel.Font = new Font("Segoe UI", 12);
            pomodoroLabel.Location = new Point(50, 150);
            mainPanel.Controls.Add(pomodoroLabel);

            NumericUpDown pomodoroInput = new NumericUpDown();
            pomodoroInput.Value = 25;
            pomodoroInput.Minimum = 1;
            pomodoroInput.Maximum = 60;
            pomodoroInput.Width = 60;
            pomodoroInput.Location = new Point(250, 150);
            pomodoroInput.ValueChanged += (s, e) =>
            {
                pomodoroDuration = (int)pomodoroInput.Value * 60;
                remainingSeconds = pomodoroDuration;
                UpdateTimeLabel();
            };
            mainPanel.Controls.Add(pomodoroInput);

            // Break duration controls
            Label breakLabel = new Label();
            breakLabel.Text = "Break Duration (minutes):";
            breakLabel.Font = new Font("Segoe UI", 12);
            breakLabel.Location = new Point(50, 200);
            mainPanel.Controls.Add(breakLabel);

            NumericUpDown breakInput = new NumericUpDown();
            breakInput.Value = 5;
            breakInput.Minimum = 1;
            breakInput.Maximum = 30;
            breakInput.Width = 60;
            breakInput.Location = new Point(250, 200);
            breakInput.ValueChanged += (s, e) =>
            {
                breakDuration = (int)breakInput.Value * 60;
                breakRemainingSeconds = breakDuration;
            };
            mainPanel.Controls.Add(breakInput);

            // Start/Stop button
            Button startButton = new Button();
            startButton.Text = "Start";
            startButton.Font = new Font("Segoe UI", 14);
            startButton.Size = new Size(150, 50);
            startButton.Location = new Point(150, 300);
            startButton.FlatStyle = FlatStyle.Flat;
            startButton.BackColor = Color.FromArgb(0, 120, 215);
            startButton.ForeColor = Color.White;
            startButton.FlatAppearance.BorderSize = 0;
            startButton.Click += StartButton_Click;
            mainPanel.Controls.Add(startButton);

            // Initialize dark overlay form
            InitializeDarkOverlay();
        }

        private void InitializeDarkOverlay()
        {
            darkOverlayForm = new Form();
            darkOverlayForm.FormBorderStyle = FormBorderStyle.None;
            darkOverlayForm.WindowState = FormWindowState.Maximized;
            darkOverlayForm.BackColor = Color.FromArgb(0, 0, 0);
            darkOverlayForm.Opacity = 0.8;
            darkOverlayForm.TopMost = true;

            overlayTimerLabel = new Label();
            overlayTimerLabel.Font = new Font("Segoe UI", 72, FontStyle.Bold);
            overlayTimerLabel.ForeColor = Color.White;
            overlayTimerLabel.AutoSize = true;
            overlayTimerLabel.Location = new Point(
                (Screen.PrimaryScreen.Bounds.Width - overlayTimerLabel.Width) / 2,
                (Screen.PrimaryScreen.Bounds.Height - overlayTimerLabel.Height) / 2
            );
            darkOverlayForm.Controls.Add(overlayTimerLabel);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            remainingSeconds--;
            UpdateTimeLabel();

            if (remainingSeconds <= 0)
            {
                timer.Stop();
                ShowDarkOverlay();
                BlockInput(true);
                isInputBlocked = true;
                breakRemainingSeconds = breakDuration;
                breakTimer.Start();
            }
        }

        private void BreakTimer_Tick(object sender, EventArgs e)
        {
            breakRemainingSeconds--;
            UpdateOverlayTimer();

            if (breakRemainingSeconds <= 0)
            {
                breakTimer.Stop();
                HideDarkOverlay();
                BlockInput(false);
                isInputBlocked = false;
                remainingSeconds = pomodoroDuration;
                UpdateTimeLabel();
                MessageBox.Show("Break time is over! Time to get back to work.", "Break Complete");
            }
        }

        private void ShowDarkOverlay()
        {
            darkOverlayForm.Show();
            UpdateOverlayTimer();
        }

        private void HideDarkOverlay()
        {
            darkOverlayForm.Hide();
        }

        private void UpdateOverlayTimer()
        {
            if (darkOverlayForm.Visible)
            {
                overlayTimerLabel.Text = $"Take a break for:\n{FormatTime(breakRemainingSeconds)}";
                overlayTimerLabel.Location = new Point(
                    (Screen.PrimaryScreen.Bounds.Width - overlayTimerLabel.Width) / 2,
                    (Screen.PrimaryScreen.Bounds.Height - overlayTimerLabel.Height) / 2
                );
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (timer.Enabled)
            {
                timer.Stop();
                breakTimer.Stop();
                button.Text = "Start";
                button.BackColor = Color.FromArgb(0, 120, 215);
                if (isInputBlocked)
                {
                    HideDarkOverlay();
                    BlockInput(false);
                    isInputBlocked = false;
                }
            }
            else
            {
                timer.Start();
                button.Text = "Stop";
                button.BackColor = Color.FromArgb(215, 0, 0);
            }
        }

        private void UpdateTimeLabel()
        {
            Control timeLabel = this.Controls.Find("timeLabel", true)[0];
            timeLabel.Text = FormatTime(remainingSeconds);
        }

        private string FormatTime(int seconds)
        {
            int minutes = seconds / 60;
            int remainingSeconds = seconds % 60;
            return $"{minutes:D2}:{remainingSeconds:D2}";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (isInputBlocked)
            {
                BlockInput(false);
            }
            base.OnFormClosing(e);
        }
    }
} 