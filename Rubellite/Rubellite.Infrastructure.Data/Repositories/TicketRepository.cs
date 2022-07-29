using Microsoft.EntityFrameworkCore;
using Rubellite.Domain.Core;
using Rubellite.Domain.Interfaces;

namespace Rubellite.Infrastructure.Data.Repositories;

public class TicketRepository: ITicketRepository
{
    private RubelliteContext db;

    public TicketRepository(RubelliteContext db)
    {
        this.db = db;
    }

    public async Task<IEnumerable<Ticket>> GetBookList()
    {
        return await db.Tickets.ToListAsync();
    }

    public Task<Ticket?> GetTicket(Guid id)
    {
        return db.Tickets.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async ValueTask<Ticket> Create(Ticket item)
    {
        return (await db.Tickets.AddAsync(item)).Entity;
    }

    public Task Update(Ticket item)
    {
        db.Entry(item).State = EntityState.Modified;
        db.Tickets.Update(item);
        
        return Task.CompletedTask;
    }

    public async ValueTask Delete(Guid id)
    {
        var ticket = await db.Tickets.FindAsync(id);
        if (ticket != null)
            db.Tickets.Remove(ticket);
    }

    public Task Save()
    {
        return db.SaveChangesAsync();
    }
    
    private bool _disposed;
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                
            }
            
            db.Dispose();
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~TicketRepository()
    {
        Dispose(false);
    }
}