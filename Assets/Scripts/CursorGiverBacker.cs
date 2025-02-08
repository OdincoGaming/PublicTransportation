using UnityEngine;

public class CursorGiverBacker : MonoBehaviour
{
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Resources.UnloadUnusedAssets();
    }
}
