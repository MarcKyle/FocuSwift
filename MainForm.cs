using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PomodoroApp.Constants;
using PomodoroApp.Models;
using PomodoroApp.Services;
using PomodoroApp.UI;
using PomodoroApp.Utils;

namespace PomodoroApp
{
    /// <summary>
    /// Main form for the FocusSwift Pomodoro Timer application
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly PomodoroTimerService _timerService;
        private readonly OverlayManager _overlayManager;
        private Label? _timeLabel;
        private Label? _statusLabel;
        private Button? _startButton;
        private ProgressBar? _progressBar;
        private Panel? _timerCard;
        private bool _disposed;

        public MainForm()
        {
            _timerService = new PomodoroTimerService();
            _overlayManager = new OverlayManager();

            InitializeComponent();
            InitializeUI();
            AttachEventHandlers();

            try
            {
                this.Icon = new Icon(AppConstants.IconPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load icon: {ex.Message}");
            }
        }

        private void InitializeComponent()
        {
            // This method is required by the Windows Forms designer
            // but we're creating controls programmatically in InitializeUI
        }

        private void AttachEventHandlers()
        {
            _timerService.TimerTick += OnTimerTick;
            _timerService.TimerCompleted += OnTimerCompleted;
            _timerService.StateChanged += OnTimerStateChanged;
        }

        private void InitializeUI()
        {
            ConfigureFormProperties();
            var mainPanel = CreateMainPanel();
            CreateHeader(mainPanel);
            CreateTimerCard(mainPanel);
            CreateSettingsSection(mainPanel);
            CreateActionButton(mainPanel);
        }

        private void ConfigureFormProperties()
        {
            this.Text = AppConstants.AppTitle;
            this.Size = new Size(AppConstants.MainFormWidth, AppConstants.MainFormHeight);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(
                AppConstants.Colors.BackgroundColor[0],
                AppConstants.Colors.BackgroundColor[1],
                AppConstants.Colors.BackgroundColor[2]
            );
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private Panel CreateMainPanel()
        {
            var mainPanel = UIComponentFactory.CreateRoundedPanel(
                AppConstants.MainPanelWidth,
                AppConstants.MainPanelHeight,
                AppConstants.PanelMargin,
                AppConstants.PanelMargin
            );
            this.Controls.Add(mainPanel);
            return mainPanel;
        }

        private void CreateHeader(Panel parent)
        {
            // App title
            var titleLabel = new Label
            {
                Text = AppConstants.AppTitle,
                Font = new Font("Segoe UI", 28f, FontStyle.Bold),
                ForeColor = Color.FromArgb(
                    AppConstants.Colors.PrimaryText[0],
                    AppConstants.Colors.PrimaryText[1],
                    AppConstants.Colors.PrimaryText[2]
                ),
                AutoSize = true,
                Location = new Point(40, 30),
                BackColor = Color.Transparent
            };
            parent.Controls.Add(titleLabel);

            // Subtitle
            var subtitleLabel = UIComponentFactory.CreateLabel(
                AppConstants.AppSubtitle,
                AppConstants.SubtitleFontSize,
                40,
                85,
                true,
                true
            );
            parent.Controls.Add(subtitleLabel);
        }

        private void CreateTimerCard(Panel parent)
        {
            // Create timer card - increased height for better spacing
            _timerCard = UIComponentFactory.CreateCard(460, 250, 40, 120);
            parent.Controls.Add(_timerCard);

            // Status label
            _statusLabel = UIComponentFactory.CreateSectionTitle(
                AppConstants.FocusTimeText,
                0,
                25
            );
            _statusLabel.AutoSize = false;
            _statusLabel.TextAlign = ContentAlignment.MiddleCenter;
            _statusLabel.Width = 460;
            _timerCard.Controls.Add(_statusLabel);

            // Timer display
            _timeLabel = new Label
            {
                Text = TimeFormatter.FormatTime(_timerService.RemainingSeconds),
                Font = new Font("Segoe UI", AppConstants.TimerLabelFontSize, FontStyle.Bold),
                ForeColor = Color.FromArgb(
                    AppConstants.Colors.PrimaryAccent[0],
                    AppConstants.Colors.PrimaryAccent[1],
                    AppConstants.Colors.PrimaryAccent[2]
                ),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(460, 120),
                Location = new Point(0, 60),
                Name = "timeLabel",
                BackColor = Color.Transparent
            };
            _timerCard.Controls.Add(_timeLabel);

            // Progress bar
            _progressBar = UIComponentFactory.CreateProgressBar(420, 8, 20, 210);
            _progressBar.Maximum = _timerService.PomodoroDurationMinutes * 60;
            _progressBar.Value = _timerService.RemainingSeconds;
            _timerCard.Controls.Add(_progressBar);
        }

        private void CreateSettingsSection(Panel parent)
        {
            int yPos = 390;
            
            // Settings container - increased height for better spacing
            var settingsCard = UIComponentFactory.CreateCard(460, 150, 40, yPos);
            parent.Controls.Add(settingsCard);

            // Pomodoro settings
            CreateDurationControl(
                settingsCard,
                AppConstants.PomodoroLabelText,
                AppConstants.DefaultPomodoroDurationMinutes,
                AppConstants.MinPomodoroDurationMinutes,
                AppConstants.MaxPomodoroDurationMinutes,
                35,
                (value) =>
                {
                    _timerService.PomodoroDurationMinutes = (int)value;
                    UpdateTimerDisplay();
                    UpdateProgressBar();
                }
            );

            // Break settings
            CreateDurationControl(
                settingsCard,
                AppConstants.BreakLabelText,
                AppConstants.DefaultBreakDurationMinutes,
                AppConstants.MinBreakDurationMinutes,
                AppConstants.MaxBreakDurationMinutes,
                95,
                (value) => _timerService.BreakDurationMinutes = (int)value
            );
        }

        private void CreateDurationControl(Panel parent, string labelText, int defaultValue, int min, int max, int yPos, Action<decimal> onChange)
        {
            // Label
            var label = UIComponentFactory.CreateSectionTitle(labelText, 30, yPos);
            parent.Controls.Add(label);

            // Numeric input
            var input = UIComponentFactory.CreateNumericInput(
                defaultValue,
                min,
                max,
                100,
                320,
                yPos - 10
            );
            input.ValueChanged += (s, e) => onChange(input.Value);
            parent.Controls.Add(input);

            // Minutes label - positioned after the input with proper spacing
            var minutesLabel = UIComponentFactory.CreateLabel(
                AppConstants.MinutesText,
                AppConstants.SubtitleFontSize,
                245,
                yPos,
                true,
                true
            );
            parent.Controls.Add(minutesLabel);
        }

        private void CreateActionButton(Panel parent)
        {
            _startButton = UIComponentFactory.CreateStartButton(380, 60, 80, 560);
            _startButton.Click += StartButton_Click;
            parent.Controls.Add(_startButton);
        }

        #region Event Handlers

        private void OnTimerTick(object? sender, TimerTickEventArgs e)
        {
            UpdateTimerDisplay();
            UpdateStatusLabel();

            if (e.State == TimerState.Break)
            {
                UpdateBreakOverlay();
            }
        }

        private void OnTimerCompleted(object? sender, TimerCompletedEventArgs e)
        {
            if (e.CompletedState == TimerState.Running)
            {
                // Pomodoro completed, start break
                ShowBreakOverlay();
                UpdateStatusLabel();
            }
            else if (e.CompletedState == TimerState.Break)
            {
                // Break completed
                _overlayManager.HideOverlay();
                UpdateStatusLabel();
            }
        }

        private void OnTimerStateChanged(object? sender, EventArgs e)
        {
            UpdateButtonState();
            UpdateStatusLabel();
        }

        private void StartButton_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_timerService.CurrentState == TimerState.Running || 
                    _timerService.CurrentState == TimerState.Break)
                {
                    _timerService.Stop();
                    
                    if (_overlayManager.IsOverlayVisible)
                    {
                        _overlayManager.HideOverlay();
                    }
                }
                else
                {
                    _timerService.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        #endregion

        #region UI Update Methods

        private void UpdateTimerDisplay()
        {
            if (_timeLabel != null)
            {
                _timeLabel.Text = TimeFormatter.FormatTime(_timerService.RemainingSeconds);
            }
            UpdateProgressBar();
        }

        private void UpdateProgressBar()
        {
            if (_progressBar != null)
            {
                int totalSeconds = _timerService.CurrentState == TimerState.Break
                    ? _timerService.BreakDurationMinutes * 60
                    : _timerService.PomodoroDurationMinutes * 60;
                
                _progressBar.Maximum = totalSeconds;
                _progressBar.Value = Math.Max(0, Math.Min(_timerService.RemainingSeconds, totalSeconds));
            }
        }

        private void UpdateStatusLabel()
        {
            if (_statusLabel != null)
            {
                _statusLabel.Text = _timerService.CurrentState == TimerState.Break
                    ? AppConstants.BreakTimeText.ToUpper()
                    : AppConstants.FocusTimeText.ToUpper();
            }
        }

        private void UpdateButtonState()
        {
            if (_startButton != null)
            {
                bool isRunning = _timerService.CurrentState == TimerState.Running || 
                                _timerService.CurrentState == TimerState.Break;
                UIComponentFactory.UpdateButtonForState(_startButton, isRunning);
            }
        }

        private void ShowBreakOverlay()
        {
            string message = string.Format(
                AppConstants.BreakMessageFormat,
                TimeFormatter.FormatTime(_timerService.RemainingSeconds)
            );
            _overlayManager.ShowOverlay(message);
        }

        private void UpdateBreakOverlay()
        {
            if (_overlayManager.IsOverlayVisible)
            {
                string message = string.Format(
                    AppConstants.BreakMessageFormat,
                    TimeFormatter.FormatTime(_timerService.RemainingSeconds)
                );
                _overlayManager.UpdateOverlayText(message);
            }
        }

        #endregion

        #region Cleanup

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                // Stop timer and hide overlay before closing
                if (_timerService.CurrentState == TimerState.Running || 
                    _timerService.CurrentState == TimerState.Break)
                {
                    _timerService.Stop();
                }

                if (_overlayManager.IsOverlayVisible)
                {
                    _overlayManager.HideOverlay();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during form closing: {ex.Message}");
            }
            finally
            {
                base.OnFormClosing(e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _timerService?.Dispose();
                    _overlayManager?.Dispose();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}