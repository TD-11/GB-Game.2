// Interface que define o "contrato" do Padrão Command.
// Todo comando no sistema deve implementar essa interface.
public interface ICommand
{
    // Método que executa a ação encapsulada pelo comando.
    // Cada classe concreta (ex.: LoadSceneCommand, QuitCommand, RestartCommand)
    // irá implementar esse método com um comportamento específico.
    void Execute();
}
