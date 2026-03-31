using UnityEngine;
using UnityEngine.SceneManagement; 
public class Mainmenu1 : MonoBehaviour
{
    
    public void PlayGame()
    {
   
        SceneManager.LoadScene("Game");
    }

    // Hàm này sẽ gọi khi bấm nút Setting
    public void OpenSettings()
    {
        
        SceneManager.LoadScene("Tên_Scene_Setting");
    }

    // Hàm này sẽ gọi khi bấm nút Quit
    public void QuitGame()
    {
        Debug.Log("Đã thoát game!");
        Application.Quit();
    }
}