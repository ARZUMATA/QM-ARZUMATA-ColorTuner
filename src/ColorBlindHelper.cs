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

        //public static ChannelMixer channelMixerdata = new ChannelMixer();
        //public static ColorAdjustments colorAdjustments = new ColorAdjustments();
        public static ChannelMixerData channelMixerData = new ChannelMixerData();
        public static ColorAdjustmentsData colorAdjustmentsData = new ColorAdjustmentsData();
        //public static ColorAdjustments colorAdjustmentsValues;

        public static void ResetChannelMixer()
        {
            // Reset to identity matrix (no change)
            channelMixerData.redOutRedIn = 100f;
            channelMixerData.redOutGreenIn = 0f;
            channelMixerData.redOutBlueIn = 0f;

            channelMixerData.greenOutRedIn = 0f;
            channelMixerData.greenOutGreenIn = 100f;
            channelMixerData.greenOutBlueIn = 0f;

            channelMixerData.blueOutRedIn = 0f;
            channelMixerData.blueOutGreenIn = 0f;
            channelMixerData.blueOutBlueIn = 100f;
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
            channelMixerData.redOutRedIn = 56.667f;
            channelMixerData.redOutGreenIn = 43.333f;
            channelMixerData.redOutBlueIn = 0f;

            channelMixerData.greenOutRedIn = 55.833f;
            channelMixerData.greenOutGreenIn = 44.167f;
            channelMixerData.greenOutBlueIn = 0f;

            channelMixerData.blueOutRedIn = 0f;
            channelMixerData.blueOutGreenIn = 24.167f;
            channelMixerData.blueOutBlueIn = 75.833f;
        }

        public static void ApplyDeuteranopiaCorrection()
        {
            // Deuteranopia (Green-blind) - Missing M-cones
            // Transform matrix to simulate normal vision for deuteranopes
            channelMixerData.redOutRedIn = 62.5f;
            channelMixerData.redOutGreenIn = 37.5f;
            channelMixerData.redOutBlueIn = 0f;

            channelMixerData.greenOutRedIn = 70f;
            channelMixerData.greenOutGreenIn = 30f;
            channelMixerData.greenOutBlueIn = 0f;

            channelMixerData.blueOutRedIn = 0f;
            channelMixerData.blueOutGreenIn = 30f;
            channelMixerData.blueOutBlueIn = 70f;
        }

        public static void ApplyTritanopiaCorrection()
        {
            // Tritanopia (Blue-blind) - Missing S-cones
            // Transform matrix to simulate normal vision for tritanopes
            channelMixerData.redOutRedIn = 95f;
            channelMixerData.redOutGreenIn = 5f;
            channelMixerData.redOutBlueIn = 0f;

            channelMixerData.greenOutRedIn = 0f;
            channelMixerData.greenOutGreenIn = 43.333f;
            channelMixerData.greenOutBlueIn = 56.667f;

            channelMixerData.blueOutRedIn = 0f;
            channelMixerData.blueOutGreenIn = 47.5f;
            channelMixerData.blueOutBlueIn = 52.5f;
        }

        public static void ApplyProtanomalyCorrection()
        {
            // Protanomaly (Weak red perception) - Anomalous L-cones
            // Milder correction than full protanopia
            channelMixerData.redOutRedIn = 80f;
            channelMixerData.redOutGreenIn = 20f;
            channelMixerData.redOutBlueIn = 0f;

            channelMixerData.greenOutRedIn = 25.833f;
            channelMixerData.greenOutGreenIn = 74.167f;
            channelMixerData.greenOutBlueIn = 0f;

            channelMixerData.blueOutRedIn = 0f;
            channelMixerData.blueOutGreenIn = 14.167f;
            channelMixerData.blueOutBlueIn = 85.833f;
        }

        public static void ApplyDeuteranomalyCorrection()
        {
            // Deuteranomaly (Weak green perception) - Anomalous M-cones
            // Most common form of color blindness - milder correction
            channelMixerData.redOutRedIn = 80f;
            channelMixerData.redOutGreenIn = 20f;
            channelMixerData.redOutBlueIn = 0f;

            channelMixerData.greenOutRedIn = 25.833f;
            channelMixerData.greenOutGreenIn = 74.167f;
            channelMixerData.greenOutBlueIn = 0f;

            channelMixerData.blueOutRedIn = 0f;
            channelMixerData.blueOutGreenIn = 14.167f;
            channelMixerData.blueOutBlueIn = 85.833f;
        }

        public static void ApplyTritanomalyCorrection()
        {
            // Tritanomaly (Weak blue perception) - Anomalous S-cones
            // Milder correction than full tritanopia
            channelMixerData.redOutRedIn = 96.667f;
            channelMixerData.redOutGreenIn = 3.333f;
            channelMixerData.redOutBlueIn = 0f;

            channelMixerData.greenOutRedIn = 0f;
            channelMixerData.greenOutGreenIn = 73.333f;
            channelMixerData.greenOutBlueIn = 26.667f;

            channelMixerData.blueOutRedIn = 0f;
            channelMixerData.blueOutGreenIn = 18.333f;
            channelMixerData.blueOutBlueIn = 81.667f;
        }

        public static void ApplyMonochromacyCorrection()
        {
            // Monochromacy (Complete color blindness) - Convert to enhanced grayscale
            // Use luminance weights but enhance contrast to help distinguish elements
            float redLuma = 29.9f;   // Standard luminance weights
            float greenLuma = 58.7f;
            float blueLuma = 11.4f;

            // Apply same luminance calculation to all channels
            channelMixerData.redOutRedIn = redLuma;
            channelMixerData.redOutGreenIn = greenLuma;
            channelMixerData.redOutBlueIn = blueLuma;

            channelMixerData.greenOutRedIn = redLuma;
            channelMixerData.greenOutGreenIn = greenLuma;
            channelMixerData.greenOutBlueIn = blueLuma;

            channelMixerData.blueOutRedIn = redLuma;
            channelMixerData.blueOutGreenIn = greenLuma;
            channelMixerData.blueOutBlueIn = blueLuma;

            // Also boost contrast for monochromacy to help distinguish elements
            colorAdjustmentsData.contrast += 25f; // Extra contrast boost
            colorAdjustmentsData.contrastOverrideState = true;
        }

        public static void ApplyCustomCorrection(ModConfig config)
        {
            // Use custom multipliers and mixing from config
            channelMixerData.redOutRedIn = config.RedOutRedIn;
            channelMixerData.redOutGreenIn = config.RedOutGreenIn;
            channelMixerData.redOutBlueIn = config.RedOutBlueIn;

            channelMixerData.greenOutRedIn = config.GreenOutRedIn;
            channelMixerData.greenOutGreenIn = config.GreenOutGreenIn;
            channelMixerData.greenOutBlueIn = config.GreenOutBlueIn;

            channelMixerData.blueOutRedIn = config.BlueOutRedIn;
            channelMixerData.blueOutGreenIn = config.BlueOutGreenIn;
            channelMixerData.blueOutBlueIn = config.BlueOutBlueIn;
        }

        public static void EnableAllChannelMixerOverrides()
        {
            channelMixerData.redOutRedInOverrideState = true;
            channelMixerData.redOutGreenInOverrideState = true;
            channelMixerData.redOutBlueInOverrideState = true;

            channelMixerData.greenOutRedInOverrideState = true;
            channelMixerData.greenOutGreenInOverrideState = true;
            channelMixerData.greenOutBlueInOverrideState = true;

            channelMixerData.blueOutRedInOverrideState = true;
            channelMixerData.blueOutGreenInOverrideState = true;
            channelMixerData.blueOutBlueInOverrideState = true;
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
            colorAdjustmentsData.contrast = Plugin.Config.Contrast;
            colorAdjustmentsData.saturation = Plugin.Config.Saturation;
            colorAdjustmentsData.hueShift = Plugin.Config.HueShift;
            colorAdjustmentsData.postExposure = Plugin.Config.Exposure;

            // Enable the adjustments
            colorAdjustmentsData.contrastOverrideState = true;
            colorAdjustmentsData.saturationOverrideState = true;
            colorAdjustmentsData.hueShiftOverrideState = true;
            colorAdjustmentsData.postExposureOverrideState = true;

            // Apply color blind specific correction
            ApplyColorBlindCorrection((ColorBlindType)Plugin.Config.ColorBlindIndex);
        }
    }

    public class ColorAdjustmentsData
    {
        public float contrast;
        public float saturation;
        public float hueShift;
        public float postExposure;
        public bool contrastOverrideState;
        public bool saturationOverrideState;
        public bool hueShiftOverrideState;
        public bool postExposureOverrideState;

        public bool AreEqual(ColorAdjustmentsData a, ColorAdjustments b)
        {
            return (
                contrast == b.contrast.value &&
                saturation == b.saturation.value &&
                hueShift == b.hueShift.value &&
                postExposure == b.postExposure.value &&

                contrastOverrideState == b.contrast.overrideState &&
                saturationOverrideState == b.saturation.overrideState &&
                hueShiftOverrideState == b.hueShift.overrideState &&
                postExposureOverrideState == b.postExposure.overrideState);
        }

        public void CopyFrom(ColorAdjustmentsData source, ColorAdjustments destination)
        {
            destination.contrast.value = source.contrast;
            destination.saturation.value = source.saturation;
            destination.hueShift.value = source.hueShift;
            destination.postExposure.value = source.postExposure;

            destination.contrast.overrideState = source.contrastOverrideState;
            destination.saturation.overrideState = source.saturationOverrideState;
            destination.hueShift.overrideState = source.hueShiftOverrideState;
            destination.postExposure.overrideState = source.postExposureOverrideState;
        }
    }

    public class ChannelMixerData
    {
        public float redOutRedIn;
        public float redOutGreenIn;
        public float redOutBlueIn;

        public float greenOutRedIn;
        public float greenOutGreenIn;
        public float greenOutBlueIn;

        public float blueOutRedIn;
        public float blueOutGreenIn;
        public float blueOutBlueIn;

        public bool redOutRedInOverrideState;
        public bool redOutGreenInOverrideState;
        public bool redOutBlueInOverrideState;
        public bool greenOutRedInOverrideState;
        public bool greenOutGreenInOverrideState;
        public bool greenOutBlueInOverrideState;
        public bool blueOutRedInOverrideState;
        public bool blueOutGreenInOverrideState;
        public bool blueOutBlueInOverrideState;

        public bool AreEqual(ChannelMixerData a, ChannelMixer b)
        {
            return (a.redOutRedIn == b.redOutRedIn.value &&
                    a.redOutGreenIn == b.redOutGreenIn.value &&
                    a.redOutBlueIn == b.redOutBlueIn.value &&

                    a.greenOutRedIn == b.greenOutRedIn.value &&
                    a.greenOutGreenIn == b.greenOutGreenIn.value &&
                    a.greenOutBlueIn == b.greenOutBlueIn.value &&

                    a.blueOutRedIn == b.blueOutRedIn.value &&
                    a.blueOutGreenIn == b.blueOutGreenIn.value &&
                    a.blueOutBlueIn == b.blueOutBlueIn.value &&

                    a.redOutRedInOverrideState == b.redOutRedIn.overrideState &&
                    a.redOutGreenInOverrideState == b.redOutGreenIn.overrideState &&
                    a.redOutBlueInOverrideState == b.redOutBlueIn.overrideState &&

                    a.greenOutRedInOverrideState == b.greenOutRedIn.overrideState &&
                    a.greenOutGreenInOverrideState == b.greenOutGreenIn.overrideState &&
                    a.greenOutBlueInOverrideState == b.greenOutBlueIn.overrideState &&

                    a.blueOutRedInOverrideState == b.blueOutRedIn.overrideState &&
                    a.blueOutGreenInOverrideState == b.blueOutGreenIn.overrideState &&
                    a.blueOutBlueInOverrideState == b.blueOutBlueIn.overrideState
                    );
        }

        public bool AreEqual(ChannelMixerData a, ChannelMixerData b)
        {
            return (a.redOutRedIn == b.redOutRedIn &&
                    a.redOutGreenIn == b.redOutGreenIn &&
                    a.redOutBlueIn == b.redOutBlueIn &&

                    a.greenOutRedIn == b.greenOutRedIn &&
                    a.greenOutGreenIn == b.greenOutGreenIn &&
                    a.greenOutBlueIn == b.greenOutBlueIn &&

                    a.blueOutRedIn == b.blueOutRedIn &&
                    a.blueOutGreenIn == b.blueOutGreenIn &&
                    a.blueOutBlueIn == b.blueOutBlueIn &&

                    a.redOutRedInOverrideState == b.redOutRedInOverrideState &&
                    a.redOutGreenInOverrideState == b.redOutGreenInOverrideState &&
                    a.redOutBlueInOverrideState == b.redOutBlueInOverrideState &&

                    a.greenOutRedInOverrideState == b.greenOutRedInOverrideState &&
                    a.greenOutGreenInOverrideState == b.greenOutGreenInOverrideState &&
                    a.greenOutBlueInOverrideState == b.greenOutBlueInOverrideState &&

                    a.blueOutRedInOverrideState == b.blueOutRedInOverrideState &&
                    a.blueOutGreenInOverrideState == b.blueOutGreenInOverrideState &&
                    a.blueOutBlueInOverrideState == b.blueOutBlueInOverrideState
                    );
        }

        public void CopyFrom(ChannelMixerData source, ChannelMixer destination)
        {
            destination.redOutRedIn.value = source.redOutRedIn;
            destination.redOutGreenIn.value = source.redOutGreenIn;
            destination.redOutBlueIn.value = source.redOutBlueIn;

            destination.greenOutRedIn.value = source.greenOutRedIn;
            destination.greenOutGreenIn.value = source.greenOutGreenIn;
            destination.greenOutBlueIn.value = source.greenOutBlueIn;

            destination.blueOutRedIn.value = source.blueOutRedIn;
            destination.blueOutGreenIn.value = source.blueOutGreenIn;
            destination.blueOutBlueIn.value = source.blueOutBlueIn;

            destination.redOutRedIn.overrideState = source.redOutRedInOverrideState;
            destination.redOutGreenIn.overrideState = source.redOutGreenInOverrideState;
            destination.redOutBlueIn.overrideState = source.redOutBlueInOverrideState;

            destination.greenOutRedIn.overrideState = source.greenOutRedInOverrideState;
            destination.greenOutGreenIn.overrideState = source.greenOutGreenInOverrideState;
            destination.greenOutBlueIn.overrideState = source.greenOutBlueInOverrideState;

            destination.blueOutRedIn.overrideState = source.blueOutRedInOverrideState;
            destination.blueOutGreenIn.overrideState = source.blueOutGreenInOverrideState;
            destination.blueOutBlueIn.overrideState = source.blueOutBlueInOverrideState;
        }

        public void CopyFrom(ChannelMixerData source, ChannelMixerData destination)
        {
            destination.redOutRedIn = source.redOutRedIn;
            destination.redOutGreenIn = source.redOutGreenIn;
            destination.redOutBlueIn = source.redOutBlueIn;

            destination.greenOutRedIn = source.greenOutRedIn;
            destination.greenOutGreenIn = source.greenOutGreenIn;
            destination.greenOutBlueIn = source.greenOutBlueIn;

            destination.blueOutRedIn = source.blueOutRedIn;
            destination.blueOutGreenIn = source.blueOutGreenIn;
            destination.blueOutBlueIn = source.blueOutBlueIn;

            destination.redOutRedInOverrideState = source.redOutRedInOverrideState;
            destination.redOutGreenInOverrideState = source.redOutGreenInOverrideState;
            destination.redOutBlueInOverrideState = source.redOutBlueInOverrideState;

            destination.greenOutRedInOverrideState = source.greenOutRedInOverrideState;
            destination.greenOutGreenInOverrideState = source.greenOutGreenInOverrideState;
            destination.greenOutBlueInOverrideState = source.greenOutBlueInOverrideState;

            destination.blueOutRedInOverrideState = source.blueOutRedInOverrideState;
            destination.blueOutGreenInOverrideState = source.blueOutGreenInOverrideState;
            destination.blueOutBlueInOverrideState = source.blueOutBlueInOverrideState;
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
