using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float rotationSpeed = -240.0f;
    private float launchForce = 200.0f;
    private float launchCooldown = 0.25f;
    private bool canLaunch = true;
    public bool playerAlive = true;
    public bool wonGame = false;
    public bool gameStarted = false;
    private Rigidbody2D playerRb;
    public AudioSource playerAudio;
    public AudioClip boostSound;
    public AudioClip deathSound;
    public ParticleSystem explosionParticle;
    public ParticleSystem boostParticle;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        playerRb = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space)) 
        {
            gameStarted = true;
        }
        MovePlayer();
        winCondition();
    }

    // provides a cooldown for the launch function, prevents spamming
    IEnumerator LaunchCooldown()
    {
        canLaunch = false;
        yield return new WaitForSeconds(launchCooldown);
        canLaunch = true;
    }


    // keeps the player spinning and checks for spacebar input so it can shoot forward
    void MovePlayer() 
    {
        if (gameStarted) {
        transform.Rotate(transform.forward, Time.deltaTime * rotationSpeed);

           if (Input.GetKeyDown(KeyCode.Space) && canLaunch)
            {
            boostParticle.Play();
            playerRb.AddForce(transform.up * launchForce, ForceMode2D.Force);
            playerAudio.PlayOneShot(boostSound, 0.5f);
            StartCoroutine(LaunchCooldown());
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Delete)) // delete all save data shortcut
            {
                PlayerPrefs.DeleteKey("BestTime");
                Debug.Log("save data deleted");
            }
        }

    }

    void winCondition()
    {
        if (Mathf.Abs(transform.position.x) > 260 || Mathf.Abs(transform.position.y) > 260)
        {
            wonGame = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 cameraPosition = new Vector3(transform.position.x, transform.position.y, -13);
        explosionParticle.Play();
        AudioSource.PlayClipAtPoint(deathSound, cameraPosition, 1.0f);
        playerAlive = false;
        canLaunch = false;
        gameObject.SetActive(false);
    }
}
