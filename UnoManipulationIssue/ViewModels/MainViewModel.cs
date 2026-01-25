// <copyright file="MainViewModel.cs" company="Visual Software Systems Ltd.">Copyright (c) 2026 All rights reserved</copyright>
namespace UnoManipulationIssue.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json.Linq;

/// <summary>
/// The view model of the main page.
/// </summary>
public partial class MainViewModel : ObservableObject, IMainViewModel
{
    private int gestureTrackingFactor = 2;

    private double gestureDelaX = 0.0D;

    private double gestureDelaY = 0.0D;

    private double lastX;

    private double lastY;

    /// <summary>
    /// The redraw frequency
    /// </summary>
    private int freq;

    private CancellationTokenSource? gameLoopCancellationTokenSource;

    /// <summary>
    /// The async task runnig the game loop.
    /// </summary>
    private Task? gameLoopTask;

    /// <summary>
    /// True until the stop button is pressed
    /// </summary>
    private bool keepGoing;

    /// <summary>
    /// Indicates whether gesture tracking is in use.
    /// </summary>
    private GestureTrackingState useGestureDeltas = GestureTrackingState.NotActive;

    /// <summary>
    /// The current drawing area width
    /// </summary>
    private double drawingAreaWidth;

    /// <summary>
    /// The current drawing area height
    /// </summary>
    private double drawingAreaHeight;

    private bool responseFlip = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
    public MainViewModel()
    {
        this.freq = 250;
    }

    /// <summary>
    /// Gets the X co-ordinate of the sprite
    /// </summary>
    [ObservableProperty]
    public partial double XPosition { get; private set; }

    /// <summary>
    /// Gets the Y co-ordinate of the sprite
    /// </summary>
    [ObservableProperty]
    public partial double YPosition { get; private set; }

    /// <summary>
    /// Gets the button hit response
    /// </summary>
    [ObservableProperty]
    public partial string? Response { get; private set; }

    /// <summary>
    /// Gets the window size state
    /// </summary>
    [ObservableProperty]
    public partial string? WindowSizeState { get; private set; }

    /// <summary>
    /// Called when the page and visual elements have been loaded
    /// </summary>
    public void OnLoaded()
    {
        this.keepGoing = true;
        this.gameLoopCancellationTokenSource = new CancellationTokenSource();
        var token = this.gameLoopCancellationTokenSource.Token;

        this.gameLoopTask = Task.Run(
            async () =>
            {
                await this.Draw();
            },
            token);
    }

    /// <summary>
    /// Called when the page size changes
    /// </summary>
    /// <param name="width">The page width</param>
    /// <param name="height">The page height</param>
    public void OnPageSizeChanged(double width, double height)
    {
        string? newState = null;
        if (height > width)
        {
            newState = "Portrait";
        }
        else
        {
            newState = "Landscape";
        }

        if (this.WindowSizeState != newState)
        {
            this.WindowSizeState = newState;
            this.OnVisualStateChanged(newState);
        }
    }

    /// <summary>
    /// Called when the drawing surface size changes
    /// </summary>
    /// <param name="width">The page width</param>
    /// <param name="height">The page height</param>
    public void OnDrawingSurfaceSizeChanged(double width, double height)
    {
        this.drawingAreaWidth = width;
        this.drawingAreaHeight = height;

        var newX = width / 2.0;
        var newY = height / 2.0;
        if (newX != this.XPosition)
        {
            this.XPosition = newX;
        }

        if (newY != this.YPosition)
        {
            this.YPosition = newY;
        }

        System.Diagnostics.Debug.WriteLine($"Sprite X: {this.XPosition:0.0}, Y: {this.YPosition:0.0}, Drawing area width {width}, height {height}");
    }

    /// <summary>
    /// Starts gesture tracking.
    /// </summary>
    /// <param name="xPosition">The movement in the X direction</param>
    /// <param name="yPosition">The movement in the Y direction</param>
    public void TrackGestureStart(double xPosition, double yPosition)
    {
        this.useGestureDeltas = GestureTrackingState.Active;
        this.lastX = xPosition;
        this.lastY = xPosition;
    }

