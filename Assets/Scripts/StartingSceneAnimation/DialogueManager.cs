using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    [SerializeField] String[] names;
    public Sprite[] textSprite;
    public Image dialogueName;
    public TextMeshProUGUI dialogueText;

    public Animator animator;
    public Animator shipAnimator;
    public Animator shakingShip;

    public GameObject backgroundImage;
    public GameObject speechBubble1;
    public GameObject speechBubble2;   
    public GameObject speechText1;
    public GameObject speechText2;

    private Queue<string> sentences;
    private int diagMatch;

    private bool nameArray = true;

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
            EndDialogue();
            return;
        }
        if (nameArray) { diagMatch = diagMatch + 1; }
        nameText.text = names[diagMatch];
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
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
        speechBubble1.SetActive(false);
        speechBubble2.SetActive(false);
        speechText1.SetActive(false);
        speechText2.SetActive(false);

        


        //remove the dialogue screen
        animator.SetBool("isOpen", false);
        
        // stop the ships animation in previous scene
        shipAnimator.SetBool("StopShip", true);

        // stop the background image
        backgroundImage.GetComponent<ImageScroller>().enabled = false;

        shakingShip.SetBool("isBeingBoarded", true);
    }

}
