// �ʿ��� ���ӽ����̽� �ҷ�����
using System.Collections;
// ���׸� �÷��� ���ӽ����̽� �ҷ�����
using System.Collections.Generic;
// Unity ���� ���ӽ����̽� �ҷ�����
using UnityEngine;

// ����� �÷��� �����͸� �����ϴ� Ŭ����, IUserData �������̽� ����
public class UserPlayData : IUserData
{
    // �ִ� Ŭ������ é�� ��ȣ ������Ƽ
    public int MaxClearedChapter { get; set; }
    // PlayerPrefs�� ������� �ʴ� ���� ���õ� é�� ��ȣ (�⺻�� 1)
    //not saved to playerprefs
    public int SelectedChapter { get; set; } = 1;

    // �⺻ ������ ���� �޼���
    public void SetDefaultData()
    {
        // �޼��� ȣ�� �α� ���
        Logger.Log($"{GetType()}::SetDefaultData");

        // �ִ� Ŭ���� é�͸� 0���� �ʱ�ȭ
        MaxClearedChapter = 0;
        // ���õ� é�͸� 1�� �ʱ�ȭ
        SelectedChapter = 1;
    }

    // ����� �����͸� �ҷ����� �޼���
    public bool LoadData()
    {
        // �޼��� ȣ�� �α� ���
        Logger.Log($"{GetType()}::LoadData");

        // �ε� ����� ������ ����
        bool result = false;

        // ���� ó���� ���� try-catch ��� ����
        try
        {
            // PlayerPrefs���� �ִ� Ŭ���� é�� �� �ҷ�����
            MaxClearedChapter = PlayerPrefs.GetInt("MaxClearedChapter");
            // ���õ� é�͸� �ִ� Ŭ���� é�� + 1�� ����
            SelectedChapter = MaxClearedChapter + 1;

            // �ε� �������� ����
            result = true;

            // �ҷ��� �ִ� Ŭ���� é�� �α� ���
            Logger.Log($"MaxClearedChpater:{MaxClearedChapter}");
        }
        // ���� �߻� �� ó��
        catch (System.Exception e)
        {
            // �ε� ���� �α� ���
            Logger.Log($"Load failed. (" + e.Message + ")");
        }

        // �ε� ��� ��ȯ
        return result;
    }

    // �����͸� �����ϴ� �޼���
    public bool SaveData()
    {
        // �޼��� ȣ�� �α� ���
        Logger.Log($"{GetType()}::SaveData");

        // ���� ����� ������ ����
        bool result = false;

        // ���� ó���� ���� try-catch ��� ����
        try
        {
            // PlayerPrefs�� �ִ� Ŭ���� é�� �� ����
            PlayerPrefs.SetInt("MaxClearedChapter", MaxClearedChapter);
            // PlayerPrefs ���� ����
            PlayerPrefs.Save();

            // ���� �������� ����
            result = true;

            // ����� �ִ� Ŭ���� é�� �α� ���
            Logger.Log($"MaxClearedChpater:{MaxClearedChapter}");
        }
        // ���� �߻� �� ó��
        catch (System.Exception e)
        {
            // ���� ���� �α� ���
            Logger.Log($"Save failed. (" + e.Message + ")");
        }

        // ���� ��� ��ȯ
        return result;
    }
}