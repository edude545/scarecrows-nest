using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Crop : MonoBehaviour
{

    

    public TMP_Text Text;
    public Image TextBG;
    public Canvas Canvas;

    // Highlight system from Interactable
    /*private bool wasHovering = false;
    private bool isHovering = false;
    public bool highlightOnHover = true;
    protected MeshRenderer[] highlightRenderers;
    protected MeshRenderer[] existingRenderers;
    protected GameObject highlightHolder;
    protected SkinnedMeshRenderer[] highlightSkinnedRenderers;
    protected SkinnedMeshRenderer[] existingSkinnedRenderers;
    protected static Material highlightMat;*/
    // ---

    public int GrowthStage;
    public Plant PlantType;
    public int PlantedSeeds = 0;

    public float HP;

    protected virtual void Start()
    {
        // Highlight code from Interactable
        /*if (highlightMat == null)
#if UNITY_URP
                highlightMat = (Material)Resources.Load("SteamVR_HoverHighlight_URP", typeof(Material));
#else
            highlightMat = (Material)Resources.Load("SteamVR_HoverHighlight", typeof(Material));
#endif

        if (highlightMat == null)
            Debug.LogError("<b>[SteamVR Interaction]</b> Hover Highlight Material is missing. Please create a material named 'SteamVR_HoverHighlight' and place it in a Resources folder", this);
*/
        /*if (skeletonPoser != null)
        {
            if (useHandObjectAttachmentPoint)
            {
                //Debug.LogWarning("<b>[SteamVR Interaction]</b> SkeletonPose and useHandObjectAttachmentPoint both set at the same time. Ignoring useHandObjectAttachmentPoint.");
                useHandObjectAttachmentPoint = false;
            }
        }*/
    }

    private void Update()
    {
        Canvas.transform.LookAt(GameController.Instance.Head.transform);
    }

    public void ReceiveSeed(Seed seed)
    {
        if (PlantType == null)
        {
            PlantType = seed.PlantType;
            PlantedSeeds = 0;
        }
        if (PlantType == seed.PlantType)
        {
            PlantedSeeds++;
            UpdateText();
        }
    }

    protected void UpdateText()
    {
        if (PlantType == null) {
            Canvas.gameObject.SetActive(false);
        } else {
            Canvas.gameObject.SetActive(true);
            string s;
            if (PlantedSeeds < PlantType.RequiredSeeds) {
                s = PlantType.Name + "\n" + PlantedSeeds + " / " + PlantType.RequiredSeeds;
            } else {
                s = PlantType.Name;
            }
            Text.SetText(s);
            Text.ForceMeshUpdate();
            TextBG.GetComponent<RectTransform>().sizeDelta = Text.GetRenderedValues() + new Vector2(1, 1);
        }
    }

    // Returns the number of resources yielded by the crop this cycle. Called from GameController#onCycleEnd.
    public int OnCycleEnd() {
        updateModel();
        GrowthStage++;
        if (GrowthStage == PlantType.GrowthTime) {
            int yield = Random.Range(PlantType.MinYield, PlantType.MaxYield);
            PlantType = null;
            GrowthStage = 0;
            return yield;
        }
        return 0;
    }

    private void updateModel() {
        // todo: allow separate models for growth stages
        if (PlantType != null)
        {
            float s = (GrowthStage + 1) / (PlantType.GrowthTime + 1); // Add 1 to values so the crop is visible at stage 0
            gameObject.transform.localScale = new Vector3(s, s, s);
        }
    }

    // Everything below is highlight code copied from Interactable
    /*public virtual void OnPointerEnter()
    {
        wasHovering = isHovering;
        isHovering = true;

        if (highlightOnHover == true && wasHovering == false)
        {
            CreateHighlightRenderers();
            UpdateHighlightRenderers();
        }
    }

    public virtual void OnPointerExit()
    {
        wasHovering = isHovering;
        isHovering = false;
        if (highlightHolder != null) {
            Destroy(highlightHolder);
        }
    }

    protected virtual void CreateHighlightRenderers()
    {
        existingSkinnedRenderers = this.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        highlightHolder = new GameObject("Highlighter");
        highlightSkinnedRenderers = new SkinnedMeshRenderer[existingSkinnedRenderers.Length];

        for (int skinnedIndex = 0; skinnedIndex < existingSkinnedRenderers.Length; skinnedIndex++)
        {
            SkinnedMeshRenderer existingSkinned = existingSkinnedRenderers[skinnedIndex];

            *//*if (ShouldIgnoreHighlight(existingSkinned))
                continue;*//*

            GameObject newSkinnedHolder = new GameObject("SkinnedHolder");
            newSkinnedHolder.transform.parent = highlightHolder.transform;
            SkinnedMeshRenderer newSkinned = newSkinnedHolder.AddComponent<SkinnedMeshRenderer>();
            Material[] materials = new Material[existingSkinned.sharedMaterials.Length];
            for (int materialIndex = 0; materialIndex < materials.Length; materialIndex++)
            {
                materials[materialIndex] = highlightMat;
            }

            newSkinned.sharedMaterials = materials;
            newSkinned.sharedMesh = existingSkinned.sharedMesh;
            newSkinned.rootBone = existingSkinned.rootBone;
            newSkinned.updateWhenOffscreen = existingSkinned.updateWhenOffscreen;
            newSkinned.bones = existingSkinned.bones;

            highlightSkinnedRenderers[skinnedIndex] = newSkinned;
        }

        MeshFilter[] existingFilters = this.GetComponentsInChildren<MeshFilter>(true);
        existingRenderers = new MeshRenderer[existingFilters.Length];
        highlightRenderers = new MeshRenderer[existingFilters.Length];

        for (int filterIndex = 0; filterIndex < existingFilters.Length; filterIndex++)
        {
            MeshFilter existingFilter = existingFilters[filterIndex];
            MeshRenderer existingRenderer = existingFilter.GetComponent<MeshRenderer>();

            if (existingFilter == null || existingRenderer == null)// || ShouldIgnoreHighlight(existingFilter))
                continue;

            GameObject newFilterHolder = new GameObject("FilterHolder");
            newFilterHolder.transform.parent = highlightHolder.transform;
            MeshFilter newFilter = newFilterHolder.AddComponent<MeshFilter>();
            newFilter.sharedMesh = existingFilter.sharedMesh;
            MeshRenderer newRenderer = newFilterHolder.AddComponent<MeshRenderer>();

            Material[] materials = new Material[existingRenderer.sharedMaterials.Length];
            for (int materialIndex = 0; materialIndex < materials.Length; materialIndex++)
            {
                materials[materialIndex] = highlightMat;
            }
            newRenderer.sharedMaterials = materials;

            highlightRenderers[filterIndex] = newRenderer;
            existingRenderers[filterIndex] = existingRenderer;
        }
    }

    protected virtual void UpdateHighlightRenderers()
    {
        if (highlightHolder == null)
            return;

        for (int skinnedIndex = 0; skinnedIndex < existingSkinnedRenderers.Length; skinnedIndex++)
        {
            SkinnedMeshRenderer existingSkinned = existingSkinnedRenderers[skinnedIndex];
            SkinnedMeshRenderer highlightSkinned = highlightSkinnedRenderers[skinnedIndex];

            if (existingSkinned != null && highlightSkinned != null)
            {
                highlightSkinned.transform.position = existingSkinned.transform.position;
                highlightSkinned.transform.rotation = existingSkinned.transform.rotation;
                highlightSkinned.transform.localScale = existingSkinned.transform.lossyScale;
                highlightSkinned.localBounds = existingSkinned.localBounds;
                highlightSkinned.enabled = isHovering && existingSkinned.enabled && existingSkinned.gameObject.activeInHierarchy;

                int blendShapeCount = existingSkinned.sharedMesh.blendShapeCount;
                for (int blendShapeIndex = 0; blendShapeIndex < blendShapeCount; blendShapeIndex++)
                {
                    highlightSkinned.SetBlendShapeWeight(blendShapeIndex, existingSkinned.GetBlendShapeWeight(blendShapeIndex));
                }
            }
            else if (highlightSkinned != null)
                highlightSkinned.enabled = false;

        }

        for (int rendererIndex = 0; rendererIndex < highlightRenderers.Length; rendererIndex++)
        {
            MeshRenderer existingRenderer = existingRenderers[rendererIndex];
            MeshRenderer highlightRenderer = highlightRenderers[rendererIndex];

            if (existingRenderer != null && highlightRenderer != null)
            {
                highlightRenderer.transform.position = existingRenderer.transform.position;
                highlightRenderer.transform.rotation = existingRenderer.transform.rotation;
                highlightRenderer.transform.localScale = existingRenderer.transform.lossyScale;
                highlightRenderer.enabled = isHovering && existingRenderer.enabled && existingRenderer.gameObject.activeInHierarchy;
            }
            else if (highlightRenderer != null)
                highlightRenderer.enabled = false;
        }
    }*/

}
