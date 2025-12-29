using LetCakes.Domain.Common;

namespace LetCakes.Domain.Interfaces;

/// <summary>
/// Interface genérica de repositório que define operações CRUD padrão para entidades.
/// Implementa o padrão Repository para abstrair o acesso a dados.
/// </summary>
/// <typeparam name="T">Tipo da entidade que herda de BaseEntity</typeparam>
public interface IRepository<T> where T : BaseEntity
{
    /// <summary>
    /// Obtém uma entidade por ID (apenas registros ativos).
    /// </summary>
    /// <param name="id">ID único da entidade</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Entidade encontrada ou null se não existir/estiver inativa</returns>
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém todas as entidades ativas.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Coleção de entidades ativas</returns>
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém todas as entidades incluindo as inativas.
    /// Útil para relatórios e auditorias.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Coleção de todas as entidades</returns>
    Task<IEnumerable<T>> GetAllIncludingInactiveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adiciona uma nova entidade ao repositório.
    /// </summary>
    /// <param name="entity">Entidade a ser adicionada</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Entidade adicionada com ID gerado</returns>
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza uma entidade existente.
    /// </summary>
    /// <param name="entity">Entidade com dados atualizados</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove logicamente uma entidade (soft delete).
    /// Define IsActive como false sem remover fisicamente do banco.
    /// </summary>
    /// <param name="id">ID da entidade a ser desativada</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove fisicamente uma entidade do banco de dados (hard delete).
    /// Use com cautela - operação irreversível.
    /// </summary>
    /// <param name="id">ID da entidade a ser removida</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se uma entidade ativa existe pelo ID.
    /// </summary>
    /// <param name="id">ID da entidade</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se existir e estiver ativa, False caso contrário</returns>
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Conta o total de entidades ativas.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Quantidade de entidades ativas</returns>
    Task<int> CountAsync(CancellationToken cancellationToken = default);
}
