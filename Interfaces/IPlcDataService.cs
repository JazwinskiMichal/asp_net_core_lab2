namespace GameStoreMono.BlazorServer.Interfaces;

public interface IPlcDataService
{
    Task StartMonitoring();
    Task StopMonitoring();
}
