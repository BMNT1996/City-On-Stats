using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CreditsControls : MonoBehaviour
{

    public VideoPlayer video;
    private double endTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        PlayMyClip();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > endTime || Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("COS Main Menu");
    }

    void PlayMyClip()
    {
        video.Play();
        endTime = Time.time + 9.1091;

    }

}
