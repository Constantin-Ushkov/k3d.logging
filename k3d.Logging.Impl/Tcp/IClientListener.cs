namespace k3d.Logging.Impl.Tcp
{
    internal interface IClientListener: IDisposable
    {
        string Name { get; }
    }
}