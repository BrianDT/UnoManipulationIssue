// <copyright file="UIDispatcher.cs" company="Visual Software Systems Ltd.">Copyright (c) 2019, 2025 All rights reserved</copyright>

namespace Vssl.Samples.Services;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vssl.Samples.ViewModelInterfaces;

/// <summary>
/// A cross platform implementation of the UI Dispatcher facade
/// </summary>
public class UIDispatcher : IDispatchOnUIThread
{
    /// <summary>
    /// Marshals actions onto the UI thread
    /// </summary>
    private SynchronizationContext? dispatcher;

    /// <summary>
    /// Initialise the dispatcher
    /// </summary>
    public void Initialise()
    {
        this.dispatcher = SynchronizationContext.Current;
    }

    /// <summary>
    /// Dispatch an action onto the UI thread
    /// </summary>
    /// <param name="action">The action</param>
    public void Invoke(Action action)
    {
        if (this.dispatcher == null || SynchronizationContext.Current == this.dispatcher)
        {
            action();
        }
        else
        {
            // Not awated execution will continue before the action has completed
            this.dispatcher.Post((object? state) => action(), null);
        }
    }

    /// <summary>
    /// Execute a function via the dispatcher
    /// </summary>
    /// <param name="action">The action</param>
    /// <returns>An awaitable task</returns>
    public async Task InvokeTask(Func<Task> action)
    {
        if (this.dispatcher == null || SynchronizationContext.Current == this.dispatcher)
        {
            await action();
        }
        else
        {
            // Send is not supported when called from a Windows app
            var tcs = new TaskCompletionSource<object?>();
            try
            {
                this.dispatcher.Post(
                    async (object? state) =>
                    {
                        await action();
                        tcs.TrySetResult(null);
                    },
                    null);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }

            await tcs.Task.ConfigureAwait(false);
        }
    }
}
