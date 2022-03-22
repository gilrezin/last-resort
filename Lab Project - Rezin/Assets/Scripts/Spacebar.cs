using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spacebar : MonoBehaviour
{
    public Sprite spacebarUnpressed;
    public Sprite spacebarPressed;
    private Image spaceBarImage;
    bool isActive;

    void Awake() {
        spaceBarImage = gameObject.GetComponent<Image>();
        isActive = gameObject.activeSelf;
        StartCoroutine(SpaceBar());
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SpaceBar() // plays animation until game starts
    {
        gameObject.SetActive(true);
        isActive = gameObject.activeSelf;
        while (isActive)
        {
            spaceBarImage.sprite = spacebarPressed;
            yield return new WaitForSeconds(0.5f);
            spaceBarImage.sprite = spacebarUnpressed;
            yield return new WaitForSeconds(0.5f);
            isActive = gameObject.activeSelf;
        }
    }
}



