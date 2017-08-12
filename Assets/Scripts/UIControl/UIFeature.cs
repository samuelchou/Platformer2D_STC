//UI Feature made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2017/08/05
//usage: this script provides basic feature in UI, such as pause & continue, exit game, etc. Put it on every button, or on a empty obj and call the funct.

using UnityEngine;
using System.Collections;

public class UIFeature : MonoBehaviour
{
    public GameObject workonThese;
    //assign it to  "all" gaming-use objects, such as walls, grounds, enemies, etc.
    //WITHOUT things you don't want to stop, like UI.
    //suggestion: empty obj "level" with all things in.
    

    internal bool isPaused = false;
    internal float oringinalTimeScale;


    
    private void OnEnable()
    {
        if (!workonThese)
        {
            workonThese = GameObject.Find("Level");
        }
        oringinalTimeScale = Time.timeScale;
    }

    public void QuitGame()
    {
        //here do the things before game ends.
        Debug.Log("Exiting the game...");
        Application.Quit();
    }

    public void PauseOrResume()
    {
        //this is used by buttons / events.
        TogglePauseAndResume(!isPaused);
        //if (!isPaused) PauseGame();
        //else ResumeGame();
        
    }

    public void TogglePauseAndResume(bool wantPause)
    {
        if (wantPause)
        {
            //pausing
            Debug.Log("Paused.");
            oringinalTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        else
        {
            //resuming
            Debug.Log("Continue...");
            Time.timeScale = oringinalTimeScale;
        }
        
        //below call things you want to "stop/resume".
        GoUpWithTime[] g = workonThese.GetComponentsInChildren<GoUpWithTime>();
        foreach (GoUpWithTime i in g)
        {
            i.enabled = !wantPause;
        }
        //beyond call things you want to "stop/resume".

        
        isPaused = wantPause;

    }
    

    public void PauseGame()
    {
        Debug.Log("Pausing...");
        //below do things when you need pause.
        //GetComponents<GoUpWithTime>()
        GoUpWithTime[] g = workonThese.GetComponentsInChildren<GoUpWithTime>();
        foreach (GoUpWithTime i in g)
        {
            i.enabled = false;
            Debug.Log("UNENABLE!!!");
        }

        Debug.Log("Paused.");
        oringinalTimeScale = Time.timeScale;
        Time.timeScale = 0;
        //beyond do things when you need pause.

        isPaused = true;
    }
    
    public void ResumeGame()
    {
        Debug.Log("Continue...");
        GoUpWithTime[] g = GetComponentsInChildren<GoUpWithTime>();
        foreach (GoUpWithTime i in g)
        {
            i.enabled = false;
        }

        Time.timeScale = oringinalTimeScale;
        isPaused = false;
    }


}
