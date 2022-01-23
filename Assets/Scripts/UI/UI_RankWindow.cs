using JKFrame;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[UIElement(true, "UI/RankWindow", 1)]
public class UI_RankWindow : UI_WindowBase
{
    [SerializeField] private Button Close_Button;
    [SerializeField] private Transform ItemParent;
    private List<UI_RankItem> UI_RankItemList = new List<UI_RankItem>();
    private bool wantUpdate = true;
    public override void Init()
    {
        Close_Button.onClick.AddListener(Close);
    }
    public override void OnShow()
    {
        base.OnShow();
        if (wantUpdate)
        {
            UpdateAllRankItem();
        }
    }
    public override void OnClose()
    {
        AudioManager.Instance.PlayOnShot("Audio/Button", UIManager.Instance);
        base.OnClose();
    }

    protected override void RegisterEventListener()
    {
        base.RegisterEventListener();
        EventManager.AddEventListener("UpdateRankItem", UpdateRankItemFlag);
    }

    private void UpdateRankItemFlag()
    {
        wantUpdate = true;
        // 如果当前我是激活状态，那么应该立刻刷新，而不是等到下一次打开刷新
        if (gameObject.activeInHierarchy)
        {
            UpdateAllRankItem();
        }
    }
    /// <summary>
    /// 更新排名数据
    /// </summary>
    private void UpdateAllRankItem()
    {
        // 清空已有的
        for (int i = 0; i < UI_RankItemList.Count; i++)
        {
            UI_RankItemList[i].Destroy();
        }
        UI_RankItemList.Clear();

        // 获取所有存档
        List<SaveItem> saveItems = SaveManager.GetAllSaveItem();
        List<UserData> userDatas = new List<UserData>(saveItems.Count);
        // 获取所有存档所对应的用户数据
        for (int i = 0; i < saveItems.Count; i++)
        {
            userDatas.Add(SaveManager.LoadObject<UserData>(saveItems[i]));
        }
        // 对用户数据进行排序
        userDatas = userDatas.OrderByDescending((userData) =>
        {
            return userData.Score;
        }).ToList();

        // 实例化所有的Item
        for (int i = 0; i < userDatas.Count; i++)
        {
            UI_RankItem item = ResManager.Load<UI_RankItem>("UI/RankItem", ItemParent);
            item.Init(userDatas[i],i+1);
            UI_RankItemList.Add(item);
        }

        // 更新标志
        wantUpdate = false;
    }
}
