using Microsoft.EntityFrameworkCore;
using LetCakes.Domain.Common;

namespace LetCakes.Infrastructure.Data;

/// <summary>
/// Contexto do banco de dados da aplicação LetCakes.
/// Gerencia as entidades e configurações do Entity Framework Core.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Construtor que recebe as opções de configuração do DbContext.
    /// Injetado via Dependency Injection.
    /// </summary>
    /// <param name="options">Opções de configuração do contexto (connection string, provider, etc)</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // DbSets serão adicionados aqui conforme as entidades forem criadas
    // Exemplo: public DbSet<Product> Products => Set<Product>();
    // Exemplo: public DbSet<Customer> Customers => Set<Customer>();







    

    /// <summary>
    /// Configuração do modelo de dados (fluent API).
    /// Aplica configurações de todas as entidades automaticamente.
    /// </summary>
    /// <param name="modelBuilder">Construtor do modelo de dados</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurações globais para todas as entidades que herdam de BaseEntity
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Verifica se a entidade herda de BaseEntity
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                // Configura índice na coluna IsActive para melhor performance em queries
                modelBuilder.Entity(entityType.ClrType)
                    .HasIndex(nameof(BaseEntity.IsActive))
                    .HasDatabaseName($"IX_{entityType.GetTableName()}_IsActive");

                // Configura índice na coluna CreatedAt para queries de ordenação por data
                modelBuilder.Entity(entityType.ClrType)
                    .HasIndex(nameof(BaseEntity.CreatedAt))
                    .HasDatabaseName($"IX_{entityType.GetTableName()}_CreatedAt");

                // Configura filtro global para buscar apenas entidades ativas por padrão
                // Pode ser sobrescrito usando .IgnoreQueryFilters() nas queries
                var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                var body = System.Linq.Expressions.Expression.Equal(
                    System.Linq.Expressions.Expression.Property(parameter, nameof(BaseEntity.IsActive)),
                    System.Linq.Expressions.Expression.Constant(true)
                );
                var lambda = System.Linq.Expressions.Expression.Lambda(body, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }



    /// <summary>
    /// Sobrescreve o método SaveChangesAsync para aplicar lógica automática
    /// de auditoria (CreatedAt, UpdatedAt) antes de salvar no banco.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelamento da operação</param>
    /// <returns>Número de registros afetados</returns>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Busca todas as entidades que estão sendo adicionadas ou modificadas
        var entries = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            // Define CreatedAt automaticamente ao adicionar uma nova entidade
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }

            // Define UpdatedAt automaticamente ao modificar uma entidade existente
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }



    /// <summary>
    /// Sobrescreve o método SaveChanges (síncrono) para aplicar a mesma lógica.
    /// Mantido por compatibilidade, mas recomenda-se usar SaveChangesAsync.
    /// </summary>
    /// <returns>Número de registros afetados</returns>
    public override int SaveChanges()
    {
        var entries = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChanges();
    }
}