using System;
[Serializable]
public class UserData
{
    public string UserName;
    public int Score = 0;

    public UserData(string userName)
    {
        UserName = userName;
    }
}
