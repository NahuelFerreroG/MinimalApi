public class  LibroDTO
{
    public int Id {get; set;}
    public string? Titulo {get; set;}
    public string? Autor {get; set;}
    public bool EsSaga {get; set;}

    public LibroDTO(){}
    public LibroDTO(Libro libro) => 
    (Id, Titulo, Autor, EsSaga) = (libro.Id, libro.Titulo, libro.Autor, libro.EsSaga);
}