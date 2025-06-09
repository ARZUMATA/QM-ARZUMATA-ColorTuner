using HarmonyLib;
using MGSC;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace QM_ColorBlindHelper
{
    internal class ColorTuner
    {
        private static bool isInitialized = false;
        private static bool enabled = false;
        private static ColorBlindComponent colorBlindComponent;
        private static CanvasManager canvasManager;
        private static CanvasColorBlindHelper canvasColorBlindHelper;
        private static float lastUpdateTime = 0f;
        public static float currentTime = Time.time;
        public static float updateInterval = 1f / Plugin.Config.UpdateRate; // Convert updates per second to interval

        [Hook(ModHookType.DungeonStarted)]
        public static void DungeonStarted(IModContext context)
        {
            enabled = true;
            lastUpdateTime = 0f; // Reset timer
        }

        [Hook(ModHookType.DungeonFinished)]
        public static void DungeonFinished(IModContext context)
        {
            enabled = false;
            isInitialized = false;
            canvasColorBlindHelper.enabled = false;
            colorBlindComponent.enabled = false;
            lastUpdateTime = 0f; // Reset timer
        }


        [Hook(ModHookType.SpaceStarted)]
        public static void SpaceStarted(IModContext context)
        {
            enabled = true;
            lastUpdateTime = 0f; // Reset timer
        }

        [Hook(ModHookType.SpaceFinished)]
        public static void SpaceFinished(IModContext context)
        {
            enabled = false;
            isInitialized = false;
            canvasColorBlindHelper.enabled = false;
            colorBlindComponent.enabled = false;
            lastUpdateTime = 0f; // Reset timer
        }

        [Hook(ModHookType.SpaceUpdateBeforeGameLoop)]
        public static void SpaceUpdateBeforeGameLoop(IModContext context)
        {
            enabled = Plugin.Config.EnableColorBlindAssist;
            currentTime = Time.time;

            // Calculate time-based updates instead of frame-based
            if (isInitialized && (currentTime - lastUpdateTime) >= updateInterval)
            // Only update if the color blind type has changed or every few frames to avoid performance issues
            // if (isInitialized && Time.frameCount % Plugin.Config.UpdateRate == 0)
            {
                if (enabled)
                {
                    colorBlindComponent.enabled = enabled;
                    canvasColorBlindHelper.enabled = enabled;
                }
                else
                {
                    colorBlindComponent.enabled = !enabled;
                    canvasColorBlindHelper.enabled = !enabled;
                }

                ColorBlindHelper.ApplyColorBlindSettings();
                lastUpdateTime = currentTime; // Update the last update time
            }

            if (!isInitialized)
            {
                try
                {
                    var cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
                    var camera = cameraObject.GetComponent<Camera>();

                    var ui = GameObject.FindObjectOfType<UI>(true);

                    // I don't want to canvas manager to be intrusive.
                    if (!canvasColorBlindHelper && ui._canvas.gameObject.GetComponent<CanvasColorBlindHelper>() == null)
                    {
                        canvasColorBlindHelper = ui._canvas.gameObject.AddComponent<CanvasColorBlindHelper>();
                    }

                    if (camera != null)
                    {
                        // Add our color blind helper component to main camera for 3D content
                        if (colorBlindComponent == null)
                        {
                            colorBlindComponent = camera.gameObject.AddComponent<ColorBlindComponent>();
                            //canvasManager = camera.gameObject.AddComponent<CanvasManager>();
                        }

                        // Initialize canvas manager for UI elements (singleton pattern)
                        //_ = CanvasManager.Instance; // Discard operator to show intentional unused assignment

                        // Initialize global image manager for all Image components
                        // _ = ImageColorBlindManager.Instance;

                        isInitialized = true;
                        lastUpdateTime = currentTime; // Initialize the timer
                        Plugin.Logger.Log("ColorBlind Helper initialized with 3D Camera and Canvas URP post-processing");
                    }
                }
                catch (Exception ex)
                {
                    Plugin.Logger.Log($"Error in ColorTuner initialization: {ex.Message}");
                }
            }
        }





        [Hook(ModHookType.DungeonUpdateBeforeGameLoop)]
        public static void DungeonUpdateBeforeGameLoop(IModContext context)
        {
            enabled = Plugin.Config.EnableColorBlindAssist;
            currentTime = Time.time;

            // Calculate time-based updates instead of frame-based
            if (isInitialized && (currentTime - lastUpdateTime) >= updateInterval)
            // Only update if the color blind type has changed or every few frames to avoid performance issues
            // if (isInitialized && Time.frameCount % Plugin.Config.UpdateRate == 0)
            {
                if (enabled)
                {
                    colorBlindComponent.enabled = enabled;
                    canvasColorBlindHelper.enabled = enabled;
                }
                else
                {
                    colorBlindComponent.enabled = !enabled;
                    canvasColorBlindHelper.enabled = !enabled;
                }

                ColorBlindHelper.ApplyColorBlindSettings();
                lastUpdateTime = currentTime; // Update the last update time
            }

            if (!isInitialized)
            {
                try
                {
                    var cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
                    //var dungeonGameMode = GameObject.FindObjectOfType<DungeonGameMode>(true);
                    //var camera = dungeonGameMode._camera.GetComponent<Camera>();
                    var camera = cameraObject.GetComponent<Camera>();

                    var ui = GameObject.FindObjectOfType<UI>(true);

                    // I don't want to canvas manager to be intrusive.
                    if (!canvasColorBlindHelper && ui._canvas.gameObject.GetComponent<CanvasColorBlindHelper>() == null)
                    {
                        canvasColorBlindHelper = ui._canvas.gameObject.AddComponent<CanvasColorBlindHelper>();
                    }

                    if (camera != null)
                    {
                        // Add our color blind helper component to main camera for 3D content
                        if (colorBlindComponent == null)
                        {
                            colorBlindComponent = camera.gameObject.AddComponent<ColorBlindComponent>();
                            //canvasManager = camera.gameObject.AddComponent<CanvasManager>();
                        }

                        // Initialize canvas manager for UI elements (singleton pattern)
                        //_ = CanvasManager.Instance; // Discard operator to show intentional unused assignment

                        // Initialize global image manager for all Image components
                        // _ = ImageColorBlindManager.Instance;

                        isInitialized = true;
                        lastUpdateTime = currentTime; // Initialize the timer
                        Plugin.Logger.Log("ColorBlind Helper initialized with 3D Camera and Canvas URP post-processing");
                    }
                }
                catch (Exception ex)
                {
                    Plugin.Logger.Log($"Error in ColorTuner initialization: {ex.Message}");
                }
            }
        }
    }
}