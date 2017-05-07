using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.DialogueTrees;

//Based upon the example "DialogueUGUI"
//Will rewrite this at some point, as I do not agree with some of this code.
public class DialoguePanel : MonoBehaviour
{
    [System.Serializable]
    public class SubtitleDelays
    {
        [SerializeField]
        private float m_CharacterDelay = 0.05f;
        public float CharacterDelay
        {
            get { return m_CharacterDelay; }
        }

        [SerializeField]
        private float m_SentenceDelay = 0.5f;
        public float SentenceDelay
        {
            get { return m_SentenceDelay; }
        }

        [SerializeField]
        private float m_CommaDelay = 0.1f;
        public float CommaDelay
        {
            get { return m_CommaDelay; }
        }

        [SerializeField]
        private float m_FinalDelay = 1.2f;
        public float FinalDelay
        {
            get { return m_FinalDelay; }
        }
    }

    //Options
    [Header("Input Options")]
    [SerializeField]
    private bool m_SkipOnInput;

    [SerializeField]
    private bool m_WaitForInput;

    //Group
    [Header("Subtitles")]
    [SerializeField]
    private RectTransform m_SubtitlesGroup;

    [SerializeField]
    private Text m_ActorSpeech;

    [SerializeField]
    private Text m_ActorName;

    [SerializeField]
    private Image m_ActorPortrait;

    [SerializeField]
    private RectTransform m_WaitInputIndicator;

    [SerializeField]
    private SubtitleDelays m_SubtitleDelays = new SubtitleDelays();

    //Group
    [Header("Multiple Choice")]
    [SerializeField]
    private RectTransform m_OptionsGroup;

    [SerializeField]
    private Button m_OptionButton;

    private Dictionary<Button, int> m_CachedButtons;
    private Vector2 m_OriginalSubsPosition;
    private bool m_IsWaitingChoice;

    private AudioSource m_LocalSource;
    private AudioSource LocalSource
    {
        get { return m_LocalSource != null ? m_LocalSource : m_LocalSource = gameObject.AddComponent<AudioSource>(); }
    }

    //Functions
    private void OnEnable()
    {
        DialogueTree.OnDialogueStarted += OnDialogueStarted;
        DialogueTree.OnDialoguePaused += OnDialoguePaused;
        DialogueTree.OnDialogueFinished += OnDialogueFinished;
        DialogueTree.OnSubtitlesRequest += OnSubtitlesRequest;
        DialogueTree.OnMultipleChoiceRequest += OnMultipleChoiceRequest;
    }

    private void OnDisable()
    {
        DialogueTree.OnDialogueStarted -= OnDialogueStarted;
        DialogueTree.OnDialoguePaused -= OnDialoguePaused;
        DialogueTree.OnDialogueFinished -= OnDialogueFinished;
        DialogueTree.OnSubtitlesRequest -= OnSubtitlesRequest;
        DialogueTree.OnMultipleChoiceRequest -= OnMultipleChoiceRequest;
    }

    private void Start()
    {
        m_SubtitlesGroup.gameObject.SetActive(false);
        m_OptionsGroup.gameObject.SetActive(false);
        m_OptionButton.gameObject.SetActive(false);
        m_WaitInputIndicator.gameObject.SetActive(false);
        m_OriginalSubsPosition = m_SubtitlesGroup.transform.position;
    }

    private void OnDialogueStarted(DialogueTree dlg)
    {
        //nothing special...
    }

    private void OnDialoguePaused(DialogueTree dlg)
    {
        m_SubtitlesGroup.gameObject.SetActive(false);
        m_OptionsGroup.gameObject.SetActive(false);
    }

    private void OnDialogueFinished(DialogueTree dlg)
    {
        m_SubtitlesGroup.gameObject.SetActive(false);
        m_OptionsGroup.gameObject.SetActive(false);

        if (m_CachedButtons != null)
        {
            foreach (Button tempBtn in m_CachedButtons.Keys)
            {
                if (tempBtn != null)
                {
                    Destroy(tempBtn.gameObject);
                }
            }
            m_CachedButtons = null;
        }
    }

    private void OnSubtitlesRequest(SubtitlesRequestInfo info)
    {
        StartCoroutine(Internal_OnSubtitlesRequestInfo(info));
    }


