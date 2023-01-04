using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float rotationSpeed = -240.0f;
    private float launchForce = 200.0f;
    private float launchCooldown = 0.25f;
    private float touchX;
    private float touchY;
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
    public AudioReverbFilter reverb;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        playerRb = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<AudioSource>();
        reverb = GameObject.Find("Main Camera").GetComponent<AudioReverbFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            touchX = touch.position.x;
            touchY = touch.position.y;
            //Debug.Log("x: " + touchX + "\ty: " + touchY);
        }   
        if (!gameStarted && (Input.GetKey(KeyCode.Space)) || (Input.touchCount > 0 && touchX > Globals.MENU_BUTTON_BOUNDS && touchY > Globals.MENU_BUTTON_BOUNDS))
        {
            gameStarted = true;
        }
        MovePlayer();
        winCondition();
        float reverbLevel = Mathf.Max(Mathf.Abs(transform.position.x), Mathf.Abs(transform.position.y)) * 44 - 10250; // reverb increases as player nears edge of level
        reverb.reverbLevel = reverbLevel;
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

           if ((Input.GetKey(KeyCode.Space) || Input.touchCount > 0) && canLaunch)
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
                PlayerPrefs.DeleteAll();
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
