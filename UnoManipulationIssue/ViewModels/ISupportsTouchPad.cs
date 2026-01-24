// <copyright file="ISupportsTouchPad.cs" company="Visual Software Systems Ltd.">Copyright (c) 2025, 2026 All rights reserved</copyright>
namespace UnoManipulationIssue.ViewModels;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The interface for viewmodels that support the touch pad control.
/// </summary>
public interface ISupportsTouchPad : INotifyPropertyChanged
{
    /// <summary>
    /// Starts gesture tracking.
    /// </summary>
    /// <param name="xPosition">The movement in the X direction</param>
    /// <param name="yPosition">The movement in the Y direction</param>
    void TrackGestureStart(double xPosition, double yPosition);

    /// <summary>
    /// Tracks a gesture movement.
    /// </summary>
    /// <param name="xPosition">The movement in the X direction</param>
    /// <param name="yPosition">The movement in the Y direction</param>
    void TrackGesture(double xPosition, double yPosition);

    /// <summary>
    /// Stop gesture tracking.
    /// </summary>
    /// <param name="xPosition">The movement in the X direction</param>
    /// <param name="yPosition">The movement in the Y direction</param>
    void TrackGestureStop(double xPosition, double yPosition);
}
