using System.Collections;
using UnityEngine;
public class StairManager : MonoBehaviour {

    public string scoreString = "Score: ";
    public double score = 0;
    public UnityEngine.UI.Text scoreText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        

	}

    public void AddScore(double scoreAdded)
    {
        score += scoreAdded;
        if (scoreText)
        {
            scoreText.text = scoreString + score.ToString();
        }
    }

}
