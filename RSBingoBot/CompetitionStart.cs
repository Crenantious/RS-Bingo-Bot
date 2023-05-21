// <copyright file="CompetitionStart.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot;

using RSBingo_Framework.DAL;
using System;
using System.Timers;

public static class CompetitionStart
{
    private static System.Timers.Timer timer;

    public delegate void EventArgs();
    public delegate void EventArgsAsync();

    public static event EventArgs CompetitionStarted;
    public static event EventArgsAsync CompetitionStartedAsync;

    public static void Setup()
    {
        timer = new(GetTimerDuration());
        timer.Elapsed += TimerElapsed;
        timer.AutoReset = false;
        timer.Enabled = true;
    }

    private static double GetTimerDuration()
    {
        try
        {
            double timeSpan = (DataFactory.CompetitionStartDateTime - DateTime.UtcNow).TotalMilliseconds;
            if (timeSpan > int.MaxValue) { throw new ArgumentOutOfRangeException("Start DateTime."); }

            // 1000 is arbitrary; just needs to be > 0. 1ms didn't seem to work so it's 1s to be safe.
            return timeSpan > 0 ? timeSpan : 1000;
        }
        catch
        {
            int maxDays = TimeSpan.FromMilliseconds(int.MaxValue).Days;
            throw new ArgumentOutOfRangeException("CompetitionStartDateTime", "The start date specified is too far into the future; " +
                $"It cannot exceed {maxDays} days from the point of execution. Allow leeway for start up time.");
        }
    }

    private static void TimerElapsed(object? obj, ElapsedEventArgs args)
    {
        General.HasCompetitionStarted = true;
        CompetitionStarted?.Invoke();
        CompetitionStartedAsync?.Invoke();
    }
}