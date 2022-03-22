using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public PlayerController playerControllerScript;
    private Vector3 offset = new Vector3(0, 0, -12);
    public float smoothSpeed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerVelocity = player.GetComponent<Rigidbody2D>().velocity;
        //Camera.main.orthographicSize = Mathf.Max(playerVelocity.x, playerVelocity.y) + 5;
        float targetCameraZoom = Mathf.Max(Mathf.Abs(playerVelocity.x), Mathf.Abs(playerVelocity.y));
        if (targetCameraZoom < 5)
        {
            targetCameraZoom = 5;
        }
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetCameraZoom, smoothSpeed * Time.deltaTime);
        if (playerControllerScript.playerAlive) 
        {
            transform.position = player.transform.position + offset;
        }
    }

}
