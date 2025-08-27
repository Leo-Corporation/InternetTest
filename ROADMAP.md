# Roadmap and features of InternetTest Pro 9

<img src=".github/images/banner.png" alt="banner">

## 🚀 Project Overview

InternetTest Pro 9 is the next big leap forward—a ground-up rewrite of InternetTest, rearchitected using the MVVM pattern in WPF. We’re retiring the legacy code in favor of a cleaner and more maintainable architecture. Visually, v9 introduces a modern MicaWindow style, aligning with Windows 11 design concepts, and unveils a completely redesigned Home dashboard that consolidates functionality in one streamlined view. The legacy “History” page has been removed to make space for this richer entry point.

## 📋 Roadmap
* **MVVM Architecture**

  * UI Views will bind to `ViewModel` objects exposing properties and `ICommand`s. UI logic stays entirely separate from business logic through `INotifyPropertyChanged` and `ObservableCollection<T>` .

* **MicaWindow Style**

  * Entire app window uses a Mica backdrop (or Mica Alt variant if desired) to adopt OS theme and desktop wallpaper, giving it a lightweight yet visually rich appearance.

* **Redesigned Home Dashboard**

  * Newly designed landing page aggregates:

    * Internet connection checks, Speed Test
    * Recent pings/traceroute
    * Details about the current connection
    * WiFi scanning
  * This becomes the default view; **History page is removed**.

* **Dashboard Widgets & Interaction Patterns**

  * Each section is represented as a widget card with real-time status, most-used actions, and interactive elements (e.g. “Test again” button with a bound `ICommand`).

* New and modern navigation sidebar
* Redesigned Settings page, removal of the accordion UI in favor of cards

