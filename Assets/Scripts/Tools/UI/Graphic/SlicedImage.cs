using System;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.U2D;

namespace UnityEngine.UI
{
    /// <summary>
    /// Image is a textured element in the UI hierarchy.
    /// </summary>

    [AddComponentMenu("UI/SlicedImage", 11)]
    /// <summary>
    ///   Displays a Sprite inside the UI System.
    /// </summary>
    public class SlicedImage : MaskableGraphic, ISerializationCallbackReceiver, ILayoutElement, ICanvasRaycastFilter {

        static protected Material s_ETC1DefaultUI = null;

        [FormerlySerializedAs("m_Frame")]
        [SerializeField] private Sprite m_Sprite;
        private bool m_SkipLayoutUpdate;
        private bool m_SkipMaterialUpdate;

        /// <summary>
        /// The sprite that is used to render this image.
        /// </summary>
        public Sprite sprite {
            get { return m_Sprite; }
            set {
                if (m_Sprite != null) {
                    if (m_Sprite != value) {
                        m_SkipLayoutUpdate = m_Sprite.rect.size.Equals(value ? value.rect.size : Vector2.zero);
                        m_SkipMaterialUpdate = m_Sprite.texture == (value ? value.texture : null);
                        m_Sprite = value;

                        SetAllDirty();
                        TrackSprite();
                    }
                }
                else if (value != null) {
                    m_SkipLayoutUpdate = value.rect.size == Vector2.zero;
                    m_SkipMaterialUpdate = value.texture == null;
                    m_Sprite = value;

                    SetAllDirty();
                    TrackSprite();
                }
            }
        }


        /// <summary>
        /// Disable all automatic sprite optimizations.
        /// </summary>
        /// <remarks>
        /// When a new Sprite is assigned update optimizations are automatically applied.
        /// </remarks>
        public void DisableSpriteOptimizations() {
            m_SkipLayoutUpdate = false;
            m_SkipMaterialUpdate = false;
        }

        [NonSerialized]
        private Sprite m_OverrideSprite;

        /// <summary>
        /// Set an override sprite to be used for rendering.
        /// </summary>
        public Sprite overrideSprite {
            get { return activeSprite; }
            set {
                if (SetPropertyUtility.SetClass(ref m_OverrideSprite, value)) {
                    SetAllDirty();
                    TrackSprite();
                }
            }
        }

        private Sprite activeSprite { get { return m_OverrideSprite != null ? m_OverrideSprite : sprite; } }

        [SerializeField] private bool m_PreserveAspect = false;
        public bool preserveAspect { get { return m_PreserveAspect; } set { if (SetPropertyUtility.SetStruct(ref m_PreserveAspect, value)) SetVerticesDirty(); } }

        [SerializeField] private bool m_FillCenter = true;

        /// <summary>
        /// Whether or not to render the center of a Tiled or Sliced image.
        /// </summary>
        public bool fillCenter { get { return m_FillCenter; } set { if (SetPropertyUtility.SetStruct(ref m_FillCenter, value)) SetVerticesDirty(); } }

        // Not serialized until we support read-enabled sprites better.
        private float m_AlphaHitTestMinimumThreshold = 0;

        // Whether this is being tracked for Atlas Binding.
        private bool m_Tracked = false;

        [Obsolete("eventAlphaThreshold has been deprecated. Use eventMinimumAlphaThreshold instead (UnityUpgradable) -> alphaHitTestMinimumThreshold")]

        /// <summary>
        /// Obsolete. You should use UI.Image.alphaHitTestMinimumThreshold instead.
        /// The alpha threshold specifies the minimum alpha a pixel must have for the event to considered a "hit" on the Image.
        /// </summary>
        public float eventAlphaThreshold { get { return 1 - alphaHitTestMinimumThreshold; } set { alphaHitTestMinimumThreshold = 1 - value; } }

