using UnityEngine;
using System;

public class ActionOnTimer : MonoBehaviour {
    private Action timerCallback;
    private float timer;

    public void StartTimer(float timer, Action timerCallback) {
        this.timer = timer;
        this.timerCallback = timerCallback;
    }

    private void Update() {
        if(timer > 0.0f) {
            timer -= Time.deltaTime;

            if (IsTimerComplete()) {
                timerCallback();
            }
        }
    }

    public bool IsTimerComplete() {
        return timer <= 0.0f;
    }
}
