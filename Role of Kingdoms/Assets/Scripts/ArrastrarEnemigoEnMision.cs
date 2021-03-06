﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArrastrarEnemigoEnMision : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    //Drag and Drop:
    private Vector3 posicionInicial;
    private GraphicRaycaster _GR;
    private PointerEventData _punteroInfo;
    private List<RaycastResult> _resultadoRaycast;
    private ManejadorGeneralMundo MGM;

    void Start()
    {
        MGM = GameObject.FindGameObjectWithTag("MGM").GetComponent<ManejadorGeneralMundo>();
        _resultadoRaycast = new List<RaycastResult>();
        _punteroInfo = new PointerEventData(null);
        _GR = MGM.GraphicRaycasterPrincipal;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(Input.GetMouseButton(0) && !Input.GetMouseButton(1))
            posicionInicial = this.transform.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0) && FindObjectOfType<ManejadorMisionPanel>().AnimatorMMP.GetBool("Bajar"))
            this.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Input.GetMouseButtonUp(0) && !Input.GetMouseButtonUp(1))
        {
            _punteroInfo.position = Input.mousePosition;
            _GR.Raycast(_punteroInfo, _resultadoRaycast);
            if (_resultadoRaycast.Count > 0)
            {
                GameObject ObjetoInfoDetalles = _resultadoRaycast.Find(x => x.gameObject.name == "Panel_Mostrar_Detalles").gameObject;
                if (ObjetoInfoDetalles != null)
                {
                    string nombre = MGM.MisionPanel.NombreMisionMapa;
                    Mision MActual = GameObject.Find(nombre).GetComponent<Mision>();
                    int total = Convert.ToInt32(this.name.Replace("Enemigo_", ""));
                    MActual.MostrarInformacionEnemigo(total);
                }

            }
            this.transform.localPosition = posicionInicial;
            _resultadoRaycast = new List<RaycastResult>();
        }else
            this.transform.localPosition = posicionInicial;
    }

}