        /// <summary>
        /// The alpha threshold specifies the minimum alpha a pixel must have for the event to considered a "hit" on the Image.
        /// </summary>
        /// <remarks>
        /// Alpha values less than the threshold will cause raycast events to pass through the Image. An value of 1 would cause only fully opaque pixels to register raycast events on the Image. The alpha tested is retrieved from the image sprite only, while the alpha of the Image [[UI.Graphic.color]] is disregarded.
        ///
        /// alphaHitTestMinimumThreshold defaults to 0; all raycast events inside the Image rectangle are considered a hit. In order for greater than 0 to values to work, the sprite used by the Image must have readable pixels. This can be achieved by enabling Read/Write enabled in the advanced Texture Import Settings for the sprite and disabling atlassing for the sprite.
        /// </remarks>
        /// <example>
        /// <code>
        /// using UnityEngine;
        /// using System.Collections;
        /// using UnityEngine.UI; // Required when Using UI elements.
        ///
        /// public class ExampleClass : MonoBehaviour
        /// {
        ///     public Image theButton;
        ///
        ///     // Use this for initialization
        ///     void Start()
        ///     {
        ///         theButton.alphaHitTestMinimumThreshold = 0.5f;
        ///     }
        /// }
        /// </code>
        /// </example>
        public float alphaHitTestMinimumThreshold { get { return m_AlphaHitTestMinimumThreshold; } set { m_AlphaHitTestMinimumThreshold = value; } }

        /// Controls whether or not to use the generated mesh from the sprite importer.
        [SerializeField] private bool m_UseSpriteMesh;

        /// <summary>
        /// Allows you to specify whether the UI Image should be displayed using the mesh generated by the TextureImporter, or by a simple quad mesh.
        /// </summary>
        /// <remarks>
        /// When this property is set to false, the UI Image uses a simple quad. When set to true, the UI Image uses the sprite mesh generated by the [[TextureImporter]]. You should set this to true if you want to use a tightly fitted sprite mesh based on the alpha values in your image.
        /// Note: If the texture importer's SpriteMeshType property is set to SpriteMeshType.FullRect, it will only generate a quad, and not a tightly fitted sprite mesh, which means this UI image will be drawn using a quad regardless of the value of this property. Therefore, when enabling this property to use a tightly fitted sprite mesh, you must also ensure the texture importer's SpriteMeshType property is set to Tight.
        /// </remarks>
        public bool useSpriteMesh { get { return m_UseSpriteMesh; } set { if (SetPropertyUtility.SetStruct(ref m_UseSpriteMesh, value)) SetVerticesDirty(); } }


        protected SlicedImage() {
            useLegacyMeshGeneration = false;
        }

        /// <summary>
        /// Cache of the default Canvas Ericsson Texture Compression 1 (ETC1) and alpha Material.
        /// </summary>
        /// <remarks>
        /// Stores the ETC1 supported Canvas Material that is returned from GetETC1SupportedCanvasMaterial().
        /// Note: Always specify the UI/DefaultETC1 Shader in the Always Included Shader list, to use the ETC1 and alpha Material.
        /// </remarks>
        static public Material defaultETC1GraphicMaterial {
            get {
                if (s_ETC1DefaultUI == null)
                    s_ETC1DefaultUI = Canvas.GetETC1SupportedCanvasMaterial();
                return s_ETC1DefaultUI;
            }
        }

        /// <summary>
        /// Image's texture comes from the UnityEngine.Image.
        /// </summary>
        public override Texture mainTexture {
            get {
                if (activeSprite == null) {
                    if (material != null && material.mainTexture != null) {
                        return material.mainTexture;
                    }
                    return s_WhiteTexture;
                }

                return activeSprite.texture;
            }
        }

        /// <summary>
        /// Whether the Sprite of the image has a border to work with.
        /// </summary>

        public bool hasBorder {
            get {
                if (activeSprite != null) {
                    Vector4 v = activeSprite.border;
                    return v.sqrMagnitude > 0f;
                }
                return false;
            }
        }

        public bool UseSpritePixelPerUnit = false;
        public bool CompensateSpriteSize = true;
        public int PixelsPerUnit = 100;
        // case 1066689 cache referencePixelsPerUnit when canvas parent is disabled;
        private float m_CachedReferencePixelsPerUnit = 100;

        public float pixelsPerUnit {
            get {
                float spritePixelsPerUnit = PixelsPerUnit;
                if (activeSprite && UseSpritePixelPerUnit)
                    spritePixelsPerUnit = activeSprite.pixelsPerUnit;
                if (CompensateSpriteSize)
                    spritePixelsPerUnit *= activeSprite.pixelsPerUnit / 37f;

                if (canvas)
                    m_CachedReferencePixelsPerUnit = canvas.referencePixelsPerUnit;

                return spritePixelsPerUnit / m_CachedReferencePixelsPerUnit;
            }
        }

