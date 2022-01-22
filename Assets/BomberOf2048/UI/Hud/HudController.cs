using BomberOf2048.Utils;
using UnityEngine;

namespace BomberOf2048.UI.Hud
{
    public class HudController : MonoBehaviour
    {
        
        public void OnInfo()
        {
            WindowUtils.CreateWindow("UI/InfoWindow");
        }
    }
}