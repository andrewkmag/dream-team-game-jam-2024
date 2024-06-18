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
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private Animator animator;
    [SerializeField] private Animator shipAnimator;
    [SerializeField] private Animator shakingShip;

    [SerializeField] private GameObject backgroundImage;
    [SerializeField] private GameObject speechBubble1;
    [SerializeField] private GameObject speechText1;

    private Queue<string> sentences;

    public float typingSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue (Dialogue dialogue)
    {
        animator.SetBool("isOpen", true);

        nameText.text = dialogue.name;
        
        sentences.Clear();

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
            Debug.Log("End of conversation");
            EndDialogue();
            return;
        }

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
        speechText1.SetActive(false);

        //remove the dialogue screen
        animator.SetBool("isOpen", false);
        
        // stop the ships animation in previous scene
        shipAnimator.SetBool("StopShip", true);

        // stop the background image
        backgroundImage.GetComponent<ImageScroller>().enabled = false;

        // load the next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