        /// <summary>
        /// The specified Material used by this Image. The default Material is used instead if one wasn't specified.
        /// </summary>
        public override Material material {
            get {
                if (m_Material != null)
                    return m_Material;
#if UNITY_EDITOR
                if (Application.isPlaying && activeSprite && activeSprite.associatedAlphaSplitTexture != null)
                    return defaultETC1GraphicMaterial;
#else

                if (activeSprite && activeSprite.associatedAlphaSplitTexture != null)
                    return defaultETC1GraphicMaterial;
#endif

                return defaultMaterial;
            }

            set {
                base.material = value;
            }
        }

        /// <summary>
        /// See ISerializationCallbackReceiver.
        /// </summary>
        public virtual void OnBeforeSerialize() { }

        /// <summary>
        /// See ISerializationCallbackReceiver.
        /// </summary>
        public virtual void OnAfterDeserialize() { }

        /// Image's dimensions used for drawing. X = left, Y = bottom, Z = right, W = top.
        private Vector4 GetDrawingDimensions(bool shouldPreserveAspect) {
            var padding = activeSprite == null ? Vector4.zero : Sprites.DataUtility.GetPadding(activeSprite);
            var size = activeSprite == null ? Vector2.zero : new Vector2(activeSprite.rect.width, activeSprite.rect.height);

            Rect r = GetPixelAdjustedRect();
            // Debug.Log(string.Format("r:{2}, size:{0}, padding:{1}", size, padding, r));

            int spriteW = Mathf.RoundToInt(size.x);
            int spriteH = Mathf.RoundToInt(size.y);

            var v = new Vector4(
                padding.x / spriteW,
                padding.y / spriteH,
                (spriteW - padding.z) / spriteW,
                (spriteH - padding.w) / spriteH);

            v = new Vector4(
                r.x + r.width * v.x,
                r.y + r.height * v.y,
                r.x + r.width * v.z,
                r.y + r.height * v.w
            );

            return v;
        }

        /// <summary>
        /// Adjusts the image size to make it pixel-perfect.
        /// </summary>
        /// <remarks>
        /// This means setting the Images RectTransform.sizeDelta to be equal to the Sprite dimensions.
        /// </remarks>
        public override void SetNativeSize() {
            if (activeSprite != null) {
                float w = activeSprite.rect.width / pixelsPerUnit;
                float h = activeSprite.rect.height / pixelsPerUnit;
                rectTransform.anchorMax = rectTransform.anchorMin;
                rectTransform.sizeDelta = new Vector2(w, h);
                SetAllDirty();
            }
        }

        /// <summary>
        /// Update the UI renderer mesh.
        /// </summary>
        protected override void OnPopulateMesh(VertexHelper toFill) {
            if (activeSprite == null) {
                base.OnPopulateMesh(toFill);
                return;
            }

            GenerateSlicedSprite(toFill);
        }

        private void TrackSprite() {
            if (activeSprite != null && activeSprite.texture == null) {
                TrackImage(this);
                m_Tracked = true;
            }
        }

        protected override void OnEnable() {
            base.OnEnable();
            TrackSprite();
        }

        protected override void OnDisable() {
            base.OnDisable();

            if (m_Tracked)
                UnTrackImage(this);
        }

        /// <summary>
        /// Update the renderer's material.
        /// </summary>

        protected override void UpdateMaterial() {
            base.UpdateMaterial();

            // check if this sprite has an associated alpha texture (generated when splitting RGBA = RGB + A as two textures without alpha)

            if (activeSprite == null) {
                canvasRenderer.SetAlphaTexture(null);
                return;
            }

            Texture2D alphaTex = activeSprite.associatedAlphaSplitTexture;

            if (alphaTex != null) {
                canvasRenderer.SetAlphaTexture(alphaTex);
            }
        }

        protected override void OnCanvasHierarchyChanged() {
            base.OnCanvasHierarchyChanged();
            if (canvas == null) {
                m_CachedReferencePixelsPerUnit = 100;
            }
            else if (canvas.referencePixelsPerUnit != m_CachedReferencePixelsPerUnit) {
                m_CachedReferencePixelsPerUnit = canvas.referencePixelsPerUnit;

                SetVerticesDirty();
                SetLayoutDirty();
            }
        }

