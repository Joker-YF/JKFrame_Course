using JKFrame;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Pool]
public class UI_SaveItem : MonoBehaviour
{
    [SerializeField] private Image BG;
    [SerializeField] private Text UserName_Text;
    [SerializeField] private Text Time_Text;
    [SerializeField] private Text Score_Text;
    [SerializeField] private Text Del_Button_Text;
    [SerializeField] private Button Del_Button;

    private static Color normalColor = new Color(0,0,0,0.6f);
    private static Color selectColor = new Color(0,0.6f,1,0.6f);

    private SaveItem saveItem;
    private UserData userData;
    private void Start()
    {
        Del_Button.onClick.AddListener(DelButtonClick);
        this.OnMouseEnter(MouseEnter);
        this.OnMouseExit(MouseExit);
        this.OnClick(Click);
    }
    private void OnEnable()
    {
        Del_Button_Text.JKLocaSet("UI_SaveWindow", "SaveItem_DelButtonText");
    }
    public void Init(SaveItem saveItem)
    {
        this.saveItem = saveItem;
        Time_Text.text = saveItem.lastSaveTime.ToString("g");
        userData = SaveManager.LoadObject<UserData>(saveItem);
        UserName_Text.text = userData.UserName;
        Score_Text.text = userData.Score.ToString();
    }

    public void Destroy()
    {
        saveItem = null;
        userData = null;
        this.JKGameObjectPushPool();
    }
    public void DelButtonClick()
    {
        AudioManager.Instance.PlayOnShot("Audio/Button", UIManager.Instance);
        SaveManager.DeleteSaveItem(saveItem);
        EventManager.EventTrigger("UpdateSaveItem");
        EventManager.EventTrigger("UpdateRankItem");
    }

    private void MouseEnter(PointerEventData pointerEventData,params object[] args)
    {
        BG.color = selectColor;
    }

    private void MouseExit(PointerEventData pointerEventData, params object[] args)
    {
        BG.color = normalColor;
    }
    private void Click(PointerEventData pointerEventData, params object[] args)
    {
        // 进入游戏
        EventManager.EventTrigger<SaveItem, UserData>("EnterGame", saveItem, userData);
    }
}
