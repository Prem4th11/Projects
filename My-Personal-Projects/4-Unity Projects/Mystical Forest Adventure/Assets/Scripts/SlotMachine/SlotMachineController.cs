using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SlotMachineController : MonoBehaviour
{
    #region fields
    [SerializeField] private ReelWorker[] reels;
    [SerializeField] private SlotSymbol[] symbols;
    [SerializeField] private Button spinButton, betIncreaseButton, betDecreaseButton;
    [SerializeField] private TMP_Text gameType,balanceText, betAmountText, winAmountText, freeSpinsText, popupMessage;
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private LineRenderer paylineRenderer;
    [SerializeField] private SpineAnimationController spineController;

    private int paylineCount = 0;
    private float playerBalance = 100.00f; 
    private float betAmount = 0.10f;
    private int freeSpins = 0;
    private int reelsStopped = 0;
    private float totalWin = 0f;
    private string currentGameMode = "Base Game";
    private const string FREESPINSSTR = "Free Game";
    private const string BONUSSPINSTR = "Bonus Game";
    private const string BASEGAMESTR = "Base Game";
    private bool runningFreeGame = false;
    #endregion

    #region Mono & Methods
    private void Start()
    {
        if (symbols == null || symbols.Length == 0)
        {
            Debug.LogError($"{gameObject.name}: symbolSlots is not assigned or empty! Check in Inspector.");
        }
        spinButton.onClick.AddListener(StartSpin);
        betIncreaseButton.onClick.AddListener(IncreaseBet);
        betDecreaseButton.onClick.AddListener(DecreaseBet);

        foreach (ReelWorker reel in reels)
        {
            reel.OnReelStopped += HandleReelStop;
        }
        UpdateUI();
    }

    public void StartSpin()
    {
        AudioManager.instance?.Play("click");
        paylineRenderer.gameObject.SetActive(false);
        if (freeSpins > 0)  
        {
            freeSpins--;
            UpdateUI();
        }
        else if (playerBalance >= betAmount)
        {
            playerBalance -= betAmount;
            UpdateUI();
        }
        else
        {
            Debug.LogWarning("Not enough balance!");
            return;
        }

        reelsStopped = 0;
        spinButton.interactable = false;

        StartCoroutine(SpinReels());
    }

    private IEnumerator SpinReels()
    {
        for (int i = 0; i < reels.Length; i++)
        {
            Debug.Log(reels[i].gameObject.name);
            reels[i].StartSpin();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void HandleReelStop()
    {
        reelsStopped++;
        if (reelsStopped == reels.Length)
        {
            CalculateFinalVisibleSymbols(); 
            CheckWinningCombinations();

            if (totalWin > 0)
            {
                playerBalance += totalWin;
            }
            UpdateUI();
            AudioManager.instance?.Play("reelstop");
        }
    }

    private void CalculateFinalVisibleSymbols()
    {
        int scatterCount = 0;
        int bonusCount = 0;
        int wildCount = 0;

        float[] visiblePositions = { -200f, 0f, 200f };

        for (int reel = 0; reel < reels.Length; reel++)
        {
            for (int row = 0; row < reels[reel].symbolSlots.Length; row++)
            {
                SlotSymbol visibleSymbol = GetSymbolAt(reel, row);
                if (visibleSymbol == null) continue;

                float symbolY = reels[reel].symbolSlots[row].anchoredPosition.y;

                if (System.Array.Exists(visiblePositions, pos => Mathf.Abs(pos - symbolY) < 1f))
                {
                    if (visibleSymbol.type == SymbolType.Scatter)
                        scatterCount++;
                    if (visibleSymbol.type == SymbolType.Bonus)
                        bonusCount++;
                    if (visibleSymbol.type == SymbolType.Wild)
                        wildCount++;
                }
            }
        }

        if (scatterCount >= 3 && bonusCount >= 3)
        {
            int randomChoice = Random.Range(0, 2);
            bool IsFreeGame = randomChoice == 0 ? true : false;
            string gameStr = randomChoice == 0 ? FREESPINSSTR : BONUSSPINSTR;
            string audio = randomChoice == 0 ? "freespinbg" : "bonuswin";
            AudioManager.instance?.StopBackgroundMusic();
            StartCoroutine(ShowPopup(gameStr, IsFreeGame, scatterCount,audio));
        }
        else if (scatterCount >= 3)
        {
            AudioManager.instance?.StopBackgroundMusic();
            StartCoroutine(ShowPopup(FREESPINSSTR, true, scatterCount, "freespinbg"));
        }
        else if (bonusCount >= 3)
        {
            AudioManager.instance?.StopBackgroundMusic();
            StartCoroutine(ShowPopup(BONUSSPINSTR, false, scatterCount, "bonuswin"));
        }
    }

    private void TriggerFreeSpins(int scatterCount)
    {
        paylineRenderer.gameObject.SetActive(false);
        int freeSpinsAwarded = scatterCount == 3 ? 10 : scatterCount == 4 ? 15 : 20;
        StartCoroutine(ExecuteFreeSpins(freeSpinsAwarded));
    }

    private IEnumerator ExecuteFreeSpins(int freeSpinsAwarded)
    {
        freeSpins += freeSpinsAwarded;
        UpdateUI();
        for (int i = 0; i < freeSpinsAwarded; i++)
        {
            Debug.Log($"Free Spin {i + 1}/{freeSpinsAwarded}");
            StartSpin();
            yield return new WaitForSeconds(2f);
        }
        Debug.Log("Free Spins Over!");
        yield return new WaitForSeconds(2f);
        currentGameMode = BASEGAMESTR;
        AudioManager.instance?.PlayBackgroundMusic("basegamebg");
        runningFreeGame = false;
        ToggleFields(false);
    }

    private IEnumerator TriggerBonusGame()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Bonus Game Triggered!");
        // ToDo : Open Bonus Game UI
        currentGameMode = BASEGAMESTR;
        AudioManager.instance?.PlayBackgroundMusic("basegamebg");
    }

    private void CheckWinningCombinations()
    {
        totalWin = 0f;
        List<int[]> winningPaylines = new List<int[]>();
        foreach (int[] payline in PaylineData.paylines)
        {
            int matchCount = 1;
            SlotSymbol firstSymbol = GetSymbolAt(0, payline[0]);
            string debugSymbols = firstSymbol?.name;

            for (int reel = 1; reel < reels.Length; reel++)
            {
                SlotSymbol nextSymbol = GetSymbolAt(reel, payline[reel]);
                debugSymbols += $" | {nextSymbol?.name}";

                if (firstSymbol?.type == SymbolType.Wild || nextSymbol?.type == SymbolType.Wild || firstSymbol?.name == nextSymbol?.name)
                {
                    matchCount++;
                }
                else break;
            }

            if (firstSymbol !=null && matchCount >= 3 && firstSymbol?.type != SymbolType.Scatter && firstSymbol?.type != SymbolType.Bonus)
            {
                float payoutMultiplier = firstSymbol.payouts[matchCount - 3];
                totalWin += betAmount * payoutMultiplier;
                winningPaylines.Add(payline);
                Debug.Log($"Winning Payline {PaylineData.paylines.IndexOf(payline) + 1}: {debugSymbols} | Matches: {matchCount} | Payout: {betAmount * payoutMultiplier:F2}");
            }
        }
        paylineCount = winningPaylines.Count;
        if (winningPaylines.Count > 0)
        {
            StartCoroutine(ShowWinningPaylines(winningPaylines));
        }
        else
        {
            spinButton.interactable = true;
        }
        winAmountText.text = $"Win: {totalWin:F2}";
    }

    private IEnumerator ShowWinningPaylines(List<int[]> winningPaylines)
    {
        foreach (var payline in winningPaylines)
        {
            paylineCount = paylineCount - 1;
            List<RectTransform> paylineSlots = new List<RectTransform>();
            int paylineIndex = PaylineData.paylines.IndexOf(payline);
            for (int reel = 0; reel < reels.Length; reel++)
            {
                int rowIndex = payline[reel];
                paylineSlots.Add(reels[reel].symbolSlots[rowIndex]);
            }

            //enable once the free and bonus features implemented
            if (currentGameMode == BASEGAMESTR)
            {
                paylineRenderer.gameObject.SetActive(true);
                spineController?.ToggleSpine(true);
                StartCoroutine(HighlightWinningSymbols(paylineSlots));
                spineController?.PlayPaylineEffect(paylineRenderer, paylineIndex);
                yield return new WaitForSeconds(1f);
                spineController?.StopAnimation();
            }
        }

        if (paylineCount == 0)
        {
            spinButton.interactable = true;
        }

        paylineRenderer.gameObject.SetActive(false);
        spineController?.ToggleSpine(false);
    }

    private IEnumerator HighlightWinningSymbols(List<RectTransform> winningSlots)
    {
        if (winningSlots.Count == 0) yield break;

        paylineRenderer.positionCount = winningSlots.Count;
        paylineRenderer.startWidth = 2f;
        paylineRenderer.endWidth = 2f;
        paylineRenderer.sortingLayerName = "Above UI";
        paylineRenderer.sortingOrder = 100;

        if (paylineRenderer.material == null)
        {
            paylineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            paylineRenderer.material.color = Color.red;
        }
        else
        {
            paylineRenderer.material.color = Color.yellow;
        }

        for (int i = 0; i < winningSlots.Count; i++)
        {
            Vector3 worldPos = winningSlots[i].position;
            paylineRenderer.SetPosition(i, worldPos);
        }

        foreach (var slot in winningSlots)
        {
            Image img = slot.GetComponent<Image>();
            img.color = Color.yellow;

            for (int i = 0; i < 5; i++)
            {
                slot.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, i / 5f);
                yield return new WaitForSeconds(0.01f);
            }
        }
        yield return new WaitForSeconds(1f);

        foreach (var slot in winningSlots)
        {
            slot.GetComponent<Image>().color = Color.white;
            slot.localScale = Vector3.one;
        }
        paylineRenderer.positionCount = 0;
    }

    private SlotSymbol GetSymbolAt(int reel, int row)
    {
        Image image = reels[reel].symbolSlots[row].GetComponent<Image>();
        foreach (SlotSymbol symbol in symbols)
        {
            if (image.sprite == symbol.sprite)
                return symbol;
        }
        return null;
    }

    private void IncreaseBet()
    {
        AudioManager.instance?.Play("click");
        if (betAmount < 5.00f)
        {
            betAmount += 0.10f;
            UpdateUI();
        }
    }

    private void DecreaseBet()
    {
        AudioManager.instance?.Play("click");
        if (betAmount > 0.10f)
        {
            betAmount -= 0.10f;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        balanceText.text = $"Balance: {playerBalance:F2}";
        betAmountText.text = $"Bet: {betAmount:F2}";
        winAmountText.text = $"Win: {totalWin:F2}";

        if (freeSpins > 0)
        {
            freeSpinsText.text = $"Free Spins: {freeSpins}";
            ToggleFields(true);
        }
        else
        {
            ToggleFields(false);
        }
        gameType.text = currentGameMode;
    }

    private void ToggleFields(bool status)
    {
        freeSpinsText.gameObject.SetActive(status);
        betAmountText.gameObject.SetActive(!status);
        betIncreaseButton.gameObject.SetActive(!status);
        betDecreaseButton.gameObject.SetActive(!status);
    }

    private IEnumerator ShowPopup(string message ,bool IsFreeGame, int scatterCount,string audio)
    {
        UpdateGameMode(IsFreeGame);
        popupMessage.text = message;
        popupPanel.SetActive(true); 
        yield return new WaitForSeconds(3f);
        popupPanel.SetActive(false);
        AudioManager.instance?.Play(audio);

        Debug.Log($" game mode : {IsFreeGame}");

        if (IsFreeGame && !runningFreeGame)
        {
            Debug.Log($"{FREESPINSSTR} called");
            runningFreeGame = true;
            TriggerFreeSpins(scatterCount);
        }
        else if(!runningFreeGame && !IsFreeGame)
        {
            Debug.Log($"{BONUSSPINSTR} called");
            TriggerBonusGame();
        }
    }

    private void UpdateGameMode(bool IsFreeGame)
    {
        if (IsFreeGame && currentGameMode != FREESPINSSTR)
        {
            currentGameMode = FREESPINSSTR;
        }
        else if (!runningFreeGame && !IsFreeGame && currentGameMode != BONUSSPINSTR)
        {
            currentGameMode = BONUSSPINSTR;
        }
    }
    #endregion
}