        /// <summary>
        /// Generate vertices for a simple Image.
        /// </summary>
        void GenerateSimpleSprite(VertexHelper vh, bool lPreserveAspect) {
            Vector4 v = GetDrawingDimensions(lPreserveAspect);
            var uv = (activeSprite != null) ? Sprites.DataUtility.GetOuterUV(activeSprite) : Vector4.zero;

            var color32 = color;
            vh.Clear();
            vh.AddVert(new Vector3(v.x, v.y), color32, new Vector2(uv.x, uv.y));
            vh.AddVert(new Vector3(v.x, v.w), color32, new Vector2(uv.x, uv.w));
            vh.AddVert(new Vector3(v.z, v.w), color32, new Vector2(uv.z, uv.w));
            vh.AddVert(new Vector3(v.z, v.y), color32, new Vector2(uv.z, uv.y));

            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(2, 3, 0);
        }

        private void GenerateSprite(VertexHelper vh, bool lPreserveAspect) {
            var spriteSize = new Vector2(activeSprite.rect.width, activeSprite.rect.height);

            // Covert sprite pivot into normalized space.
            var spritePivot = activeSprite.pivot / spriteSize;
            var rectPivot = rectTransform.pivot;
            Rect r = GetPixelAdjustedRect();

            var drawingSize = new Vector2(r.width, r.height);
            var spriteBoundSize = activeSprite.bounds.size;

            // Calculate the drawing offset based on the difference between the two pivots.
            var drawOffset = (rectPivot - spritePivot) * drawingSize;

            var color32 = color;
            vh.Clear();

            Vector2[] vertices = activeSprite.vertices;
            Vector2[] uvs = activeSprite.uv;
            for (int i = 0; i < vertices.Length; ++i) {
                vh.AddVert(new Vector3((vertices[i].x / spriteBoundSize.x) * drawingSize.x - drawOffset.x, (vertices[i].y / spriteBoundSize.y) * drawingSize.y - drawOffset.y), color32, new Vector2(uvs[i].x, uvs[i].y));
            }

            UInt16[] triangles = activeSprite.triangles;
            for (int i = 0; i < triangles.Length; i += 3) {
                vh.AddTriangle(triangles[i + 0], triangles[i + 1], triangles[i + 2]);
            }
        }

        /// <summary>
        /// Generate vertices for a 9-sliced Image.
        /// </summary>

        static readonly Vector2[] s_VertScratch = new Vector2[4];
        static readonly Vector2[] s_UVScratch = new Vector2[4];

        private void GenerateSlicedSprite(VertexHelper toFill) {
            if (!hasBorder) {
                GenerateSimpleSprite(toFill, false);
                return;
            }

            Vector4 outer, inner, padding, border;

            if (activeSprite != null) {
                outer = Sprites.DataUtility.GetOuterUV(activeSprite);
                inner = Sprites.DataUtility.GetInnerUV(activeSprite);
                padding = Sprites.DataUtility.GetPadding(activeSprite);
                border = activeSprite.border;
            }
            else {
                outer = Vector4.zero;
                inner = Vector4.zero;
                padding = Vector4.zero;
                border = Vector4.zero;
            }

            Rect rect = GetPixelAdjustedRect();
            Vector4 adjustedBorders = GetAdjustedBorders(border / pixelsPerUnit, rect);
            padding = padding / pixelsPerUnit;

            s_VertScratch[0] = new Vector2(padding.x, padding.y);
            s_VertScratch[3] = new Vector2(rect.width - padding.z, rect.height - padding.w);

            s_VertScratch[1].x = adjustedBorders.x;
            s_VertScratch[1].y = adjustedBorders.y;

            s_VertScratch[2].x = rect.width - adjustedBorders.z;
            s_VertScratch[2].y = rect.height - adjustedBorders.w;

            for (int i = 0; i < 4; ++i) {
                s_VertScratch[i].x += rect.x;
                s_VertScratch[i].y += rect.y;
            }

            s_UVScratch[0] = new Vector2(outer.x, outer.y);
            s_UVScratch[1] = new Vector2(inner.x, inner.y);
            s_UVScratch[2] = new Vector2(inner.z, inner.w);
            s_UVScratch[3] = new Vector2(outer.z, outer.w);

            toFill.Clear();

            for (int x = 0; x < 3; ++x) {
                int x2 = x + 1;

                for (int y = 0; y < 3; ++y) {
                    if (!m_FillCenter && x == 1 && y == 1)
                        continue;

                    int y2 = y + 1;


                    AddQuad(toFill,
                        new Vector2(s_VertScratch[x].x, s_VertScratch[y].y),
                        new Vector2(s_VertScratch[x2].x, s_VertScratch[y2].y),
                        color,
                        new Vector2(s_UVScratch[x].x, s_UVScratch[y].y),
                        new Vector2(s_UVScratch[x2].x, s_UVScratch[y2].y));
                }
            }
        }

