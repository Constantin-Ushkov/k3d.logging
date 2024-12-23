namespace k3d.Logging.Tcp
{
    internal interface IClientListener: IDisposable
    {
        string Name { get; }
    }
}