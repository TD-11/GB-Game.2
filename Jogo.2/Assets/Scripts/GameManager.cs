using UnityEngine;

public class GameManager : MonoBehaviour
{
    private SD_Serial _sd_serial;
    private ConnectSDBalance _connect_sd_balance;
    private GameObject _gameObject;
    
    
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        
        _gameObject = GameObject.Find("GameManager");
        _connect_sd_balance = _gameObject.GetComponent<ConnectSDBalance>();
        
        if (_gameObject != gameObject)
        {
            _sd_serial = _gameObject.GetComponent<SD_Serial>();
            
            gameObject.name = "GameManager_C";
            
            _sd_serial.portDropdown = gameObject.GetComponent<SD_Serial>().portDropdown;
            _sd_serial.connectButton = gameObject.GetComponent<SD_Serial>().connectButton;
            _sd_serial.statusText = gameObject.GetComponent<SD_Serial>().statusText;
            _sd_serial.valuesText = gameObject.GetComponent<SD_Serial>().valuesText;
            _sd_serial.portListText = gameObject.GetComponent<SD_Serial>().portListText;
            _sd_serial.SetConnectButton = gameObject.GetComponent<SD_Serial>().SetConnectButton;
            _sd_serial.timerText = gameObject.GetComponent<SD_Serial>().timerText;
            _sd_serial.uiBlocker = gameObject.GetComponent<SD_Serial>().uiBlocker;
            _connect_sd_balance.connectStartText = gameObject.GetComponent<ConnectSDBalance>().connectStartText;
            _connect_sd_balance.connectConfigText = gameObject.GetComponent<ConnectSDBalance>().connectConfigText;
         
         
            Destroy(gameObject);
        }

    }
}
