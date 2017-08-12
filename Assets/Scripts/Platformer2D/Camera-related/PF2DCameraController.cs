//PF (Platformer) 2D CameraController made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2017/07/22
//NOTE: 2D only.
//Usage: add it to MainCamera, and this will work with other "PF-" scripts.
using UnityEngine;
using System.Collections;

public class PF2DCameraController : MonoBehaviour
{

    public GameObject player;

    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = player.transform.position + offset;
    }
}
