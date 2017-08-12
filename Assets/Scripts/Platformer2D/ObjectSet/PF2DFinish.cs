//PF (Platformer) 2D Finish made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2017/07/22
//NOTE: 2D only.
//Usage: add it to the "finish point" objects. It'll work with other "PF-" scripts.

using UnityEngine;
using System.Collections;

public class PF2DFinish : MonoBehaviour {

    //public GameObject managerObj;


    //in this example script, the "ending-special-effect" is to make the player floating into air.
    public float floatingDist = 3.0f;
    private GameObject playerCatched;
    private bool catched = false;
    private Rigidbody2D playerRb2D;
    private Vector3 floatingPos;
    private int step = 0;

    //in-script use
    private PFManager thePlatformerManager;

    //debug use
    private bool executable = false;

    private void Start()
    {
        floatingPos = transform.position + new Vector3(0, floatingDist, 0);
        //if return before setting executable to true, the whole script won't work.
        if (FindObjectOfType<PFManager>() == null)
        {
            Debug.LogWarning("This scene lacks a object containing PFManager, thus the " + GetType().Name + ".cs won't work.");
            return;
        }
        thePlatformerManager = FindObjectOfType<PFManager>();
        executable = true;
        /*
        if (!managerObj)
        {
            Debug.LogWarning("The " + GetType().Name + " in " + name + " doesn't setup managerObj. This will make " + GetType().Name + " don't work.");
            return;
        }
        if (managerObj.GetComponent<PFManager>() == null)
        {
            Debug.LogWarning("Warning: object " + name + " has wrong managerObj set. Set it with the right object with PFManager script, otherwise it won't work." );
            return;
        }
        thePlatformerManager = managerObj.GetComponent<PFManager>();
        //if return before this line, it won't be executable.
        
        executable = true;
        */
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!executable) return;
        GameObject objTouched = collision.gameObject;
        if (!thePlatformerManager.JudgePlayer(objTouched)) return;

        

        thePlatformerManager.Finish(objTouched);
        //status/overall change will happen in PFManager.Finish funct.
        //below, do the special effect.
        if (objTouched.GetComponent<Rigidbody2D>() != null)
        {
            playerCatched = objTouched;
            playerRb2D = playerCatched.GetComponent<Rigidbody2D>();
            catched = true;
        }

    }

    private void Update()
    {
        if (!executable) return;
        #region Floating effect
        //this part do the floating-effect. Can be deleted when don't need.
        if (catched)
        {
            if (step < 20)
            {
                step ++;
            }
            else
            {
                if ((floatingPos - playerCatched.transform.position).magnitude <=0.2)
                {
                    playerCatched.transform.position = floatingPos;
                    playerCatched.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    catched = false;
                    return;
                }
                playerRb2D.velocity = (float)5.0 * (floatingPos - playerCatched.transform.position);
                step = 0;
            }
        }
        
        #endregion
        
    }

}
