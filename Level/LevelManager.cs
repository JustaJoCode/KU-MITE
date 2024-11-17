using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    
    WaitForSeconds oneSec;
    public Transform[] spawnPositions;


    CameraManager camM;
    CharacterManager charM;
    LevelUI levelUI;

    public int maxTurns = 2;
    int currentTurn = 1;
    public bool countdown;
    public int maxTurnTimer = 30;
    int currentTimer;
    float internalTimer;

	void Start () {
        
        charM = CharacterManager.GetInstance();
        levelUI = LevelUI.GetInstance();
        camM = CameraManager.GetInstance();

     
        oneSec = new WaitForSeconds(1);

        levelUI.AnnouncerTextLine1.gameObject.SetActive(false);
        levelUI.AnnouncerTextLine2.gameObject.SetActive(false);

        StartCoroutine("StartGame");
       
	}

    void FixedUpdate()
    {
        
        if(charM.players[0].playerStates.transform.position.x < 
            charM.players[1].playerStates.transform.position.x)
        {
            charM.players[0].playerStates.lookRight = true;
            charM.players[1].playerStates.lookRight = false;
        }
        else
        {
            charM.players[0].playerStates.lookRight = false;
            charM.players[1].playerStates.lookRight = true;
        }
        levelUI.SetScoreText();

    }

    void Update()
    {
        if (countdown)
        {
            HandleTurnTimer();
        }

       

        if (levelUI.player1Score == 8)
        {
            FinalTurnFunction(true);

        }

        if (levelUI.player2Score == 8)
        {
            FinalTurnFunction(true);

        }
    }

    void HandleTurnTimer()
    {
        levelUI.LevelTimer.text = currentTimer.ToString();

        internalTimer += Time.deltaTime; 

        if (internalTimer > 1)
        {
            currentTimer--; 
            internalTimer = 0;
        }

        if (currentTimer <= 0) 
        {
            EndTurnFunction(true);
            countdown = false;
        }

      
       
    }

   IEnumerator StartGame()
    {
        yield return CreatePlayers();
        yield return InitTurn();
    }
	
    IEnumerator InitTurn()
    {
        
        levelUI.AnnouncerTextLine1.gameObject.SetActive(false);
        levelUI.AnnouncerTextLine2.gameObject.SetActive(false);

        currentTimer = maxTurnTimer;
        countdown = false;

       
        yield return InitPlayers();
        yield return EnableControl();

    }

    IEnumerator CreatePlayers()
    {
       
        for (int i = 0; i < charM.players.Count; i++)
        {
          
            GameObject go = Instantiate(charM.players[i].playerPrefab
            , spawnPositions[i].position, Quaternion.identity)
            as GameObject;

            
            charM.players[i].playerStates = go.GetComponent<StateManager>();

            charM.players[i].playerStates.healthSlider = levelUI.healthSliders[i];
            camM.players.Add(go.transform);
        }

        yield return null;
    }

    IEnumerator InitPlayers()
    {
       
        for (int i = 0; i < charM.players.Count; i++)
        {
            charM.players[i].playerStates.health = 100;
            charM.players[i].playerStates.handleAnim.anim.Play("Locomotion");
            charM.players[i].playerStates.transform.position = spawnPositions[i].position;
        }

        yield return null;
    }

	IEnumerator EnableControl()
    {
        

        levelUI.AnnouncerTextLine1.gameObject.SetActive(true);
        levelUI.AnnouncerTextLine1.text = "Turn " + currentTurn;
        levelUI.AnnouncerTextLine1.color = Color.white;
        yield return oneSec;
        yield return oneSec;

       
        levelUI.AnnouncerTextLine1.text = "3";
        levelUI.AnnouncerTextLine1.color = Color.green;
        yield return oneSec;
        levelUI.AnnouncerTextLine1.text = "2";
        levelUI.AnnouncerTextLine1.color = Color.yellow;
        yield return oneSec;
        levelUI.AnnouncerTextLine1.text = "1";
        levelUI.AnnouncerTextLine1.color = Color.red;
        yield return oneSec;
        levelUI.AnnouncerTextLine1.color = Color.red;
        levelUI.AnnouncerTextLine1.text = "FIGHT!";

       
        for (int i = 0; i < charM.players.Count; i++)
        {
            
            if(charM.players[i].playerType == PlayerBase.PlayerType.user)
            {
                InputHandler ih = charM.players[i].playerStates.gameObject.GetComponent<InputHandler>();
                ih.playerInput = charM.players[i].inputId;
                ih.enabled = true;
            }

            //If it's an AI character
             if(charM.players[i].playerType == PlayerBase.PlayerType.ai)
             {
                 AICharacter ai = charM.players[i].playerStates.gameObject.GetComponent<AICharacter>();
                 ai.enabled = true;
                 
                 //assign the enemy states to be the one from the opposite player
                 ai.enStates = charM.returnOppositePlater(charM.players[i]).playerStates;
             }
        }

        //after a second, disable the announcer text
        yield return oneSec;
        levelUI.AnnouncerTextLine1.gameObject.SetActive(false);
        countdown = true;
    } 

    void DisableControl()
    {
        //to disable the controls, you need to disable the component that makes a character controllable
        for (int i = 0; i < charM.players.Count; i++)
        {
           
            charM.players[i].playerStates.ResetStateInputs();
            if(charM.players[i].playerType == PlayerBase.PlayerType.user)
            {
                charM.players[i].playerStates.GetComponent<InputHandler>().enabled = false;
            }

            if(charM.players[i].playerType == PlayerBase.PlayerType.ai)
            {
                charM.players[i].playerStates.GetComponent<AICharacter>().enabled = false;
            }
        }
    }

    public void EndTurnFunction(bool timeOut = false)
    {
        
        countdown = false;
        levelUI.LevelTimer.text = maxTurnTimer.ToString() ;
        if (timeOut)
        {
           
            levelUI.AnnouncerTextLine1.gameObject.SetActive(true);
            levelUI.AnnouncerTextLine1.text = "Time Out!";
            levelUI.AnnouncerTextLine1.color = Color.cyan;
        }
        else
        {
            levelUI.AnnouncerTextLine1.gameObject.SetActive(true);
            levelUI.AnnouncerTextLine1.text = "K.O.";
            levelUI.AnnouncerTextLine1.color = Color.red;
        }

        DisableControl();
        StartCoroutine("EndTurn");
    }
    public void FinalTurnFunction(bool timeOut = false)
    {

        countdown = false;
        levelUI.LevelTimer.text = maxTurnTimer.ToString();
        if (timeOut)
        {

            levelUI.AnnouncerTextLine1.gameObject.SetActive(true);
            levelUI.AnnouncerTextLine1.text = "Time Out!";
            levelUI.AnnouncerTextLine1.color = Color.cyan;
        }
        else
        {
            levelUI.AnnouncerTextLine1.gameObject.SetActive(true);
            levelUI.AnnouncerTextLine1.text = "K.O.";
            levelUI.AnnouncerTextLine1.color = Color.red;
        }

        DisableControl();
        StartCoroutine("FinalTurn");
    }

    IEnumerator EndTurn()
    {
       
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;

      
        PlayerBase vPlayer = FindWinningPlayer();

        if(vPlayer == null) 
        {
            
            levelUI.AnnouncerTextLine1.text = "Draw";
            levelUI.AnnouncerTextLine1.color = Color.blue;
        }
        else
        {
          
            levelUI.AnnouncerTextLine1.text = vPlayer.playerId + " Wins!";
            levelUI.AnnouncerTextLine1.color = Color.red;
        }

       
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;

        //check to see if the victorious player has taken any damage
        if (vPlayer != null)
        {
            //if not, then it's a flawless victory
            if (vPlayer.playerStates.health == 100)
            {
                levelUI.AnnouncerTextLine2.gameObject.SetActive(true);
                levelUI.AnnouncerTextLine2.text = "Flawless Victory!";
            }
        }

       
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;

        currentTurn++;

        bool matchOver = isMatchOver();

        if (!matchOver)
        {
            StartCoroutine("InitTurn"); 
        }
        else
        {
            for (int i = 0; i < charM.players.Count; i++)
            {
                charM.players[i].score = 0;
                charM.players[i].hasCharacter = false;
            }

            if (charM.solo)
            {
                if(vPlayer == charM.players[0])
                    MySceneManager.GetInstance().LoadNextOnProgression();
               
            }
            else
            {
                MySceneManager.GetInstance().RequestLevelLoad(SceneType.main, "select");
            }
        }
    }
  
    bool isMatchOver()
    {
        bool retVal = false;

        for (int i = 0; i < charM.players.Count; i++)
        {
            if(charM.players[i].score >= maxTurns)
            {
                retVal = true;
                break;
            }
        }

        return retVal;
    }

    PlayerBase FindWinningPlayer()
    {
        //to find who won the turn
        PlayerBase retVal = null;

        StateManager targetPlayer = null;

        //check first to see if both players have equal health
        if(charM.players[0].playerStates.health != charM.players[1].playerStates.health)
        {
            //if not, then check who has the lower health, the other one is the winner
            if(charM.players[0].playerStates.health < charM.players[1].playerStates.health)
            {
                charM.players[1].score++;
                levelUI.player1Score++;
                targetPlayer = charM.players[1].playerStates;
                
            }
            else
            {
                charM.players[0].score++;
                levelUI.player2Score++;
                targetPlayer = charM.players[0].playerStates;
                
            }

            retVal = charM.returnPlayerFromStates(targetPlayer); 
        }

        return retVal;
    }

    IEnumerator FinalTurn()
    {

        yield return oneSec;
        yield return oneSec;
        yield return oneSec;


        PlayerBase vPlayer = FindWinningPlayer();

        if (vPlayer == null)
        {

            levelUI.AnnouncerTextLine1.text = "Draw";
            levelUI.AnnouncerTextLine1.color = Color.blue;
        }
        else
        {

            levelUI.AnnouncerTextLine1.text = vPlayer.playerId + " Wins!";
            levelUI.AnnouncerTextLine1.color = Color.red;
        }


        yield return oneSec;
        yield return oneSec;
        yield return oneSec;

        //check to see if the victorious player has taken any damage
        if (vPlayer != null)
        {
            //if not, then it's a flawless victory
            if (vPlayer.playerStates.health == 100)
            {
                levelUI.AnnouncerTextLine2.gameObject.SetActive(true);
                levelUI.AnnouncerTextLine2.text = "Flawless Victory!";
            }
        }


        yield return oneSec;
        yield return oneSec;
        yield return oneSec;

    }

    public static LevelManager instance;
    public static LevelManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }
   
}

