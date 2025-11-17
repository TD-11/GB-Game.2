// Interface que define o padrão Observer para objetos que precisam ser notificados
// sobre mudanças relacionadas ao tempo. 
// Qualquer classe que implemente essa interface poderá "observar" e reagir
// a eventos de contagem regressiva ou de tempo acabando.
public interface ITimeObserver
{
    // Método chamado sempre que o tempo restante é alterado.
    // 'timeLeft' representa quanto tempo ainda falta.
    void OnTimeChanged(float timeLeft);

    // Método chamado quando o tempo chega ao fim.
    // Usado para executar ações como encerrar uma rodada,
    // bloquear movimentos, disparar animações, etc.
    void OnTimeEnded();
}