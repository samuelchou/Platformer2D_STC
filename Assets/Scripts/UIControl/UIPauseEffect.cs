///UI Pause Effect made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2017/08/12
//usage:            this will be called by UIFeature. Write pause effects here (ex. text change between "pause" and "continue", show up a transparent black screen).

using UnityEngine;
using System.Collections;

public class UIPauseEffect : MonoBehaviour
{

    public UIFeature motherUI;
    //this must be assigned. If not, script will try to find one.

    private bool workable = true;
    
    private void OnEnable()
    {
        if (!motherUI)
        {
            motherUI = FindObjectOfType<UIFeature>();
            if (!motherUI)
            {
                Debug.LogError("the " + GetType().Name + " apply on " + name + " is missing motherUI (UIFeature) and thus it can't work.");
                workable = false;
            }

        }
        

    }


    public void DoPauseEffect()
    {
        //the basic function, called by UIFeature.
        if (!workable) return;


    }

    public void ChangeText(UnityEngine.UI.Text text)
    {
        //this will change text if there's a text component.
        if (motherUI.isPaused)
        {


        }

    }



}
