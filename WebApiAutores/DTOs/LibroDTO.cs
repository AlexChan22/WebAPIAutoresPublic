﻿namespace WebApiAutores.DTOs
{
    public class LibroDTO
    {

        public int Id { get; set; }
        public string Titulo { get; set; }

        public List<ComentarioDTO> comentarios { get; set; }

        public DateTime FechaPublicacion { get; set; }


    }
}
