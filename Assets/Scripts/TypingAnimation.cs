using System.Collections;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using TMPro;
using UnityEngine;

public class TypingAnimation : MonoBehaviour
{
    [SerializeField] private string text;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private Animator animator;
    [SerializeField] private TimerCanvasBehaviour timerCanvasBehaviour;

    [SerializeField] private CharController charController;
    [SerializeField] private KinematicCharacterMotor charMotor;
    [SerializeField] private CharPlayer charPlayer;

    [SerializeField] private EndGameTypingAnim egta;

    public void StartTyping()
    {
        StartCoroutine(TypingAnimationEnum(text));
    }

    private IEnumerator TypingAnimationEnum(string s)
    {
        string typedText = "";
        textMeshProUGUI.text = typedText;
        foreach (char c in s)
        {
            typedText += c;
            textMeshProUGUI.text = typedText;
            yield return new WaitForSeconds(.025f);
        }

        yield return new WaitForSeconds(1.5f);
        for (int i = typedText.Length; i > 0; i--)
        {
            typedText = typedText.Substring(0, i);
            textMeshProUGUI.text = typedText;
            yield return new WaitForSeconds(.01f);
        }
        textMeshProUGUI.text = "";
        animator.SetTrigger("OpenEyes");
        yield return null;
    }

    public void StartTimer()
    {
        timerCanvasBehaviour.StartTimer();
        timerCanvasBehaviour.StartPing();
    }

    public void ActivatePlayer()
    {
        charController.enabled = true;
        charMotor.enabled = true;
        charPlayer.enabled = true;
    }

    public void StartEGTA()
    {
        egta.OpenThoughtBubble();
    }
}
