using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReelWorker : MonoBehaviour
{
    #region fields
    public RectTransform[] symbolSlots;  // The 6 moving symbols positions inside the reel

    [SerializeField] private Sprite[] symbols; // List of possible symbols
    [SerializeField] private float spinSpeed = 1000f; // Speed of reel movement

    private bool isSpinning = false;
    private float reelHeight = 200f; // Height of each symbol slot - y-axis
    private int totalSymbols = 6; // Total symbols moving in the reel
    private float minStopTime = 2f, maxStopTime = 4f; // Randomized spin time

    public event Action OnReelStopped;
    #endregion
    #region Mono & Methods
    public void StartSpin()
    {
        if (!isSpinning)
        {
            isSpinning = true;
            AudioManager.instance?.Play("reelspin");
            StartCoroutine(SpinReel());
        }
    }
    
    private IEnumerator SpinReel()
    {
        float elapsedTime = 0f;
        //float totalSpinTime = UnityEngine.Random.Range(minStopTime, maxStopTime);
        float totalSpinTime = 2f;

        while (elapsedTime < totalSpinTime)
        {
            elapsedTime += Time.deltaTime;
            for (int i = 0; i < totalSymbols; i++)
            {
                symbolSlots[i].anchoredPosition -= new Vector2(0, spinSpeed * Time.deltaTime);
                // Reset symbol position when it moves past the lowest visible area
                if (symbolSlots[i].anchoredPosition.y <= -reelHeight * 3)//visible 3 reels height
                {
                    float highestY = GetHighestPosition() + reelHeight;
                    symbolSlots[i].anchoredPosition = new Vector2(0, highestY);
                    symbolSlots[i].GetComponent<Image>().sprite = GetUniqueRandomSymbol(i);
                }
            }
            yield return null;
        }
        SnapToFinalPositions();
        Debug.Log(this.gameObject.name);
    }
    
    private void SnapToFinalPositions()
    {
        //Strictly enforce unique Y positions
        float[] correctPositions = { -200f, 0f, 200f, 400f, 600f, 800f };
        List<float> availablePositions = new List<float>(correctPositions);
        //Sort symbols by Y position before snapping
        Array.Sort(symbolSlots, (a, b) => a.anchoredPosition.y.CompareTo(b.anchoredPosition.y));

        for (int i = 0; i < symbolSlots.Length; i++)
        {
            if (availablePositions.Count == 0)
            {
                Debug.LogError("No available positions left! This should not happen.");
                break;
            }
            float assignedY = availablePositions[0];
            symbolSlots[i].anchoredPosition = new Vector2(0, assignedY);
            availablePositions.RemoveAt(0); // Remove assigned position to ensure uniqueness
        }
        isSpinning = false;
        OnReelStopped?.Invoke();
    }

    private float GetHighestPosition()
    {
        float highestY = float.MinValue;
        for (int i = 0; i < totalSymbols; i++)
        {
            if (symbolSlots[i].anchoredPosition.y > highestY)
            {
                highestY = symbolSlots[i].anchoredPosition.y;
            }
        }
        return highestY;
    }
   
    private Sprite GetUniqueRandomSymbol(int currentIndex)
    {
        Sprite newSymbol;
        do
        {
            newSymbol = symbols[UnityEngine.Random.Range(0, symbols.Length)];
        }
        while (currentIndex > 0 && newSymbol == symbolSlots[currentIndex - 1].GetComponent<Image>().sprite);

        return newSymbol;
    }
    #endregion
}
