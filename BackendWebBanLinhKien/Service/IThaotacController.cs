namespace BackendWebBanLinhKien.Service
{
    public interface IThaotacController<T,A>
    {
        public Task<A> GetAll();
        public Task<A> GetById(string Id);
        public Task<A> Create(T model);
        public Task<A> Edit(T model, string Id);
        public Task<A> Delete(string Id);
    }
}
