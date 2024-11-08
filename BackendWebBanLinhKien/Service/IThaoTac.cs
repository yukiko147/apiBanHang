namespace BackendWebBanLinhKien.Service
{
    public interface IThaoTac<T>
    {
        public Task<List<T>> GetAll();
        public Task<T> GetByIdOrName(string Id,string ten);
        public Task<bool> Them(T Model);
        public Task<bool> Sua(T Model, string Id);
        public Task<bool> Xoa(string Id);
    }
}
