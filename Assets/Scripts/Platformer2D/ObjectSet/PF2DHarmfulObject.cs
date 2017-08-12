//PF (Platformer) 2D Harmful Object made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2017/07/22
//NOTE: 2D only.
//Usage: add it to "Harmful objects", such as a blade, and set it to trigger, it will call funct. from manager once there's a thing droped in. 
//NOTE: require PF2DDeadArea.cs

using UnityEngine;

public class PF2DHarmfulObject : PF2DDeadArea
{

    public bool totallyDeadly = false;
    public double damageWhenTouched = 40;
    public float pushAwayForces = 10f;
    public float pushAwaySpeed = 0f;
    //if >0, the pushaway effect will be making speed instead of giving forces.

    public float freezeTime = 0.5f;
    //just imagine that player touched this, it will temporarily lose control for a period.

    //if someone touches it
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!executable) return;

        //judge if it is a player
        GameObject objTouched = collision.gameObject;
        if (thePlatformerManager.JudgePlayer(objTouched))
        {
            //here write down what happened.
            PFPlayer player = objTouched.GetComponent<PFPlayer>();
            if (totallyDeadly)
            {
                player.DeathAndRespawn(deadReason);
            }
            else
            {
                player.TakeDamage(damageWhenTouched, deadReason);
            }

            //freeze control
            objTouched.GetComponent<PF2DController>().FreezeControl(freezeTime);

            //push-Away effects
            if (pushAwaySpeed > 0f)
            {
                PushAwayWithSpeed(objTouched, pushAwaySpeed);
                Debug.Log("hi");
                return;
            }
            PushAway(objTouched, pushAwayForces);

        }
        else
        {
            //not character... destroy it to save resource?
            //Destroy(other.gameObject);
        }
    }
    
    private void PushAway (GameObject obj, float forceMagnitude)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (!rb)
        {
            Debug.Log("The obj " + obj.name + " doesn't have Rigidbody. That means the PushAway effect on touching " + name + "won't be executed.");
            return;
        }
        Vector3 force = forceMagnitude * (obj.transform.position - transform.position);
        rb.AddForce(force);
    }

    private void PushAwayWithSpeed (GameObject obj, float speedMagnitude)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (!rb)
        {
            Debug.Log("The obj " + obj.name + " doesn't have Rigidbody. That means the PushAwayWithSpeed effect on touching " + name + "won't be executed.");
            return;
        }
        Vector3 velocity = speedMagnitude * (obj.transform.position - transform.position);
        rb.velocity = velocity;
    }

    


}
