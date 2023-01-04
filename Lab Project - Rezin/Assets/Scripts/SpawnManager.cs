using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] asteroids;
    public GameObject player;

    public GameObject star;
    private float rotationBound = 360.0f;
    private float spawnBound = 60;
    private bool[,] map = new bool[51, 51];
    private float maxXSpawnBound;
    private float minXSpawnBound;
    private float maxYSpawnBound;
    private float minYSpawnBound;

    // Start is called before the first frame update
    void Start()
    {
        string asteroidLayout = "";
        for (int y = -250; y <= 250; y += 10)
        {
            for (int x = -250; x <= 250; x += 10)
            {
                // generates more asteroids further away from the origin
                float randomValue = UnityEngine.Random.Range(0, 100);
                float distanceFromOrigin = Mathf.Abs(Mathf.Sqrt(Mathf.Pow(y, 2) + Mathf.Pow(x, 2)));
                if (randomValue > 90 - (0.001 * Mathf.Pow(distanceFromOrigin, 2)))
                {
                    //Debug.Log(99 - (0.1 * distanceFromOrigin));
                    map[x/10 + 25, y/10 + 25] = true;
                    asteroidLayout += "0";
                    Instantiate(star, new Vector3(UnityEngine.Random.Range(-150, 150), UnityEngine.Random.Range(-150, 150), -1), Quaternion.Euler(new Vector3(0,0,0))); // spawns in a bunch of stars around the starting location. Done to give players points of reference when learning the controls.
                }
                else
                {
                    asteroidLayout += "-";
                }
            }
            asteroidLayout += "\n";
        }
        // remove asteroids from the center
        for (int y = 24; y <= 26; y++)
        {
            for (int x = 24; x <= 26; x++)
            {
                map[x, y] = false;
            }
        }
        UnityEngine.Debug.Log(asteroidLayout);
        //Invoke("SpawnAsteroid", 1f);
    }

    // Update is called once per frame
    void Update() 
    {
        // checks the map for asteroids within the player's range
        maxXSpawnBound = player.transform.position.x + spawnBound;
        minXSpawnBound = player.transform.position.x - spawnBound;
        maxYSpawnBound = player.transform.position.y + spawnBound;
        minYSpawnBound = player.transform.position.y - spawnBound;
        for (int y = (int) minYSpawnBound; y < maxYSpawnBound; y += 10)
        {
            for (int x = (int) minXSpawnBound; x < maxXSpawnBound; x += 10)
            {
                // creates boundaries around the player in which asteroids can spawn
                try {
                    if (y < maxYSpawnBound && y > minYSpawnBound && x < maxXSpawnBound && x > minXSpawnBound && map[x / 10 + 25, y / 10 + 25])
                    {
                        map[x / 10 + 25, y / 10 + 25] = false;
                        Instantiate(asteroids[UnityEngine.Random.Range(0, asteroids.Length)], new Vector3(x + UnityEngine.Random.Range(-10, 10), y + UnityEngine.Random.Range(-10, 10), -1), Quaternion.Euler(0, 0, UnityEngine.Random.Range(-rotationBound, rotationBound)));
                        Vector3 playerVelocity = player.GetComponent<Rigidbody2D>().velocity;
                        for (int i = 0; i < 20; i++) { // spawns 10 stars per asteroid
                            //UnityEngine.Debug.Log((playerVelocity.x / Math.Abs(playerVelocity.x) + " " + (playerVelocity.y / Math.Abs(playerVelocity.y))));
                            if (playerVelocity.x != 0 && playerVelocity.y != 0) {// must be moving in order to spawn stars
                                // depending on direction, stars will spawn intentionally out of view of the player, represented by playerVelocity.x / Math.Abs(playerVelocity.x)
                                Instantiate(star, new Vector3(x + UnityEngine.Random.Range(0, 50 * (playerVelocity.x / Math.Abs(playerVelocity.x))), y + UnityEngine.Random.Range(0, 50 * (playerVelocity.y / Math.Abs(playerVelocity.y))), -1), Quaternion.Euler(new Vector3(0,0,0)));
                            }
                        }
                    }
                }
                catch (IndexOutOfRangeException) {
                    ;
                }
            }
        }
    }

    void SpawnAsteroid() // checks for asteroids within the player's view, then spawns them accordingly
    {
        ;
    }
}
