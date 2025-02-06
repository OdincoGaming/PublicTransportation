using UnityEngine;
using TMPro;
using System.Collections;
public class TimerCanvasBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject timerObj;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private VoidEventChannelSO timerFinishedEventChannel;
    private float timeToComplete = 0;
    private float timeLeft = 0;
    private Timer timer;
    [SerializeField] private RectTransform pingTransform;
    [SerializeField] private Animator pingAnimator;

    private Vector3 pingPosition;

    public void StartTimer()
    {
        timer = Timer.Register
        (
            duration: timeToComplete,
            onComplete: () => timerFinishedEventChannel.RaiseEvent(),
            onUpdate: secondsElapsed =>
            {
                timerText.text = ConvertToMinutesAndSeconds(timer.GetTimeRemaining());
            },
            isLooped: false,
            useRealTime: true
        );
        timerObj.SetActive(true);
    }

    private string ConvertToMinutesAndSeconds(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60);
        int seconds = Mathf.FloorToInt(totalSeconds % 60);
        int milliseconds = Mathf.FloorToInt((totalSeconds % 1) * 1000);

        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    public void SetPing(Vector3 v3)
    {
        pingTransform.localPosition = Camera.main.WorldToScreenPoint(v3);
        pingPosition = v3;
    }

    IEnumerator PingUpdate(Vector3 v3)
    {
        yield return new WaitForEndOfFrame();
        SetPing(v3);
        if (timer.GetRatioRemaining() >= .9)
        {
            StartCoroutine(PingUpdate(pingPosition));
        }
    }

    public void StartPing()
    {
        pingAnimator.SetTrigger("Ping");
        StartCoroutine(PingUpdate(pingPosition));
    }

    public void SetTimetoComplete(float t)
    {
        timeToComplete = t;
    }
}
