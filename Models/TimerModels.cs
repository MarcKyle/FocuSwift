using PomodoroApp.Constants;

namespace PomodoroApp.Models
{
    /// <summary>
    /// Represents the state of the Pomodoro timer
    /// </summary>
    public enum TimerState
    {
        Idle,
        Running,
        Paused,
        Break
    }

    /// <summary>
    /// Event arguments for timer tick events
    /// </summary>
    public class TimerTickEventArgs : EventArgs
    {
        public int RemainingSeconds { get; }
        public TimerState State { get; }

        public TimerTickEventArgs(int remainingSeconds, TimerState state)
        {
            RemainingSeconds = remainingSeconds;
            State = state;
        }
    }

    /// <summary>
    /// Event arguments for timer completion events
    /// </summary>
    public class TimerCompletedEventArgs : EventArgs
    {
        public TimerState CompletedState { get; }

        public TimerCompletedEventArgs(TimerState completedState)
        {
            CompletedState = completedState;
        }
    }
}
