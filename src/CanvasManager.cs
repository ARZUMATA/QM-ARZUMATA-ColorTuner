using System.Collections.Generic;
using UnityEngine;

namespace QM_ColorBlindHelper
{
    public class CanvasManager : MonoBehaviour
    {
        //private static CanvasManager instance;
        private List<CanvasColorBlindHelper> canvasHelpers = new List<CanvasColorBlindHelper>();

        //public static CanvasManager Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            GameObject go = new GameObject("CanvasManager");
        //            instance = go.AddComponent<CanvasManager>();
        //            DontDestroyOnLoad(go);
        //        }
        //        return instance;
        //    }
        //}

        void Update()
        {
            // Find new canvases and add helpers to them
            Canvas[] allCanvases = FindObjectsOfType<Canvas>();

            foreach (Canvas canvas in allCanvases)
            {
                if (canvas.GetComponent<CanvasColorBlindHelper>() == null)
                {
                    var helper = canvas.gameObject.AddComponent<CanvasColorBlindHelper>();
                    canvasHelpers.Add(helper);
                    Plugin.Logger.Log($"Added ColorBlind helper to canvas: {canvas.name}");
                }
            }

            // Clean up null references
            canvasHelpers.RemoveAll(helper => helper == null);
        }
    }
}
