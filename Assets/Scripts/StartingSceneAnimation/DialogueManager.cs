using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private String[] names;
    [SerializeField] private Sprite[] textSprite;
    [SerializeField] private Image dialogueName;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private Animator animator;
    [SerializeField] private Animator shipAnimator;
    [SerializeField] private Animator shakingShip;
    [SerializeField] private Animator enemyCharge;

    [SerializeField] private GameObject backgroundImage;
    [SerializeField] private GameObject speechBubble1;
    [SerializeField] private GameObject speechBubble2;
    [SerializeField] private GameObject speechText2;
    [SerializeField] private GameObject speechText1;


    private Queue<string> sentences;
    private int diagMatch;

    private bool nameArray = true;
    private bool endedDialogue = false;

    public string firstScene;

    public float typingSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        diagMatch = 0;
        nameText.text = names[diagMatch];
    }

    void Update(){
        
        if(diagMatch == 1){
            dialogueName.sprite = textSprite[1];
        } else{
            dialogueName.sprite = textSprite[0];
        }

        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "Sweetsylvania")
        {
            Time.timeScale = 0;

            if (endedDialogue == true)
            {
                Time.timeScale = 1;
            }
        }
    }

    public void StartDialogue (Dialogue dialogue)
    {
        animator.SetBool("isOpen", true);
        
        sentences.Clear();

        foreach (string sentence in dialogue.sentences) 
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count < 1)
        {
            nameArray = false;
            Debug.Log("End of conversation");
            EndDialogue();
            return;
        }
        if (nameArray) { diagMatch = diagMatch + 1; }
        nameText.text = names[diagMatch];

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        Debug.Log("Count is: " + sentences.Count);
    }

    IEnumerator TypeSentence (string sentence)
    {
        // type sentence in letter by letter
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void EndDialogue()
    {
        Debug.Log("Ending the dialogue");
        speechBubble1.SetActive(false);
        speechBubble2.SetActive(false);
        speechText1.SetActive(false);
        speechText2.SetActive(false);

        endedDialogue = true;

        //remove the dialogue screen
        animator.SetBool("isOpen", false);
        
        // stop the ships animation in previous scene
        shipAnimator.SetBool("StopShip", true);

        shakingShip.SetBool("isBeingBoarded", true);

        enemyCharge.SetBool("isCharging", true);

        // stop the background image
        backgroundImage.GetComponent<ImageScroller>().enabled = false;

        StartCoroutine(LoadNextScene());

        Debug.Log(SceneManager.GetActiveScene());

        // load the next scene
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator LoadNextScene()
    {
        if (endedDialogue == true)
        {
            if (firstScene == "Scene")
            {
                yield return new WaitForSecondsRealtime(1);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

            if (firstScene == "EndScene")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

            if (firstScene != "SecondScene")
            {
                yield return new WaitForSecondsRealtime(24);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        if (firstScene == "CreditScene")
        {
            yield return new WaitForSecondsRealtime(24);
            SceneManager.LoadScene("MainMenu");

        }
    }

}
