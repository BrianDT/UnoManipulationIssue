// <copyright file="MainPage.xaml.cs" company="Visual Software Systems Ltd.">Copyright (c) 2016, 2024 All rights reserved</copyright>
namespace UnoManipulationIssue;

using Microsoft.UI.Xaml.Input;
using UnoManipulationIssue.ViewModels;

/// <summary>
/// The main page.
/// </summary>
public sealed partial class MainPage : Page
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainPage"/> class.
    /// </summary>
    public MainPage()
    {
        this.InitializeComponent();

        this.DataContext = new MainViewModel();

        this.Loaded += (s, e) =>
        {
            this.VM.OnLoaded();
        };
    }

    /// <summary>
    /// Gets the interface to the view model for compile time binding
    /// </summary>
    public IMainViewModel VM => this.DataContext as IMainViewModel;

    /// <summary>
    /// The event handler called when the page size changes
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The event args</param>
    private void OnDrawingSurfaceSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (e.NewSize.Width > 0 && e.NewSize.Height > 0)
        {
            if (this.VM != null)
            {
                this.VM.OnDrawingSurfaceSizeChanged(e.NewSize.Width, e.NewSize.Height);
            }
        }
    }

    private void Path_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
    {
        this.VM.TrackGestureStart(e.Position.X, e.Position.Y);
    }

    private void Path_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
    {
        this.VM.TrackGesture(e.Position.X, e.Position.Y);
    }

    private void Path_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
    {
        this.VM.TrackGestureStop(e.Position.X, e.Position.Y);
    }
}
