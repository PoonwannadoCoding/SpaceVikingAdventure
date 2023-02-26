using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class MAINMENU : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;
    
    public void Exit(){
        SoundManager.instance.PlatSound(clickSound);
        Application.Quit();
        
    }
    public void Play(){
        SoundManager.instance.PlatSound(clickSound);
        SceneManager.LoadScene(1);
    }

    public void MainMenu(){
        SoundManager.instance.PlatSound(clickSound);
        SceneManager.LoadScene(0);
    }
}
