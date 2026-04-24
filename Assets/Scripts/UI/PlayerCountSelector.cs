using TMPro;
using UnityEngine;

public class PlayerCountSelector : MonoBehaviour
{
    [SerializeField] private TMP_Text valueText;

    private int current = 2;
    private int min = 2;
    private int max = 10;

    private void Start()
    {
        UpdateUI();
    }

    public void Increase()
    {
        current++;

        if (current > max)
            current = min;

        UpdateUI();
    }

    public void Decrease()
    {
        current--;

        if (current < min)
            current = max;

        UpdateUI();
    }

    void UpdateUI()
    {
        valueText.text = current.ToString(); 
    }

    public int GetValue()
    {
        return current;
    }
}