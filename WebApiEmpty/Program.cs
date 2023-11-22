using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<LibroDb>(opt => opt.UseInMemoryDatabase("LibrosList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

var libros = app.MapGroup("/libros");

libros.MapGet("/", GetAllLibros);
libros.MapGet("/saga", GetSaga);
libros.MapGet("/{id}", GetLibro);
libros.MapPost("/", PostLibro);
libros.MapPut("/{id}", PutLibro);
libros.MapDelete("/{id}", DeleteLibro);

static async Task<IResult> GetAllLibros(LibroDb db)
{
    return TypedResults.Ok(await db.Libros.Select(x => new LibroDTO(x)).ToArrayAsync());    
}

static async Task<IResult> GetSaga(LibroDb db)
{
    return TypedResults.Ok(await db.Libros.Where(libro => libro.EsSaga).Select(x => new LibroDTO(x)).ToArrayAsync());
}

static async Task<IResult> GetLibro(int id, LibroDb db)
{
    return await db.Libros.FindAsync(id)
        is Libro libro
            ? TypedResults.Ok(new LibroDTO(libro))
            : TypedResults.NotFound();
}

static async Task<IResult> PostLibro(LibroDTO libroDTO, LibroDb db)
{
    var nuevolibro = new Libro
    {
        EsSaga = libroDTO.EsSaga,
        Titulo = libroDTO.Titulo,
        Autor = libroDTO.Autor,
    };
    
    db.Libros.Add(nuevolibro);
    await db.SaveChangesAsync();

    libroDTO = new LibroDTO(nuevolibro);
        
    return TypedResults.Created($"/libros/{libroDTO.Id}", libroDTO);
}

static async Task<IResult> PutLibro(int id, LibroDTO inputLibro, LibroDb db)
{
    var libro = await db.Libros.FindAsync(id);
    
    if (libro is null) return TypedResults.NotFound();

    libro.Titulo = inputLibro.Titulo;
    libro.EsSaga = inputLibro.EsSaga;
    libro.Autor = inputLibro.Autor;
    
    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteLibro(int id, LibroDb db)
{
    if (await db.Libros.FindAsync(id) is Libro libro)
    {
        db.Libros.Remove(libro);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}

app.Run();