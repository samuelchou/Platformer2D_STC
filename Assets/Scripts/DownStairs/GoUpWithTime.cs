//Go Up With Time made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2017/08/06
//usage: apply it to any objects, and it will go up with time---as thought. Disable it by setting the enabled to false, or Time.timescale to 0.
//NOTICE that the effects are COMPULSORY -- won't work with usual force physics.

using UnityEngine;
using System.Collections;

public class GoUpWithTime : MonoBehaviour
{
    public float velocity = 1f;
    
    public void SetVelocity(float vel)
    {
        velocity = vel;
    }
    void Update()
    {
        transform.position += new Vector3(0, velocity*Time.deltaTime);
    }
}
