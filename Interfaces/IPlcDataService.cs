namespace GameStoreMono.BlazorServer.Interfaces;

public interface IPlcDataService
{
    Task StartMonitoring();
    Task StopMonitoring();
    Task NotifyClientsAsync<T>(string method, T data);
}
