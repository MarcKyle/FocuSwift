# FocuSwift# FocuSwift



FocuSwift is a simple and elegant Pomodoro timer application designed to help you stay focused and productive. Built with Windows Forms and following modern software architecture principles, it features a clean user interface and customizable timer settings for work and break intervals.FocuSwift is a simple and elegant Pomodoro timer application designed to help you stay focused and productive. Built with Windows Forms, it features a clean user interface and customizable timer settings for work and break intervals.



------



## Features## Features



- **Pomodoro Timer**: Set a work duration (default: 25 minutes) to focus on your tasks.- **Pomodoro Timer**: Set a work duration (default: 25 minutes) to focus on your tasks.

- **Break Timer**: Take short breaks (default: 5 minutes) to recharge after each Pomodoro session.- **Break Timer**: Take short breaks (default: 5 minutes) to recharge after each Pomodoro session.

- **Customizable Durations**: Adjust the work and break durations to fit your preferences.- **Customizable Durations**: Adjust the work and break durations to fit your preferences.

- **Dark Overlay Mode**: Blocks input during breaks to ensure you take a proper rest.- **Dark Overlay Mode**: Blocks input during breaks to ensure you take a proper rest.

- **Multi-Monitor Support**: Overlays appear on all connected screens.- **Rounded UI Design**: A visually appealing interface with rounded panels and modern styling.

- **Rounded UI Design**: A visually appealing interface with rounded panels and modern styling.- **Start/Stop Functionality**: Easily control the timer with a single button.

- **Start/Stop Functionality**: Easily control the timer with a single button.

- **Clean Architecture**: Well-organized codebase following SOLID principles.---



---## Getting Started



## Architecture### Prerequisites



FocuSwift has been completely refactored to follow best practices:- Windows OS

- .NET 6.0 Runtime or later

- **Separation of Concerns**: Business logic separated from UI

- **SOLID Principles**: Each class has a single responsibility### Installation

- **Event-Driven Design**: Decoupled components communicate via events

- **Resource Management**: Proper disposal of all resources1. Clone the repository:

- **Error Handling**: Graceful error handling throughout   ```bash

   git clone https://github.com/MarcKyle/FocuSwift.git

### Project Structure   ```

2. Build the application

```    ```bash

FocuSwift/    dotnet build

├── Constants/        # Application-wide constants    ```

├── Models/          # Data models and event arguments3. Run the application

├── Services/        # Business logic and services    ```bash

├── UI/              # UI component factory    dotnet run

├── Utils/           # Utility functions    ```
├── MainForm.cs      # Main application form
└── Program.cs       # Entry point
```

For detailed architecture documentation, see [ARCHITECTURE.md](ARCHITECTURE.md).

---

## Getting Started

### Prerequisites

- Windows OS
- .NET 6.0 Runtime or later

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/MarcKyle/FocuSwift.git
   ```
2. Build the application:
    ```bash
    dotnet build
    ```
3. Run the application:
    ```bash
    dotnet run
    ```

---

## Usage

1. **Set Work Duration**: Adjust the "Pomodoro Duration (minutes)" value
2. **Set Break Duration**: Adjust the "Break Duration (minutes)" value
3. **Click Start**: Begin your focus session
4. **Work**: The timer counts down your work period
5. **Break**: When work is complete, a full-screen overlay appears for your break
6. **Repeat**: After the break, start a new cycle

---

## Configuration

All application settings can be customized in `Constants/AppConstants.cs`:

- Timer durations and limits
- UI colors and dimensions
- Font sizes
- Text messages

---

## Development

### Building from Source

```bash
# Clone the repository
git clone https://github.com/MarcKyle/FocuSwift.git

# Navigate to project directory
cd FocuSwift/FocuSwift

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

### Code Organization

- **Constants/AppConstants.cs**: Configuration values
- **Services/PomodoroTimerService.cs**: Core timer logic
- **Services/OverlayManager.cs**: Break overlay management
- **UI/UIComponentFactory.cs**: UI component creation
- **Utils/TimeFormatter.cs**: Time formatting utilities
- **Models/TimerModels.cs**: Data models and events

---

## Contributing

Contributions are welcome! Please:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Follow the existing code structure and patterns
4. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
5. Push to the branch (`git push origin feature/AmazingFeature`)
6. Open a Pull Request

---

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## Acknowledgments

- Pomodoro Technique® by Francesco Cirillo
- Built with .NET 6.0 and Windows Forms
- Icons and visual design inspired by modern productivity apps

---

## Contact

Marc Kyle - [@MarcKyle](https://github.com/MarcKyle)

Project Link: [https://github.com/MarcKyle/FocuSwift](https://github.com/MarcKyle/FocuSwift)
