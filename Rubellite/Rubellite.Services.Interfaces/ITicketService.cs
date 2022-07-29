using Rubellite.Domain.Core;

namespace Rubellite.Services.Interfaces;

public interface ITicketService
{
    void IssueTicket(Ticket ticket);
}