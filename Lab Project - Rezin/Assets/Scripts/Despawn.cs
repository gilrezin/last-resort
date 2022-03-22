using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Despawn : MonoBehaviour
{
    private float despawnRange = 70;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // despawn when out of range of player
        try {
            if (Mathf.Abs(player.transform.position.x - transform.position.x) > despawnRange || Mathf.Abs(player.transform.position.y - transform.position.y) > despawnRange)
            {
                Destroy(gameObject);
            }
        }
        catch {
            ;
        }
    }
}
