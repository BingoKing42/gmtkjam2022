using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, ROLLING, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public GameObject diceManager;
    DiceManager diceManagerScript;

    /*
    public Slider timer;
    Timer timerScript;*/

    public GameObject d4_text;
    public GameObject d6_text;
    public GameObject d8_text;
    public GameObject d10_text;
    public GameObject d12_text;
    public GameObject d20_text;

    public GameObject dieSlot1;
    public GameObject dieSlot2;
    public GameObject dieSlot3;
    DieSlot dieSlot1Script;
    DieSlot dieSlot2Script;
    DieSlot dieSlot3Script;

    public Button spellButton;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    UnitInfo playerUnitInfo;
    UnitInfo enemyUnitInfo;
    
    public BattleHUDScript playerHUD;
    public BattleHUDScript enemyHUD;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;

        //set dice text to their highest values
        d4_text.GetComponent<TextMeshProUGUI>().text = "4";
        d6_text.GetComponent<TextMeshProUGUI>().text = "6";
        d8_text.GetComponent<TextMeshProUGUI>().text = "8";
        d10_text.GetComponent<TextMeshProUGUI>().text = "10";
        d12_text.GetComponent<TextMeshProUGUI>().text = "12";
        d20_text.GetComponent<TextMeshProUGUI>().text = "20";
        StartCoroutine(SetupBattle());
    }

    //Sets up battle, HUD UI
    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation.transform);
        playerUnitInfo = playerGO.GetComponent<UnitInfo>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation.transform);
        enemyUnitInfo = enemyGO.GetComponent<UnitInfo>();

        diceManagerScript = diceManager.GetComponent<DiceManager>();

        //timerScript = timer.GetComponent<Timer>();

        dieSlot1Script = dieSlot1.GetComponent<DieSlot>();
        dieSlot2Script = dieSlot2.GetComponent<DieSlot>();
        dieSlot3Script = dieSlot3.GetComponent<DieSlot>();
    
        //sets up HUD info (names and health)
        playerHUD.SetHUD(playerUnitInfo);
        enemyHUD.SetHUD(enemyUnitInfo);

        yield return new WaitForSeconds(3f);

        state = BattleState.ROLLING;
        StartCoroutine(RollTheDice());
    }

    IEnumerator RollTheDice()
    {
        //rolls dice and determines numbers on each face

        int[] dieValues = diceManagerScript.RollDice();

        Debug.Log("Die values: " + dieValues[0] + " " + dieValues[1] + " " + dieValues[2]);

        d4_text.GetComponent<TextMeshProUGUI>().text = dieValues[0].ToString();
        d6_text.GetComponent<TextMeshProUGUI>().text = dieValues[1].ToString();
        d8_text.GetComponent<TextMeshProUGUI>().text = dieValues[2].ToString();
        d10_text.GetComponent<TextMeshProUGUI>().text = dieValues[3].ToString();
        d12_text.GetComponent<TextMeshProUGUI>().text = dieValues[4].ToString();
        d20_text.GetComponent<TextMeshProUGUI>().text = dieValues[5].ToString();

        //do dice animation, just text cycling through numbers
        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        Debug.Log("finished rolling dice!");
        PlayerTurn();
    }
    
    //function to deal damage and do effects
    IEnumerator PlayerAttack()
    {
        int dmg;
        int heal;
        string effects;
        //IDK HOW TO RECEIVE THE VALUES
        diceManagerScript.ScorePicks(dieSlot1Script.slottedDie, dieSlot2Script.slottedDie, dieSlot3Script.slottedDie, out dmg, out heal, out effects);

        //INCLUDE function to return the three dice to their original spots
        dieSlot1Script.slottedDie.GetComponent<DragDrop>().ResetPosition();
        dieSlot2Script.slottedDie.GetComponent<DragDrop>().ResetPosition();
        dieSlot3Script.slottedDie.GetComponent<DragDrop>().ResetPosition();

        dieSlot1Script.slottedDie = null;
        dieSlot2Script.slottedDie = null;
        dieSlot3Script.slottedDie = null;

        dieSlot1Script.isEmpty = true;
        dieSlot2Script.isEmpty = true;
        dieSlot3Script.isEmpty = true;

        //deal damage and heal
        bool isDead = enemyUnitInfo.TakeDamage(dmg);
        playerUnitInfo.Heal(heal);

        //SPACE FOR OTHER, NON DIRECT DAMAGE OR HEAL EFFECTS
        
        //changes enemy HP 
        enemyHUD.SetHP(enemyUnitInfo.currentHP);
        playerHUD.SetHP(playerUnitInfo.currentHP);

        yield return new WaitForSeconds(4f);

        //check if dead
        if(isDead)
        {
            //end battle
            state = BattleState.WON;
            EndBattle();
        } else
        {
            StartCoroutine(EnemyTurn());
        }

    }

    //during player turn, wait until button is clicked. buttons for dice not in the pool are disabled
    void PlayerTurn()
    {
        Debug.Log("player turn started"); 
        //timerScript.BeginTimer();

        /*
        while (timerScript.gameTime > 0)
        {
            if (dieSlot1Script.isEmpty || dieSlot2Script.isEmpty || dieSlot3Script.isEmpty)
                spellButton.enabled = false;
        } 

        if (timerScript.gameTime == 0)
        {
            //revert dice back to their spaces
            //message to say turn was lost
            
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }*/


    }

    public void OnSpellButton()
    {
        //if not player turn, do nothing
        if (state != BattleState.PLAYERTURN)
        {
            Debug.Log("NOT YOUR TURN");
            return;
        }

        //if all slots filled, calcualte damage and effects
        if (!dieSlot1Script.isEmpty && !dieSlot2Script.isEmpty && !dieSlot3Script.isEmpty)
        {
            //timerScript.stopTimer = true;
            Debug.Log("Button Worked!");
            state = BattleState.ENEMYTURN;
            StartCoroutine(PlayerAttack());
        }
    }

    

    //enemy does flat damage, changes player HP, checks if game is over
    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(2f);

        int dwagonDamage = Random.Range(1, 7);
        bool isDead = playerUnitInfo.TakeDamage(dwagonDamage);

        playerHUD.SetHP(playerUnitInfo.currentHP);

        yield return new WaitForSeconds(1f);

        if(isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        } else
        {
            state = BattleState.ROLLING;
            StartCoroutine(RollTheDice());
        }
    }

    //gives final message when battle is over, should transition to a final scene
    void EndBattle()
    {
        
    }

}
