using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public int Score 
    {
        get { return score; }
        set { score = value; }
    }

    private int score;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GameOver ()
    {
        SceneManager.LoadScene("Main Scene");
    }

    public void IncreaseScore ()
    {
        ++score;
    }
}
