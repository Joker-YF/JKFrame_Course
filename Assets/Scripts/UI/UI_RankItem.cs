using UnityEngine;
using JKFrame;
using UnityEngine.UI;

[Pool]
public class UI_RankItem : MonoBehaviour
{
    [SerializeField] private Text RankNum_Text;
    [SerializeField] private Text UserName_Text;
    [SerializeField] private Text Score_Text;
    public void Init(UserData userData,int rankNum)
    {
        RankNum_Text.text = rankNum.ToString(); 
        UserName_Text.text = userData.UserName;
        Score_Text.text = userData.Score.ToString();
    }

    public void Destroy()
    {
        this.JKGameObjectPushPool();
    }
}
