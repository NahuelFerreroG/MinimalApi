using Microsoft.EntityFrameworkCore;

class LibroDb : DbContext
{
    public LibroDb(DbContextOptions<LibroDb> options)
        : base(options) { }

    public DbSet<Libro> Libros => Set<Libro>();
}