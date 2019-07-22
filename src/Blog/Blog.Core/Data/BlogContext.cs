using System.Threading;
using Blog.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Blog.Core.Data
{
    public class BlogContext : DbContext
    {
        private IDbContextTransaction _transaction;

        public BlogContext()
        {
        }

        public BlogContext(DbContextOptions<BlogContext> options) :
            base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public async void BeginTransactionAsync(CancellationToken cancellationToken)
        {
            if (Database.IsInMemory()) return;

            if (_transaction != null) return;

            _transaction = await Database.BeginTransactionAsync(cancellationToken);
        }

        public void CommitTransaction()
        {
            if (_transaction == null) return;

            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }

        public void RollBackTransaction()
        {
            if (_transaction == null) return;

            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }
    }
}
