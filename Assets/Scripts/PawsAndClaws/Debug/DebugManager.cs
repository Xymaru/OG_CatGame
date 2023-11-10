using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Debugging
{
    public class DebugManager : MonoBehaviour
    {
        [SerializeField] private GameObject imguiRenderer;
        [SerializeField] private GameObject debugConsole;

        private void Start()
        {
            imguiRenderer.SetActive(true);
            debugConsole.SetActive(true);
        }
    }
}
