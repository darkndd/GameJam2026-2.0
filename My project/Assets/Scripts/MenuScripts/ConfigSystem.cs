using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class ConfigSystem : MonoBehaviour
{

    [SerializeField] private AudioMixer audioMixer;

    public void Fullscreen(bool fullscreen) 
    {
        Screen.fullScreen = fullscreen;
    }
    // Update is called once per frame
    public void setVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume); 
    }

    public void goBack() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
