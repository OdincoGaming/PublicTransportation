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

    public void OpenMainMenu()
    {
        titleText.SetActive(true);
        animator.SetTrigger("Open");
    }
    public void StartGame()
    {
        animator.SetTrigger("StartGame");
    }

    public void InitGamePlay()
    {
        eyelidsAnimator.SetTrigger("OpenThoughts");
        titleText.SetActive(false);
    }
}
