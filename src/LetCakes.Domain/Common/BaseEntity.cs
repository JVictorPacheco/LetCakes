namespace LetCakes.Domain.Common;


public abstract class BaseEntity
{
    public Guid Id {get; set;} = Guid.NewGuid(); // Unique identifier for the entity, que é gerado automaticamente ao criar uma nova instância.

    public DateTime CreatedAt {get; set;} = DateTime.Now; // Data e hora de criação da entidade, que é definida automaticamente ao criar uma nova instância.

    public DateTime? UpdatedAt {get; set;} // Data e hora da última atualização da entidade, que pode ser nula se a entidade nunca foi atualizada.

    public bool IsActive {get; set;} = true; // Indica se a entidade está ativa ou não, com valor padrão como true.

}