// GPM UI ���ӽ����̽� ����
namespace Gpm.Ui
{
    // �ý��� ���� ���̺귯�� ���
    using System;
    using System.Collections.Generic;
    using UnityEngine.Events;

    // InfiniteScroll Ŭ������ �κ� Ŭ���� ����
    public partial class InfiniteScroll
    {
        // ������ ���ؽ�Ʈ Ŭ���� - �� �������� �����Ϳ� ���¸� ����
        public class DataContext
        {
            // ������ ���ؽ�Ʈ ������ - �����Ϳ� �ε����� �޾� �ʱ�ȭ
            public DataContext(InfiniteScrollData data, int index)
            {
                // �ε��� ����
                this.index = index;
                // ������ ����
                this.data = data;
            }

            // �б� ���� ������ ������Ƽ
            public InfiniteScrollData data { get; private set; }

            // ������ ����Ʈ������ �ε��� (-1�� �ʱ�ȭ)
            internal int index = -1;

            // ������ ����Ʈ������ �ε��� (-1�� �ʱ�ȭ)
            internal int itemIndex = -1;

            // ��ũ�� ������ ��
            internal float offset = 0;

            // ������ ������ ������Ʈ �ʿ� ���� �÷���
            internal bool needUpdateItemData = true;

            // ��ũ�� �������� ũ��
            internal float scrollItemSize = 0;

            // ���� ������ ������Ʈ ����
            internal InfiniteScrollItem itemObject;

            // ������ ������ ������Ʈ�� �ʿ����� Ȯ���ϴ� �޼���
            public bool IsNeedUpdateItemData()
            {
                // ������Ʈ �ʿ� �÷��� ��ȯ
                return needUpdateItemData;
            }

            // �����۰��� ������ �����ϴ� �޼���
            public void UnlinkItem(bool notifyEvent = false)
            {
                // ������ ������Ʈ�� �����ϴ� ���
                if (itemObject != null)
                {
                    // ������ ������ Ŭ���� (�̺�Ʈ �˸� ���� ����)
                    itemObject.ClearData(notifyEvent);
                    // ������ ������Ʈ ���� ����
                    itemObject = null;
                }

                // ������ �ε����� -1�� ����
                itemIndex = -1;
            }

            // �����͸� ������Ʈ�ϴ� �޼���
            public void UpdateData(InfiniteScrollData data)
            {
                // ���ο� �����ͷ� ��ü
                this.data = data;
                // ������ ������Ʈ �ʿ� �÷��� ����
                needUpdateItemData = true;
            }

            // ������ ũ�⸦ ��ȯ�ϴ� �޼���
            public float GetItemSize()
            {
                // ��ũ�� ������ ũ�� ��ȯ
                return scrollItemSize;
            }

            // ������ ũ�⸦ �����ϴ� �޼���
            public void SetItemSize(float value)
            {
                // ��ũ�� ������ ũ�� ����
                scrollItemSize = value;
            }
        }

        // ������ ���ؽ�Ʈ ����Ʈ - ��� �����͸� ����
        protected List<DataContext> dataList = new List<DataContext>();
        // ���� ������ ����
        protected int itemCount = 0;

        // ������ ����Ʈ ������Ʈ �ʿ� ���� �÷���
        protected bool needUpdateItemList = true;

        // ���� ���õ� �������� �ε���
        protected int selectDataIndex = -1;
        // ���� �� ȣ��� �ݹ� �Լ�
        protected Action<InfiniteScrollData> selectCallback = null;

        // Ư�� �������� �ε����� ã�� �޼���
        public int GetDataIndex(InfiniteScrollData data)
        {
            // �ʱ�ȭ�� �ȵ� ��� �ʱ�ȭ ����
            if (isInitialize == false)
            {
                Initialize();
            }

            // ������ ����Ʈ���� �ش� �������� �ε��� ã�Ƽ� ��ȯ
            return dataList.FindIndex((context) =>
            {
                // �����Ͱ� ��ġ�ϴ��� Ȯ��
                return context.data.Equals(data);
            });
        }

