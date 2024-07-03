using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ToNextPlanet : MonoBehaviour
{
    #region Fields

    [SerializeField] private Image planetLocked;
    [SerializeField] private GameObject ship;
    [SerializeField] private GameObject initialShipPos;
    [SerializeField] private int showNextPlanetEvent=2;
    [SerializeField] private int moveShipEvent=5;
    

    private Vector2 _endPosition;
    private float _timeToTarget;

    #endregion

    #region Constants

    private const float WAIT_ANIMATION_TIME = 1f;
    private const float TIME_TO_TARGET = 1f;

    private const int VISIBLE = 1;
    private const int INVISIBLE = 0;

    #endregion

    #region UnityMethods

    private void OnEnable()
    {
        DialogueManager.OnEndDialogue += EndDialogue;
        DialogueManager.OnStartDialogue += StartDialogue;
        DialogueManager.OnDialogueShow += DialogueShow;
    }

    private void OnDisable()
    {
        DialogueManager.OnEndDialogue -= EndDialogue;
        DialogueManager.OnStartDialogue -= StartDialogue;
        DialogueManager.OnDialogueShow -= DialogueShow;
    }

    #endregion

    #region Methods

    private void StartDialogue()
    {
        planetLocked.enabled = true;
        _endPosition = planetLocked.transform.position;
        ship.transform.position = initialShipPos.transform.position;
    }

    private void EndDialogue()
    {
        StartCoroutine(WaitForAnims());
    }

    private void DialogueShow(int dialogueN)
    {
        if (dialogueN == showNextPlanetEvent)
        {
            StartCoroutine(FadeImage(planetLocked));
        }
        if (dialogueN == moveShipEvent)
        {
            StartCoroutine(SpriteMoveTowards(ship.transform.position, _endPosition));
        }
    }


    private IEnumerator SpriteMoveTowards(Vector2 startPos, Vector2 targetPos)
    {
        var moveDurationTimer = 0.0f;

        while (moveDurationTimer < TIME_TO_TARGET)
        {
            moveDurationTimer += Time.deltaTime;
            // Lerp using initial value!
            ship.transform.position = Vector2.Lerp(startPos, targetPos, moveDurationTimer / TIME_TO_TARGET);
            yield return null;
        }
    }

    private IEnumerator FadeImage(Image img)
    {
        for (float i = VISIBLE; i >= INVISIBLE; i -= Time.deltaTime)
        {
            var tempColor = img.color;
            tempColor.a = i;
            img.color = tempColor;
            yield return null;
        }

        img.enabled = false;
    }

    private static IEnumerator WaitForAnims()
    {
        yield return new WaitForSeconds(WAIT_ANIMATION_TIME);
        SceneChanger.Instance.LoadGameManagerNextScene();
    }

    #endregion
}