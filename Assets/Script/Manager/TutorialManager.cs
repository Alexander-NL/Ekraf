using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("References")]
    private GameObject heroObj;
    private Transform hero;
    [SerializeField] Transform point1;
    [SerializeField] RectTransform boxPlacer;
    [SerializeField] RectTransform hiddenPos; 
    [SerializeField] RectTransform tutorialBox;   

    [Header("Tutorial Items")]
    [SerializeField] GameObject slapInstruction;
    [SerializeField] GameObject parryInstruction;

    [Header("Settings")]
    public float moveSpeed = 3f;

    private PlayerAttackHero playerAttack;
    private HeroRespawn heroRespawn;
    private bool triggered = false;
    private Vector2 targetPosition;

    private void Start()
    {
        triggered = false;
        heroObj = GameObject.FindGameObjectWithTag("Hero");
        hero = heroObj.transform;
        heroRespawn = heroObj.GetComponent<HeroRespawn>();

        GameObject playerObj = GameObject.Find("Player");
        playerAttack = playerObj.GetComponent<PlayerAttackHero>();

        slapInstruction.SetActive(false);
        parryInstruction.SetActive(false);
        targetPosition = hiddenPos.anchoredPosition;
    }

    private void Update()
    {
        hero = heroObj.transform;
        //Debug.Log("Hero X Position: " + (hero.position.x - point1.position.x));

        tutorialBox.anchoredPosition = Vector2.Lerp(
            tutorialBox.anchoredPosition,
            targetPosition,
            Time.deltaTime * moveSpeed
        );

        if (!triggered && Mathf.Abs(hero.position.x - point1.position.x) < 0.2f)
        {
            Debug.Log("Show Parry Tutorial");
            ShowTutorial(parryInstruction);
        }

        if (triggered && (hero.position.x - point1.position.x) == -35f)
        {
            HideTutorial();
        }

        if (triggered && (hero.position.x - point1.position.x) == 50f)
        {
            HideTutorial();
        }

        if(heroRespawn.DeadCounter == 1 && !triggered)
        {
            Debug.Log("Death: " + SaveSystem.Instance.GetDeathTotal());
            playerAttack.TurnOnPlayerHelp();
            ShowTutorial(slapInstruction);

        }
    }

    public void HideTutorial()
    {
        //tutorialBox.anchoredPosition = Vector3.Lerp(
        //    tutorialBox.anchoredPosition,
        //    hiddenPos.anchoredPosition,
        //    Time.deltaTime * moveSpeed
        //);
        targetPosition = hiddenPos.anchoredPosition;
        triggered = false;
    }

    public void ShowTutorial(GameObject tutorialObj)
    {
        //tutorialBox.anchoredPosition = Vector3.Lerp(
        //    tutorialBox.anchoredPosition,
        //    boxPlacer.anchoredPosition,
        //    Time.deltaTime * moveSpeed
        //);
        targetPosition = boxPlacer.anchoredPosition;
        slapInstruction.SetActive(false);
        parryInstruction.SetActive(false);

        tutorialObj.SetActive(true);
        triggered = true;
    }
}
