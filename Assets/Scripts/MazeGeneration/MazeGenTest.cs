using System.Collections.Generic;
using RootMotion.Dynamics;
using UnityEngine;
using System.Linq;
using System.Collections;

public class MazeGenTest : MonoBehaviour
{

}

public static class Extensions
{
    private static System.Random rand = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}