        /// <summary>
        /// Generate vertices for a tiled Image.
        /// </summary>

        void GenerateTiledSprite(VertexHelper toFill) {
            Vector4 outer, inner, border;
            Vector2 spriteSize;

            if (activeSprite != null) {
                outer = Sprites.DataUtility.GetOuterUV(activeSprite);
                inner = Sprites.DataUtility.GetInnerUV(activeSprite);
                border = activeSprite.border;
                spriteSize = activeSprite.rect.size;
            }
            else {
                outer = Vector4.zero;
                inner = Vector4.zero;
                border = Vector4.zero;
                spriteSize = Vector2.one * 100;
            }

            Rect rect = GetPixelAdjustedRect();
            float tileWidth = (spriteSize.x - border.x - border.z) / pixelsPerUnit;
            float tileHeight = (spriteSize.y - border.y - border.w) / pixelsPerUnit;
            border = GetAdjustedBorders(border / pixelsPerUnit, rect);

            var uvMin = new Vector2(inner.x, inner.y);
            var uvMax = new Vector2(inner.z, inner.w);

            // Min to max max range for tiled region in coordinates relative to lower left corner.
            float xMin = border.x;
            float xMax = rect.width - border.z;
            float yMin = border.y;
            float yMax = rect.height - border.w;

            toFill.Clear();
            var clipped = uvMax;

            // if either width is zero we cant tile so just assume it was the full width.
            if (tileWidth <= 0)
                tileWidth = xMax - xMin;

            if (tileHeight <= 0)
                tileHeight = yMax - yMin;

            if (activeSprite != null && (hasBorder || activeSprite.packed || activeSprite.texture.wrapMode != TextureWrapMode.Repeat)) {
                // Sprite has border, or is not in repeat mode, or cannot be repeated because of packing.
                // We cannot use texture tiling so we will generate a mesh of quads to tile the texture.

                // Evaluate how many vertices we will generate. Limit this number to something sane,
                // especially since meshes can not have more than 65000 vertices.

                long nTilesW = 0;
                long nTilesH = 0;
                if (m_FillCenter) {
                    nTilesW = (long)Math.Ceiling((xMax - xMin) / tileWidth);
                    nTilesH = (long)Math.Ceiling((yMax - yMin) / tileHeight);

                    double nVertices = 0;
                    if (hasBorder) {
                        nVertices = (nTilesW + 2.0) * (nTilesH + 2.0) * 4.0; // 4 vertices per tile
                    }
                    else {
                        nVertices = nTilesW * nTilesH * 4.0; // 4 vertices per tile
                    }

                    if (nVertices > 65000.0) {
                        Debug.LogError("Too many sprite tiles on Image \"" + name + "\". The tile size will be increased. To remove the limit on the number of tiles, set the Wrap mode to Repeat in the Image Import Settings", this);

                        double maxTiles = 65000.0 / 4.0; // Max number of vertices is 65000; 4 vertices per tile.
                        double imageRatio;
                        if (hasBorder) {
                            imageRatio = (nTilesW + 2.0) / (nTilesH + 2.0);
                        }
                        else {
                            imageRatio = (double)nTilesW / nTilesH;
                        }

                        double targetTilesW = Math.Sqrt(maxTiles / imageRatio);
                        double targetTilesH = targetTilesW * imageRatio;
                        if (hasBorder) {
                            targetTilesW -= 2;
                            targetTilesH -= 2;
                        }

                        nTilesW = (long)Math.Floor(targetTilesW);
                        nTilesH = (long)Math.Floor(targetTilesH);
                        tileWidth = (xMax - xMin) / nTilesW;
                        tileHeight = (yMax - yMin) / nTilesH;
                    }
                }
                else {
                    if (hasBorder) {
                        // Texture on the border is repeated only in one direction.
                        nTilesW = (long)Math.Ceiling((xMax - xMin) / tileWidth);
                        nTilesH = (long)Math.Ceiling((yMax - yMin) / tileHeight);
                        double nVertices = (nTilesH + nTilesW + 2.0 /*corners*/) * 2.0 /*sides*/ * 4.0 /*vertices per tile*/;
                        if (nVertices > 65000.0) {
                            Debug.LogError("Too many sprite tiles on Image \"" + name + "\". The tile size will be increased. To remove the limit on the number of tiles, set the Wrap mode to Repeat in the Image Import Settings", this);

                            double maxTiles = 65000.0 / 4.0; // Max number of vertices is 65000; 4 vertices per tile.
                            double imageRatio = (double)nTilesW / nTilesH;
                            double targetTilesW = (maxTiles - 4 /*corners*/) / (2 * (1.0 + imageRatio));
                            double targetTilesH = targetTilesW * imageRatio;

                            nTilesW = (long)Math.Floor(targetTilesW);
                            nTilesH = (long)Math.Floor(targetTilesH);
                            tileWidth = (xMax - xMin) / nTilesW;
                            tileHeight = (yMax - yMin) / nTilesH;
                        }
                    }
                    else {
                        nTilesH = nTilesW = 0;
                    }
                }

                if (m_FillCenter) {
                    // TODO: we could share vertices between quads. If vertex sharing is implemented. update the computation for the number of vertices accordingly.
                    for (long j = 0; j < nTilesH; j++) {
                        float y1 = yMin + j * tileHeight;
                        float y2 = yMin + (j + 1) * tileHeight;
                        if (y2 > yMax) {
                            clipped.y = uvMin.y + (uvMax.y - uvMin.y) * (yMax - y1) / (y2 - y1);
                            y2 = yMax;
                        }
                        clipped.x = uvMax.x;
                        for (long i = 0; i < nTilesW; i++) {
                            float x1 = xMin + i * tileWidth;
                            float x2 = xMin + (i + 1) * tileWidth;
                            if (x2 > xMax) {
                                clipped.x = uvMin.x + (uvMax.x - uvMin.x) * (xMax - x1) / (x2 - x1);
                                x2 = xMax;
                            }
                            AddQuad(toFill, new Vector2(x1, y1) + rect.position, new Vector2(x2, y2) + rect.position, color, uvMin, clipped);
                        }
                    }
                }
                if (hasBorder) {
                    clipped = uvMax;
                    for (long j = 0; j < nTilesH; j++) {
                        float y1 = yMin + j * tileHeight;
                        float y2 = yMin + (j + 1) * tileHeight;
                        if (y2 > yMax) {
                            clipped.y = uvMin.y + (uvMax.y - uvMin.y) * (yMax - y1) / (y2 - y1);
                            y2 = yMax;
                        }
                        AddQuad(toFill,
                            new Vector2(0, y1) + rect.position,
                            new Vector2(xMin, y2) + rect.position,
                            color,
                            new Vector2(outer.x, uvMin.y),
                            new Vector2(uvMin.x, clipped.y));
                        AddQuad(toFill,
                            new Vector2(xMax, y1) + rect.position,
                            new Vector2(rect.width, y2) + rect.position,
                            color,
                            new Vector2(uvMax.x, uvMin.y),
                            new Vector2(outer.z, clipped.y));
                    }

                    // Bottom and top tiled border
                    clipped = uvMax;
                    for (long i = 0; i < nTilesW; i++) {
                        float x1 = xMin + i * tileWidth;
                        float x2 = xMin + (i + 1) * tileWidth;
                        if (x2 > xMax) {
                            clipped.x = uvMin.x + (uvMax.x - uvMin.x) * (xMax - x1) / (x2 - x1);
                            x2 = xMax;
                        }
                        AddQuad(toFill,
                            new Vector2(x1, 0) + rect.position,
                            new Vector2(x2, yMin) + rect.position,
                            color,
                            new Vector2(uvMin.x, outer.y),
                            new Vector2(clipped.x, uvMin.y));
                        AddQuad(toFill,
                            new Vector2(x1, yMax) + rect.position,
                            new Vector2(x2, rect.height) + rect.position,
                            color,
                            new Vector2(uvMin.x, uvMax.y),
                            new Vector2(clipped.x, outer.w));
                    }

                    // Corners
                    AddQuad(toFill,
                        new Vector2(0, 0) + rect.position,
                        new Vector2(xMin, yMin) + rect.position,
                        color,
                        new Vector2(outer.x, outer.y),
                        new Vector2(uvMin.x, uvMin.y));
                    AddQuad(toFill,
                        new Vector2(xMax, 0) + rect.position,
                        new Vector2(rect.width, yMin) + rect.position,
                        color,
                        new Vector2(uvMax.x, outer.y),
                        new Vector2(outer.z, uvMin.y));
                    AddQuad(toFill,
                        new Vector2(0, yMax) + rect.position,
                        new Vector2(xMin, rect.height) + rect.position,
                        color,
                        new Vector2(outer.x, uvMax.y),
                        new Vector2(uvMin.x, outer.w));
                    AddQuad(toFill,
                        new Vector2(xMax, yMax) + rect.position,
                        new Vector2(rect.width, rect.height) + rect.position,
                        color,
                        new Vector2(uvMax.x, uvMax.y),
                        new Vector2(outer.z, outer.w));
                }
            }
            else {
                // Texture has no border, is in repeat mode and not packed. Use texture tiling.
                Vector2 uvScale = new Vector2((xMax - xMin) / tileWidth, (yMax - yMin) / tileHeight);

                if (m_FillCenter) {
                    AddQuad(toFill, new Vector2(xMin, yMin) + rect.position, new Vector2(xMax, yMax) + rect.position, color, Vector2.Scale(uvMin, uvScale), Vector2.Scale(uvMax, uvScale));
                }
            }
        }

