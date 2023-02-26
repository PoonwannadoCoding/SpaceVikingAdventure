using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private GameObject gameOverScreen;

    private void Awake(){
        gameOverScreen.SetActive(false);
    }

    public void GameOver(){
        gameOverScreen.SetActive(true);
    }

    public void MainMenu(){
        SoundManager.instance.PlatSound(clickSound);
        SceneManager.LoadScene(0);
    }

    public void Restart(){
        SoundManager.instance.PlatSound(clickSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit(){
        SoundManager.instance.PlatSound(clickSound);
        Application.Quit();
        
    }
}
