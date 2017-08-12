//PF (Platformer) 2D Dead Area made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2017/07/22
//NOTE: 2D only.
//Usage: add it to the "Dead Area" and set it to trigger, it will call funct. from manager once there's a thing droped in. 

using UnityEngine;
using System.Collections;

public class PF2DDeadArea : MonoBehaviour {

    //protected GameObject platformerManager;
    public string deadReason = null;

    //in-script using
    protected PFManager thePlatformerManager;

    //debuging area
    protected bool executable = false;

	//initialization
	void Start () {
        
        //debug: the platformerManager should be the right manager.
		
        if (FindObjectOfType<PFManager>() == null)
        {
            Debug.LogWarning("This scene lacks a object containing PFManager.cs, thus the " + GetType().Name + ".cs won't work.");
            return;
        }
        thePlatformerManager = FindObjectOfType<PFManager>();
        executable = true;

    }

    //if something touches the dead area
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!executable) return;
        GameObject objTouched = collision.gameObject;
        //judge if it is a player
        if (thePlatformerManager.JudgePlayer(objTouched))
        {
            PFPlayer player = objTouched.GetComponent<PFPlayer>();
            player.DeathAndRespawn(deadReason);
        }
        else
        {
            //not character... destroy it to save resource?
            Destroy(objTouched);
        }
    }
    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (!executable) return;
        GameObject objTouched = collision.gameObject;
        if (!thePlatformerManager.JudgePlayer(objTouched))
        {
            Destroy(objTouched);
        }
    }*/

}