        static void AddQuad(VertexHelper vertexHelper, Vector3[] quadPositions, Color32 color, Vector3[] quadUVs) {
            int startIndex = vertexHelper.currentVertCount;

            for (int i = 0; i < 4; ++i)
                vertexHelper.AddVert(quadPositions[i], color, quadUVs[i]);

            vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vertexHelper.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
        }

        static void AddQuad(VertexHelper vertexHelper, Vector2 posMin, Vector2 posMax, Color32 color, Vector2 uvMin, Vector2 uvMax) {
            int startIndex = vertexHelper.currentVertCount;

            vertexHelper.AddVert(new Vector3(posMin.x, posMin.y, 0), color, new Vector2(uvMin.x, uvMin.y));
            vertexHelper.AddVert(new Vector3(posMin.x, posMax.y, 0), color, new Vector2(uvMin.x, uvMax.y));
            vertexHelper.AddVert(new Vector3(posMax.x, posMax.y, 0), color, new Vector2(uvMax.x, uvMax.y));
            vertexHelper.AddVert(new Vector3(posMax.x, posMin.y, 0), color, new Vector2(uvMax.x, uvMin.y));

            vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vertexHelper.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
        }

        private Vector4 GetAdjustedBorders(Vector4 border, Rect adjustedRect) {
            Rect originalRect = rectTransform.rect;
            Vector4 borderScaleRatios = new Vector2();
            for (int axis = 0; axis <= 1; axis++) {
                float borderScaleRatio;

                // The adjusted rect (adjusted for pixel correctness)
                // may be slightly larger than the original rect.
                // Adjust the border to match the adjustedRect to avoid
                // small gaps between borders (case 833201).
                if (originalRect.size[axis] != 0) {
                    borderScaleRatio = adjustedRect.size[axis] / originalRect.size[axis];
                    borderScaleRatios[axis] = borderScaleRatio;
                    border[axis] *= borderScaleRatio;
                    border[axis + 2] *= borderScaleRatio;
                }

                // If the rect is smaller than the combined borders, then there's not room for the borders at their normal size.
                // In order to avoid artefacts with overlapping borders, we scale the borders down to fit.
                float combinedBorders = border[axis] + border[axis + 2];
                if (adjustedRect.size[axis] < combinedBorders && combinedBorders != 0) {
                    borderScaleRatio = adjustedRect.size[axis] / combinedBorders;
                    borderScaleRatios[axis] = borderScaleRatio;
                }
            }

            //Apply scale ratio
            for (int axis = 0; axis <= 1; axis++) {
                var borderScaleRatio = m_PreserveAspect ? Mathf.Min(borderScaleRatios.x, borderScaleRatios.y) : borderScaleRatios[axis];
                border[axis] *= borderScaleRatio;
                border[axis + 2] *= borderScaleRatio;
            }
            return border;
        }

