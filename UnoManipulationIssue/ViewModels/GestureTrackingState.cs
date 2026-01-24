// <copyright file="GestureTrackingState.cs" company="Visual Software Systems Ltd.">Copyright (c) 2026 All rights reserved</copyright>
namespace UnoManipulationIssue.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// An enumeration of gen sture tracking states
/// </summary>
public enum GestureTrackingState
{
    /// <summary>
    /// Gesture Tracking is not being used
    /// </summary>
    NotActive = 0,

    /// <summary>
    /// Gesture Tracking is active
    /// </summary>
    Active = 1,

    /// <summary>
    /// Gesture Tracking has finished but the final deltas have not been used yet
    /// </summary>
    StopedPendingFinalUse,
}