    /// <summary>
    /// Stop gesture tracking.
    /// </summary>
    /// <param name="xPosition">The movement in the X direction</param>
    /// <param name="yPosition">The movement in the Y direction</param>
    public void TrackGestureStop(double xPosition, double yPosition)
    {
        this.TrackGesture(xPosition, yPosition);
        this.useGestureDeltas = GestureTrackingState.StopedPendingFinalUse;
        this.lastX = 0;
        this.lastY = 0;
    }

    /// <summary>
    /// Tracks a gesture movement.
    /// </summary>
    /// <param name="xPosition">The movement in the X direction</param>
    /// <param name="yPosition">The movement in the Y direction</param>
    public void TrackGesture(double xPosition, double yPosition)
    {
        var deltaX = xPosition - this.lastX;
        var deltaY = yPosition - this.lastY;

        this.lastX = xPosition;
        this.lastY = yPosition;

        this.gestureDelaX += deltaX * this.gestureTrackingFactor;
        this.gestureDelaY += deltaY * this.gestureTrackingFactor;
    }

    /// <summary>
    /// Called when the visual state changes
    /// </summary>
    /// <param name="pageVisualState">The new state</param>
    protected virtual void OnVisualStateChanged(string pageVisualState)
    {
    }

    /// <summary>
    /// Implements the command the records any action furing the maqnipulaqtion
    /// </summary>
    [RelayCommand]
    private Task Strike()
    {
        this.Response += this.responseFlip ? "POW " : "BIFF ";
        this.responseFlip = !this.responseFlip;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Implements the command that stops the sprite movement
    /// </summary>
    [RelayCommand]
    private Task Stop()
    {
        this.keepGoing = false;
        if (this.gameLoopCancellationTokenSource != null)
        {
            this.gameLoopCancellationTokenSource.Cancel();
            this.gameLoopCancellationTokenSource = null;
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Draw the snake
    /// </summary>
    /// <returns>An awaitable task</returns>
    private async Task Draw()
    {
        try
        {
            while (this.keepGoing)
            {
                DateTimeOffset start = DateTimeOffset.UtcNow;

                this.MoveSprite();

                TimeSpan calcTime = DateTimeOffset.UtcNow - start;

                double delay = this.freq - calcTime.TotalMilliseconds;

                await Task.Yield();
                if (delay > 0)
                {
                    await Task.Delay((int)delay);
                }
                else
                {
                    ////this.loggingService.TraceEvent($"Negative draw frequency: {delay:0.0}", 1);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            ////this.loggingService.LogException(ex);
        }
    }

    /// <summary>
    /// The sprite moving loop
    /// </summary>
    private void MoveSprite()
    {
        if (!this.keepGoing || this.useGestureDeltas == GestureTrackingState.NotActive)
        {
            return;
        }

        try
        {
            // Animate the sight
            double deltaX = this.gestureDelaX;
            double deltaY = this.gestureDelaY;
            this.gestureDelaX = 0.0D;
            this.gestureDelaY = 0.0D;
            if (this.useGestureDeltas == GestureTrackingState.StopedPendingFinalUse)
            {
                this.useGestureDeltas = GestureTrackingState.NotActive;
            }

            double x = this.XPosition + deltaX;
            double y = this.YPosition + deltaY;
            var limitX = this.drawingAreaWidth - 40;
            var limitY = this.drawingAreaHeight - 40;

            if (x < 0)
            {
                x = 0;
            }
            else if (x > limitX)
            {
                x = limitX;
            }

            if (y < 0)
            {
                y = 0;
            }
            else if (y > limitY)
            {
                y = limitY;
            }

            System.Diagnostics.Debug.WriteLine($"Sprite X: {this.XPosition:0.0}, Y: {this.YPosition:0.0}");

            this.XPosition = x;
            this.YPosition = y;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }

#if DEBUG
        ////this.loggingService.TraceToLog($"Sprite X: {this.XPosition:0.0}, Y: {this.YPosition:0.0}");
#endif
    }
}
