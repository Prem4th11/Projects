using UnityEngine;
using UnityEngine.EventSystems;
public class Teammate : MonoBehaviour, IPointerClickHandler
{
    #region fields | variables
    public GameManager gameManager;
    #endregion
    #region Mono and methods
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Teammate clicked: " + gameObject.name); 
        gameManager.CheckInput(gameObject);
    }
    #endregion
}