//StairGenerator made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2017/07/23
//Usage: add it on an object, set it, and it will generate "stair" time by time.
//(Just think about the game DownStairs)

using System.Collections;
using UnityEngine;

public class StairGenerator : MonoBehaviour {

    public GameObject stair;
    public float generateTimeInterval = 1.0f;
    private Renderer stairRenderer;
    private float stairLength;
    public GameObject attachTo;
    //if assigned, the obj generated will be reassign to this obj 's children.
    

    //generation control
    public Transform leftPos;
    public Transform rightPos;
    //locate the "edge". the script will autocorrect to make objs won't generate outside the edge.
    public float gap = 1f;
    //if gap <= 1, generation might be stack at near position.
    //if gap <=0, it have a chance to generate at the same position.
    public float stairGoUpVelocity = 10;

    //in-script use
    private float timeNow = 0f;
    private Vector3 generatePos;
    private Vector3 lastGeneratePos;
    private Vector3 interval;
    private bool needGapCheck = true;
    private bool needtoAttach = false;
    private StairManager theStairManager;

    //debug usage
    private bool executable = false;
    
    //Initialize
    private void OnEnable()
    {
        interval = rightPos.position - leftPos.position;
        stairRenderer = stair.GetComponent<Renderer>();
        stairLength = stairRenderer.bounds.size.x;

        //auto correct (the generated pos is always set to be the obj center, thus the edge need "reduce")
        rightPos.position = rightPos.position - interval.normalized * stairLength;
        leftPos.position = leftPos.position + interval.normalized * stairLength;

        //generate one at center when activated?
        //Instantiate(stair, leftPos.position + interval / 2, Quaternion.identity);
        lastGeneratePos = leftPos.position + interval / 2;
        if (gap <= 0) needGapCheck = false;
        if (attachTo != null) needtoAttach = true;
        if (FindObjectOfType<StairManager>() == null)
        {
            Debug.LogWarning("This scene lacks a object containing StairManager.cs, thus some funct. of the " + GetType().Name + ".cs won't work.");
            
        }
        else
        {
            theStairManager = FindObjectOfType<StairManager>();
        }
        executable = true;
    }


    // Update is called once per frame
    void Update () {
        if (!executable) return;
        
        //time loop
        if (timeNow < generateTimeInterval)
        {
            timeNow += Time.deltaTime;
        }
        else
        {
            //time to generate
            GeneratePosBetweenTwoPos(leftPos, rightPos, ref generatePos);

            //gap check
            if (needGapCheck) RandomUntilOverGap();

            //generate and set
            GameObject objGenerated = Instantiate(stair, generatePos, Quaternion.identity) as GameObject;
            if(needtoAttach)
            {
                objGenerated.transform.parent = attachTo.transform;
            }
            lastGeneratePos = generatePos;
            if (theStairManager)
            {
                theStairManager.AddScore(1);
            }

            //make stair "go up"
            objGenerated.AddComponent<GoUpWithTime>();
            objGenerated.GetComponent<GoUpWithTime>().SetVelocity(stairGoUpVelocity);

            //reset the Timer
            timeNow = 0;
        }

    }
    private void GeneratePosBetweenTwoPos(Transform left, Transform right, ref Vector3 returnPos)
    {
        returnPos = new Vector3(
                Random.Range(left.position.x, right.position.x),
                Random.Range(left.position.y, right.position.y),
                Random.Range(left.position.z, right.position.z)
                );
    }

    private void RandomUntilOverGap()
    {
        while ((generatePos - lastGeneratePos).magnitude <= stairLength * gap)
        {
            GeneratePosBetweenTwoPos(leftPos, rightPos, ref generatePos);

        }
        return;
    }

}
