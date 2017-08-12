//PF (Platformer) Manager made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2017/07/22
//Usage: add it to empty object (as platformer manager, etc) just below the scene, it will work when "PF-" script need its help.

using System.Collections;
using System.Threading;
using UnityEngine;

public class PFManager : MonoBehaviour{

    public Animator animSMManager;
    
    //debug-use
    private bool executable = false;

    private void Start()
    {
        //debug
        //find all objects that tagged "Player", and if doesn't exist Manager should be get a warning?

        executable = true;
        if (!executable)
        {
            Debug.LogError("The " + GetType().Name + ".cs in " + name + " is un-executable. Please find out why.");
        }
    }
    
    
    #region PF function

    //judge if it's the player
    public bool JudgePlayer(GameObject obj)
    {
        if (obj.tag == "Player")
        {
            return true;
        }
        else return false;

    }
    public bool JudgePlayerData(PFPlayer pfPlayerData)
    {
        //use this to judge if the pfPlayerData exists.
        if (pfPlayerData == null)
        {
            Debug.LogWarning("The " + pfPlayerData.GetComponentInParent<GameObject>().name + " doesn't have a PFPlayer.cs to record but still use a function in " + GetType().Name + ".cs. This will thus make the function failed.");
            return false;
        }
        return true;
    }

    public void Finish(GameObject player)
    {
        //do things after finish a level, ex. back to level select, archivement, etc.
        Debug.Log("Player " + player.name + " has finished the game!");
        player.GetComponent<PF2DController>().FreezeControl();

    }
    

    #endregion //PF function

}
