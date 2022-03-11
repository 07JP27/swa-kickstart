using api.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Repository
{
  public interface IToDoRepository
  {
    public Task<IEnumerable<ToDoItem>> GetToDoItems();
    public Task<int> CreateToDoItems(string toDoTitle, ClientPrincipal principal);
    public Task<int> DeleteToDoItems(int toDoId);
    public Task<int> MarkCompleteStatusToDoItems(int toDoId, bool completed, ClientPrincipal principal);
  }
}