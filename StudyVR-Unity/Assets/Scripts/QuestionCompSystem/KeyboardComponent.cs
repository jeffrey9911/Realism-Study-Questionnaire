using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyboardComponent : MonoBehaviour
{
    public enum KeysEnum
    {
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z,
        ONE,
        TWO,
        THREE,
        FOUR,
        FIVE,
        SIX,
        SEVEN,
        EIGHT,
        NINE,
        ZERO,
        COMMA,
        PERIOD,
        SPACE,
        BACKSPACE,
        SHIFT
    }

    private char[] KeysChar =
    {
        'A',
        'B',
        'C',
        'D',
        'E',
        'F',
        'G',
        'H',
        'I',
        'J',
        'K',
        'L',
        'M',
        'N',
        'O',
        'P',
        'Q',
        'R',
        'S',
        'T',
        'U',
        'V',
        'W',
        'X',
        'Y',
        'Z',
        '1',
        '2',
        '3',
        '4',
        '5',
        '6',
        '7',
        '8',
        '9',
        '0',
        ',',
        '.',
        ' ',
        '\b'
    };

    [SerializeField] private QSComponent qsComponent;
    [SerializeField] private KeysEnum keysEnum;
    [SerializeField] private TMP_Text keyText;
    

    private bool isCaptial = false;

    public void KeyOnClick()
    {
        if(keysEnum == KeysEnum.SHIFT)
        {
            foreach(Transform key in this.transform.parent)
            {
                KeyboardComponent keyC;
                if(key.TryGetComponent<KeyboardComponent>(out keyC))
                {
                    keyC.ShiftOnClick();
                }
            }
            return;
        }

        if (char.IsLetter(KeysChar[(int)keysEnum]))
        {
            if(isCaptial)
            {
                qsComponent.KeyOnClick(char.ToUpper(KeysChar[(int)keysEnum]));
            }
            else
            {
                qsComponent.KeyOnClick(char.ToLower(KeysChar[(int)keysEnum]));
            }
        }
        else
        {
            qsComponent.KeyOnClick(KeysChar[(int)keysEnum]);
        }
    }

    public void ShiftOnClick()
    {
        isCaptial = !isCaptial;
        LoadDisplayText();
    }

    private void LoadDisplayText()
    {
        if(keysEnum == KeysEnum.SHIFT)
        {
            if(isCaptial)
            {
                keyText.text = "● CAPS ●";
            }
            else
            {
                keyText.text = "○ CAPS ○";
            }
            return;
        }

        if(keysEnum == KeysEnum.SPACE)
        {
            keyText.text = "Space";
            return;
        }

        if(keysEnum == KeysEnum.BACKSPACE)
        {
            keyText.text = "Backspace";
            return;
        }
        
        if (char.IsLetter(KeysChar[(int)keysEnum]))
        {
            if (isCaptial)
            {
                keyText.text = char.ToUpper(KeysChar[(int)keysEnum]).ToString();
            }
            else
            {
                keyText.text = char.ToLower(KeysChar[(int)keysEnum]).ToString();
            }

            return;
        }

        keyText.text = KeysChar[(int)keysEnum].ToString();

    }

    private void Awake()
    {
        LoadDisplayText();
    }
}
