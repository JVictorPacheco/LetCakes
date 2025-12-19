using LetCakes.Domain.Common;

namespace LetCakes.Domain.Interfaces;


public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> GetAllAsync (CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> GetAllActiveAndInactiveAsync(CancellationToken cancellationToken = default);


    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default); // T entity representa a entidade a ser adicionada ao repositório.

    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);


     /// <summary>
    /// Remove logicamente uma entidade (soft delete), marcando IsActive como false.
    /// Seguindo o princípio de que dados nunca devem ser perdidos permanentemente.
    /// </summary>
    /// <param name="id">Identificador da entidade a ser removida</param>
    /// <param name="cancellationToken">Token para cancelamento assíncrono da operação</param>
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);


        /// <summary>
    /// Remove fisicamente uma entidade do banco de dados (hard delete).
    /// Use apenas em casos específicos onde a remoção permanente é necessária.
    /// </summary>
    /// <param name="id">Identificador da entidade a ser removida</param>
    /// <param name="cancellationToken">Token para cancelamento assíncrono da operação</param>
    Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default);



      /// <summary>
    /// Verifica se existe uma entidade com o identificador especificado.
    /// </summary>
    /// <param name="id">Identificador da entidade</param>
    /// <param name="cancellationToken">Token para cancelamento assíncrono da operação</param>
    /// <returns>True se a entidade existe, False caso contrário</returns>
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);


    /// <summary>
    /// Retorna a quantidade total de entidades ativas.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelamento assíncrono da operação</param>
    /// <returns>Número total de entidades ativas</returns>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

}



