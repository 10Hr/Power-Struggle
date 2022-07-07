using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class SubCount : NetworkBehaviour
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

    public void Decrement()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        this.netIdentity.AssignClientAuthority(connectionToClient);
        chaCount = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Charisma;
        cunCount = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Cunning;
        strCount = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Strength;
        intCount = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Intelligence++;

        GameObject thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string thisButName = thisButton.name;

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Available < GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Max)
        {
            switch (thisButName)
            {
                case "subCharisma":
                    if (chaCount > 0)
                    {
                        chaCount--;
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Available++;
                        GameObject.Find("CharismaCounter").GetComponent<Text>().text = "Charisma: " + chaCount.ToString();
                    }
                    break;

                case "subCunning":
                    if (cunCount > 0)
                    {
                        cunCount--;
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Available++;
                        GameObject.Find("CunningCounter").GetComponent<Text>().text = "Cunning: " + cunCount.ToString();
                    }
                    break;

                case "subStrength":
                    if (strCount > 0)
                    {
                        strCount--;
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Available++;
                        GameObject.Find("StrengthCounter").GetComponent<Text>().text = "Strength: " + strCount.ToString();
                    }
                    break;

                case "subIntelligence":
                    if (intCount > 0)
                    {
                        intCount--;
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Available++;
                        GameObject.Find("IntelligenceCounter").GetComponent<Text>().text = "Intelligence: " + intCount.ToString();
                    }
                    break;
            }
        }

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Charisma = chaCount;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Cunning = cunCount;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Strength = strCount;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Intelligence = intCount;
    }
}
