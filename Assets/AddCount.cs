using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class AddCount : NetworkBehaviour
{    
    private int cunCount;
    private int chaCount;
    private int intCount;
    private int strCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallReady()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().ReadyUp();
    }

    public void Increment()
    {
        //futile attempts to make things work.
        //this.netIdentity.AssignClientAuthority(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().connectionToClient);
        //GameObject.Find("CharismaCounter").GetComponent<NetworkIdentity>().AssignClientAuthority(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().connectionToClient);
        if (!isLocalPlayer)
        {
            return;
        }
        GameObject thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string thisButName = thisButton.name;
        //chaCount = GameObject.Find("Player [connId=0]").GetComponent<PlayerScript>().Charisma;
        chaCount = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Charisma;
        cunCount = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Cunning;
        strCount = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Strength;
        intCount = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Intelligence;

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Available > 0)
        {

            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Available -= 1;
            switch (thisButName)
            {
                case "addCharisma":
                    chaCount++;
                    GameObject.Find("CharismaCounter").GetComponent<Text>().text = "Charisma: " + chaCount.ToString();
                    break;

                case "addCunning":
                    cunCount++;
                    GameObject.Find("CunningCounter").GetComponent<Text>().text = "Cunning: " + cunCount.ToString();
                    break;

                case "addStrength":
                    strCount++;
                    GameObject.Find("StrengthCounter").GetComponent<Text>().text = "Strength: " + strCount.ToString();
                    break;

                case "addIntelligence":
                    intCount++;
                    GameObject.Find("IntelligenceCounter").GetComponent<Text>().text =  "Intelligence: " + intCount.ToString();
                    break;
            }
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Charisma = chaCount;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Cunning = cunCount;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Strength = strCount;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Intelligence = intCount;

    }
}