    private IEnumerator Internal_OnSubtitlesRequestInfo(SubtitlesRequestInfo info)
    {
        string text = info.statement.text;
        AudioClip audio = info.statement.audio;
        IDialogueActor actor = info.actor;

        m_SubtitlesGroup.gameObject.SetActive(true);
        m_ActorSpeech.text = "";

        m_ActorName.text = actor.name;
        m_ActorSpeech.color = actor.dialogueColor;

        m_ActorPortrait.gameObject.SetActive(actor.portraitSprite != null);
        m_ActorPortrait.sprite = actor.portraitSprite;

        if (audio != null)
        {
            AudioSource actorSource = actor.transform != null ? actor.transform.GetComponent<AudioSource>() : null;
            AudioSource playSource = actorSource != null ? actorSource : LocalSource;
            playSource.clip = audio;
            playSource.Play();
            m_ActorSpeech.text = text;
            float timer = 0f;
            while (timer < audio.length)
            {
                if (m_SkipOnInput && Input.anyKeyDown)
                {
                    playSource.Stop();
                    break;
                }
                timer += Time.deltaTime;
                yield return null;
            }
        }

        if (audio == null)
        {
            string tempText = "";
            bool inputDown = false;
            if (m_SkipOnInput)
            {
                StartCoroutine(CheckInput(() => { inputDown = true; }));
            }

            for (int i = 0; i < text.Length; i++)
            {

                if (m_SkipOnInput && inputDown)
                {
                    m_ActorSpeech.text = text;
                    yield return null;
                    break;
                }

                if (m_SubtitlesGroup.gameObject.activeSelf == false)
                {
                    yield break;
                }

                tempText += text[i];
                if (m_SubtitleDelays.CharacterDelay > 0.0f)
                    yield return StartCoroutine(DelayPrint(m_SubtitleDelays.CharacterDelay));
                char c = text[i];
                if (c == '.' || c == '!' || c == '?')
                {
                    if (m_SubtitleDelays.SentenceDelay > 0.0f)
                        yield return StartCoroutine(DelayPrint(m_SubtitleDelays.SentenceDelay));
                }
                if (c == ',')
                {
                    if (m_SubtitleDelays.CommaDelay > 0.0f)
                        yield return StartCoroutine(DelayPrint(m_SubtitleDelays.CommaDelay));
                }

                m_ActorSpeech.text = tempText;
            }

            if (!m_WaitForInput)
            {
                if (m_SubtitleDelays.FinalDelay > 0.0f)
                    yield return StartCoroutine(DelayPrint(m_SubtitleDelays.FinalDelay));
            }
        }

        if (m_WaitForInput)
        {
            m_WaitInputIndicator.gameObject.SetActive(true);
            while (!Input.anyKeyDown)
            {
                yield return null;
            }
            m_WaitInputIndicator.gameObject.SetActive(false);
        }

        yield return null;
        m_SubtitlesGroup.gameObject.SetActive(false);
        info.Continue();
    }

    private IEnumerator CheckInput(System.Action action)
    {
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        action();
    }

    private IEnumerator DelayPrint(float time)
    {
        float timer = 0f;
        while (timer < time)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }




    private void OnMultipleChoiceRequest(MultipleChoiceRequestInfo info)
    {
        m_OptionsGroup.gameObject.SetActive(true);
        float buttonHeight = m_OptionButton.GetComponent<RectTransform>().rect.height + 10;
        m_OptionsGroup.sizeDelta = new Vector2(m_OptionsGroup.sizeDelta.x, (info.options.Values.Count * buttonHeight) + 10);

        m_CachedButtons = new Dictionary<Button, int>();
        int i = 0;

        foreach (KeyValuePair<IStatement, int> pair in info.options)
        {
            Button btn = (Button)Instantiate(m_OptionButton);
            btn.gameObject.SetActive(true);
            btn.transform.SetParent(m_OptionsGroup.transform, false);
            btn.transform.localPosition = (Vector2)m_OptionButton.transform.localPosition - new Vector2(0, buttonHeight * i);
            btn.GetComponentInChildren<Text>().text = pair.Key.text;
            m_CachedButtons.Add(btn, pair.Value);
            btn.onClick.AddListener(() => { Finalize(info, m_CachedButtons[btn]); });
            i++;
        }

        if (info.showLastStatement)
        {
            m_SubtitlesGroup.gameObject.SetActive(true);
            //STIJN: No Need so far
            //float newY = m_OptionsGroup.position.y + m_OptionsGroup.sizeDelta.y + 1;
            //m_SubtitlesGroup.position = new Vector2(m_SubtitlesGroup.position.x, newY);
        }

        if (info.availableTime > 0)
        {
            StartCoroutine(CountDown(info));
        }
    }

    private IEnumerator CountDown(MultipleChoiceRequestInfo info)
    {
        m_IsWaitingChoice = true;
        float timer = 0f;
        while (timer < info.availableTime)
        {
            if (m_IsWaitingChoice == false)
            {
                yield break;
            }
            timer += Time.deltaTime;
            SetMassAlpha(m_OptionsGroup, Mathf.Lerp(1, 0, timer / info.availableTime));
            yield return null;
        }

        if (m_IsWaitingChoice)
        {
            Finalize(info, info.options.Values.Last());
        }
    }

    private void Finalize(MultipleChoiceRequestInfo info, int index)
    {
        m_IsWaitingChoice = false;
        SetMassAlpha(m_OptionsGroup, 1f);
        m_OptionsGroup.gameObject.SetActive(false);
        if (info.showLastStatement)
        {
            m_SubtitlesGroup.gameObject.SetActive(false);
            m_SubtitlesGroup.transform.position = m_OriginalSubsPosition;
        }
        foreach (Button tempBtn in m_CachedButtons.Keys)
        {
            Destroy(tempBtn.gameObject);
        }
        info.SelectOption(index);
    }

    private void SetMassAlpha(RectTransform root, float alpha)
    {
        foreach (CanvasRenderer graphic in root.GetComponentsInChildren<CanvasRenderer>())
        {
            graphic.SetAlpha(alpha);
        }
    }
}