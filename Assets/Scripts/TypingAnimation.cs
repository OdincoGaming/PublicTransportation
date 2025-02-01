using System.Collections;
using TMPro;
using UnityEngine;

public class TypingAnimation : MonoBehaviour
{
    [SerializeField] private string text;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private Animator animator;

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
            yield return new WaitForSeconds(.05f);
        }

        yield return new WaitForSeconds(2f);
        for (int i = typedText.Length; i > 0; i--)
        {
            typedText = typedText.Substring(0, i);
            textMeshProUGUI.text = typedText;
            yield return new WaitForSeconds(.015f);
        }
        textMeshProUGUI.text = "";
        animator.SetTrigger("OpenEyes");
        yield return null;
    }
}
