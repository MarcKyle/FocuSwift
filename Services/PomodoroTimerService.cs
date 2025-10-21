using System.Windows.Forms;
using PomodoroApp.Constants;
using PomodoroApp.Models;

namespace PomodoroApp.Services
{
    /// <summary>
    /// Manages Pomodoro timer logic and state
    /// </summary>
    public class PomodoroTimerService : IDisposable
    {
        private System.Windows.Forms.Timer? _workTimer;
        private System.Windows.Forms.Timer? _breakTimer;
        private int _remainingSeconds;
        private int _breakRemainingSeconds;
        private int _pomodoroDuration;
        private int _breakDuration;
        private TimerState _currentState;
        private bool _disposed;

        // Events
        public event EventHandler<TimerTickEventArgs>? TimerTick;
        public event EventHandler<TimerCompletedEventArgs>? TimerCompleted;
        public event EventHandler? StateChanged;

        public TimerState CurrentState
        {
            get => _currentState;
            private set
            {
                if (_currentState != value)
                {
                    _currentState = value;
                    StateChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public int PomodoroDurationMinutes
        {
            get => _pomodoroDuration / AppConstants.SecondsPerMinute;
            set
            {
                _pomodoroDuration = value * AppConstants.SecondsPerMinute;
                if (CurrentState == TimerState.Idle)
                {
                    _remainingSeconds = _pomodoroDuration;
                    OnTimerTick();
                }
            }
        }

        public int BreakDurationMinutes
        {
            get => _breakDuration / AppConstants.SecondsPerMinute;
            set
            {
                _breakDuration = value * AppConstants.SecondsPerMinute;
                if (CurrentState == TimerState.Break)
                {
                    _breakRemainingSeconds = _breakDuration;
                }
            }
        }

        public int RemainingSeconds => CurrentState == TimerState.Break ? _breakRemainingSeconds : _remainingSeconds;

        public PomodoroTimerService()
        {
            _pomodoroDuration = AppConstants.DefaultPomodoroDurationMinutes * AppConstants.SecondsPerMinute;
            _breakDuration = AppConstants.DefaultBreakDurationMinutes * AppConstants.SecondsPerMinute;
            _remainingSeconds = _pomodoroDuration;
            _breakRemainingSeconds = _breakDuration;
            _currentState = TimerState.Idle;

            InitializeTimers();
        }

        private void InitializeTimers()
        {
            _workTimer = new System.Windows.Forms.Timer
            {
                Interval = AppConstants.TimerIntervalMilliseconds
            };
            _workTimer.Tick += WorkTimer_Tick;

            _breakTimer = new System.Windows.Forms.Timer
            {
                Interval = AppConstants.TimerIntervalMilliseconds
            };
            _breakTimer.Tick += BreakTimer_Tick;
        }

        public void Start()
        {
            if (CurrentState == TimerState.Idle || CurrentState == TimerState.Paused)
            {
                _workTimer?.Start();
                CurrentState = TimerState.Running;
            }
        }

        public void Stop()
        {
            _workTimer?.Stop();
            _breakTimer?.Stop();

            if (CurrentState == TimerState.Running || CurrentState == TimerState.Paused)
            {
                CurrentState = TimerState.Paused;
            }
            else if (CurrentState == TimerState.Break)
            {
                CurrentState = TimerState.Idle;
                _remainingSeconds = _pomodoroDuration;
                OnTimerTick();
            }
        }

        public void Reset()
        {
            _workTimer?.Stop();
            _breakTimer?.Stop();
            _remainingSeconds = _pomodoroDuration;
            _breakRemainingSeconds = _breakDuration;
            CurrentState = TimerState.Idle;
            OnTimerTick();
        }

        private void WorkTimer_Tick(object? sender, EventArgs e)
        {
            _remainingSeconds--;
            OnTimerTick();

            if (_remainingSeconds <= 0)
            {
                _workTimer?.Stop();
                OnTimerCompleted(TimerState.Running);
                StartBreak();
            }
        }

        private void BreakTimer_Tick(object? sender, EventArgs e)
        {
            _breakRemainingSeconds--;
            OnTimerTick();

            if (_breakRemainingSeconds <= 0)
            {
                _breakTimer?.Stop();
                OnTimerCompleted(TimerState.Break);
                EndBreak();
            }
        }

        private void StartBreak()
        {
            _breakRemainingSeconds = _breakDuration;
            CurrentState = TimerState.Break;
            _breakTimer?.Start();
            OnTimerTick();
        }

        private void EndBreak()
        {
            _remainingSeconds = _pomodoroDuration;
            CurrentState = TimerState.Idle;
            OnTimerTick();
        }

        private void OnTimerTick()
        {
            TimerTick?.Invoke(this, new TimerTickEventArgs(RemainingSeconds, CurrentState));
        }

        private void OnTimerCompleted(TimerState completedState)
        {
            TimerCompleted?.Invoke(this, new TimerCompletedEventArgs(completedState));
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
                    _workTimer?.Dispose();
                    _breakTimer?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