        /// <summary>
        /// See ILayoutElement.CalculateLayoutInputHorizontal.
        /// </summary>
        public virtual void CalculateLayoutInputHorizontal() { }

        /// <summary>
        /// See ILayoutElement.CalculateLayoutInputVertical.
        /// </summary>
        public virtual void CalculateLayoutInputVertical() { }

        /// <summary>
        /// See ILayoutElement.minWidth.
        /// </summary>
        public virtual float minWidth { get { return 0; } }

        /// <summary>
        /// If there is a sprite being rendered returns the size of that sprite.
        /// In the case of a slided or tiled sprite will return the calculated minimum size possible
        /// </summary>
        public virtual float preferredWidth {
            get {
                if (activeSprite == null)
                    return 0;
                return Sprites.DataUtility.GetMinSize(activeSprite).x / pixelsPerUnit;
            }
        }

        /// <summary>
        /// See ILayoutElement.flexibleWidth.
        /// </summary>
        public virtual float flexibleWidth { get { return -1; } }

        /// <summary>
        /// See ILayoutElement.minHeight.
        /// </summary>
        public virtual float minHeight { get { return 0; } }

        /// <summary>
        /// If there is a sprite being rendered returns the size of that sprite.
        /// In the case of a slided or tiled sprite will return the calculated minimum size possible
        /// </summary>
        public virtual float preferredHeight {
            get {
                if (activeSprite == null)
                    return 0;
                return Sprites.DataUtility.GetMinSize(activeSprite).y / pixelsPerUnit;
            }
        }

