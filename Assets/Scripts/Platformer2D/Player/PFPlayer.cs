//PF (Platformer) Player made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2017/07/22
//Usage: add it to objects that represent players (remember to set their tags to "Player"), it will provide data for other  "PF-" scripts usage.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFPlayer : MonoBehaviour {

    //edit basic player data here.
    public double healthPoint = 100;
    public double manaPoint = 100;
    public float standardRespawnTime = 3f;

    //inner player data.
    internal bool isDead = false;
    internal Vector3 respawnPos;
    internal bool isRespawning = false;
    internal double initialHP;
    internal double initialMP;
    //QUESTION: making it into ASM?

    //in-script using.
    private Rigidbody rb;
    private Rigidbody2D rb2d;
    
    private void OnEnable()
    {
        respawnPos = transform.position;
        initialHP = healthPoint;
        initialMP = manaPoint;
        rb = GetComponent<Rigidbody>();
        rb2d = GetComponent<Rigidbody2D>();

        //debug area
        if (tag != "Player")
        {
            Debug.LogWarning("Warning: the object " + name + " have script " + GetType().Name + ".cs" 
                + " (and thus should represents player?) but doesn't have tagged \"Player\". "
                + "It might cause some unpredictable errors using some \"PF-\" scripts.");
            
        }
    }

    //LivingJudge is here to prevent a "second-death" ex. moving in & out the dead area.
    //return true for still living; return false to be dead.
    //NOTE that die in action (etc. hp goes under 0) is also written here.
    public bool LivingJudge()
    {
        if (isDead)
        {
            //this guy has been dead already.
            return false;
        }
        if (healthPoint <= 0.0)
        {
            //this guy die in action. return that he should be dead now.
            Debug.Log("lower than hp");
            return false;
        }

        //it is alive.
        return true;
    }

    #region Death
    //Death funct. should be called when player is dead. Modify this to produce death message / gameover / etc
    //NOTE: the gameover funct. should be written in PFManager.cs, NOT HERE.
    public void Death()
    {
        Death("");
    }
    public void Death(string deadReason)
    {
        if (isDead)
        {
            Debug.Log("This guy " + name + " is already dead. Don't let him die twice.");
            return;
        }
        //if the player has been dead already, skip the whole procedure (to avoid second-death).

        if (deadReason == "")
        {
            //the death reason is not assigned.
            Debug.Log("The player " + name + " is dead.");
        }
        else
        {
            Debug.Log("The player " + name + " is dead due to " + deadReason + ".");
        }
        AfterDeathProcess();
    }
    public void AfterDeathProcess()
    {
        //doing the death process here, like "life-1", "deathrecord +1", state change, some animation, etc.
        //NOTE: DON'T respawn / resurrect player here. Use Respawn or other function.
        GetComponent<PF2DController>().EnableControl(false, false);
        isDead = true;

    }
    #endregion //Death

    //Respawn funct. should be called when you want to "respawn" a player.
    //NOTE: Respawn is worked as "setting back" the player obj. data, NOT re-spawning a new one.
    public void Respawn()
    {
        //do the respawn action below, like reset position, reset state, start game, etc.
        //the funct. will respawn player ON INSTANT. For a delayed-respawn, use RespawnAfterTime.
        //NOTE that DON'T do the death process (like "life-1") here. Do them in AfterDeathProcess funct.

        transform.position = respawnPos;
        Debug.Log("Player " + name + " has respawn to " + transform.position + ".");
        
        //reset velocity to zero in case some bugs occur.
        if (rb!=null) rb.velocity = Vector3.zero;
        if (rb2d != null) rb2d.velocity = Vector2.zero;

        healthPoint = initialHP;
        manaPoint = initialMP;
        GetComponent<PF2DController>().EnableControl(true, true);

        isDead = false;
        isRespawning = false;
    }
    public IEnumerator RespawnAfterTime(float timeWaiting)
    {
        //RespawnAfterTime funct. can be only called in writting "StartCoroutine(RespawnAfterTime(time));" due to the code attribute.
        if (!isRespawning)
        {
            isRespawning = true;
            yield return new WaitForSeconds(timeWaiting);
            Respawn();
        }

    }

    public void DeathAndRespawn(string deadReason)
    {
        //if that guy is dead already, just don't do things.
        if (isDead)
        {
            return;
        }
        Death(deadReason);
        StartCoroutine(RespawnAfterTime(standardRespawnTime));
    }
    public void DeathAndRespawn(string deadReason, float timeOnDemand)
    {
        //if that guy is dead already, just don't do things.
        if (isDead)
        {
            return;
        }
        Death(deadReason);
        StartCoroutine(RespawnAfterTime(timeOnDemand));
    }

    public void TakeDamage(double damageTaken, string reason)
    {
        healthPoint -= damageTaken;
        if(reason== null)
        {
            Debug.Log("Player " + name + " is hurt.");
            return;
        }
        Debug.Log("Player " + name + " is hurt by " + reason + ".");
        if (!LivingJudge())
        {
            DeathAndRespawn(reason);
        }
    }
    public void TakeDamage(double damageTaken)
    {
        TakeDamage(damageTaken, "");
    }

    

}
