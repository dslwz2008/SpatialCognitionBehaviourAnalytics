using UnityEngine;
using System;
using System.Collections;

public class Timer : MonoBehaviour {

    public bool stop = true;
    public float timeLeft = 0.0f;
    private float lastTime = 0.0f;

    public event Action OnTimerStarted;
    public event Action OnTimerTicked;
    public event Action OnTimerFinished;

    public void StartTimer(float from)
    {
        timeLeft = from;
        lastTime = timeLeft;
        stop = false;
        if (OnTimerStarted != null)
        {
            OnTimerStarted();
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (stop) return;
        if (timeLeft < 0)//time is up
        {
            stop = true;
            if (OnTimerFinished != null)
            {
                OnTimerFinished();
            }
            return;
        }
        timeLeft -= Time.deltaTime;
        if (lastTime - timeLeft > 1.0f)
        {
            if (OnTimerTicked != null)
            {
                OnTimerTicked();
            }
            lastTime = timeLeft;
        }
    }
}
