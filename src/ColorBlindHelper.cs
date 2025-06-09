using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace QM_ColorBlindHelper
{
    internal class ColorBlindHelper
    {

        public static ChannelMixer channelMixer = new ChannelMixer();
        public static ColorAdjustments colorAdjustments = new ColorAdjustments();
        public static ColorAdjustments colorAdjustmentsValues;

        public static bool AreEqual(ChannelMixer a, ChannelMixer b)
        {
            return (a.redOutRedIn.value == b.redOutRedIn.value &&
                    a.redOutGreenIn.value == b.redOutGreenIn.value &&
                    a.redOutBlueIn.value == b.redOutBlueIn.value &&

                    a.greenOutRedIn.value == b.greenOutRedIn.value &&
                    a.greenOutGreenIn.value == b.greenOutGreenIn.value &&
                    a.greenOutBlueIn.value == b.greenOutBlueIn.value &&

                    a.blueOutRedIn.value == b.blueOutRedIn.value &&
                    a.blueOutGreenIn.value == b.blueOutGreenIn.value &&
                    a.blueOutBlueIn.value == b.blueOutBlueIn.value &&
                    
                    a.redOutRedIn.overrideState == b.redOutRedIn.overrideState &&
                    a.redOutGreenIn.overrideState == b.redOutGreenIn.overrideState &&
                    a.redOutBlueIn.overrideState == b.redOutBlueIn.overrideState &&

                    a.greenOutRedIn.overrideState == b.greenOutRedIn.overrideState &&
                    a.greenOutGreenIn.overrideState == b.greenOutGreenIn.overrideState &&
                    a.greenOutBlueIn.overrideState == b.greenOutBlueIn.overrideState &&
                    
                    a.blueOutRedIn.overrideState == b.blueOutRedIn.overrideState &&
                    a.blueOutGreenIn.overrideState == b.blueOutGreenIn.overrideState &&
                    a.blueOutBlueIn.overrideState == b.blueOutBlueIn.overrideState
                    );
        }

        public static bool AreEqual(ColorAdjustments a, ColorAdjustments b)
        {
            return (a.contrast.value == b.contrast.value &&
                    a.saturation.value == b.saturation.value &&
                    a.hueShift.value == b.hueShift.value &&
                    a.postExposure.value == b.postExposure.value &&

                    a.contrast.overrideState == b.contrast.overrideState &&
                    a.saturation.overrideState == b.saturation.overrideState &&
                    a.hueShift.overrideState == b.hueShift.overrideState &&
                    a.postExposure.overrideState == b.postExposure.overrideState);
        }
        public static void CopyFrom(ChannelMixer source, ChannelMixer destination)
        {
            destination.redOutRedIn.value = source.redOutRedIn.value;
            destination.redOutGreenIn.value = source.redOutGreenIn.value;
            destination.redOutBlueIn.value = source.redOutBlueIn.value;

            destination.greenOutRedIn.value = source.greenOutRedIn.value;
            destination.greenOutGreenIn.value = source.greenOutGreenIn.value;
            destination.greenOutBlueIn.value = source.greenOutBlueIn.value;

            destination.blueOutRedIn.value = source.blueOutRedIn.value;
            destination.blueOutGreenIn.value = source.blueOutGreenIn.value;
            destination.blueOutBlueIn.value = source.blueOutBlueIn.value;

            destination.redOutRedIn.overrideState = source.redOutRedIn.overrideState; 
            destination.redOutGreenIn.overrideState = source.redOutGreenIn.overrideState; 
            destination.redOutBlueIn.overrideState = source.redOutBlueIn.overrideState; 

            destination.greenOutRedIn.overrideState = source.greenOutRedIn.overrideState; 
            destination.greenOutGreenIn.overrideState = source.greenOutGreenIn.overrideState; 
            destination.greenOutBlueIn.overrideState = source.greenOutBlueIn.overrideState; 

            destination.blueOutRedIn.overrideState = source.blueOutRedIn.overrideState; 
            destination.blueOutGreenIn.overrideState = source.blueOutGreenIn.overrideState; 
            destination.blueOutBlueIn.overrideState = source.blueOutBlueIn.overrideState; 
        }

        public static void CopyFrom(ColorAdjustments source, ColorAdjustments destination)
        {
            destination.contrast.value = source.contrast.value;
            destination.saturation.value = source.saturation.value;
            destination.hueShift.value = source.hueShift.value;
            destination.postExposure.value = source.postExposure.value;

            destination.contrast.overrideState = source.contrast.overrideState;
            destination.saturation.overrideState = source.saturation.overrideState;
            destination.hueShift.overrideState = source.hueShift.overrideState;
            destination.postExposure.overrideState = source.postExposure.overrideState;
        }

        public static void ResetChannelMixer()
        {
            // Reset to identity matrix (no change)
            channelMixer.redOutRedIn.value = 100f;
            channelMixer.redOutGreenIn.value = 0f;
            channelMixer.redOutBlueIn.value = 0f;

            channelMixer.greenOutRedIn.value = 0f;
            channelMixer.greenOutGreenIn.value = 100f;
            channelMixer.greenOutBlueIn.value = 0f;

            channelMixer.blueOutRedIn.value = 0f;
            channelMixer.blueOutGreenIn.value = 0f;
            channelMixer.blueOutBlueIn.value = 100f;
        }

        public static void ApplyNormalVision()
        {
            // Normal vision - no adjustments needed
            // Channel mixer remains at identity matrix
            Plugin.Logger.Log("Applied Normal Vision - no color adjustments");
        }

        public static void ApplyProtanopiaCorrection()
        {
            // Protanopia (Red-blind) - Missing L-cones
            // Transform matrix to simulate normal vision for protanopes
            channelMixer.redOutRedIn.value = 56.667f;
            channelMixer.redOutGreenIn.value = 43.333f;
            channelMixer.redOutBlueIn.value = 0f;

            channelMixer.greenOutRedIn.value = 55.833f;
            channelMixer.greenOutGreenIn.value = 44.167f;
            channelMixer.greenOutBlueIn.value = 0f;

            channelMixer.blueOutRedIn.value = 0f;
            channelMixer.blueOutGreenIn.value = 24.167f;
            channelMixer.blueOutBlueIn.value = 75.833f;
        }

        public static void ApplyDeuteranopiaCorrection()
        {
            // Deuteranopia (Green-blind) - Missing M-cones
            // Transform matrix to simulate normal vision for deuteranopes
            channelMixer.redOutRedIn.value = 62.5f;
            channelMixer.redOutGreenIn.value = 37.5f;
            channelMixer.redOutBlueIn.value = 0f;

            channelMixer.greenOutRedIn.value = 70f;
            channelMixer.greenOutGreenIn.value = 30f;
            channelMixer.greenOutBlueIn.value = 0f;

            channelMixer.blueOutRedIn.value = 0f;
            channelMixer.blueOutGreenIn.value = 30f;
            channelMixer.blueOutBlueIn.value = 70f;
        }

        public static void ApplyTritanopiaCorrection()
        {
            // Tritanopia (Blue-blind) - Missing S-cones
            // Transform matrix to simulate normal vision for tritanopes
            channelMixer.redOutRedIn.value = 95f;
            channelMixer.redOutGreenIn.value = 5f;
            channelMixer.redOutBlueIn.value = 0f;

            channelMixer.greenOutRedIn.value = 0f;
            channelMixer.greenOutGreenIn.value = 43.333f;
            channelMixer.greenOutBlueIn.value = 56.667f;

            channelMixer.blueOutRedIn.value = 0f;
            channelMixer.blueOutGreenIn.value = 47.5f;
            channelMixer.blueOutBlueIn.value = 52.5f;
        }

        public static void ApplyProtanomalyCorrection()
        {
            // Protanomaly (Weak red perception) - Anomalous L-cones
            // Milder correction than full protanopia
            channelMixer.redOutRedIn.value = 80f;
            channelMixer.redOutGreenIn.value = 20f;
            channelMixer.redOutBlueIn.value = 0f;

            channelMixer.greenOutRedIn.value = 25.833f;
            channelMixer.greenOutGreenIn.value = 74.167f;
            channelMixer.greenOutBlueIn.value = 0f;

            channelMixer.blueOutRedIn.value = 0f;
            channelMixer.blueOutGreenIn.value = 14.167f;
            channelMixer.blueOutBlueIn.value = 85.833f;
        }

        public static void ApplyDeuteranomalyCorrection()
        {
            // Deuteranomaly (Weak green perception) - Anomalous M-cones
            // Most common form of color blindness - milder correction
            channelMixer.redOutRedIn.value = 80f;
            channelMixer.redOutGreenIn.value = 20f;
            channelMixer.redOutBlueIn.value = 0f;

            channelMixer.greenOutRedIn.value = 25.833f;
            channelMixer.greenOutGreenIn.value = 74.167f;
            channelMixer.greenOutBlueIn.value = 0f;

            channelMixer.blueOutRedIn.value = 0f;
            channelMixer.blueOutGreenIn.value = 14.167f;
            channelMixer.blueOutBlueIn.value = 85.833f;
        }

        public static void ApplyTritanomalyCorrection()
        {
            // Tritanomaly (Weak blue perception) - Anomalous S-cones
            // Milder correction than full tritanopia
            channelMixer.redOutRedIn.value = 96.667f;
            channelMixer.redOutGreenIn.value = 3.333f;
            channelMixer.redOutBlueIn.value = 0f;

            channelMixer.greenOutRedIn.value = 0f;
            channelMixer.greenOutGreenIn.value = 73.333f;
            channelMixer.greenOutBlueIn.value = 26.667f;

            channelMixer.blueOutRedIn.value = 0f;
            channelMixer.blueOutGreenIn.value = 18.333f;
            channelMixer.blueOutBlueIn.value = 81.667f;
        }

        public static void ApplyMonochromacyCorrection()
        {
            // Monochromacy (Complete color blindness) - Convert to enhanced grayscale
            // Use luminance weights but enhance contrast to help distinguish elements
            float redLuma = 29.9f;   // Standard luminance weights
            float greenLuma = 58.7f;
            float blueLuma = 11.4f;

            // Apply same luminance calculation to all channels
            channelMixer.redOutRedIn.value = redLuma;
            channelMixer.redOutGreenIn.value = greenLuma;
            channelMixer.redOutBlueIn.value = blueLuma;

            channelMixer.greenOutRedIn.value = redLuma;
            channelMixer.greenOutGreenIn.value = greenLuma;
            channelMixer.greenOutBlueIn.value = blueLuma;

            channelMixer.blueOutRedIn.value = redLuma;
            channelMixer.blueOutGreenIn.value = greenLuma;
            channelMixer.blueOutBlueIn.value = blueLuma;

            // Also boost contrast for monochromacy to help distinguish elements
            colorAdjustments.contrast.value += 25f; // Extra contrast boost
            colorAdjustments.contrast.overrideState = true;
        }

        public static void ApplyCustomCorrection(ModConfig config)
        {
            // Use custom multipliers and mixing from config
            channelMixer.redOutRedIn.value = config.RedOutRedIn;
            channelMixer.redOutGreenIn.value = config.RedOutGreenIn;
            channelMixer.redOutBlueIn.value = config.RedOutBlueIn;

            channelMixer.greenOutRedIn.value = config.GreenOutRedIn;
            channelMixer.greenOutGreenIn.value = config.GreenOutGreenIn;
            channelMixer.greenOutBlueIn.value = config.GreenOutBlueIn;

            channelMixer.blueOutRedIn.value = config.BlueOutRedIn;
            channelMixer.blueOutGreenIn.value = config.BlueOutGreenIn;
            channelMixer.blueOutBlueIn.value = config.BlueOutBlueIn;
        }

        public static void EnableAllChannelMixerOverrides()
        {
            channelMixer.redOutRedIn.overrideState = true;
            channelMixer.redOutGreenIn.overrideState = true;
            channelMixer.redOutBlueIn.overrideState = true;

            channelMixer.greenOutRedIn.overrideState = true;
            channelMixer.greenOutGreenIn.overrideState = true;
            channelMixer.greenOutBlueIn.overrideState = true;

            channelMixer.blueOutRedIn.overrideState = true;
            channelMixer.blueOutGreenIn.overrideState = true;
            channelMixer.blueOutBlueIn.overrideState = true;
        }

        public static void ApplyColorBlindCorrection(ColorBlindType colorBlindType)
        {
            try
            {
                // Reset all channel mixer values first
                ResetChannelMixer();

                switch (colorBlindType)
                {
                    case ColorBlindType.Normal:
                        ApplyNormalVision();
                        break;
                    case ColorBlindType.Protanopia:
                        ApplyProtanopiaCorrection();
                        break;
                    case ColorBlindType.Deuteranopia:
                        ApplyDeuteranopiaCorrection();
                        break;
                    case ColorBlindType.Tritanopia:
                        ApplyTritanopiaCorrection();
                        break;
                    case ColorBlindType.Protanomaly:
                        ApplyProtanomalyCorrection();
                        break;
                    case ColorBlindType.Deuteranomaly:
                        ApplyDeuteranomalyCorrection();
                        break;
                    case ColorBlindType.Tritanomaly:
                        ApplyTritanomalyCorrection();
                        break;
                    case ColorBlindType.Monochromacy:
                        ApplyMonochromacyCorrection();
                        break;
                    case ColorBlindType.Custom:
                        ApplyCustomCorrection(Plugin.Config);
                        break;
                }

                // Enable all channel mixer overrides
                EnableAllChannelMixerOverrides();

                Plugin.Logger.Log($"Applied {colorBlindType} correction");
            }
            catch (Exception ex)
            {
                Plugin.Logger.Log($"Error in ApplyColorBlindCorrection: {ex.Message}");
            }
        }

        public static void ApplyColorBlindSettings()
        {
            // Apply basic color adjustments
            colorAdjustments.contrast.value = Plugin.Config.Contrast;
            colorAdjustments.saturation.value = Plugin.Config.Saturation;
            colorAdjustments.hueShift.value = Plugin.Config.HueShift;
            colorAdjustments.postExposure.value = Plugin.Config.Exposure;

            // Enable the adjustments
            colorAdjustments.contrast.overrideState = true;
            colorAdjustments.saturation.overrideState = true;
            colorAdjustments.hueShift.overrideState = true;
            colorAdjustments.postExposure.overrideState = true;

            // Apply color blind specific correction
            ApplyColorBlindCorrection((ColorBlindType)Plugin.Config.ColorBlindnessIndex);
        }
    }

    public enum ColorBlindType
    {
        Normal = 0,        // Normal color vision - no adjustments, identity matrix
        Protanopia = 1,    // Red-blind (missing L-cones), strong red-green redistribution
        Deuteranopia = 2,  // Green-blind (missing M-cones), strong green-red redistribution
        Tritanopia = 3,    // Blue-blind (missing S-cones), blue-green redistribution
        Protanomaly = 4,   // Weak red perception (anomalous L-cones), mild red adjustments
        Deuteranomaly = 5, // Weak green perception (anomalous M-cones) - most common form, mild green adjustments (most common)
        Tritanomaly = 6,   // Weak blue perception (anomalous S-cones), mild blue adjustments
        Monochromacy = 7,  // Complete color blindness - sees only in grayscale, converts to enhanced grayscale with extra contrast
        Custom = 8         // User-defined settings, custom multipliers
    }
}
