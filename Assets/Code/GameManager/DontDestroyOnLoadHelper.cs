using UnityEngine;

public class DontDestroyOnLoadHelper : MonoBehaviour
{
    private static DontDestroyOnLoadHelper instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
