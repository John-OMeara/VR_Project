using UnityEngine;
using System.Collections;

public class TrackCreator : MonoBehaviour {

    public Object enemyCube;    // Walls, don't touch!
    public Object endGoal;      // Goal, do touch!

    Vector3 pos = new Vector3(0, 2.5f, -40);   // position to start drawing walls from
    float offset = 3f;                      // gap between walls

    // Possible path types
    enum Path
    {
        straight,
        right,
        left
    };
    Path nextPath;
    Path prevPath;
    
    int totalLength;    // How long the track is in total

	// Use this for initialization
	void Start ()
    {
        // These two loops create a funnel shape
        // This is hardly visible but stops the player going OOB immediatly
        for (int block = 0; block < 10; block++)
        {
            Instantiate(enemyCube, new Vector3((pos.x - block) - offset, pos.y, pos.z - block), Quaternion.identity);
        }
        for (int block = 0; block < 10; block++)
        {
            Instantiate(enemyCube, new Vector3((pos.x + block) + offset, pos.y, pos.z - block), Quaternion.identity);
        }

        // Game starts with a straight corridor always
        CreateStraight(20);


        // The main creation loop
        while (totalLength < 100)
        {
            // A random number is chosen to represent what type of path is made
            int path = Random.Range(0, 3);
            // Then another random number chooses how long the segment is
            int len = Random.Range(5, 10);

            switch (path)
            {
                case 0:
                    nextPath = Path.straight;
                    break;
                case 1:
                    nextPath = Path.right;
                    break;
                case 2:
                    nextPath = Path.left;
                    break;
                default:
                    break;
            };


            // Stops the game from throwing in too many difficult zig-zags
            // Makes the game easier to beat
            if ((nextPath == Path.right && prevPath == Path.left) ||
                (nextPath == Path.left && prevPath == Path.right))
            {
                nextPath = Path.straight;
            }

            // These statements stop the track from leaving the bounds of the ground plane
            if ((pos.x - len) < -50 &&
                nextPath == Path.left)
            {
                nextPath = Path.right;
            }
            else if ((pos.x + len) > 50 &&
                nextPath == Path.right)
            {
                nextPath = Path.left;
            }

            switch (nextPath)
            {
                case Path.straight:
                    CreateStraight(len);
                    break;
                case Path.right:
                    CreateRight(len);
                    break;
                case Path.left:
                    CreateLeft(len);
                    break;
                default:
                    CreateStraight(len);
                    break;
            }

            prevPath = nextPath;

            totalLength += len; // The length of the segment is added to the track's total length
        }

        CreateEnd();
    }
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    // Straight corridors
    void CreateStraight(int length)
    {
        // Creates a wall going along the Z-Axis
        // It then flips the offset and creates another parallel
        for (int side = 0; side < 2; side++)
        {
            for (int block = 0; block < length; block++)
            {
                Instantiate(enemyCube, new Vector3(pos.x + offset, pos.y, pos.z + block), Quaternion.identity);
            }

            offset *= -1;
        }

        // Where to draw the next track is updated
        pos.z += length;
    }

    // Right turns
    void CreateRight(int length)
    {
        // A diagonal line is drawn going along the X-axis and the Z-axis
        // It then flips the offset and creates another parallel
        for (int side = 0; side < 2; side++)
        {
            for (int block = 0; block < length; block++)
            {
                Instantiate(enemyCube, new Vector3((pos.x + block) + offset, pos.y, pos.z + block), Quaternion.identity);
            }

            offset *= -1;
        }

        // Where to draw the next track is updated
        pos.x += length;
        pos.z += length;
    }

    // Left turn
    void CreateLeft(int length)
    {
        // A diagonal line is drawn going along the X-axis and the Z-axis
        // It then flips the offset and creates another parallel
        for (int side = 0; side < 2; side++)
        {
            for (int block = 0; block < length; block++)
            {
                Instantiate(enemyCube, new Vector3((pos.x - block) + offset, pos.y, pos.z + block), Quaternion.identity);
            }

            offset *= -1;
        }

        // Where to draw the next track is updated
        pos.x -= length;
        pos.z += length;
    }

    // The end (yay)
    void CreateEnd()
    {
        // Ensures the offset is positive
        // We don't know how many different segments are made so we don't know if it's positive or negative
        // Not extremely important, but is nice
        offset = Mathf.Abs(offset);


        // The following loops create a square room at the end of the course.

        // Entrance Wall
        for(int block = 0; block < 5; block ++)
        {
            Instantiate(enemyCube, new Vector3(pos.x - block - offset, pos.y, pos.z), Quaternion.identity);
        }
        for (int block = 0; block < 5; block++)
        {
            Instantiate(enemyCube, new Vector3(pos.x + block + offset, pos.y, pos.z), Quaternion.identity);
        }

        offset += 5;

        // Side Walls
        for(int block = 0; block < 10; block ++)
        {
            Instantiate(enemyCube, new Vector3(pos.x - offset, pos.y, pos.z + block), Quaternion.identity);
            Instantiate(enemyCube, new Vector3(pos.x + offset, pos.y, pos.z + block), Quaternion.identity);
        }

        pos.z += 10;

        // Back Walls
        for(int block = 0; block < 10; block++)
        {
            Instantiate(enemyCube, new Vector3(pos.x - offset + block, pos.y, pos.z), Quaternion.identity);
            Instantiate(enemyCube, new Vector3(pos.x + offset - block, pos.y, pos.z), Quaternion.identity);
        }

        Instantiate(endGoal, new Vector3(pos.x, 1.2f, pos.z - 5), Quaternion.identity);
    }
}
