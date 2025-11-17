using System.Collections.Generic;

// Interface que representa o "Subject" (ou observado) no padrão Observer.
// Classes que controlam ou monitoram o tempo (ex.: cronômetros, timers de rodada)
// devem implementar essa interface para permitir que outros objetos se registrem
// como observadores e recebam notificações sobre mudanças no tempo.
public interface ITimeSubject
{
    // Adiciona um observador à lista. 
    // Esse observador passará a receber notificações sempre que o tempo mudar.
    void AddObserver(ITimeObserver observer);

    // Remove um observador da lista.
    // Após ser removido, ele deixa de receber notificações sobre o tempo.
    void RemoveObserver(ITimeObserver observer);

    // Notifica todos os observadores que o tempo restante mudou.
    // O parâmetro 'timeLeft' representa o tempo atualizado.
    void NotifyTimeChanged(float timeLeft);

    // Notifica todos os observadores de que o tempo chegou ao fim.
    // Usado para acionar eventos como fim de rodada, fim de partida, etc.
    void NotifyTimeEnded();
}