        /// <summary>
        /// See ILayoutElement.flexibleHeight.
        /// </summary>
        public virtual float flexibleHeight { get { return -1; } }

        /// <summary>
        /// See ILayoutElement.layoutPriority.
        /// </summary>
        public virtual int layoutPriority { get { return 0; } }

        /// <summary>
        /// Calculate if the ray location for this image is a valid hit location. Takes into account a Alpha test threshold.
        /// </summary>
        /// <param name="screenPoint">The screen point to check against</param>
        /// <param name="eventCamera">The camera in which to use to calculate the coordinating position</param>
        /// <returns>If the location is a valid hit or not.</returns>
        /// <remarks> Also see See:ICanvasRaycastFilter.</remarks>
        public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera) {
            if (alphaHitTestMinimumThreshold <= 0)
                return true;

            if (alphaHitTestMinimumThreshold > 1)
                return false;

            if (activeSprite == null)
                return true;

            Vector2 local;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out local))
                return false;

            Rect rect = GetPixelAdjustedRect();

            // Convert to have lower left corner as reference point.
            local.x += rectTransform.pivot.x * rect.width;
            local.y += rectTransform.pivot.y * rect.height;

            local = MapCoordinate(local, rect);

            // Convert local coordinates to texture space.
            Rect spriteRect = activeSprite.textureRect;
            float x = (spriteRect.x + local.x) / activeSprite.texture.width;
            float y = (spriteRect.y + local.y) / activeSprite.texture.height;

            try {
                return activeSprite.texture.GetPixelBilinear(x, y).a >= alphaHitTestMinimumThreshold;
            }
            catch (UnityException e) {
                Debug.LogError("Using alphaHitTestMinimumThreshold greater than 0 on Image whose sprite texture cannot be read. " + e.Message + " Also make sure to disable sprite packing for this sprite.", this);
                return true;
            }
        }

        private Vector2 MapCoordinate(Vector2 local, Rect rect) {
            Rect spriteRect = activeSprite.rect;

            Vector4 border = activeSprite.border;
            Vector4 adjustedBorder = GetAdjustedBorders(border / pixelsPerUnit, rect);

            for (int i = 0; i < 2; i++) {
                if (local[i] <= adjustedBorder[i])
                    continue;

                if (rect.size[i] - local[i] <= adjustedBorder[i + 2]) {
                    local[i] -= (rect.size[i] - spriteRect.size[i]);
                    continue;
                }

                float lerp = Mathf.InverseLerp(adjustedBorder[i], rect.size[i] - adjustedBorder[i + 2], local[i]);
                local[i] = Mathf.Lerp(border[i], spriteRect.size[i] - border[i + 2], lerp);
                continue;
            }

            return local;
        }

        // To track textureless images, which will be rebuild if sprite atlas manager registered a Sprite Atlas that will give this image new texture
        static List<SlicedImage> m_TrackedTexturelessImages = new List<SlicedImage>();
        static bool s_Initialized;

        static void RebuildImage(SpriteAtlas spriteAtlas) {
            for (var i = m_TrackedTexturelessImages.Count - 1; i >= 0; i--) {
                var g = m_TrackedTexturelessImages[i];
                if (spriteAtlas.CanBindTo(g.activeSprite)) {
                    g.SetAllDirty();
                    m_TrackedTexturelessImages.RemoveAt(i);
                }
            }
        }

        private static void TrackImage(SlicedImage g) {
            if (!s_Initialized) {
                SpriteAtlasManager.atlasRegistered += RebuildImage;
                s_Initialized = true;
            }

            m_TrackedTexturelessImages.Add(g);
        }

        private static void UnTrackImage(SlicedImage g) {
            m_TrackedTexturelessImages.Remove(g);
        }
    }
}