using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : SingletonBehaviour<InGameManager>
{
    public InGameUIController InGameUIController { get; private set; }

    private int m_SelectedChapter;
    private int m_CurrStage;
    private const string STAGE_PATH = "Stages/";
    private Transform m_StageTrs;
    private SpriteRenderer m_Bg;
    public bool IsPaused { get; private set; }

    protected override void Init()
    {
        m_IsDestroyOnLoad = true;

        base.Init();

        InitVariables();
        LoadStage();

        UIManager.Instance.Fade(Color.black, 1f, 0f, 0.5f, 0f, true);
    }

    private void InitVariables()
    {
        Logger.Log($"{GetType()}::InitVariables");

        //m_StageTrs = GameObject.Find("Stage").transform;
        //m_Bg = GameObject.Find("Bg").GetComponent<SpriteRenderer>();
        //m_CurrStage = 1;

        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if (userPlayData == null)
        {
            Logger.LogError("UserPlayData does not exist.");
            return;
        }

        m_SelectedChapter = userPlayData.SelectedChapter;
    }

    private void LoadStage()
    {
        Logger.Log($"{GetType()}::LoadStage");
        Logger.Log($"Chapter:{m_SelectedChapter} Stage:{m_CurrStage}");

        // var bgTexture = Resources.Load($"ChapterBG/Background_{m_SelectedChapter.ToString("D3")}") as Texture2D;
        // if(bgTexture != null)
        // {
        //     m_Bg.sprite = Sprite.Create(bgTexture, new Rect(0, 0, bgTexture.width, bgTexture.height), new Vector2(0.5f, 0.5f));
        // }

        // var stageObj = Instantiate(Resources.Load($"{STAGE_PATH}C{m_SelectedChapter}/C{m_SelectedChapter}_S{m_CurrStage}", typeof(GameObject))) as GameObject;
        // stageObj.transform.SetParent(m_StageTrs);
        // stageObj.transform.localScale = Vector3.one;
        // stageObj.transform.localPosition = Vector3.zero;
    }

    private void Start()
    {
        InGameUIController = FindObjectOfType<InGameUIController>();
        if (!InGameUIController)
        {
            Logger.LogError("InGameUIController does not exist.");
            return;
        }

        InGameUIController.Init();
    }

    // ���� �Ͻ����� �޼���
    public void PauseGame()
    {
        // �Ͻ����� ���¸� true�� ����
        IsPaused = true;

        // ���� �Ŵ����� �Ͻ����� �÷��� ����
        //GameManager.Instance.Paused = true;
        // ���� �Ŵ����� ĳ���� �Ͻ����� ���
        //LevelManager.Instance.ToggleCharacterPause();
    }

    // ���� �簳 �޼���
    public void ResumeGame()
    {
        // �Ͻ����� ���¸� false�� ����
        IsPaused = false;

        // ���� �Ŵ����� �Ͻ����� �÷��� ����
        //GameManager.Instance.Paused = false;
        // ���� �Ŵ����� ĳ���� �Ͻ����� ���
        //LevelManager.Instance.ToggleCharacterPause();
    }
}