using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Movement : MonoBehaviour {

    bool alive = true;       // if the player is alive or not
    float speed = 6;         // Speed the player mvoes at
    float countdown = 1.5f;  // How long until the level resets when done

    bool gameOver = false;   // is the game done or not

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (alive)
        {
            // Moves player in the position they are looking.
            // Turn head to steer
            transform.position += (Camera.main.transform.forward * speed * Time.deltaTime);
        }

        if (gameOver)
        {
            // Game doesn't reset immediatly, you get 1.5 seconds to think about what you've done
            countdown -= Time.deltaTime;
            if(countdown < 0)
            {
                SceneManager.LoadScene(0);
            }
        }
	}

    // Collision detection
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.name == "Goal" ||
           col.gameObject.name == "Goal(Clone)")
        {
            gameOver = true;
        }

        if(col.gameObject.name == "EnemyCube" ||
           col.gameObject.name == "EnemyCube(Clone)")
        {
            gameOver = true;
            alive = false;
        }
    }
}
