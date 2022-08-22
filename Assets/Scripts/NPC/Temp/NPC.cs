using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class NPC : MonoBehaviour
{
    //public Transform chatBG;
    public Transform NPCchar;

    public diasystem Diasystem;
    //[SerializeField] private KeyCode DiaInput = KeyCode.F;
    //public string DiaInputString = "TempDiaInput";

    public string Name;

    public float DiaboxPOS = 20f;

    [TextArea(20, 50)]
    public string[] sentances;



    // Start is called before the first frame update
    void Start()
    {
        Diasystem = FindObjectOfType<diasystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Pos = Camera.main.WorldToScreenPoint(NPCchar.position);
        Pos.y += 1000f; //DiaboxPOS;
    }

    public void OnTriggerStay(Collider other)
    {
        this.gameObject.GetComponent<NPC>().enabled = true;
        FindObjectOfType<diasystem>().EnterRangeOfNPC();
        if ((other.CompareTag("GameController")) && CMF.CharacterDefaultInput.instance.Interact_Input)
        {
            this.gameObject.GetComponent<NPC>().enabled = true;
            Diasystem = FindObjectOfType<diasystem>();
            Diasystem.Names = Name;
            Diasystem.diaLines = sentances;
            FindObjectOfType<diasystem>().NPCName();
        }
       
    }

    public void OnTriggerExit()
    {
        FindObjectOfType<diasystem>().OutOfRange();
        this.gameObject.GetComponent<NPC>().enabled = false;
    }



}
