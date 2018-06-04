using Json;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public Situation Situation;
    public Choice Choice;

    public void OnClick()
    {
        if (Situation != null && Choice != null)
        {
            GameManager.Instance.Choose(Situation, Choice);
        }
        else
        {
            GameManager.Instance.Start();
        }
    }
}