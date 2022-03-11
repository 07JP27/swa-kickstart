using api.Context;
using api.Model;
using Dapper;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace api.Repository
{
  public class ToDoRepository : IToDoRepository
  {
    private readonly DbContext _context;

    public ToDoRepository(DbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<ToDoItem>> GetToDoItems()
    {
      string sql = "select * from [dbo].[todos]";
      using (var conn = _context.CreateConnection())
      {
        var items = await conn.QueryAsync<ToDoItem>(sql);
        return items;
      }
    }

    public async Task<int> CreateToDoItems(string toDoTitle, ClientPrincipal principal)
    {
      string sql = @"INSERT INTO [dbo].[todos] 
      (toDo, created_at,created_by, updated_at, updated_by) 
      VALUES (@ToDo, @CreatedAt, @CreatedBy, @UpdatedAt, @UpdatedBy)";
      using (var conn = _context.CreateConnection())
      {
        var now = DateTime.UtcNow;
        var items = await conn.ExecuteAsync(sql, new { 
          ToDo = toDoTitle,
          CreatedAt = now,
          CreatedBy = principal.UserDetails,
          UpdatedAt = now,
          UpdatedBy = principal.UserDetails
        });
        return items;
      }
    }

    public async Task<int> DeleteToDoItems(int toDoId)
    {
      string sql = "DELETE FROM [dbo].[todos] WHERE id = @id";
      using (var conn = _context.CreateConnection())
      {
        var items = await conn.ExecuteAsync(sql, new { id = toDoId });
        return items;
      }
    }

    public async Task<int> MarkCompleteStatusToDoItems(int toDoId, bool completed, ClientPrincipal principal)
    {
      string sql = @"UPDATE [dbo].[todos] SET
       completed = @Completed, 
       updated_at = @UpdatedAt, 
       updated_by = @UpdatedBy
       WHERE id = @Id";
      using (var conn = _context.CreateConnection())
      {
        var items = await conn.ExecuteAsync(sql, new {
          Id = toDoId,
          Completed = completed,
          UpdatedAt = DateTime.UtcNow,
          UpdatedBy = principal.UserDetails
        });
        return items;
      }
    }
  }
}