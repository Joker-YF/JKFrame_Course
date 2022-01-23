using JKFrame;
using UnityEngine;
using UnityEngine.UI;
[UIElement(true, "UI/GameMainWindow", 1)]
public class UI_GameMainWindow : UI_WindowBase
{
    [SerializeField] private Text Score_Text;
    [SerializeField] private Image HPBar_Fill_Image;
    [SerializeField] private Text BulletNum_Text;

    protected override void RegisterEventListener()
    {
        base.RegisterEventListener();
        EventManager.AddEventListener<int>("UpdateHP", UpdateHP);
        EventManager.AddEventListener<int>("UpdateScore", UpdateScore);
        EventManager.AddEventListener<int,int>("UpdateBullet", UpdateBullet);
    }

    protected override void CancelEventListener()
    {
        base.CancelEventListener();
        EventManager.RemoveEventListener<int>("UpdateHP", UpdateHP);
        EventManager.RemoveEventListener<int>("UpdateScore", UpdateScore);
        EventManager.RemoveEventListener<int, int>("UpdateBullet", UpdateBullet);
    }

    private void UpdateHP(int hp)
    {
        HPBar_Fill_Image.fillAmount = hp / 100f;
    }
    private void UpdateScore(int num)
    {
        Score_Text.text = num.ToString();
    }
    private void UpdateBullet(int curr,int max)
    {
        BulletNum_Text.text = curr + "/" + max;
    }
}
