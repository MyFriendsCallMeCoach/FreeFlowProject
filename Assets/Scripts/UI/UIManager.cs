using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject PauseScreen, PauseButtons;

    public GameObject OptionsScreen;

    public GameObject PauseFirstButton, OptionsFirstButton, OptionsClosedButton;

    public Slider MasterSlide, MusicSlide, SFXSlide, LutSlider;

    public GameObject Lut1, Lut2, Lut3, Lut4, Lut5;

    public Text CoinText;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void Resume()
    {
        GameManager.instance.PauseUnpause();
        PauseButtons.SetActive(true);
    }


    public void OpenOptions()
    {
        OptionsScreen.SetActive(true);
        PauseButtons.SetActive(false);

        //we clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //we set new selected object
        EventSystem.current.SetSelectedGameObject(OptionsFirstButton);

    }

    public void CloseOptions()
    {
        PauseButtons.SetActive(true);
        OptionsScreen.SetActive(false);
        //we clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //we set new selected object
        EventSystem.current.SetSelectedGameObject(OptionsClosedButton);
    }

    public void LoadSave()
    {

    }

    public void MainMenu()
    {

    }


    public void Quit()
    {
        Debug.Log("QUitGamer");
        Application.Quit();

    }


    public void SetMasterLevel()
    {
        AudioManager.instance.SetMasterLevel();
    }


    public void SetMusicLevel()
    {
        AudioManager.instance.SetMusicLevel();
    }

    public void SetSFXLevel()
    {
        AudioManager.instance.SetSFXLevel();
    }

    //redo with arrays
    public void SetCurrentLut()
    {
        if (LutSlider.value == -2)
        {
            Lut1.SetActive(true);
            Lut2.SetActive(false);
            Lut3.SetActive(false);
            Lut4.SetActive(false);
            Lut5.SetActive(false);
        }
        else if (LutSlider.value == -1)
        {
            Lut1.SetActive(false);
            Lut2.SetActive(true);
            Lut3.SetActive(false);
            Lut4.SetActive(false);
            Lut5.SetActive(false);
        }
        else if ( LutSlider.value == 0)
        {
            Lut1.SetActive(false);
            Lut2.SetActive(false);
            Lut3.SetActive(true);
            Lut4.SetActive(false);
            Lut5.SetActive(false);
        }
        else if (LutSlider.value == 1)
        {
            Lut1.SetActive(false);
            Lut2.SetActive(false);
            Lut3.SetActive(false);
            Lut4.SetActive(true);
            Lut5.SetActive(false);
        }
        else if (LutSlider.value == 2)
        {
            Lut1.SetActive(false);
            Lut2.SetActive(false);
            Lut3.SetActive(false);
            Lut4.SetActive(false);
            Lut5.SetActive(true);
        }
    }




}
