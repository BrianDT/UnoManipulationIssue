// <copyright file=Main.iOS.cs" company="Visual Software Systems Ltd.">Copyright (c) 2020 - 2024 All rights reserved</copyright>
namespace UnoManipulationIssue.iOS;

using UIKit;
using Uno.UI.Hosting;

/// <summary>
/// The entry point for the iOS head.
/// </summary>
public class EntryPoint
{
    /// <summary>
    /// This is the main entry point of the application.
    /// </summary>
    /// <param name="args">Any arguiments</param>
    public static void Main(string[] args)
    {
        App.InitializeLogging();

        var host = UnoPlatformHostBuilder.Create()
            .App(() => new App())
            .UseAppleUIKit()
            .Build();

        host.Run();
    }
}
