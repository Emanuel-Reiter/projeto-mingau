using UnityEngine;
using System;
using System.Collections.Generic;

public class GlobalTimer : MonoBehaviour {
    private Dictionary<int, TimerData> activeTimers = new Dictionary<int, TimerData>();
    private int nextTimerId = 1;

    // Internal data structure to store timer information
    private class TimerData {
        public float Duration;
        public float TimeRemaining;
        public Action Callback;
        public bool IsActive = true;
    }

    public int StartTimer(float duration, Action callback) {
        var timerData = new TimerData {
            Duration = duration,
            TimeRemaining = duration,
            Callback = callback
        };

        int timerId = nextTimerId++;
        activeTimers.Add(timerId, timerData);
        return timerId;
    }

    public bool IsTimerComplete(int timerId) {
        if (!activeTimers.TryGetValue(timerId, out TimerData timer)) {
            return true;
        }
        return !timer.IsActive;
    }

    public void CancelTimer(int timerId) {
        if (activeTimers.ContainsKey(timerId)) {
            activeTimers.Remove(timerId);
        }
    }

    public float GetRemainingTime(int timerId) {
        if (activeTimers.TryGetValue(timerId, out TimerData timer) && timer.IsActive) {
            return timer.TimeRemaining;
        }
        return 0.0f;
    }

    public float GetProgress(int timerId) {
        if (activeTimers.TryGetValue(timerId, out TimerData timer) && timer.IsActive) {
            return 1 - (timer.TimeRemaining / timer.Duration);
        }
        return 1.0f;
    }

    private void Update() {
        List<int> completedTimers = new List<int>();

        // Update all active timers
        foreach (var kvp in activeTimers) {
            int timerId = kvp.Key;
            TimerData timer = kvp.Value;

            if (timer.IsActive) {
                timer.TimeRemaining -= Time.deltaTime;

                if (timer.TimeRemaining <= 0.0f) {
                    timer.TimeRemaining = 0.0f;
                    timer.IsActive = false;
                    timer.Callback?.Invoke();
                    completedTimers.Add(timerId);
                }
            }
        }

        // Cleanup completed timers
        foreach (int timerId in completedTimers) {
            activeTimers.Remove(timerId);
        }
    }
}