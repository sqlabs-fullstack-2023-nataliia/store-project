namespace Order.Host.Services.Interfaces;

public interface IOrderService<T, TD>
{
    Task<List<T>> GetItems();
    Task<T> FindById(int id);
    Task<int?> AddItem(TD item);
    Task<T> UpdateItem(int id, TD item);
    Task<T> RemoveItem(int id);
}