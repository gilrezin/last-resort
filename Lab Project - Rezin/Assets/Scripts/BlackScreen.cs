using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Diagnostics;
using System;
using System.Text.RegularExpressions;

public class BlackScreen : MonoBehaviour
{
    private PlayerController playerControllerScript;
    public GameObject blackOutSquare;
    public GameObject TitleScreenText;
    public GameObject failText;
    public TextMeshProUGUI successText;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI bestTime;
    public TextMeshProUGUI newBest;
    public bool endOfGameTasksRun;
    public bool startOfGameTasksRun = false;
    public bool titleScreen = true;
    public AudioSource music;
    public AudioClip successSound;
    Stopwatch stopWatch = new Stopwatch();
    // Start is called before the first frame update

    void Start()
    {
        UnityEngine.Debug.Log(PlayerPrefs.GetString("BestTime"));
        if (PlayerPrefs.GetString("BestTime").Equals("")) // if no saved time exists, hide best time text
        {
            UnityEngine.Debug.Log("no save data found, hiding Best Time text");
            bestTime.text = "";
        }
        else // otherwise, show the best time text
        {
            bestTime.text += "" + PlayerPrefs.GetString("BestTime");
        }
        
        endOfGameTasksRun = false;
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        this.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        UpdateDistanceText();
        if (!playerControllerScript.playerAlive && !endOfGameTasksRun)
        {
            music.Stop();
            stopWatch.Stop();
            StartCoroutine(FadeBlackOutSquare()); // fade to black
            StartCoroutine(gameOver()); // display text
            endOfGameTasksRun = true;
        }
        else if (playerControllerScript.wonGame && !endOfGameTasksRun)
        {
            StartCoroutine(FadeBlackOutSquare()); // fade to black
            StartCoroutine(playerWin());
            music.Stop();
            endOfGameTasksRun = true;
        }
        if (playerControllerScript.gameStarted && !startOfGameTasksRun) 
        {
            TitleScreenText.SetActive(false);
            distanceText.gameObject.SetActive(true);
            stopWatch.Start();
            music.Play();
            startOfGameTasksRun = true;
        }
    }

    public IEnumerator FadeBlackOutSquare(float fadeSpeed = 0.25f)
    {
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;

        while (blackOutSquare.GetComponent<Image>().color.a < 1)
        {
            fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackOutSquare.GetComponent<Image>().color = objectColor;
            yield return null;
        }
    }

    public IEnumerator gameOver() // after failure, display each line of text one of time
    {
        yield return new WaitForSeconds(2);
        distanceText.gameObject.SetActive(false);
        failText.SetActive(true); // failure text
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MyGame");
    }

    public IEnumerator playerWin()
    {
        distanceText.gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
        TimeSpan ts = stopWatch.Elapsed;
        // Format and display the TimeSpan value.
        string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
        successText.text += "\n" + elapsedTime;
        successText.enabled = true;
        successText.gameObject.SetActive(true); // success

        Regex rgx = new Regex("\\D");
        UnityEngine.Debug.Log(rgx.Replace(PlayerPrefs.GetString("BestTime"), ""));
        float elapsedTimeFormatted = float.Parse(rgx.Replace(elapsedTime, "")); // format the best and current times so that they are comparable
        float bestTime;
        if (PlayerPrefs.GetString("BestTime") == "") {
            bestTime = 0;
        }
        else {
            bestTime = float.Parse(rgx.Replace(PlayerPrefs.GetString("BestTime"), ""));
        }

        if (elapsedTimeFormatted < bestTime || bestTime == 0) {
            PlayerPrefs.SetString("BestTime", elapsedTime);
            newBest.enabled = true;
            newBest.gameObject.SetActive(true);
        }
        UnityEngine.Debug.Log(elapsedTime);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MyGame");
    }

    public void UpdateDistanceText() // updates the distance text in the top right corner
    {
        // compares the x and y values that are closer to the edge of the map, and then updates the distance counter with the larger value
        int currentAbsoluteXPosition = (int) Math.Abs(playerControllerScript.transform.position.x);
        int currentAbsoluteYPosition = (int) Math.Abs(playerControllerScript.transform.position.y);
        distanceText.text = "exit: " + (260 - Math.Max(currentAbsoluteXPosition, currentAbsoluteYPosition)) + "m";
    }
}
