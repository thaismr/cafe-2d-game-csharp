using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideosGlo : MonoBehaviour {

    public GameObject video1;
    public GameObject video2;
    public GameObject video3;
    public GameObject video4;
    public GameObject video5;
    public GameObject video6;
    public GameObject video7;
    public GameObject video8;
    public GameObject video9;
    public GameObject video10;
    public GameObject image1;
    public GameObject image2;
    public GameObject image3;
    public GameObject image4;
    public GameObject image5;
    public GameObject image6;
    public GameObject image7;
    public GameObject image8;
    public GameObject image9;
    public GameObject image10;

    void LateUpdate()
    {
        if(video1.GetComponent<VideoPlayer>().isPlaying == false)
        {
            image1.SetActive(false);
            video1.SetActive(false);
        }
        if (video2.GetComponent<VideoPlayer>().isPlaying == false)
        {
            image2.SetActive(false);
            video2.SetActive(false);
        }
        if (video3.GetComponent<VideoPlayer>().isPlaying == false)
        {
            image3.SetActive(false);
            video3.SetActive(false);
        }
        if (video4.GetComponent<VideoPlayer>().isPlaying == false)
        {
            image4.SetActive(false);
            video4.SetActive(false);
        }
        if (video5.GetComponent<VideoPlayer>().isPlaying == false)
        {
            image5.SetActive(false);
            video5.SetActive(false);
        }
        if (video6.GetComponent<VideoPlayer>().isPlaying == false)
        {
            image6.SetActive(false);
            video6.SetActive(false);
        }
        if (video7.GetComponent<VideoPlayer>().isPlaying == false)
        {
            image7.SetActive(false);
            video7.SetActive(false);
        }
        if (video8.GetComponent<VideoPlayer>().isPlaying == false)
        {
            image8.SetActive(false);
            video8.SetActive(false);
        }
        if (video9.GetComponent<VideoPlayer>().isPlaying == false)
        {
            image9.SetActive(false);
            video9.SetActive(false);
        }
        if (video10.GetComponent<VideoPlayer>().isPlaying == false)
        {
            image10.SetActive(false);
            video10.SetActive(false);
        }

    }

    public void CocaCola()
    {
        image1.SetActive(true);
        video1.SetActive(true);
    }
    public void TostaMista()
    {
        image2.SetActive(true);
        video2.SetActive(true);
    }
    public void Chocolate()
    {
        image3.SetActive(true);
        video3.SetActive(true);
    }
    public void Cafe()
    {
        image4.SetActive(true);
        video4.SetActive(true);
    }
    public void SaladaFruta()
    {
        image5.SetActive(true);
        video5.SetActive(true);
    }
    public void Pastilha()
    {
        image6.SetActive(true);
        video6.SetActive(true);
    }
    public void SumoLa()
    {
        image7.SetActive(true);
        video7.SetActive(true);
    }
    public void Gelado()
    {
        image8.SetActive(true);
        video8.SetActive(true);
    }
    public void Sandes()
    {
        image9.SetActive(true);
        video9.SetActive(true);
    }
    public void BolaBerlim()
    {
        image10.SetActive(true);
        video10.SetActive(true);
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

}
