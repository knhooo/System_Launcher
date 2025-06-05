using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ����� �����͸� �����ϴ� �̱��� �Ŵ��� Ŭ����
public class UserDataManager : SingletonBehaviour<UserDataManager>
{
    // ����� �����Ͱ� �����ϴ��� Ȯ���ϴ� ������Ƽ
    public bool ExistsSavedData { get; private set; }
    // ��� ����� ������ �ν��Ͻ��� �����ϴ� ����Ʈ
    public List<IUserData> UserDataList { get; private set; } = new List<IUserData>();

    // �ʱ�ȭ �޼��� �������̵�
    protected override void Init()
    {
        // �θ� Ŭ������ �ʱ�ȭ �޼��� ȣ��
        base.Init();

        // ����� ���� �����͸� ����Ʈ�� �߰�
        UserDataList.Add(new UserSettingsData());
        // ����� ��ȭ �����͸� ����Ʈ�� �߰�
        UserDataList.Add(new UserGoodsData());
        // ����� �κ��丮 �����͸� ����Ʈ�� �߰�
        UserDataList.Add(new UserInventoryData());
        // ����� �÷��� �����͸� ����Ʈ�� �߰�
        UserDataList.Add(new UserPlayData());
    }

    // ��� ����� �����͸� �⺻������ �����ϴ� �޼���
    public void SetDefaultUserData()
    {
        // ����� ������ ����Ʈ ������ŭ �ݺ�
        for (int i = 0; i < UserDataList.Count; i++)
        {
            // �� ����� �������� �⺻�� ����
            UserDataList[i].SetDefaultData();
        }
    }

    // ����� ����� �����͸� �ҷ����� �޼���
    public void LoadUserData()
    {
        // PlayerPrefs���� ����� ������ ���� ���� Ȯ��
        ExistsSavedData = PlayerPrefs.GetInt("ExistsSavedData") == 1 ? true : false;

        // ����� �����Ͱ� �����ϴ� ���
        if (ExistsSavedData)
        {
            // ����� ������ ����Ʈ ������ŭ �ݺ�
            for (int i = 0; i < UserDataList.Count; i++)
            {
                // �� ����� ������ �ҷ�����
                UserDataList[i].LoadData();
            }
        }
    }

    // ����� �����͸� �����ϴ� �޼���
    public void SaveUserData()
    {
        // ���� ���� �߻� ���θ� Ȯ���ϴ� ����
        bool hasSaveError = false;

        // ����� ������ ����Ʈ ������ŭ �ݺ�
        for (int i = 0; i < UserDataList.Count; i++)
        {
            // �� ����� ������ ���� �� ���� ���� Ȯ��
            bool isSaveSuccess = UserDataList[i].SaveData();
            // ���忡 ������ ���
            if (!isSaveSuccess)
            {
                // ���� �÷��� ����
                hasSaveError = true;
            }
        }

        // ���� ������ ���� ���
        if (!hasSaveError)
        {
            // ����� ������ ���� �÷��� ����
            ExistsSavedData = true;
            // PlayerPrefs�� ����� ������ ���� ���� ����
            PlayerPrefs.SetInt("ExistsSavedData", 1);
            // PlayerPrefs ���� ����
            PlayerPrefs.Save();
        }
    }

    // ���׸��� ����Ͽ� Ư�� Ÿ���� ����� �����͸� �������� �޼���
    public T GetUserData<T>() where T : class, IUserData
    {
        // LINQ�� ����Ͽ� �ش� Ÿ���� ù ��° ������ ��ȯ
        return UserDataList.OfType<T>().FirstOrDefault();
    }
}