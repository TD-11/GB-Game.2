using UnityEngine;

// Classe responsável por gerenciar um GameManager persistente entre cenas
public class GameManager : MonoBehaviour
{
    // Referência ao script que controla a comunicação serial
    private SD_Serial _sd_serial;

    // Referência ao script responsável pela conexão com a balança
    private ConnectSDBalance _connect_sd_balance;

    // Referência ao GameObject que contém o GameManager já existente
    private GameObject _gameObject;
    
    void Start()
    {
        // Faz com que este GameObject não seja destruído ao trocar de cena
        DontDestroyOnLoad(this.gameObject);
        
        // Procura na cena um GameObject chamado "GameManager"
        _gameObject = GameObject.Find("GameManager");

        // Obtém o componente ConnectSDBalance desse GameObject encontrado
        _connect_sd_balance = _gameObject.GetComponent<ConnectSDBalance>();
        
        // Verifica se o GameObject encontrado NÃO é este próprio objeto
        // Isso evita duplicação do GameManager entre cenas
        if (_gameObject != gameObject)
        {
            // Obtém o componente SD_Serial do GameManager original
            _sd_serial = _gameObject.GetComponent<SD_Serial>();
            
            // Renomeia o GameObject atual para indicar que é uma cópia
            gameObject.name = "GameManager_C";
            
            // Repassa as referências de UI do SD_Serial deste objeto
            // para o SD_Serial do GameManager principal
            _sd_serial.portDropdown = gameObject.GetComponent<SD_Serial>().portDropdown;
            _sd_serial.connectButton = gameObject.GetComponent<SD_Serial>().connectButton;
            _sd_serial.statusText = gameObject.GetComponent<SD_Serial>().statusText;
            _sd_serial.valuesText = gameObject.GetComponent<SD_Serial>().valuesText;
            _sd_serial.portListText = gameObject.GetComponent<SD_Serial>().portListText;
            _sd_serial.SetConnectButton = gameObject.GetComponent<SD_Serial>().SetConnectButton;
            _sd_serial.timerText = gameObject.GetComponent<SD_Serial>().timerText;
            _sd_serial.uiBlocker = gameObject.GetComponent<SD_Serial>().uiBlocker;

            // Repassa também os textos de interface do ConnectSDBalance
            _connect_sd_balance.connectStartText = gameObject.GetComponent<ConnectSDBalance>().connectStartText;
            _connect_sd_balance.connectConfigText = gameObject.GetComponent<ConnectSDBalance>().connectConfigText;
         
            // Remove o GameObject duplicado após transferir as referências
            Destroy(gameObject);
        }
    }
}