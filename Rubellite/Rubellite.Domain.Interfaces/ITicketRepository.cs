using Rubellite.Domain.Core;

namespace Rubellite.Domain.Interfaces;

public interface ITicketRepository: IDisposable
{
    Task<IEnumerable<Ticket>> GetBookList();
    Task<Ticket?> GetTicket(Guid id);
    ValueTask<Ticket> Create(Ticket item);
    Task Update(Ticket item);
    ValueTask Delete(Guid id);
    Task Save();
}