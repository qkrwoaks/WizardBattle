using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneContoller : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("CreateRoom");
        }
    }
}