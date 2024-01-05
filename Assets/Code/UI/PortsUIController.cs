using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PortsUIController : MonoBehaviour
{
    [SerializeField] private DataDownloadPanelController m_Downloader;
    [SerializeField] private GameObject m_MicrocontrollerPanel;
    [SerializeField] private Transform m_Parent;
    [SerializeField] private Button m_RefreshButton;

    private void Start()
    {
        SerialPortController.Instance.OnConnected += (sender, args) =>
        {
            FetchPorts();
        };

        SerialPortController.Instance.OnDisconnected += (sender, args) =>
        {
            FetchPorts();
        };

        m_RefreshButton.onClick.AddListener(() => FetchPorts());

        FetchPorts();
    }

    private void FetchPorts()
    {
        Clear();
        
        foreach (var port in SerialPortController.Instance.ListSerialPorts())
        {
            SetupMicrocontroller(port);
        }
    }

    private void Clear()
    {
        for (int i = 0; i < m_Parent.childCount; i++)
        {
            Destroy(m_Parent.GetChild(i).gameObject);
        }
    }

    private void SetupMicrocontroller(string path)
    {
        var obj = Instantiate(m_MicrocontrollerPanel, m_Parent);

        obj.transform.Find("Image").GetComponentInChildren<TextMeshProUGUI>().SetText(path);
        obj.transform.Find("Connect Button").GetComponentInChildren<TextMeshProUGUI>().SetText(SerialPortController.Instance.GetPath() == path ? "CONNECTED" : "CONNECT");
        obj.transform.Find("Connect Button").GetComponent<Button>().interactable = SerialPortController.Instance.GetPath() != path;
        obj.transform.Find("Connect Button").GetComponent<Button>().onClick.AddListener(() => SerialPortController.Instance.Connect(path));
    }
}