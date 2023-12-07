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
            button.gameObject.SetActive(true);
            userText.gameObject.SetActive(false);
            checkMark.gameObject.SetActive(false);
        }

        public void OnUserChange(string userName)
        {
            button.gameObject.SetActive(false);
            userText.gameObject.SetActive(true);
            userText.text = userName;
        }

        public void OnUserRemove()
        {
            button.gameObject.SetActive(true);
            userText.gameObject.SetActive(false);
        }

        public void OnUserChecked()
        {
            checkMark.gameObject.SetActive(true);
        }

        public void OnUserUnchecked()
        {
            checkMark.gameObject.SetActive(false);
        }

        public void SetUserReady(bool is_rdy)
        {
            checkMark.gameObject.SetActive(is_rdy);
        }
    }
}
