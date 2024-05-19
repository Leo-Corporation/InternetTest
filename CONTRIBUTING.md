# Contribution

## Summary

- [Knowledge](#knowledge)
- [Tools](#tools)
- [Writing code](#writing-code)

## Knowledge

To contribute to this project, you will need to have some prerequisites:

- A basic knowledge of C# (this project is written in C# 12.0)
- A basic knowledge of XAML
- A basic knowledge of Visual Studio and Blend

## Tools

You will also need to have the following tools:

- Microsoft Visual Studio 2019
  - .NET Desktop Developpement
  - Visual Studio Installer Projects
- Git
- (_optionnal_) Microsoft Visual Studio Code

## Writing code

Make you follow the following guidelines:

1. Use Tabs: To format your code, use tabs intead of spaces:

```cs
class Car
{
    /// <summary>
    /// The maximum speed of the car.
    /// </summary>
    public int MaxSpeed { get; set; }

    /// <summary>
    /// This method does stuff.
    /// </summary>
    public void DoStuff()
    {
        Console.WriteLine("DoStuff"); // Print text
    }
}
```

2. Put your code between `{ }`:

```cs
// Do this
int x = 12; // Define a number
int y = 45; // Define another number

if (x < y) // If y is bigger than x
{
    Console.WriteLine("y is bigger than x"); // Print text
}

// Dont do this
if (x < y) // If y is bigger than x
    Console.WriteLine("y is bigger than x"); // Print text
```

3. Comment your code:

```cs
int a = 10; // Define a number
int b = 15; // Define another number

if (a > b) // If a is bigger than b
{
    //TODO
}
else
{
    //TODO
}
```

4. Use XML Documentation for `public` and `internal` methods, fields and properties:

```cs
/// <summary>
/// This method does stuff.
/// </summary>
internal void DoStuff()
{
    Thread.Sleep(2000); // Do nothing for 2 seconds
}
```

That's pretty much all you need right now. Keep in mind this document can be updated at any time, so make sure to keep checking these guidelines.
