// <copyright file="IDispatchOnUIThread.cs" company="Visual Software Systems Ltd.">Copyright (c) 2013, 2024, 2025 All rights reserved</copyright>
namespace Vssl.Samples.ViewModelInterfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// A cross platform implementation of the UI Dispatcher facade
/// </summary>
public interface IDispatchOnUIThread
{
    /// <summary>
    /// Initialise the dispatcher
    /// </summary>
    void Initialise();

    /// <summary>
    /// Execute an action via the dispatcher
    /// </summary>
    /// <param name="action">The action</param>
    void Invoke(Action action);

    /// <summary>
    /// Execute a function via the dispatcher
    /// </summary>
    /// <param name="action">The action</param>
    /// <returns>An awaitable task</returns>
    Task InvokeTask(Func<Task> action);
}
