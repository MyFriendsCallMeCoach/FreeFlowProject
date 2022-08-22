using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [Header("Game Functionality")]
    public int CurrentCoins;

    private Vector3 RespawnPos, CameraPos;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UIManager.instance.CoinText.text = "" + CurrentCoins;

        RespawnPos = CMF.AdvancedWalkerController.instance.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        bool Paused = false;

        if (CMF.CharacterDefaultInput.instance.Pause_Input)
        {
            CMF.CharacterDefaultInput.instance.Pause_Input = false;
            //onoff = !onoff;
            PauseUnpause();
            Paused = true;
        }

        if (!Paused)
            return;
    }

    public void PauseUnpause()
    {
        if (UIManager.instance.PauseScreen.activeInHierarchy)
        {
            UIManager.instance.PauseScreen.SetActive(false);
            Time.timeScale = 1f;
           
            UIManager.instance.OptionsScreen.SetActive(false);

            //temp for testing menu functions
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            UIManager.instance.PauseScreen.SetActive(true);
            Time.timeScale = 0f;
            UIManager.instance.PauseButtons.SetActive(true);
            //temp for testing menu functions
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;


        }
    }


    public void AddCoins(int Amount)
    {
        CurrentCoins += Amount;

        UIManager.instance.CoinText.text = "" + CurrentCoins;
    }


    public void Respawn()
    {

        StartCoroutine(RespawnCo());
    }

    public IEnumerator RespawnCo()
    {
        Debug.Log("Respawning");
        CMF.AdvancedWalkerController.instance.gameObject.SetActive(false);
        CameraController.instance.theCMB.enabled = false;

        yield return new WaitForSeconds(2f);


        CMF.AdvancedWalkerController.instance.transform.position = RespawnPos;

        CameraController.instance.theCMB.enabled = true;

        CMF.AdvancedWalkerController.instance.gameObject.SetActive(true);
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        RespawnPos = newSpawnPoint;
    }

}
