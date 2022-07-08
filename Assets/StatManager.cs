using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class StatManager : NetworkBehaviour {
    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;
    PlayerManager playerManager;
    [SyncVar]
    private int count;

    public void getCount() { count++; }

    void Awake() { 
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        //GameObject.Find("Main Camera").SetActive(false);
    }

    private void Update()
    {
        //NEVER HAPPENS
        if (player4 != null)
        {
            Debug.Log("ALL 4 ARE HERE");
            getREADYBABYWOOOOOOOOOOOO();
        }
    }

    public void Increment() {
        //Get the button that is being pressed
        GameObject thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string thisButName = thisButton.name;

            string buttontag =  UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.tag;
       //     Debug.Log(playerManager.getPlayer(0) + " is also " + player1);
            switch (buttontag)
            {
                case "Player1":
                    player1 = playerManager.getPlayer(0);
                    if (player1.GetComponent<PlayerScript>().Available > 0)
                    {
                        player1.GetComponent<PlayerScript>().Available -= 1;

                        Debug.Log(playerManager.getPlayer(0) + " is also " + player1);
                        switch (thisButName) {
                            case "addCharisma":
                                player1.GetComponent<PlayerScript>().Charisma++;
                                GameObject.Find("CharismaCounter1").GetComponent<Text>().text = "Charisma: " + player1.GetComponent<PlayerScript>().Charisma.ToString();
                                break;

                            case "addCunning":
                                player1.GetComponent<PlayerScript>().Cunning++;
                                GameObject.Find("CunningCounter1").GetComponent<Text>().text = "Cunning: " + player1.GetComponent<PlayerScript>().Cunning.ToString();
                                break;

                            case "addStrength":
                                player1.GetComponent<PlayerScript>().Strength++;
                                GameObject.Find("StrengthCounter1").GetComponent<Text>().text = "Strength: " + player1.GetComponent<PlayerScript>().Strength.ToString();
                                break;

                            case "addIntelligence":
                                player1.GetComponent<PlayerScript>().Intelligence++;
                                GameObject.Find("IntelligenceCounter1").GetComponent<Text>().text = "Intelligence: " + player1.GetComponent<PlayerScript>().Intelligence.ToString();
                                break;
                        }
                    }
                    break;
                case "Player2":
                    player2 = playerManager.getPlayer(1);
                    if (player2.GetComponent<PlayerScript>().Available > 0) {
                        player2.GetComponent<PlayerScript>().Available -= 1;
                    Debug.Log(playerManager.getPlayer(1) + " is also " + player2);
                        switch (thisButName) {
                            case "addCharisma":
                                player2.GetComponent<PlayerScript>().Charisma++;
                                GameObject.Find("CharismaCounter2").GetComponent<Text>().text = "Charisma: " + player2.GetComponent<PlayerScript>().Charisma.ToString();
                                break;

                            case "addCunning":
                                player2.GetComponent<PlayerScript>().Cunning++;
                                GameObject.Find("CunningCounter2").GetComponent<Text>().text = "Cunning: " + player2.GetComponent<PlayerScript>().Cunning.ToString();
                                break;

                            case "addStrength":
                                player2.GetComponent<PlayerScript>().Strength++;
                                GameObject.Find("StrengthCounter2").GetComponent<Text>().text = "Strength: " + player2.GetComponent<PlayerScript>().Strength.ToString();
                                break;

                            case "addIntelligence":
                                player2.GetComponent<PlayerScript>().Intelligence++;
                                GameObject.Find("IntelligenceCounter2").GetComponent<Text>().text = "Intelligence: " + player2.GetComponent<PlayerScript>().Intelligence.ToString();
                                break;
                        }
                    }
                    break;

                case "Player3":
                     player3 = playerManager.getPlayer(2);
                    if (player3.GetComponent<PlayerScript>().Available > 0)
                    {
                        player3.GetComponent<PlayerScript>().Available -= 1;
                        switch (thisButName) {
                            case "addCharisma":
                                player3.GetComponent<PlayerScript>().Charisma++;
                                GameObject.Find("CharismaCounter3").GetComponent<Text>().text = "Charisma: " + player3.GetComponent<PlayerScript>().Charisma.ToString();
                                break;

                            case "addCunning":
                                player3.GetComponent<PlayerScript>().Cunning++;
                                GameObject.Find("CunningCounter3").GetComponent<Text>().text = "Cunning: " + player3.GetComponent<PlayerScript>().Cunning.ToString();
                                break;

                            case "addStrength":
                                player3.GetComponent<PlayerScript>().Strength++;
                                GameObject.Find("StrengthCounter3").GetComponent<Text>().text = "Strength: " + player3.GetComponent<PlayerScript>().Strength.ToString();
                                break;

                            case "addIntelligence":
                                player3.GetComponent<PlayerScript>().Intelligence++;
                                GameObject.Find("IntelligenceCounter3").GetComponent<Text>().text = "Intelligence: " + player3.GetComponent<PlayerScript>().Intelligence.ToString();
                                break;
                        }
                    }
                    break;

                case "Player4":
                    player4 = playerManager.getPlayer(3);
                    if (player4.GetComponent<PlayerScript>().Available > 0) {
                        player4.GetComponent<PlayerScript>().Available -= 1;
                        switch (thisButName) {
                            case "addCharisma":
                                player4.GetComponent<PlayerScript>().Charisma++;
                                GameObject.Find("CharismaCounter4").GetComponent<Text>().text = "Charisma: " + player4.GetComponent<PlayerScript>().Charisma.ToString();
                                break;

                            case "addCunning":
                                player4.GetComponent<PlayerScript>().Cunning++;
                                GameObject.Find("CunningCounter4").GetComponent<Text>().text = "Cunning: " + player4.GetComponent<PlayerScript>().Cunning.ToString();
                                break;

                            case "addStrength":
                                player4.GetComponent<PlayerScript>().Strength++;
                                GameObject.Find("StrengthCounter4").GetComponent<Text>().text = "Strength: " + player4.GetComponent<PlayerScript>().Strength.ToString();
                                break;

                            case "addIntelligence":
                                player4.GetComponent<PlayerScript>().Intelligence++;
                                GameObject.Find("IntelligenceCounter4").GetComponent<Text>().text = "Intelligence: " + player4.GetComponent<PlayerScript>().Intelligence.ToString();
                                break;
                        }
                    }
                    break;

            }

    }

    // make more efficient later
    public void Decrement() {


        GameObject thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string thisButName = thisButton.name;

        string buttontag =  UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.tag;

        switch (buttontag) {

                case "Player1":
                    player1 = playerManager.getPlayer(0);
                    if (player1.GetComponent<PlayerScript>().Available != player1.GetComponent<PlayerScript>().Max) {
                        switch (thisButName) {
                            case "subCharisma":
                                if (player1.GetComponent<PlayerScript>().Charisma > 0) {
                                    player1.GetComponent<PlayerScript>().Charisma--;
                                    player1.GetComponent<PlayerScript>().Available += 1;
                                    GameObject.Find("CharismaCounter1").GetComponent<Text>().text = "Charisma: " + player1.GetComponent<PlayerScript>().Charisma.ToString();
                                }
                            break;
                            case "subCunning":
                                if (player1.GetComponent<PlayerScript>().Cunning > 0) {
                                    player1.GetComponent<PlayerScript>().Cunning--;
                                    player1.GetComponent<PlayerScript>().Available += 1;
                                    GameObject.Find("CunningCounter1").GetComponent<Text>().text = "Cunning: " + player1.GetComponent<PlayerScript>().Cunning.ToString();
                                }
                            break;
                            case "subStrength":
                                if (player1.GetComponent<PlayerScript>().Strength > 0) {
                                    player1.GetComponent<PlayerScript>().Strength--;
                                    player1.GetComponent<PlayerScript>().Available += 1;
                                    GameObject.Find("StrengthCounter1").GetComponent<Text>().text = "Strength: " + player1.GetComponent<PlayerScript>().Strength.ToString();
                                }
                            break;
                            case "subIntelligence":
                                if (player1.GetComponent<PlayerScript>().Intelligence > 0) {
                                    player1.GetComponent<PlayerScript>().Intelligence--;
                                    player1.GetComponent<PlayerScript>().Available += 1;
                                    GameObject.Find("IntelligenceCounter1").GetComponent<Text>().text = "Intelligence: " + player1.GetComponent<PlayerScript>().Intelligence.ToString();
                                }
                            break;
                        }
                    }
                    break;
                case "Player2":
                    player2 = playerManager.getPlayer(1);
                    if (player2.GetComponent<PlayerScript>().Available != player2.GetComponent<PlayerScript>().Max) {
                        switch (thisButName) {
                            case "subCharisma":
                                if (player2.GetComponent<PlayerScript>().Charisma > 0) {
                                    player2.GetComponent<PlayerScript>().Charisma--;
                                    player2.GetComponent<PlayerScript>().Available += 1;
                                    GameObject.Find("CharismaCounter2").GetComponent<Text>().text = "Charisma: " + player2.GetComponent<PlayerScript>().Charisma.ToString();
                                }
                                break;
                            case "subCunning":
                                if (player2.GetComponent<PlayerScript>().Cunning > 0) {
                                    player2.GetComponent<PlayerScript>().Cunning--;
                                    player2.GetComponent<PlayerScript>().Available += 1;
                                    GameObject.Find("CunningCounter2").GetComponent<Text>().text = "Cunning: " + player2.GetComponent<PlayerScript>().Cunning.ToString();
                                }
                                  break;
                            case "subStrength":
                                if (player2.GetComponent<PlayerScript>().Strength > 0) {
                                    player2.GetComponent<PlayerScript>().Strength--;
                                    player2.GetComponent<PlayerScript>().Available += 1;
                                    GameObject.Find("StrengthCounter2").GetComponent<Text>().text = "Strength: " + player2.GetComponent<PlayerScript>().Strength.ToString();
                                }
                                break;
                            case "subIntelligence":
                                if (player2.GetComponent<PlayerScript>().Intelligence > 0) {
                                    player2.GetComponent<PlayerScript>().Intelligence--;
                                    player2.GetComponent<PlayerScript>().Available += 1;
                                    GameObject.Find("IntelligenceCounter2").GetComponent<Text>().text = "Intelligence: " + player2.GetComponent<PlayerScript>().Intelligence.ToString();
                                }
                                break;
                        }
                    }
                    break;

                case "Player3":
                     player3 = playerManager.getPlayer(2);
                    if (player3.GetComponent<PlayerScript>().Available != player3.GetComponent<PlayerScript>().Max) {
                        switch (thisButName) {
                            case "subCharisma":
                             if (player3.GetComponent<PlayerScript>().Charisma > 0) {
                                player3.GetComponent<PlayerScript>().Charisma--;
                                player3.GetComponent<PlayerScript>().Available += 1;
                                GameObject.Find("CharismaCounter3").GetComponent<Text>().text = "Charisma: " + player3.GetComponent<PlayerScript>().Charisma.ToString();
                             }
                                break;

                            case "subCunning":
                             if (player3.GetComponent<PlayerScript>().Cunning > 0) {
                                player3.GetComponent<PlayerScript>().Cunning--;
                                player3.GetComponent<PlayerScript>().Available += 1;
                                GameObject.Find("CunningCounter3").GetComponent<Text>().text = "Cunning: " + player3.GetComponent<PlayerScript>().Cunning.ToString();
                             }
                                break;

                            case "subStrength":
                             if (player3.GetComponent<PlayerScript>().Strength > 0) {
                                player3.GetComponent<PlayerScript>().Strength--;
                                player3.GetComponent<PlayerScript>().Available += 1;
                                GameObject.Find("StrengthCounter3").GetComponent<Text>().text = "Strength: " + player3.GetComponent<PlayerScript>().Strength.ToString();
                             }
                                break;

                            case "subIntelligence":
                             if (player3.GetComponent<PlayerScript>().Intelligence > 0) {
                                player3.GetComponent<PlayerScript>().Intelligence--;
                                player3.GetComponent<PlayerScript>().Available += 1;
                                GameObject.Find("IntelligenceCounter3").GetComponent<Text>().text = "Intelligence: " + player3.GetComponent<PlayerScript>().Intelligence.ToString();
                             }
                                break;
                        }
                    }
                    break;

                case "Player4":
                player4 = playerManager.getPlayer(3);
                    if (player4.GetComponent<PlayerScript>().Available != player4.GetComponent<PlayerScript>().Max) {
                        switch (thisButName) {
                            case "subCharisma":
                             if (player4.GetComponent<PlayerScript>().Charisma > 0) {
                                player4.GetComponent<PlayerScript>().Charisma--;
                                player4.GetComponent<PlayerScript>().Available += 1;
                                GameObject.Find("CharismaCounter4").GetComponent<Text>().text = "Charisma: " + player4.GetComponent<PlayerScript>().Charisma.ToString();
                             }
                                break;

                            case "subCunning":
                             if (player4.GetComponent<PlayerScript>().Cunning > 0) {
                                player4.GetComponent<PlayerScript>().Cunning--;
                                player4.GetComponent<PlayerScript>().Available += 1;
                                GameObject.Find("CunningCounter4").GetComponent<Text>().text = "Cunning: " + player4.GetComponent<PlayerScript>().Cunning.ToString();
                             }
                                break;

                            case "subStrength":
                             if (player4.GetComponent<PlayerScript>().Strength > 0) {
                                player4.GetComponent<PlayerScript>().Strength--;
                                player4.GetComponent<PlayerScript>().Available += 1;
                                GameObject.Find("StrengthCounter4").GetComponent<Text>().text = "Strength: " + player4.GetComponent<PlayerScript>().Strength.ToString();
                             }
                                break;

                            case "subIntelligence":
                             if (player4.GetComponent<PlayerScript>().Intelligence > 0) {
                                player4.GetComponent<PlayerScript>().Intelligence--;
                                player4.GetComponent<PlayerScript>().Available += 1;
                                GameObject.Find("IntelligenceCounter4").GetComponent<Text>().text = "Intelligence: " + player4.GetComponent<PlayerScript>().Intelligence.ToString();
                             }
                                break;
                        }
                    }
                    break;

            }


    }


    void getREADYBABYWOOOOOOOOOOOO() {

        if (player1.GetComponent<PlayerScript>().Available == 0)//player1.GetComponent<PlayerScript>().Max) 
            player1.GetComponent<PlayerScript>().ReadyUp();

        if (player2.GetComponent<PlayerScript>().Available == 0)//player2.GetComponent<PlayerScript>().Max) 
            player2.GetComponent<PlayerScript>().ReadyUp();
        
        if (player3.GetComponent<PlayerScript>().Available == 0)//player3.GetComponent<PlayerScript>().Max) 
            player3.GetComponent<PlayerScript>().ReadyUp();
    
        if (player4.GetComponent<PlayerScript>().Available == 0)//player4.GetComponent<PlayerScript>().Max) 
            player4.GetComponent<PlayerScript>().ReadyUp();

        if (player1.GetComponent<PlayerScript>().ReadyUp() == true && player2.GetComponent<PlayerScript>().ReadyUp() == true && player3.GetComponent<PlayerScript>().ReadyUp() == true && player4.GetComponent<PlayerScript>().ReadyUp() == true) {
            Debug.Log("All players are ready! WOOOOOOOOOOOO!!?!");
        }

    }

}
