using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Kéo thả Panel Menu vào đây")]
    public GameObject pauseMenuUI;

    // Biến kiểm tra xem game có đang tạm dừng hay không
    private bool isPaused = false;

    void Start()
    {
        // Khi mới bắt đầu game, tự động ẩn Menu đi
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        // Kiểm tra nếu người chơi ấn nút ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame(); // Nếu đang dừng thì chơi tiếp
            }
            else
            {
                Pause(); // Nếu đang chơi thì tạm dừng
            }
        }
    }


    public void Pause()
    {
        pauseMenuUI.SetActive(true); // Bật giao diện Menu
        Time.timeScale = 0f; 
        isPaused = true;
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;                 
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Ẩn giao diện Menu
        Time.timeScale = 1f; // Cho thời gian chạy lại bình thường
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    public void OpenSettings()
    {
        
        Time.timeScale = 1f;
        SceneManager.LoadScene("Tên_Scene_Setting_Của_Bạn");
    }

    public void GoHome()
    {
        Time.timeScale = 1f; // Tương tự, trả lại thời gian về 1
        SceneManager.LoadScene("Home");
    }
}