        // ��ü ������ ������ ��ȯ�ϴ� �޼���
        public int GetDataCount()
        {
            // ������ ����Ʈ�� ���� ��ȯ
            return dataList.Count;
        }

        // Ư�� �ε����� �����͸� ��ȯ�ϴ� �޼���
        public InfiniteScrollData GetData(int index)
        {
            // �ش� �ε����� ������ ��ȯ
            return dataList[index].data;
        }

        // ��ü ������ ����Ʈ�� ��ȯ�ϴ� �޼���
        public List<InfiniteScrollData> GetDataList()
        {
            // ���ο� ������ ����Ʈ ����
            List<InfiniteScrollData> list = new List<InfiniteScrollData>();

            // ��� ������ ���ؽ�Ʈ�� ��ȸ
            for (int index = 0; index < dataList.Count; index++)
            {
                // �����͸� �����ؼ� ����Ʈ�� �߰�
                list.Add(dataList[index].data);
            }
            // �ϼ��� ����Ʈ ��ȯ
            return list;
        }

        // ���� ǥ�õǴ� �����۵��� ������ ����Ʈ�� ��ȯ�ϴ� �޼���
        public List<InfiniteScrollData> GetItemList()
        {
            // ���ο� ������ ����Ʈ ����
            List<InfiniteScrollData> list = new List<InfiniteScrollData>();

            // ��� ������ ���ؽ�Ʈ�� ��ȸ
            for (int index = 0; index < dataList.Count; index++)
            {
                // ������ �ε����� ��ȿ�� ��츸 (���� ǥ�õǴ� ������)
                if (dataList[index].itemIndex != -1)
                {
                    // �ش� �����͸� ����Ʈ�� �߰�
                    list.Add(dataList[index].data);
                }
            }
            // �ϼ��� ������ ����Ʈ ��ȯ
            return list;
        }

        // ���� ������ ������ ��ȯ�ϴ� �޼���
        public int GetItemCount()
        {
            // ������ ���� ��ȯ
            return itemCount;
        }

        // Ư�� �������� ������ �ε����� ��ȯ�ϴ� �޼���
        public int GetItemIndex(InfiniteScrollData data)
        {
            // �ش� �������� ���ؽ�Ʈ ã��
            var context = GetDataContext(data);
            // ������ �ε��� ��ȯ
            return context.itemIndex;
        }

        // ���� �ݹ� �Լ��� �߰��ϴ� �޼���
        public void AddSelectCallback(Action<InfiniteScrollData> callback)
        {
            // �ʱ�ȭ�� �ȵ� ��� �ʱ�ȭ ����
            if (isInitialize == false)
            {
                Initialize();
            }

            // �ݹ� �Լ� �߰�
            selectCallback += callback;
        }

        // ���� �ݹ� �Լ��� �����ϴ� �޼���
        public void RemoveSelectCallback(Action<InfiniteScrollData> callback)
        {
            // �ʱ�ȭ�� �ȵ� ��� �ʱ�ȭ ����
            if (isInitialize == false)
            {
                Initialize();
            }

            // �ݹ� �Լ� ����
            selectCallback -= callback;
        }

        // �������� Ȱ�� ���� ���� �� ȣ��Ǵ� �޼���
        public void OnChangeActiveItem(int dataIndex, bool active)
        {
            // Ȱ�� ���� ���� �̺�Ʈ �߻�
            onChangeActiveItem.Invoke(dataIndex, active);
        }

        // Ư�� �������� ���ؽ�Ʈ�� ã�� �޼���
        protected DataContext GetDataContext(InfiniteScrollData data)
        {
            // �ʱ�ȭ�� �ȵ� ��� �ʱ�ȭ ����
            if (isInitialize == false)
            {
                Initialize();
            }

            // ������ ����Ʈ���� �ش� �������� ���ؽ�Ʈ ã�Ƽ� ��ȯ
            return dataList.Find((context) =>
            {
                // �����Ͱ� ��ġ�ϴ��� Ȯ��
                return context.data.Equals(data);
            });
        }

