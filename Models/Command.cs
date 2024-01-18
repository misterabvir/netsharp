﻿namespace App.Models;

public class Command
{
    public string Name { get; private set; }
    private Command(string name) => Name = name;
    public bool Is(string? text) => !string.IsNullOrEmpty(text) && Name.ToLower().Equals(text.ToLower());
    public static Command Exit => new("/exit");
    public static Command Join => new("/join");
    public static Command ShutDown => new("/shutdown");
}
