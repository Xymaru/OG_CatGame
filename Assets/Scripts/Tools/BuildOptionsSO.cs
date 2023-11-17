using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZaroDev.Tools
{
    [CreateAssetMenu(menuName ="Tools/Build Options", fileName = "Build Options")]
    public class BuildOptionsSO : ScriptableObject
    {
        public string BuildPath;
        public string BuildName;
    }
}
