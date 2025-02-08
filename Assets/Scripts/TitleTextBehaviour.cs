using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class TitleTextBehaviour : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Animator eyelidsAnimator;
    [SerializeField] private Button btn;
    [SerializeField] private GameObject titleText;
    private void Awake()
    {
        animator.SetTrigger("Open");
    }

    public void EnableButton()
    {
        btn.gameObject.SetActive(true);
    }

    public void OpenMainMenu() // 3d text drops down and start button comes up
    {
        titleText.SetActive(true);
        animator.SetTrigger("Open");
    }

    public void EndGame(bool isVictoryAchieved)
    {
        if (isVictoryAchieved)
        {
            eyelidsAnimator.SetTrigger("CloseEyes");
        }
        else
        {
            StartCoroutine(EndGameEnum());
        }
    }

    IEnumerator EndGameEnum()
    {
        yield return new WaitForSeconds(2f);
        eyelidsAnimator.SetTrigger("CloseEyes");
    }

    public void StartGame() // drops 3d text and triggers initgameplay at end of anim
    {
        animator.SetTrigger("StartGame");
    }

    public void InitGamePlay()
    {
        eyelidsAnimator.SetTrigger("OpenThoughts");
        titleText.SetActive(false);
    }
}
