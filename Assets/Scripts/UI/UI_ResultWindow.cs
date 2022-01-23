using UnityEngine;
using UnityEngine.UI;
using JKFrame;
using System;

[UIElement(true, "UI/ResultWindow", 1)]
public class UI_ResultWindow : UI_WindowBase
{
    [SerializeField] private Text Score_Text;
    [SerializeField] private Button Back_Button;
    [SerializeField] private Button Re_Button;
    [SerializeField] private Text Back_Button_Text;
    [SerializeField] private Text Re_Button_Text;
    private const string LocalSetPackName = "UI_ResultWindow";
    public override void Init()
    {
        Back_Button.onClick.AddListener(Back_Button_Click);
        Re_Button.onClick.AddListener(Re_Button_Click);
    }

    protected override void OnUpdateLanguage()
    {
        Back_Button_Text.JKLocaSet(LocalSetPackName, "Back_Button_Text");
        Re_Button_Text.JKLocaSet(LocalSetPackName, "Re_Button_Text");
    }
    public void Init(int score)
    {
        Score_Text.text = LocalizationManager.Instance.GetContent<L_Text>(LocalSetPackName,"Score").content +score.ToString();
        GameManager.Instance.PauseGame();
    }


    private void Back_Button_Click()
    {
        // 去主菜单
        GameManager.Instance.ContiuneGame();
        SceneManager.LoadScene("MainMenu");
    }
    private void Re_Button_Click()
    {
        // 再次加载当前场景进行游戏
        GameManager.Instance.ContiuneGame();
        GameManager.Instance.RepeatGame();
    }
}
