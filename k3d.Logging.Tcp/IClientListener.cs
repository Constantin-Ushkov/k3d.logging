namespace Ice.TcpLogger
{
    internal interface IClientListener: IDisposable
    {
        string Name { get; }
    }
}