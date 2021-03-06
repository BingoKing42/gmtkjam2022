using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;

    public GameObject startButton;
    public GameObject dialogueBox;

    private Queue<string> sentences;

    public bool gameEnd;
    private bool moveOn;

    // Start is called before the first frame update
    void Start()
    {
        moveOn = false;
        sentences = new Queue<string>();
    }

    public void StartDialogue (Dialogue dialogue)
    {
        sentences.Clear();
        moveOn = true;

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void killButton()
    {
        startButton.transform.position = startButton.transform.position + new Vector3(0, 10, 0);
    }

    public void summonBox()
    {
        dialogueBox.transform.position = dialogueBox.transform.position + new Vector3(0, 9, 0);
    }

    private void EndDialogue()
    {
        if (gameEnd && moveOn)
        {
            Application.Quit();
        } else if (moveOn)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            FindObjectOfType<AudioManager>().Play("Pencil");
        }
    }
}