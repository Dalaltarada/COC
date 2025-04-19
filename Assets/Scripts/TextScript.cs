using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{
    [SerializeField] private TMP_Text _textMeshPro;
    public string[] stringArray;
    [SerializeField] private float timeBtwnChars = 0.05f;
    [SerializeField] private float timeBtwnWords = 0.5f;

    private int i = 0;
    private bool isTyping = false;
    private Coroutine cursorCoroutine;
    private string currentText = "";

    void Start()
    {
        StartCursor(); // Start blinking empty cursor initially if needed
    }

    public void OnButtonClick()
    {
        if (!isTyping)
        {
            StopAllCoroutines(); // Stop blinking if active
            endCheck();
        }
    }

    public void endCheck()
    {
        if (i < stringArray.Length)
        {
            currentText = stringArray[i];
            StartCoroutine(TextVisible());
        }
    }

    private IEnumerator TextVisible()
    {
        isTyping = true;
        _textMeshPro.text = ""; // clear the screen first

        int counter = 0;

        while (counter <= currentText.Length)
        {
            _textMeshPro.text = currentText.Substring(0, counter) + "_"; // always add a static cursor
            counter++;
            yield return new WaitForSeconds(timeBtwnChars);
        }

        _textMeshPro.text = currentText; // after typing, just the full text (no cursor yet)
        isTyping = false;
        i++;

        StartCursor(); // Start blinking now
    }

    private void StartCursor()
    {
        cursorCoroutine = StartCoroutine(BlinkCursor());
    }

    private IEnumerator BlinkCursor()
    {
        bool cursorVisible = true;

        while (true)
        {
            if (!isTyping) // Only blink when NOT typing
            {
                _textMeshPro.text = currentText + (cursorVisible ? "_" : " ");
                cursorVisible = !cursorVisible;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
