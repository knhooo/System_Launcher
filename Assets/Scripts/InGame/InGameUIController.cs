// �ڷ�ƾ �� �÷��� ���� ���ӽ����̽� ���
using System.Collections;
// ���׸� �÷��� ���� ���ӽ����̽� ���
using System.Collections.Generic;
// Unity ���� ���� ���ӽ����̽� ���
using UnityEngine;

// MonoBehaviour�� ��ӹ޴� �ΰ��� UI ��Ʈ�ѷ� Ŭ���� ����
public class InGameUIController : MonoBehaviour
{
    // �ʱ�ȭ �޼��� (���� �������)
    public void Init()
    {

    }

    // ���ø����̼� ��Ŀ�� ���� �� ȣ��Ǵ� Unity �̺�Ʈ �޼���
    private void OnApplicationFocus(bool focus)
    {
        // ���� ��Ŀ���� ���� ��� (��׶���� �̵�)
        if (!focus)
        {
            // ���� ������ �Ͻ����� ���°� �ƴ� ���
            if (!InGameManager.Instance.IsPaused)
            {
                // �⺻ UI ������ ����
                var uiData = new BaseUIData();
                // �Ͻ����� UI ����
                UIManager.Instance.OpenUI<PauseUI>(uiData);

                // ���� �Ͻ����� ����
                InGameManager.Instance.PauseGame();
            }
        }
    }
    // Unity�� Update ����������Ŭ �޼���
    private void Update()
    {
        // ������ �Ͻ����� ���°� �ƴ� ���
        if (!InGameManager.Instance.IsPaused)
        {
            // �Է� ó�� �޼��� ȣ��
            HandleInput();
        }
    }

    // �Է� ó���� ����ϴ� �޼���
    private void HandleInput()
    {
        // ESC Ű�� �� ���
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            // UI ��ư Ŭ�� ���� ���
            AudioManager.Instance.PlaySFX(SFX.ui_button_click);

            // �⺻ UI ������ ����
            var uiData = new BaseUIData();
            // �Ͻ����� UI ����
            UIManager.Instance.OpenUI<PauseUI>(uiData);

            // ���� �Ͻ����� ����
            InGameManager.Instance.PauseGame();
        }
    }

    // �Ͻ����� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnClickPauseBtn()
    {
        // UI ��ư Ŭ�� ���� ���
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);

        // �⺻ UI ������ ����
        var uiData = new BaseUIData();
        // �Ͻ����� UI ����
        UIManager.Instance.OpenUI<PauseUI>(uiData);

        // ���� �Ͻ����� ����
        InGameManager.Instance.PauseGame();
    }
}