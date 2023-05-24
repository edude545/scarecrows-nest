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

    public int GrowthStage = 0;
    public Plant PlantType;
    public int PlantedSeeds = 0;

    public float HP;

    private GameObject model;

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
        updateText();
    }

    private void Update()
    {
        Canvas.transform.LookAt(GameController.Instance.Head.transform);
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        if (IsDead())
        {
            transform.parent = GameController.Instance.DeadCrops.transform;
            updateModel();
        }
    }

    public bool IsDead()
    {
        return HP < 0f;
    }

    public void ReceiveSeed(Plant plantType)
    {
        if (PlantType == null)
        {
            PlantType = plantType;
            PlantedSeeds = 0;
            Canvas.gameObject.SetActive(true);
        }
        if (PlantType == plantType)
        {
            PlantedSeeds++;
            updateText();
            updateModel();
        }
        if (PlantedSeeds == PlantType.RequiredSeeds)
        {
            OnCropPlanted();
        }
    }

    protected void OnCropPlanted()
    {
        transform.parent = GameController.Instance.LiveCrops.transform;
        HP = PlantType.MaxHP;
        updateModel();
    }

    protected void updateText()
    {
        if (PlantType == null) {
            Canvas.gameObject.SetActive(false);
        } else {
            Canvas.gameObject.SetActive(true);
            if (PlantedSeeds < PlantType.RequiredSeeds) {
                Text.SetText(PlantType.Name + "\n" + PlantedSeeds + " / " + PlantType.RequiredSeeds);
                Text.ForceMeshUpdate();
                TextBG.GetComponent<RectTransform>().sizeDelta = Text.GetRenderedValues() + new Vector2(1, 1);
            } else {
                Canvas.gameObject.SetActive(false);
            }
        }
    }

    // Returns the number of resources yielded by the crop this cycle. Called from GameController#onCycleEnd.
    public int OnCycleEnd() {
        GrowthStage++;
        if (PlantType != null && GrowthStage == PlantType.GrowthTime) {
            int yield = Random.Range(PlantType.MinYield, PlantType.MaxYield);
            GameController.Instance.AddResource(PlantType, yield);
            PlantType = null;
            GrowthStage = 0;
        }
        updateText();
        updateModel();
        return 0;
    }

    private void updateModel() {
        // todo: allow separate models for growth stages
        if (PlantType == null) {
            if (model != null) {
                Destroy(model);
            }
        } else {
            if (PlantedSeeds >= PlantType.RequiredSeeds)
            {
                if (model == null)
                {
                    model = Instantiate(PlantType.Model, transform);
                    model.transform.position += new Vector3(0f, 0.155f, 0f);
                }
                float s = (GrowthStage * 4.0f + 4) / (PlantType.GrowthTime + 4); // Add 4 to values so the crop is visible at stage 0
                model.transform.localScale = new Vector3(s, s, s);
            }
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
