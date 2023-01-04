using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Sprite sfxEnabled;
    public Sprite sfxDisabled;
    private Image sfxToggle;
    // Start is called before the first frame update
    void Start()
    {
        sfxToggle = gameObject.GetComponent<Image>();
        //Debug.Log("Volume: " + PlayerPrefs.GetInt("Volume"));
        if (PlayerPrefs.GetInt("PlayedBefore") == 0) // set volume if not played before
        {
            PlayerPrefs.SetInt("PlayedBefore", 1);
            PlayerPrefs.SetInt("Volume", 1);
        }
        AudioListener.volume = PlayerPrefs.GetInt("Volume"); // get the volume info from the last session the user played
        sfxToggle.sprite = AudioListener.volume == 1 ? sfxEnabled : sfxDisabled; // get the correct toggle sprite
    }

    // Update is called once per frame
    void Update()
    {
        // mute button by keypress M or touched mute button
        if (Input.GetKeyDown(KeyCode.M) || (Input.touchCount > 0 && Input.GetTouch(0).position.x < Globals.MENU_BUTTON_BOUNDS && Input.GetTouch(0).position.y < Globals.MENU_BUTTON_BOUNDS && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            AudioListener.volume = 1 - AudioListener.volume; // toggle volume
            PlayerPrefs.SetInt("Volume", (int)AudioListener.volume); // remember volume
            sfxToggle.sprite = AudioListener.volume == 1 ? sfxEnabled : sfxDisabled; // adjust sprite
        }
    }
}
