using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.Rendering.Universal;

namespace QM_ColorBlindHelper
{
    public class CanvasColorBlindHelper : MonoBehaviour
    {
        private Canvas targetCanvas;
        private Dictionary<string, SpriteContainer> spriteTextureCache = new Dictionary<string, SpriteContainer>();
        private Dictionary<int, ImageContainer> containers = new Dictionary<int, ImageContainer>();
        public bool needUpdate = false;
        public ChannelMixer channelMixer = new ChannelMixer();
        private static float lastUpdateTime = 0f;

        internal class ImageContainer
        {
            internal int instanceId;
            internal Graphic graphic;
            internal Sprite sprite;
            internal bool exists = false;

            internal ImageContainer(int instanceId, Graphic graphic, bool exists = true)
            {
                this.instanceId = instanceId;
                this.graphic = graphic;
                this.sprite = (graphic as Image)?.sprite;
                this.exists = exists;
            }
        }

        internal class SpriteContainer
        {
            internal Sprite oldSprite;
            internal Sprite newSprite;
            internal SpriteContainer(Sprite oldSprite, Sprite newSprite)
            {
                this.oldSprite = oldSprite;
                this.newSprite = newSprite;
            }
        }

        void Start()
        {
            targetCanvas = GetComponent<Canvas>();
            StoreOriginalColors();
        }

        void Update()
        {
            if (enabled)
            {
                if (!ColorBlindHelper.AreEqual(channelMixer, ColorBlindHelper.channelMixer))
                {
                    ColorBlindHelper.CopyFrom(ColorBlindHelper.channelMixer, channelMixer);
                    needUpdate = true;
                }

                if (needUpdate)
                {
                    // Clear sprite texture cache
                    spriteTextureCache.Clear();
                    needUpdate = false;
                }

                if ((ColorTuner.currentTime - lastUpdateTime) >= ColorTuner.updateInterval)
                {
                    StoreOriginalColors();
                    ApplyColorBlindCorrection();
                }
            }
            else
            {
                // Clear sprite texture cache
                spriteTextureCache.Clear();
                containers.Clear();
            }
        }

        void StoreOriginalColors()
        {
            Graphic[] graphics = targetCanvas.GetComponentsInChildren<Graphic>(true);
            containers.Clear();

            // Mark all as potentially dead
            foreach (var kvp in containers)
            {
                kvp.Value.exists = false;
            }

            // Process current images
            for (int i = 0; i < graphics.Length; i++)
            {
                var graphic = graphics[i];
                var graphicId = graphic.GetInstanceID();
                containers[graphicId] = new ImageContainer(graphicId, graphic, true);
            }

            //Plugin.Logger.Log($"Stored {containers.Count} UI element colors for canvas: {targetCanvas.name}");
        }

        void ApplyColorBlindCorrection()
        {
            foreach (var imgcont in containers)
            {
                // Handle Image components with sprites
                if (imgcont.Value.graphic is Image image && imgcont.Value.sprite != null && imgcont.Value.sprite.name.Length > 1)
                {
                    ApplyImageCorrection(image, imgcont.Value.sprite, imgcont.Value);
                }
                else
                {
                    // Handle other UI elements (Text, etc.)
                    Color originalColor = imgcont.Value.graphic.color;
                    Color correctedColor = ApplyChannelMixerTransform(originalColor);
                    imgcont.Value.graphic.color = correctedColor;
                }
            }
        }

        void ApplyImageCorrection(Image image, Sprite originalSprite, ImageContainer imgcont)
        {
            // Create or reuse processed processedTexture
            if (spriteTextureCache.TryGetValue(originalSprite.name, out var spriteCont))
            {
                image.sprite = spriteCont.newSprite;
                return;
            }

            Texture2D newTexture = ProcessSpriteTexture(originalSprite);

            // Ensure the wrap mode is set to Repeat to avoid tiling limits
            if (newTexture != null)
            {
                newTexture.wrapMode = TextureWrapMode.Repeat;
            }
            else
            {
                Plugin.Logger.Log("Failed to process sprite texture for: " + originalSprite.name);
                return;
            }

            // Create sprite with exact same properties to maintain pixel-perfect rendering
            Sprite newSprite = Sprite.Create(
            newTexture,
            originalSprite.rect,
            originalSprite.pivot / originalSprite.rect.size, // Normalized pivot
            originalSprite.pixelsPerUnit,
            0, // No extrude
            SpriteMeshType.FullRect,
            originalSprite.border,
            false // Don't generate fallback physics shape
            );

            image.sprite = newSprite;

            spriteTextureCache.Add(originalSprite.name, new SpriteContainer(originalSprite, newSprite));
        }

