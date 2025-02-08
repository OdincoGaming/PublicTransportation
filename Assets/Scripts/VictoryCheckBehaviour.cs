using UnityEngine;

public class VictoryCheckBehaviour : MonoBehaviour
{
    public bool isVictoryAchieved = false;
    private bool gameended = false;
    [SerializeField] private VoidEventChannelSO endGamechannel;
    [SerializeField] private TitleTextBehaviour ttb;

    private void OnEnable()
    {
        if (endGamechannel != null)
        {
            endGamechannel.OnEventRaised += EndGame;
        }
    }

    private void OnDisable()
    {
        if (endGamechannel != null)
        {
            endGamechannel.OnEventRaised -= EndGame;
        }
    }
    private void OnTriggerEnter(Collider coll)
    {
        isVictoryAchieved = true;
        if (!gameended)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        if (!gameended)
        {
            Debug.Log("Game ended");
            gameended = true;
            ttb.EndGame(isVictoryAchieved);
        }
    }
}
