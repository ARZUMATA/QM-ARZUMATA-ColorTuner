﻿using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace QM_ColorBlindHelper
{
    public class ColorBlindComponent : MonoBehaviour
    {
        private Camera targetCamera;
        private Volume postProcessVolume;
        private VolumeProfile profile;
        private bool setupComplete = false;
        public ChannelMixer channelMixer;
        public ColorAdjustments colorAdjustments;
        private static float lastUpdateTime = 0f;

        void Start()
        {
            targetCamera = GetComponent<Camera>();
            Plugin.Logger.Log("ColorBlindComponent Start");
            SetupPostProcessing();
        }

        void SetupPostProcessing()
        {
            try
            {
                Plugin.Logger.Log("Setting up post processing...");

                // Check if camera has URP additional camera data
                var cameraData = targetCamera.GetComponent<UniversalAdditionalCameraData>();

                if (cameraData == null)
                {
                    Plugin.Logger.Log("Adding UniversalAdditionalCameraData to camera");
                    cameraData = targetCamera.gameObject.AddComponent<UniversalAdditionalCameraData>();
                }

                // Enable post-processing on the camera
                cameraData.renderPostProcessing = true;
                Plugin.Logger.Log($"Post-processing enabled on camera: {cameraData.renderPostProcessing}");

                // Create a post-process volume for color adjustments
                GameObject colorBlindVolumeGameObject = new GameObject("ColorBlind Volume");
                // Don't parent it to the camera, make it independent
                // This prevents issues if the camera gets destroyed
                DontDestroyOnLoad(colorBlindVolumeGameObject);
                
                postProcessVolume = colorBlindVolumeGameObject.AddComponent<Volume>();
                postProcessVolume.isGlobal = true;
                postProcessVolume.priority = 10;
                postProcessVolume.weight = 1.0f;

                // Create a volume profile
                profile = ScriptableObject.CreateInstance<VolumeProfile>();
                postProcessVolume.profile = profile;

                // Add effects
                colorAdjustments = profile.Add<ColorAdjustments>();
                channelMixer = profile.Add<ChannelMixer>();

                Plugin.Logger.Log("Post-processing components created");
                setupComplete = true;
            }
            catch (Exception ex)
            {
                Plugin.Logger.Log($"Error in SetupPostProcessing: {ex.Message}");
            }
        }

        void Update()
        {
            if (setupComplete && enabled)
            {
                if ((ColorTuner.currentTime - lastUpdateTime) >= ColorTuner.updateInterval)
                {
                    if (!ColorBlindHelper.channelMixerData.AreEqual(ColorBlindHelper.channelMixerData, channelMixer))
                    {
                        ColorBlindHelper.channelMixerData.CopyFrom(ColorBlindHelper.channelMixerData, channelMixer);
                    }

                    if (!ColorBlindHelper.colorAdjustmentsData.AreEqual(ColorBlindHelper.colorAdjustmentsData, colorAdjustments))
                    {
                        ColorBlindHelper.colorAdjustmentsData.CopyFrom(ColorBlindHelper.colorAdjustmentsData, colorAdjustments);
                    }
                }
            }
        }

        void OnDestroy()
        {
            Plugin.Logger.Log("ColorBlindComponent OnDestroy");

            if (postProcessVolume != null)
            {
                DestroyImmediate(postProcessVolume.gameObject);
            }

            if (profile != null)
            {
                DestroyImmediate(profile);
            }
        }

        void OnEnable()
        {
            Plugin.Logger.Log("ColorBlindComponent OnEnable");

            // Check if URP is being used
            var renderPipeline = GraphicsSettings.renderPipelineAsset;

            if (renderPipeline != null)
            {
                Plugin.Logger.Log($"Render Pipeline: {renderPipeline.GetType().Name}");
            }
            else
            {
                Plugin.Logger.Log("No render pipeline asset found - using Built-in RP");
            }
        }
    }
}