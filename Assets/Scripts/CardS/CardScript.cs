using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Mirror;

public class CardScript : NetworkBehaviour
{

    public Sprite cardFront;
    public Sprite cardBack;

    public Sprite sprCharisma;
    public Sprite sprCunning;
    public Sprite sprIntelligence;
    public Sprite sprStrength;

    public Sprite[] sprArray;

    [SerializeField]
    private string type = "";
    [SerializeField]
    private string title = "";
    [SerializeField]
    private string cost = "";
    [SerializeField]
    private string id = "1000";
    [SerializeField]
    private string description = "";
    public bool hovered;
    public bool selected = false;
    public bool prevSelected = false;
    public bool revealable = false;
    public bool revealed = false;
    public int sortingDefault;
    public float defaultY;

    [SerializeField]
    public GameState gameState;

    public string Cost
    {
        get { return cost; }
        set { cost = value; }
    }

    public string ID
    {
        get { return id; }
        set
        {
            id = value;
        }
    }

    public string Title
    {
        get { return title; }
        set { title = value; }
    }

    public string Description
    {
        get { return description; }
        set { description = value; }
    }

    public string Type
    {
        get { return type; }
        set
        {
            type = value;
            switch (type)
            {
                case "charisma":
                    cardBack = sprCharisma;
                    break;
                case "cunning":
                    cardBack = sprCunning;
                    break;
                case "intelligence":
                    cardBack = sprIntelligence;
                    break;
                case "strength":
                    cardBack = sprStrength;
                    break;
            }
        }
    }




    // Start is called before the first frame update
    void Start()
    {


        defaultY = gameObject.transform.localPosition.y;
        gameObject.GetComponent<SpriteRenderer>().sprite = cardBack;
        sortingDefault = gameObject.GetComponent<SpriteRenderer>().sortingOrder;
//        gameState = GameObject.Find("FSM").GetComponent<GameState>();

        sprArray = Resources.LoadAll<Sprite>("PlayTestSprites");
    }

    // Update is called once per frame
    void Update()
    {
        if (int.Parse(id) != 1000 && cardFront != sprArray[int.Parse(id)])
        {
            cardFront = sprArray[int.Parse(id)];
        }
        if (gameObject.tag == "CardSlot" && cardFront != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = cardFront;
        }
        else if (cardBack != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = cardBack;
        }
        if (revealed == true)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = cardFront;
        }

        Enlarge();

        if (Input.GetKeyDown(KeyCode.Mouse1) && hovered && gameState.currentState == GameStates.LoadEnemyCards) {
            //Debug.Log("Right Clicked");
           //NetworkClient.localPlayer.GetComponent<PlayerScript>().ModifyPower(-25);
            if (NetworkClient.localPlayer.GetComponent<PlayerScript>().passive.passiveName == "Blackmarket" || NetworkClient.localPlayer.GetComponent<PlayerScript>().passive2 == "Blackmarket") {
                int index = 0;
                //Debug.Log("Blackmarket");
                foreach (GameObject g in NetworkClient.localPlayer.GetComponent<PlayerScript>().cardSlots) {
                    if (g.GetComponent<CardScript>().ID == this.ID) {
                        //Debug.Log("Discarding" + index);
                        //NetworkClient.localPlayer.GetComponent<PlayerScript>().DiscardCard(index, NetworkClient.localPlayer.GetComponent<PlayerScript>().cardSlots);
                       // NetworkClient.localPlayer.GetComponent<PlayerScript>().ModifyPower(-25);
                        break;
                    }
                    index++;
                }
            }

        }
    }

    public void Enlarge() {
        if (gameObject.tag == "Display")
            return;

        if (hovered && cardBack != null && gameObject.tag == "CardSlot") {
            gameObject.transform.localScale = new Vector3(75f, 75f, 0);
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, defaultY + 100, 0);
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 7;
        }
        if (!hovered && cardBack != null) {
            transform.localScale = new Vector3(45f, 45f, 0);
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, defaultY, 0);
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = sortingDefault;
        }
    }

    public void OnMouseEnter() {
        CardScript enhance = GameObject.Find("Enhanced").GetComponent<CardScript>();
        if (cardBack != null && (gameObject.tag == "CardSlot" || revealed))
        {
            enhance.transform.localScale = new Vector3(140f, 140f, 0);
            enhance.cardBack = sprArray[int.Parse(id)];
        }
        else if (cardBack != null && gameObject.tag == "Display")
        {
            enhance.transform.localScale = new Vector3(140f, 140f, 0);
            enhance.cardBack = cardFront;
        }
        else if (cardBack != null && gameObject.tag != "CardSlot" && !revealed)
        {

            enhance.transform.localScale = new Vector3(140f, 140f, 0);
            enhance.cardBack = cardBack;
        }
        hovered = true;
    }
    public void OnMouseExit() {
            hovered = false;
    }

    public void OnMouseDown()
    {
        PlayerScript currentP = NetworkClient.localPlayer.GetComponent<PlayerScript>();
        if (cardBack != null && gameObject.tag == "CardSlot" && gameState.currentState == GameStates.LoadEnemyCards && !currentP.LockedIn)
        {
            prevSelected = selected;
            selected = !selected;
        }
        else if ((cardBack != null && gameObject.tag == "CardSlot" && gameState.currentState == GameStates.Turn) 
            && gameState.currentPlayer.netId == currentP.netId && selected && !currentP.turnTaken)
        {
            if ((currentP.passive.passiveName == "SeeDeck" || currentP.passive2 == "SeeDeck") && !currentP.selectedTrg)
                    return;
                CmdDisplayCard(int.Parse(this.id), NetworkClient.localPlayer.GetComponent<PlayerScript>());
                gameState.currentPlayer.deck.pullEff(title, id);
                title = "";
                description = "";
                cost = "";
                id = "1000";
                type = "";
                selected = false;
                prevSelected = false;
                revealable = false;
                revealed = false;
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdDisplayCard(int id, PlayerScript p)
    {
        p.turnTaken = true;
        RpcDisplayCard(id);
    }  

    [ClientRpc]
    public void RpcDisplayCard(int id)
    {
        CardScript display = GameObject.Find("LastPlayed").GetComponent<CardScript>();
        display.cardBack = sprArray[id];
    }


  
}
