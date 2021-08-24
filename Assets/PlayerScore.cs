using UnityEngine;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI text;
    private int score = 0;
    public void UpdateScore(int n)
    {
        score += n;
        UpdateText();
    }

    public void UpdateText()
    {
        text.text = score.ToString();
    }

    public int GetPoints()
    {
        return score;
    }
}
