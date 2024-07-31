using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{

    public static void GameStart(){
        SceneManager.LoadScene("SampleScene");
    }

    public static void GameStop(){
        SceneManager.LoadScene("End");

    }
}