        // ������ �ε����� ���ؽ�Ʈ�� ã�� �޼���
        protected DataContext GetContextFromItem(int itemIndex)
        {
            // �ʱ�ȭ�� �ȵ� ��� �ʱ�ȭ ����
            if (isInitialize == false)
            {
                Initialize();
            }

            // ������ �ε����� ��ȿ�� ���
            if (IsValidItemIndex(itemIndex) == true)
            {
                // �ش� ������ ��ȯ
                return GetItem(itemIndex);
            }
            else
            {
                // ��ȿ���� ���� ��� null ��ȯ
                return null;
            }
        }

        // �����͸� �߰��ϴ� �޼���
        protected void AddData(InfiniteScrollData data)
        {
            // ���ο� ������ ���ؽ�Ʈ ���� (�ε����� ���� ����Ʈ ũ��)
            DataContext addData = new DataContext(data, dataList.Count);
            // ���ؽ�Ʈ �ʱ�ȭ
            InitFitContext(addData);

            // ������ ����Ʈ�� �߰�
            dataList.Add(addData);

            // ������ �߰� �� ������ üũ
            CheckItemAfterAddData(addData);
        }

        // ������ �߰� �� ������ ���¸� üũ�ϴ� �޼���
        private bool CheckItemAfterAddData(DataContext addData)
        {
            // ���Ͱ� �ְ� ���͸� �� ���
            if (onFilter != null &&
                onFilter(addData.data) == true)
            {
                // ó�� �ߴ�
                return false;
            }

            // ���ο� �������� �ε��� ��� (�⺻�� 0)
            int itemIndex = 0;
            // ���� �������� �ִ� ���
            if (itemCount > 0)
            {
                // �߰��� ������ ������ �����͵��� �������� ��ȸ
                for (int dataIndex = addData.index - 1; dataIndex >= 0; dataIndex--)
                {
                    // ��ȿ�� ������ �ε����� ���� �����͸� ã����
                    if (dataList[dataIndex].itemIndex != -1)
                    {
                        // �� ���� �ε����� �� ������ �ε����� ����
                        itemIndex = dataList[dataIndex].itemIndex + 1;
                        break;
                    }
                }
            }

            // �� �����Ϳ� ������ �ε��� ����
            addData.itemIndex = itemIndex;
            // ��ü ������ ���� ����
            itemCount++;

            // �߰��� ������ ������ ��� �����͵��� ������ �ε��� ����
            for (int dataIndex = addData.index + 1; dataIndex < dataList.Count; dataIndex++)
            {
                // ��ȿ�� ������ �ε����� ���� �����͸� ����
                if (dataList[dataIndex].itemIndex != -1)
                {
                    dataList[dataIndex].itemIndex++;
                }
            }

            // ���̾ƿ� �籸�� �ʿ� �÷��� ����
            needReBuildLayout = true;

            // ���������� ó����
            return true;
        }

