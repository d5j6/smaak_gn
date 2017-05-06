using NodeCanvas.DialogueTrees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTreeChain : MonoBehaviour
{
    [SerializeField]
    private int m_CurrentDialogue = 0;

    [SerializeField]
    private DialogueTreeController[] m_DialogueTrees;

    private void Start()
    {
        m_DialogueTrees = gameObject.GetComponentsInChildren<DialogueTreeController>();
        StartDialogueTree(m_CurrentDialogue);
    }

    private void StartDialogueTree(int id)
    {
        if (id < 0 || id >= m_DialogueTrees.Length)
            return;

        m_CurrentDialogue = id;
        m_DialogueTrees[id].StartDialogue(OnDialogueEnd);
    }

    //Callback
    private void OnDialogueEnd(bool success)
    {
        StartDialogueTree(m_CurrentDialogue + 1);
    }
}
