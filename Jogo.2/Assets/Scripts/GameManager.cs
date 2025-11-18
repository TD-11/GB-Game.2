using UnityEngine;

public class GameManager : MonoBehaviour
{
    private SD_Serial _sd_serial;
    private GameObject _gameObject;
    
    
    void Start()
    {
        _gameObject = GameObject.Find("GameManager");
        
        

        if (_gameObject !=null && _gameObject != gameObject)
        {
            _sd_serial = _gameObject.GetComponent<SD_Serial>();
            
            _sd_serial.portDropdown = gameObject.GetComponent<SD_Serial>().portDropdown;
            _sd_serial.connectButton = gameObject.GetComponent<SD_Serial>().connectButton;
            _sd_serial.statusText = gameObject.GetComponent<SD_Serial>().statusText;
            _sd_serial.valuesText = gameObject.GetComponent<SD_Serial>().valuesText;
            _sd_serial.portListText = gameObject.GetComponent<SD_Serial>().portListText;
                
         //  portDropdown
         // connectButton
         //  statusText
          // valuesText
          //  portListText
           
           Destroy(gameObject);
        }

    }
}
