using System;
using PawsAndClaws.Game;
using PawsAndClaws.Player;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PawsAndClaws.UI
{
    public class TeamSlotUI : MonoBehaviour
    {
        [SerializeField] private int id;
        [SerializeField] private Team team;
        
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI userText;
        [SerializeField] private GameObject checkMark;
        
        
        private void Awake()
        {
            button.onClick.AddListener(OnClick);
            
            button.gameObject.SetActive(true);
            userText.gameObject.SetActive(false);
            checkMark.gameObject.SetActive(false);
        }
        
        private void OnClick()
        {
            OnUserChange(GameConstants.UserName);
        }

        private void OnUserChange(string userName)
        {
            button.gameObject.SetActive(false);
            userText.gameObject.SetActive(true);
            userText.text = userName;
        }
    }
}
