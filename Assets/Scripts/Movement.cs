using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Movement : MonoBehaviour {

    bool alive = true;       // if the player is alive or not
    float speed = 10;         // Speed the player mvoes at
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
            // Moves Moves forward at all times
            // Turn head to steer
            Vector3 movement = new Vector3(Input.acceleration.x, 0.0f, 0.7f);
            GetComponent<Rigidbody>().velocity = movement * speed;
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
