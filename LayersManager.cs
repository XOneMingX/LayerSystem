using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;

public class LayersManager : MonoBehaviour
{
    public GameObject layerPrefab;
    public GameObject layersCollection;
    private GameObject[] layers;
    internal Vector3[] layersTransform;

    private GridObjectCollection gridObjectCollection;

    bool isReset;
    // Start is called before the first frame update
    void Start()
    {
        gridObjectCollection = layersCollection.GetComponent<GridObjectCollection>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isReset)
        {
            StartCoroutine(waitForReset());
        }
    }

    public void AddLayer()
    {
        Quaternion newLayerRotation = GameObject.Find("ScrollParent").transform.rotation;
        GameObject layer = Instantiate(layerPrefab, transform.position, newLayerRotation);
        layer.transform.parent = layersCollection.transform;
        isReset = true;
        //layersList();

    }

    void layersList()
    {
        //Debug.Log(layersCollection.transform.childCount);
        layers = new GameObject[layersCollection.transform.childCount];
        layersTransform = new Vector3[layersCollection.transform.childCount];
        for (int i = 0; i < layersCollection.transform.childCount; i++)
        {
            layers[i] = layersCollection.transform.GetChild(i).gameObject;
            layersTransform[i] = layersCollection.transform.GetChild(i).position;
            layers[i].name = "Layer" + (i+1);
        }
    }

    internal void selectOneLayer(string layerName)
    {
        foreach(GameObject layer in layers)
        {
            if(layer.name != layerName)
            {
                if(layer.TryGetComponent<LayerController>(out var layerController))
                {
                    layerController.isSelected = false;
                    layerController.layerSelected();
                    layerController.deleteButton.SetActive(false);
                }
            }
            else
            {
                if (layer.TryGetComponent<LayerController>(out var layerController))
                {
                    layerController.isSelected = true;
                    layerController.layerSelected();
                }
            }
        }
    }

    internal void deleteOneLayer(string layerName)
    {
        foreach (GameObject layer in layers)
        {
            if(layersCollection.transform.childCount > 1)
            {
                if (layer.name == layerName)
                {
                    Destroy(layer);
                }
            }
        }
        isReset = true;
    }

    IEnumerator waitForReset()
    {
        layersList();
        gridObjectCollection.UpdateCollection();
        foreach (GameObject layer in layers)
        {
            if (layer.name == "Layer1")
            {
                if (layer.TryGetComponent<LayerController>(out var layerController))
                {
                    layerController.isSelected = true;
                    layerController.layerSelected();
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
        isReset = false;
    }
}
