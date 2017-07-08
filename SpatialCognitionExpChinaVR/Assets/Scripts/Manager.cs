using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Manager : MonoBehaviour
{
    public Text countDownText;
    public GameObject finishText;
    public Timer timer;

    public FirstPersonController controller;
    public GameObject startUI;
    public float totalTime = 300.0f;

    void OnEnable()
    {
        timer.OnTimerTicked += UpdateUI;
        timer.OnTimerFinished += ShowEndUI;
    }

    void OnDisable()
    {
        timer.OnTimerTicked -= UpdateUI;
        timer.OnTimerFinished -= ShowEndUI;
    }

    // Use this for initialization
    void Start () {
        controller.enabled = false;
	}

    public void StartTiming()
    {
        controller.enabled = true;
        startUI.SetActive(false);
        timer.StartTimer(totalTime);
    }

    public void Exit()
    {
        Application.Quit();
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    private void UpdateUI()
    {
        //Debug.Log(timeLeft.ToString("F0"));
        countDownText.text = timer.timeLeft.ToString("F0");
        if (timer.timeLeft < 30.0f)
        {
            countDownText.color = Color.red;
        }
    }

    private void ShowEndUI()
    {
        controller.enabled = false;
        finishText.SetActive(true);
    }
}
