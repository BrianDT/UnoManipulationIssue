// <copyright file="IMainViewModel.cs" company="Visual Software Systems Ltd.">Copyright (c) 2026 All rights reserved</copyright>
namespace UnoManipulationIssue.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

/// <summary>
/// The interface to the view model of the main page.
/// </summary>
public interface IMainViewModel : ISupportsTouchPad
{
    /// <summary>
    /// Gets the command the records any action furing the maqnipulaqtion
    /// </summary>
    IAsyncRelayCommand StrikeCommand { get; }

    /// <summary>
    /// Gets the command that stops the sprite movement
    /// </summary>
    IAsyncRelayCommand StopCommand { get; }

    /// <summary>
    /// Gets the X co-ordinate of the sprite
    /// </summary>
    double XPosition { get; }

    /// <summary>
    /// Gets the Y co-ordinate of the sprite
    /// </summary>
    double YPosition { get; }

    /// <summary>
    /// Gets the button hit response
    /// </summary>
    string? Response { get; }

    /// <summary>
    /// Called when the page and visual elements have been loaded
    /// </summary>
    public void OnLoaded();

    /// <summary>
    /// Called when the page drawing surface size changes
    /// </summary>
    /// <param name="width">The page width</param>
    /// <param name="height">The page height</param>
    void OnDrawingSurfaceSizeChanged(double width, double height);
}
