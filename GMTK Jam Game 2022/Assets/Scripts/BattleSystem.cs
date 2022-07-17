using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum BattleState { START, ROLLING, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public GameObject diceManager;
    DiceManager diceManagerScript;

    public AnimationManager AnimationManager;

    public Slider timer;
    Timer timerScript;

    public GameObject d4_text;
    public GameObject d6_text;
    public GameObject d8_text;
    public GameObject d10_text;
    public GameObject d12_text;
    public GameObject d20_text;

    public TextMeshProUGUI comboNotification;
    public TextMeshProUGUI enemyStatus;
    public TextMeshProUGUI playerStatus;

    public GameObject dieSlot1;
    public GameObject dieSlot2;
    public GameObject dieSlot3;
    DieSlot dieSlot1Script;
    DieSlot dieSlot2Script;
    DieSlot dieSlot3Script;

    public Button spellButton;

    UnitInfo playerUnitInfo;
    UnitInfo enemyUnitInfo;
    
    public BattleHUDScript playerHUD;
    public BattleHUDScript enemyHUD;

    public BattleState state;

    private bool diceSpin = false;
    private bool doubleDwagonDamage = false;
    private int elementNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        timerScript = timer.GetComponent<Timer>();

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
        playerUnitInfo = playerPrefab.GetComponent<UnitInfo>();
        enemyUnitInfo = playerPrefab.GetComponent<UnitInfo>();

        diceManagerScript = diceManager.GetComponent<DiceManager>();

        comboNotification.text = "";
        enemyStatus.text = "";
        playerStatus.text = "";

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

    void Update()
    {
        //start dice cycle up phase
        if (diceSpin == true)
        {
            d4_text.GetComponent<TextMeshProUGUI>().text = Random.Range(1, 5).ToString();
            d6_text.GetComponent<TextMeshProUGUI>().text = Random.Range(1, 7).ToString();
            d8_text.GetComponent<TextMeshProUGUI>().text = Random.Range(1, 9).ToString();
            d10_text.GetComponent<TextMeshProUGUI>().text = Random.Range(1, 11).ToString();
            d12_text.GetComponent<TextMeshProUGUI>().text = Random.Range(1, 13).ToString();
            d20_text.GetComponent<TextMeshProUGUI>().text = Random.Range(1, 21).ToString();
        }

        if (state == BattleState.PLAYERTURN && timerScript.currentTimeValue <= 0)
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(SkipTurn());
        }
    }

    IEnumerator SkipTurn()
    {
        comboNotification.text = "Oh no, your spell fizzled away\nBetter be faster next time!";
        yield return new WaitForSeconds(2f);
        StartCoroutine(EnemyTurn());
    }

    IEnumerator RollTheDice()
    {
        //rolls dice and determines numbers on each face

        int[] dieValues = diceManagerScript.RollDice();

        Debug.Log("Die values: " + dieValues[0] + " " + dieValues[1] + " " + dieValues[2]);

        diceSpin = true;
        yield return new WaitForSeconds(3f);
        diceSpin = false;

        d4_text.GetComponent<TextMeshProUGUI>().text = dieValues[0].ToString();
        d6_text.GetComponent<TextMeshProUGUI>().text = dieValues[1].ToString();
        d8_text.GetComponent<TextMeshProUGUI>().text = dieValues[2].ToString();
        d10_text.GetComponent<TextMeshProUGUI>().text = dieValues[3].ToString();
        d12_text.GetComponent<TextMeshProUGUI>().text = dieValues[4].ToString();
        d20_text.GetComponent<TextMeshProUGUI>().text = dieValues[5].ToString();

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

        comboNotification.text = effects;

        AnimationManager.PlayerAttackAnimation();

        //INCLUDE function to return the three dice to their original spots
        dieSlot1Script.slottedDie.GetComponent<DragDrop>().ResetPosition();
        dieSlot2Script.slottedDie.GetComponent<DragDrop>().ResetPosition();
        dieSlot3Script.slottedDie.GetComponent<DragDrop>().ResetPosition();

        Debug.Log(elementNum);
        switch (elementNum)
        {
            case 0:
                break;
            case 1:
                if (dieSlot1Script.slottedDie.GetComponent<DragDrop>().dieMax != 4 && dieSlot2Script.slottedDie.GetComponent<DragDrop>().dieMax != 4 && dieSlot3Script.slottedDie.GetComponent<DragDrop>().dieMax != 4)
                {
                    doubleDwagonDamage = true;
                }
                break;
            case 2:
                if (dieSlot1Script.slottedDie.GetComponent<DragDrop>().dieMax != 6 && dieSlot2Script.slottedDie.GetComponent<DragDrop>().dieMax != 6 && dieSlot3Script.slottedDie.GetComponent<DragDrop>().dieMax != 6)
                {
                    doubleDwagonDamage = true;
                }
                break;
            case 3:
                if (dieSlot1Script.slottedDie.GetComponent<DragDrop>().dieMax != 8 && dieSlot2Script.slottedDie.GetComponent<DragDrop>().dieMax != 8 && dieSlot3Script.slottedDie.GetComponent<DragDrop>().dieMax != 8)
                {
                    doubleDwagonDamage = true;
                }
                break;
            case 4:
                if (dieSlot1Script.slottedDie.GetComponent<DragDrop>().dieMax != 10 && dieSlot2Script.slottedDie.GetComponent<DragDrop>().dieMax != 10 && dieSlot3Script.slottedDie.GetComponent<DragDrop>().dieMax != 10)
                {
                    doubleDwagonDamage = true;
                }
                break;
            case 5:
                if (dieSlot1Script.slottedDie.GetComponent<DragDrop>().dieMax != 12 && dieSlot2Script.slottedDie.GetComponent<DragDrop>().dieMax != 12 && dieSlot3Script.slottedDie.GetComponent<DragDrop>().dieMax != 12)
                {
                    doubleDwagonDamage = true;
                }
                break;
            default:
                break;
        }

        dieSlot1Script.slottedDie = null;
        dieSlot2Script.slottedDie = null;
        dieSlot3Script.slottedDie = null;

        dieSlot1Script.isEmpty = true;
        dieSlot2Script.isEmpty = true;
        dieSlot3Script.isEmpty = true;
        
        ResetDice();
        
        bool isDead = false;
        //deal damage and heal
        if (dmg > 0)
        {
            isDead = enemyUnitInfo.TakeDamage(dmg);
            enemyStatus.text = "-" + dmg + " Health";
        }
        if (heal > 0)
        {
            playerStatus.text = "+" + heal + " Health";
        }

        if (effects.Contains("Triple"))
        {
            state = BattleState.ROLLING;
            StartCoroutine(RollTheDice());
        }
        
        //changes enemy HP 
        enemyHUD.SetHP(enemyUnitInfo.currentHP);
        playerHUD.SetHP(playerUnitInfo.currentHP);

        yield return new WaitForSeconds(4f);

        comboNotification.text = "";
        enemyStatus.text = "";
        playerStatus.text = "";

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

        timerScript.startTimer();
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
            timerScript.stopTimer = true;
            Debug.Log("Button Worked!");
            state = BattleState.ENEMYTURN;
            StartCoroutine(PlayerAttack());
        }
    }

    private void ResetDice()
    {
        dieSlot1Script.slottedDie.GetComponent<DragDrop>().ResetPosition();
        dieSlot2Script.slottedDie.GetComponent<DragDrop>().ResetPosition();
        dieSlot3Script.slottedDie.GetComponent<DragDrop>().ResetPosition();

        dieSlot1Script.slottedDie = null;
        dieSlot2Script.slottedDie = null;
        dieSlot3Script.slottedDie = null;

        dieSlot1Script.isEmpty = true;
        dieSlot2Script.isEmpty = true;
        dieSlot3Script.isEmpty = true;
    }

    //enemy does flat damage, changes player HP, checks if game is over
    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(2f);

        int dwagonDamage = Random.Range(1, 7);

        if (doubleDwagonDamage)
        {
            comboNotification.text = "Ouch! Double Damage!";
            dwagonDamage *= 2;
            doubleDwagonDamage = false;
            yield return new WaitForSeconds(2f);
            comboNotification.text = "";
        }

        bool isDead = playerUnitInfo.TakeDamage(dwagonDamage);

        playerHUD.SetHP(playerUnitInfo.currentHP);
    
        int tempElementNum = 0;

        if (Random.Range(0, 2) == 0)
        {
            tempElementNum = Random.Range(1, 6);
        }
        else
        {
            tempElementNum = 0;
        }

        AnimationManager.EnemyAttackAnimation(tempElementNum, elementNum);

        elementNum = tempElementNum;

        playerStatus.text = "-" + dwagonDamage + " Health";

        yield return new WaitForSeconds(1f);
        playerStatus.text = "";

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
        if (state == BattleState.LOST)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }

    }

}
