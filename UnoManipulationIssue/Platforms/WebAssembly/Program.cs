// <copyright file="Program.cs" company="Visual Software Systems Ltd.">Copyright (c) 2024 All rights reserved</copyright>
namespace UnoManipulationIssue;

using Uno.UI.Hosting;

/// <summary>
/// The entry point for the wasm browser head.
/// </summary>
public class Program
{
    /// <summary>
    /// The main function entry point.
    /// </summary>
    /// <param name="args">Any arguiments</param>
    /// <returns>The return code</returns>
    public static async Task Main(string[] args)
    {
        App.InitializeLogging();

        var host = UnoPlatformHostBuilder.Create()
            .App(() => new App())
            .UseWebAssembly()
            .Build();

        await host.RunAsync();
    }
}
