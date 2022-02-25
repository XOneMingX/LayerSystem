using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class LayerController : MonoBehaviour
{
    public GameObject selectedOn;
    public GameObject selectedOff;
    public GameObject deleteButton;

    internal bool isSelected;

    private ButtonConfigHelper buttonConfigHelper;
    private LayersManager layersManager;

    // Start is called before the first frame update
    void Start()
    {
        //selectedOn.SetActive(false);
        //selectedOff.SetActive(true);
        buttonConfigHelper = this.gameObject.GetComponent<ButtonConfigHelper>();
        layersManager = GameObject.Find("LayersListManage").GetComponent<LayersManager>();
        if (this.gameObject.name == "Layer1")
        {
            selectedOn.SetActive(true);
            selectedOff.SetActive(false);
        }
        else
        {
            selectedOn.SetActive(false);
            selectedOff.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        buttonConfigHelper.MainLabelText = this.gameObject.name;
    }

    public void selectLayer()
    {
        layersManager.selectOneLayer(this.gameObject.name);
    }

    public void deletLayer()
    {
        layersManager.deleteOneLayer(this.gameObject.name);
    }

    internal void layerSelected()
    {
        if (isSelected)
        {
            selectedOn.SetActive(true);
            selectedOff.SetActive(false);
        }
        else
        {
            selectedOn.SetActive(false);
            selectedOff.SetActive(true);
        }
    }
}
