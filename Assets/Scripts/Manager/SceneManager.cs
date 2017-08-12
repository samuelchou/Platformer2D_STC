using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

    public static SceneManager ins;

    void Awake()
    {
        if (ins == null)
        {
            ins = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (ins != this)
        {
            Destroy(gameObject);
        }
    }
}
