using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class diasystem : MonoBehaviour
{
    public Text nameText;
    public Text diaText;

    public GameObject diaGUI;
    public Transform diaBoxGUI;

    //[SerializeField] private KeyCode DiaInput = KeyCode.F;

    //public string DiaInputString = "TempDiaInput";

    public string Names;

    public string[] diaLines;

    public float letterDelay = 0.1f;
    public float letterMultiplier = 0.5f;

    public bool letterIsMultiplied = false;
    public bool diaActive = false;
    public bool diaEnded = false;
    public bool outOfRange = true;

    public AudioClip audioClip;
    AudioSource audioSource;





    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        diaText.text = "";
    }



    public void EnterRangeOfNPC()
    {
        outOfRange = false;
        diaGUI.SetActive(true);
        if (diaActive == true)
        {
            diaGUI.SetActive(false);


        }



    }


    public void NPCName()
    {
        outOfRange = false;
        diaBoxGUI.gameObject.SetActive(true);
        nameText.text = Names;
        if (CMF.CharacterDefaultInput.instance.Interact_Input)
        {
            if (!diaActive)
            {
                diaActive = true;
                StartCoroutine(StartDia());
            }
        }
        //StartCoroutine(StartDia());
    }

    private IEnumerator StartDia()
    {
        if (outOfRange == false)
        {
            int diaLength = diaLines.Length;
            int currentDiaIndex = 0;
                while (currentDiaIndex < diaLength || !letterIsMultiplied)
            {
                if (!letterIsMultiplied)
                {
                    letterIsMultiplied = true;
                    StartCoroutine(DisplayString(diaLines[currentDiaIndex++]));
                    if (currentDiaIndex >= diaLength)
                    {
                        diaEnded = true;
                    }
                }
                yield return 0;

            }
            



            while (true)
            {
                if (CMF.CharacterDefaultInput.instance.Interact_Input && diaEnded == false)
                {
                    break;
                }
                yield return 0;
            }

            diaEnded = false;
            diaActive = false;
            DropDia();


        }

    }

    private IEnumerator DisplayString(string stringToDisplay)
    {
        if (outOfRange == false)
        {
            int stringLength = stringToDisplay.Length;
            int currentCharIndex = 0;

            diaText.text = "";

            while (currentCharIndex < stringLength)
            {
                diaText.text += stringToDisplay[currentCharIndex];
                currentCharIndex++;

                if (currentCharIndex < stringLength)
                {
                    if (CMF.CharacterDefaultInput.instance.Interact_Input)
                    {
                        yield return new WaitForSeconds(letterDelay * letterMultiplier);

                       if (audioClip) audioSource.PlayOneShot(audioClip, 0.5f);
                    }
                    else
                    {
                        yield return new WaitForSeconds(letterDelay);

                        if (audioClip) audioSource.PlayOneShot(audioClip, 0.5f);

                    }
                }
                else
                {
                    diaEnded = false;
                    break;
                }
               
            }
            while (true)
            {
                if (CMF.CharacterDefaultInput.instance.Interact_Input)
                {
                    break;
                }
                yield return 0;
            }
            diaEnded = false;
            letterIsMultiplied = false;
            diaText.text = "";
        }



    }

    public void DropDia()
    {
        diaGUI.SetActive(false);
        diaBoxGUI.gameObject.SetActive(false);
        
    }

    public void OutOfRange()
    {
        outOfRange = true;
        if (outOfRange == true)
        {
            letterIsMultiplied = false;
            diaActive = false;
            StopAllCoroutines();
            diaGUI.SetActive(false);
            diaBoxGUI.gameObject.SetActive(false);
        }

    }
}
