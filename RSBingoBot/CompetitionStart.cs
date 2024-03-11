// <copyright file="CompetitionStart.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot;

using RSBingo_Framework.DAL;
using System;
using System.Timers;

public static class CompetitionStart
{
    // There's a minimum value required for the timer to work; 1ms didn't seem to work so it's 1s to be safe.
    private const int TimeSpanMinimum = 1000;

    private static Timer timer = null!;

    // If the competition start date is in the past.
    private static bool alreadyStarted = false;

    public delegate void EventArgs();
    public delegate void EventArgsAsync();

    /// <summary>
    /// Called when the competition starts. If the competition has already started when the bot loads,
    /// this will not be called.
    /// </summary>
    public static event EventArgs OnCompetitionStart = null!;

    /// <inheritdoc cref="OnCompetitionStart"/>
    public static event EventArgsAsync OnCompetitionStartAsync = null!;

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
            return GetTimeSpan();
        }
        catch
        {
            int maxDays = TimeSpan.FromMilliseconds(int.MaxValue).Days;
            throw new ArgumentOutOfRangeException("CompetitionStartDateTime", "The start date specified is too far into the future; " +
                $"It cannot exceed {maxDays} days from the point of execution. Allow leeway for start up time.");
        }
    }

    private static double GetTimeSpan()
    {
        double timeSpan = (DataFactory.CompetitionStartDateTime - DateTime.UtcNow).TotalMilliseconds;
        if (timeSpan > int.MaxValue)
        {
            throw new ArgumentOutOfRangeException("Start DateTime.");
        }

        if (timeSpan < TimeSpanMinimum)
        {
            alreadyStarted = true;
            return TimeSpanMinimum;
        }

        return timeSpan;
    }

    private static void TimerElapsed(object? obj, ElapsedEventArgs args)
    {
        General.HasCompetitionStarted = true;

        if (alreadyStarted is false)
        {
            OnCompetitionStart?.Invoke();
            OnCompetitionStartAsync?.Invoke();
        }
    }
}