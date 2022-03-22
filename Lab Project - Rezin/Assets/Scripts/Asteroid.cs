using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float rotationSpeed;
    public float movementSpeedX;
    public float movementSpeedY;
    
    private Rigidbody2D playerRb;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerRb = GetComponent<Rigidbody2D>();
        rotationSpeed = Random.Range(-5.0f, 5.0f);
        movementSpeedX = Random.Range(-5.0f, 5.0f);
        movementSpeedY = Random.Range(-5.0f, 5.0f);
        playerRb.AddForce(transform.right * movementSpeedX, ForceMode2D.Force);
        playerRb.AddForce(transform.up * movementSpeedY, ForceMode2D.Force);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.forward, Time.deltaTime * rotationSpeed);
    }
}
