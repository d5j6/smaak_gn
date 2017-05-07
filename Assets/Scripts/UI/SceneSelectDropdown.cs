using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class SceneSelectDropdown : MonoBehaviour
{
    private Dropdown m_Dropdown;

    [SerializeField]
    private DialogueTreeChain m_DialogueTreeChain;

    private void Awake()
    {
        m_Dropdown = GetComponent<Dropdown>();
    }

    private void Start()
    {
        List<string> sceneNames = m_DialogueTreeChain.GetSceneNames();

        foreach(string sceneName in sceneNames)
        {
            Dropdown.OptionData data = new Dropdown.OptionData(sceneName);
            m_Dropdown.options.Add(data);
        }

        UpdateValue();
    }

    private void OnEnable()
    {
        UpdateValue();
    }

    private void UpdateValue()
    {
        m_Dropdown.value = m_DialogueTreeChain.GetCurrentDialogueTree();
        m_Dropdown.RefreshShownValue();
    }

    public void OnValueChanged(int index)
    {
        m_DialogueTreeChain.StartDialogueTree(index);
    }
}
