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
    private List<string> m_DialogueTreeNames;

    private void Start()
    {
        m_DialogueTrees = gameObject.GetComponentsInChildren<DialogueTreeController>();

        m_DialogueTreeNames = new List<string>();
        foreach (DialogueTreeController dialogueTree in m_DialogueTrees)
        {
            m_DialogueTreeNames.Add(dialogueTree.graph.graphComments);
        }

        StartDialogueTree(m_CurrentDialogue);
    }

    public void StartDialogueTree(int id)
    {
        if (id < 0 || id >= m_DialogueTrees.Length)
            return;

        if (m_CurrentDialogue >= 0)
            m_DialogueTrees[m_CurrentDialogue].StopDialogue();

        m_CurrentDialogue = id;
        m_DialogueTrees[id].StartDialogue(OnDialogueEnd);
    }

    //Accessors
    public int GetCurrentDialogueTree()
    {
        return m_CurrentDialogue;
    }

    public List<string> GetSceneNames()
    {
        return m_DialogueTreeNames;
    }

    //Callback
    private void OnDialogueEnd(bool success)
    {
        if (success)
            StartDialogueTree(m_CurrentDialogue + 1);
    }
}
