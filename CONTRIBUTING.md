# Contribution
## Table of Contents
- [Ways You Can Contribute](#ways-you-can-contribute)
- [Prerequisites](#prerequisites)
- [Code Guidelines](#code-guidelines)
  - [MVVM & Architecture](#mvvm--architecture)
  - [C# 12 Features](#c-12-features)
  - [Styling & Formatting](#styling--formatting)
  - [Commit Message Guidelines](#commit-message-guidelines)
- [Pull Request Process](#pull-request-process)
## Ways You Can Contribute

We welcome help in many forms:
- **Code**: Fix bugs, enhance features, or refactor for improvement.
- **Documentation**: Improve README, add tutorials, clarify MVVM patterns or usage with C# 12.
- **Testing**: Add unit tests or automate existing workflows.
- **Design & UX**: Improve WPF UI, accessibility, or theme support.
## Prerequisites

To contribute to this project, you will need to have some prerequisites:

- A basic knowledge of C# (this project is written in C# 12.0)
- A basic knowledge of XAML, WPF (MVVM)
- A basic knowledge of the Visual Studio IDE

You will also need to have the following tools:

- Microsoft Visual Studio 2022
  - .NET Desktop Development
- Git
- (_optionnal_) Inno Setup v6.3+
- (_optionnal_) Microsoft Visual Studio Code

## Code Guidelines

### MVVM & Architecture

* Follow the **Model–View–ViewModel** pattern.
* Keep **ViewModels** testable (no direct UI logic).
* Use **INotifyPropertyChanged**, commands, and proper separation of concerns.

### C# 12 Features

* You may use new C# 12 features like `primary constructors`, `list patterns`, and `required` members.

### Styling & Formatting

* Use **consistent indent** (tabs).
* Follow .NET naming conventions (PascalCase for types and properties, camelCase for locals).
* Organize `using`s and sort members logically.

### Commit Messages

* Use clear messages: **Imperative mood**, short subject (≤ 50 chars), blank line, detailed body if needed.
* Reference issues: e.g., `Fixes #123 — resolve null-reference in ViewModel`.
* Keep atomic and focused PRs.


## Pull Request Process

1. Create a feature branch:
   `git checkout -b feature/your-description`
2. Write your code, tests, and documentation.
3. Run tests and build.
4. Push to your fork and open a PR against `main`.
5. In the PR description:
   * Brief summary of changes.
   * Motivation and context.
   * Link to related issue (e.g., `Fixes #...`).
6. Be responsive to review feedback.
