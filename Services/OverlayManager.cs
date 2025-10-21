using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using PomodoroApp.Constants;

namespace PomodoroApp.Services
{
    /// <summary>
    /// Manages dark overlay forms for break periods
    /// </summary>
    public class OverlayManager : IDisposable
    {
        private readonly List<Form> _overlayForms;
        private Label? _primaryOverlayLabel;
        private bool _isInputBlocked;
        private bool _disposed;

        [DllImport("user32.dll")]
        private static extern bool BlockInput(bool fBlockIt);

        public bool IsOverlayVisible => _overlayForms.Any(f => f.Visible);

        public OverlayManager()
        {
            _overlayForms = new List<Form>();
            InitializeOverlays();
        }

        private void InitializeOverlays()
        {
            foreach (var screen in Screen.AllScreens)
            {
                var overlayForm = CreateOverlayForm(screen);
                _overlayForms.Add(overlayForm);

                if (screen == Screen.PrimaryScreen)
                {
                    _primaryOverlayLabel = overlayForm.Controls.OfType<Label>().FirstOrDefault();
                }
            }
        }

        private Form CreateOverlayForm(Screen screen)
        {
            var overlayForm = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                WindowState = FormWindowState.Normal,
                StartPosition = FormStartPosition.Manual,
                Bounds = screen.Bounds,
                BackColor = Color.FromArgb(
                    AppConstants.Colors.OverlayColor[0],
                    AppConstants.Colors.OverlayColor[1],
                    AppConstants.Colors.OverlayColor[2]
                ),
                Opacity = AppConstants.OverlayOpacity,
                TopMost = true,
                ShowInTaskbar = false
            };

            var overlayLabel = new Label
            {
                Font = new Font("Segoe UI", AppConstants.OverlayFontSize, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };

            overlayForm.Controls.Add(overlayLabel);
            return overlayForm;
        }

        public void ShowOverlay(string message)
        {
            try
            {
                UpdateOverlayText(message);
                
                foreach (var overlayForm in _overlayForms)
                {
                    overlayForm.Show();
                }

                BlockInput(true);
                _isInputBlocked = true;
            }
            catch (Exception ex)
            {
                // Log or handle error gracefully
                System.Diagnostics.Debug.WriteLine($"Error showing overlay: {ex.Message}");
            }
        }

        public void HideOverlay()
        {
            try
            {
                foreach (var overlayForm in _overlayForms)
                {
                    overlayForm.Hide();
                }

                if (_isInputBlocked)
                {
                    BlockInput(false);
                    _isInputBlocked = false;
                }
            }
            catch (Exception ex)
            {
                // Log or handle error gracefully
                System.Diagnostics.Debug.WriteLine($"Error hiding overlay: {ex.Message}");
                
                // Ensure input is unblocked even if hiding fails
                if (_isInputBlocked)
                {
                    BlockInput(false);
                    _isInputBlocked = false;
                }
            }
        }

        public void UpdateOverlayText(string text)
        {
            if (_primaryOverlayLabel != null)
            {
                _primaryOverlayLabel.Text = text;
                CenterLabel(_primaryOverlayLabel);
            }
        }

        private void CenterLabel(Label label)
        {
            if (_overlayForms.Count > 0 && label.Parent is Form parentForm)
            {
                label.Location = new Point(
                    (parentForm.Bounds.Width - label.Width) / 2,
                    (parentForm.Bounds.Height - label.Height) / 2
                );
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Ensure input is unblocked
                    if (_isInputBlocked)
                    {
                        BlockInput(false);
                        _isInputBlocked = false;
                    }

                    // Dispose all overlay forms
                    foreach (var form in _overlayForms)
                    {
                        form.Dispose();
                    }
                    _overlayForms.Clear();
                }
                _disposed = true;
            }
        }
    }
}
