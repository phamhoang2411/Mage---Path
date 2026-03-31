using UnityEngine;
using UnityEngine.SceneManagement;

public class Gameover : MonoBehaviour
{
    public void RestartGame()
    {
        // Nhớ mở lại thời gian bình thường trước khi load Scene
        Time.timeScale = 1f;

        // Load lại 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoHome()
    {
        Time.timeScale = 1f;

        
        SceneManager.LoadScene("Home");
    }
}
