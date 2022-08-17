using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;

public class BranchingDialogController : MonoBehaviour
{
    [SerializeField] private GameObject branchingCanvas;
    [SerializeField] private GameObject dialogPrefab;
    [SerializeField] private GameObject choicePrefab;
    [SerializeField] private TextAssetValue dialogValue;
    [SerializeField] private Story myStory;

    [SerializeField] private GameObject dialog;
    [SerializeField] private GameObject dialogHolder;
    [SerializeField] private GameObject choiceHolder;
    [SerializeField] private ScrollRect dialogScroll;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableCanvas()
    {
        branchingCanvas.SetActive(true);
        SetStory();
        RefreshView();
    }
    public void CloseDialog()
    {
        //StartCoroutine(GetItem(DBManager.id));
        dialog.SetActive(false);
    }

    public void SetStory()
    {
        if (dialogValue.value)
        {
            myStory = new Story(dialogValue.value.text);
        }
        else
        {
            Debug.Log("Somehing went wrong");
        }
    }

    public void RefreshView()
    {
        while (myStory.canContinue)
        {
            MakeNewDialog(myStory.Continue());
            //Destroy the speech of NPC when turn to the next sentences
            for (int i = 1; i < dialogHolder.transform.childCount; i++)
            {
                Destroy(dialogHolder.transform.GetChild(i-1).gameObject);
            }
        }
        if(myStory.currentChoices.Count > 0)
        {
            MakeNewChoices();
        }
        else
        {
            branchingCanvas.SetActive(false);
        }
        //dialogScroll.verticalNormalizedPosition = 0f;
        StartCoroutine(ScrollCo());
    }
    IEnumerator ScrollCo()
    {
        yield return null;
        dialogScroll.verticalNormalizedPosition = 0f;
    }
    void MakeNewDialog(string newDialog)
    {
        DialogObject newDialogObject = Instantiate(dialogPrefab, dialogHolder.transform).GetComponent<DialogObject>();
        newDialogObject.SetUp(newDialog);
    }

    void MakeNewResponse(string newDialog, int choiceValue)
    {
        ResponseObject newResponseObject = Instantiate(choicePrefab, choiceHolder.transform).GetComponent<ResponseObject>();
        newResponseObject.SetUp(newDialog, choiceValue);
        Button responseButton = newResponseObject.gameObject.GetComponent<Button>();
        if (responseButton)
        {
            responseButton.onClick.AddListener(delegate { ChooseChoice(choiceValue); });
        }
    }
    void MakeNewChoices()
    {
        for (int i=0; i< choiceHolder.transform.childCount; i++)
        {
            Destroy(choiceHolder.transform.GetChild(i).gameObject);
        }
        for (int i=0; i< myStory.currentChoices.Count; i++)
        {
            MakeNewResponse(myStory.currentChoices[i].text, i);

            //Destroy(dialogHolder.transform.GetChild(i).gameObject);
        }
    }

    void ChooseChoice(int choice)
    {
        myStory.ChooseChoiceIndex(choice);
        RefreshView();
    }
}