        Texture2D ProcessSpriteTexture(Sprite originalSprite)
        {
            // Make processedTexture readable if it isn't
            Texture2D readableTexture = CreateReadableTexture(originalSprite.texture);

            Color[] pixels = readableTexture.GetPixels();
            Color[] processedPixels = new Color[pixels.Length];

            // Process each pixel
            for (int i = 0; i < pixels.Length; i++)
            {
                processedPixels[i] = ApplyChannelMixerTransform(pixels[i]);
            }

            // Create new processedTexture for processed result
            Texture2D processedTexture = new Texture2D(readableTexture.width, readableTexture.height, TextureFormat.RGBA32, false);

            // Set pixel-perfect filtering
            processedTexture.filterMode = FilterMode.Point; // No smoothing/bilinear filtering
            processedTexture.wrapMode = TextureWrapMode.Clamp;
            // newTexture.anisoLevel = 0; // Disable anisotropic filtering

            processedTexture.SetPixels(processedPixels);
            processedTexture.Apply();

            return processedTexture;
        }

        private static Texture2D CreateReadableTexture(Texture2D original)
        {
            // Create a temporary RenderTexture with no anti-aliasing and exact dimensions
            RenderTexture tmp = RenderTexture.GetTemporary(
                original.width,
                original.height,
                0,
                RenderTextureFormat.ARGB32,
                RenderTextureReadWrite.Default,
                1 // No anti-aliasing
            );

            // Set pixel-perfect settings for RenderTexture
            tmp.filterMode = FilterMode.Point;
            tmp.wrapMode = TextureWrapMode.Clamp;

            // Blit with pixel-perfect material (no smoothing)
            RenderTexture.active = tmp;
            GL.Clear(true, true, Color.clear);

            // Use point filtering for the blit operation
            Graphics.Blit(original, tmp);

            // Create a new readable Texture2D with pixel-perfect settings
            Texture2D readable = new Texture2D(original.width, original.height, TextureFormat.RGBA32, false);
            readable.filterMode = FilterMode.Point;
            readable.wrapMode = TextureWrapMode.Clamp;
            readable.anisoLevel = 0;

            // Read pixels at exact coordinates (pixel-perfect)
            readable.ReadPixels(new Rect(0, 0, original.width, original.height), 0, 0, false);
            readable.Apply(false); // Don't generate mipmaps

            // Clean up
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(tmp);

            return readable;
        }

        Color ApplyChannelMixerTransform(Color originalColor)
        {
            // Get the channel mixer values for the specific color blind type
            Vector3 rgb = new Vector3(originalColor.r, originalColor.g, originalColor.b);

            // Apply channel mixer transformation (same as URP ChannelMixer)
            // Each output channel is a combination of all input channels
            Vector3 transformedRgb = new Vector3(
                // Red output = (red input * redOutRedIn) + (green input * redOutGreenIn) + (blue input * redOutBlueIn)
                (rgb.x * (float)channelMixer.redOutRedIn / 100f) + (rgb.y * (float)channelMixer.redOutGreenIn / 100f) + (rgb.z * (float)channelMixer.redOutBlueIn / 100f),

                // Green output = (red input * greenOutRedIn) + (green input * greenOutGreenIn) + (blue input * greenOutBlueIn)
                (rgb.x * (float)channelMixer.greenOutRedIn / 100f) + (rgb.y * (float)channelMixer.greenOutGreenIn / 100f) + (rgb.z * (float)channelMixer.greenOutBlueIn / 100f),

                // Blue output = (red input * blueOutRedIn) + (green input * blueOutGreenIn) + (blue input * blueOutBlueIn)
                (rgb.x * (float)channelMixer.blueOutRedIn / 100f) + (rgb.y * (float)channelMixer.blueOutGreenIn / 100f) + (rgb.z * (float)channelMixer.blueOutBlueIn / 100f)
            );

            return new Color(
                Mathf.Clamp01(transformedRgb.x),
                Mathf.Clamp01(transformedRgb.y),
                Mathf.Clamp01(transformedRgb.z),
                originalColor.a
            );
        }

        // It doesn't get destroyed. Just leaving for info.
        void OnDestroy()
        {
        }
    }
}
