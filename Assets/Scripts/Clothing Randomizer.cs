using System.Collections.Generic;
using UnityEngine;

public class ClothingRandomizer : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private int chanceForOnePiece = 25;
    [SerializeField, Range(0, 100)] private int chanceForOneHat = 25;
    [SerializeField] private GameObject hair;
    [SerializeField] private bool ifHatNoHair = false;
    [SerializeField] private List<GameObject> onepiece = new();
    [SerializeField] private List<GameObject> tops = new();
    [SerializeField] private List<GameObject> bottoms = new();
    [SerializeField] private List<GameObject> hats = new();
    [SerializeField] private List<GameObject> shoes = new();
    private GameObject PickRandomFromList(List<GameObject> gos)
    {
        GameObject result = null;

        if (gos.Count > 0)
        {
            gos.Shuffle();
            int index = Random.Range(0, gos.Count);
            result = gos[index];
        }

        return result;
    }

    private void Awake()
    {
        PickRandomFromList(shoes).SetActive(true);

        int rnd = Random.Range(0, 100);
        if(rnd <= chanceForOnePiece)
        {
            PickRandomFromList(onepiece).SetActive(true);
        }
        else
        {
            PickRandomFromList(tops).SetActive(true);
            PickRandomFromList(bottoms).SetActive(true);
        }

        rnd = Random.Range(0, 100);
        if(rnd <= chanceForOneHat)
        {
            PickRandomFromList(hats).SetActive(true);
            if (ifHatNoHair)
            {
                if(hair != null)
                    hair.SetActive(false);
            }
        }
    }
}
