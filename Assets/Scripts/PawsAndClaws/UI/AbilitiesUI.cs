using PawsAndClaws.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PawsAndClaws.UI
{
    public class AbilitiesUI : MonoBehaviour
    {
        public AbilityHolderUI passiveButton;
        public AbilityHolderUI ability1Button;
        public AbilityHolderUI ability2Button;
        public AbilityHolderUI ultimateButton;


        public void SetupImages(CharacterDataSO data)
        {
            if(data == null)
            {
                Debug.LogError("Data is null");
                return;
            }
            passiveButton.SetImage(data.passiveImage);
            ability1Button.SetImage(data.ability1Sprite);
            ability2Button.SetImage (data.ability2Sprite);
            ultimateButton.SetImage(data.ultimateSprite);
        }
    }
}