        // Ư�� ��ġ�� �����͸� �����ϴ� �޼���
        protected void InsertData(InfiniteScrollData data, int insertIndex)
        {
            // ���� �ε����� ��ȿ���� ���� ��� ���� �߻�
            if (insertIndex < 0 || insertIndex > dataList.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            // ����Ʈ �߰��� �����ϴ� ���
            if (insertIndex < dataList.Count)
            {
                // ���ο� ������ ���ؽ�Ʈ ����
                DataContext addData = new DataContext(data, insertIndex);
                // ���ؽ�Ʈ �ʱ�ȭ
                InitFitContext(addData);

                // ���� ��ġ ������ ��� �����͵��� �ε��� ����
                for (int dataIndex = insertIndex; dataIndex < dataList.Count; dataIndex++)
                {
                    dataList[dataIndex].index++;
                }

                // ������ ��ġ�� ������ ����
                dataList.Insert(insertIndex, addData);

                // ������ �߰� �� ������ üũ
                CheckItemAfterAddData(addData);
            }
            else
            {
                // ����Ʈ ���� �߰��ϴ� ��� AddData �޼��� ȣ��
                AddData(data);
            }
        }

        // ���ؽ�Ʈ�� �ʱ�ȭ�ϴ� �޼���
        protected void InitFitContext(DataContext context)
        {
            // �⺻ ������ ũ�⿡�� ���� ũ�� ��������
            float size = layout.GetMainSize(defaultItemPrefabSize);
            // ���� ������ ũ�� ����ϴ� ���
            if (dynamicItemSize == true)
            {
                // ���ؽ�Ʈ���� ������ ũ�� ��������
                float ItemSize = context.GetItemSize();
                // ������ ũ�Ⱑ �����Ǿ� �ִ� ���
                if (ItemSize != 0)
                {
                    // �ش� ũ�� ���
                    size = ItemSize;
                }
            }

            // ���ؽ�Ʈ�� ������ ũ�� ����
            context.SetItemSize(size);
        }

        // ������ �ε����� ��ȿ���� Ȯ���ϴ� �޼���
        protected bool IsValidDataIndex(int index)
        {
            // �ε����� 0 �̻��̰� ������ ����Ʈ ũ�� �̸����� Ȯ��
            return (index >= 0 && index < dataList.Count) ? true : false;
        }

        // ������ �ε����� ��ȿ���� Ȯ���ϴ� �޼���
        protected bool IsValidItemIndex(int index)
        {
            // �ε����� 0 �̻��̰� ������ ���� �̸����� Ȯ��
            return (index >= 0 && index < itemCount) ? true : false;
        }

        // ������ ����Ʈ�� �����ϴ� �޼���
        protected void BuildItemList()
        {
            // ������ ���� �ʱ�ȭ
            itemCount = 0;
            // ��� ������ ���ؽ�Ʈ�� ��ȸ
            for (int i = 0; i < dataList.Count; i++)
            {
                // ���� ������ ���ؽ�Ʈ ��������
                DataContext context = dataList[i];

                // ���Ͱ� �ְ� ���͸� �� ���
                if (onFilter != null &&
                     onFilter(context.data) == true)
                {
                    // ������ ���� ���� (�̺�Ʈ �˸� ����)
                    context.UnlinkItem(false);

                    // ���� ���������� �Ѿ
                    continue;
                }
                // ��ȿ�� �����ۿ� �ε��� �Ҵ�
                context.itemIndex = itemCount;
                // ������ ���� ����
                itemCount++;
            }

            // ���̾ƿ� �籸�� �ʿ� �÷��� ����
            needReBuildLayout = true;
        }

        // ������ ���� �� ȣ��Ǵ� �޼���
        private void OnSelectItem(InfiniteScrollData data)
        {
            // ���õ� �������� �ε��� ã��
            int dataIndex = GetDataIndex(data);
            // ������ �ε����� ��ȿ�� ���
            if (IsValidDataIndex(dataIndex) == true)
            {
                // ���õ� ������ �ε��� ����
                selectDataIndex = dataIndex;

                // ���� �ݹ��� �ִ� ���
                if (selectCallback != null)
                {
                    // �ݹ� �Լ� ȣ��
                    selectCallback(data);
                }
            }
        }

        // ������ ����Ʈ�� �����ϴ� �޼���
        public void SortDataList(Comparison<DataContext> comparison)
        {
            // �־��� �� �Լ��� ������ ����Ʈ ����
            dataList.Sort(comparison);

            // ������ ����Ʈ ������Ʈ �ʿ� �÷��� ����
            needUpdateItemList = true;
            // ǥ�õǴ� ������ ������Ʈ
            UpdateShowItem();
        }
    }
}