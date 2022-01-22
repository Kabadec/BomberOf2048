using UnityEngine;

namespace BomberOf2048.Utils
{
    public static class WindowUtils
    {
        private const string CanvasTag = "MainCanvas";

        public static void CreateWindow(string resourcePath)
        {
            var window = Resources.Load<GameObject>(resourcePath);

            var canvas = GameObject.FindWithTag(CanvasTag).GetComponent<Canvas>();
            Object.Instantiate(window, canvas.transform);
        }
    }
}