using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndGameTypingAnim : MonoBehaviour
{
    [SerializeField] private string victoryText;
    [SerializeField] private string failureText;
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private VictoryCheckBehaviour victoryCheckBehaviour;

    public void OpenThoughtBubble()
    {
        animator.SetTrigger("Open");
    }
    public void StartTyping()
    {
        if (victoryCheckBehaviour.isVictoryAchieved)
        {
            StartCoroutine(TypingAnimationEnum(victoryText));
        }
        else
        {
            StartCoroutine(TypingAnimationEnum(failureText));
        }
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
        animator.SetTrigger("Close");
        yield return null;
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
