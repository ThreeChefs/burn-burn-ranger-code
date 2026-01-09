using TMPro;
using UnityEngine;

public class BigIntText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;


    public void SetValue (int value)
    {
        _text.text = FormatBigInt(value);
    }

    string FormatBigInt (long value)
    {
        int asd = 1_000;
        
        if (value >= 1_000_000_000)
        {
            return (value / 1_000_000_000D).ToString("0.##") + "B";
        }
        else if (value >= 1_000_000)
        {
            return (value / 1_000_000D).ToString("0.##") + "M";
        }
        else if (value >= 1_000)
        {
            return (value / 1_000D).ToString("0.##") + "K";
        }
        else
        {
            return value.ToString();
        }
    }

}
