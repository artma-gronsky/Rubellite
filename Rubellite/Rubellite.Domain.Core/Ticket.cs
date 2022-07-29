namespace Rubellite.Domain.Core;

public class Ticket
{
    /// <summary>
    /// Ticket id.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Ticket number.
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Ticket price.
    /// </summary>
    public decimal Price { get; set; }
}