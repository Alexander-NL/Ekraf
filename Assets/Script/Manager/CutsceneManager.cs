using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CutsceneManager : MonoBehaviour
{
    [Header("")]
    public GameObject[] CutsceneList;
    public int temp;

    [Header("Input Action Reference")]
    public InputActionReference NextAction;

    private void OnEnable()
    {
        NextAction.action.Enable();
        NextAction.action.performed += NextPart;
    }

    private void OnDisable()
    {
        NextAction.action.performed -= NextPart;
        NextAction.action.Disable();
    }

    private void Start()
    {
        StartCutscene();
    }

    public void StartCutscene()
    {
        foreach(var cutscene in CutsceneList)
        {
            cutscene.gameObject.SetActive(false);
        }

        CutsceneList[0].SetActive(true);
    }

    private void NextPart(InputAction.CallbackContext context)
    {
        temp++;
        UpdateCutscene(temp);
    }

    void UpdateCutscene(int temp)
    {
        if (temp >= CutsceneList.Length)
        {
            //End
            CutsceneList[temp - 1].SetActive(false);
            this.gameObject.SetActive(false);
        }
        else
        {
            CutsceneList[temp].SetActive(true);
            CutsceneList[temp - 1].SetActive(false);
        }
    }
}
