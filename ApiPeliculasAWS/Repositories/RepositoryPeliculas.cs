using ApiPeliculasAWS.Data;
using ApiPeliculasAWS.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculasAWS.Repositories
{
    public class RepositoryPeliculas
    {
        private PeliculasContext context;

        public RepositoryPeliculas(PeliculasContext context)
        {
            this.context = context;
        }

        public async Task<List<Pelicula>> GetPeliculasAsync()
        {
            return await this.context.Peliculas.ToListAsync();
        }

        public async Task<List<Pelicula>>
            GetPeliculasActoresAsync(string actor)
        {
            var consulta = from datos in this.context.Peliculas
                           where datos.Actores.Contains(actor)
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task<Pelicula> FindPeliculaAsync(int idPelicula)
        {
            return await this.context.Peliculas
                .FirstOrDefaultAsync(x => x.IdPelicula == idPelicula);
        }

        private async Task<int> GetMaxIdPeliculaAsync()
        {
            return await this.context.Peliculas
                .MaxAsync(x => x.IdPelicula) + 1;
        }

        public async Task 
            CreatePeliculaAsync(string genero, string titulo
            , string argumento, string foto, string actores
            , int precio, string youtube)
        {
            Pelicula pelicula = new Pelicula
            {
                IdPelicula = await this.GetMaxIdPeliculaAsync(),
                Genero = genero, 
                Titulo = titulo,
                Argumento = argumento,
                Foto = foto,
                Actores = actores,
                Precio = precio, 
                YouTube = youtube
            };
            this.context.Peliculas.Add(pelicula);
            await this.context.SaveChangesAsync();
        }

        public async Task
            UpdatePeliculaAsync(int idpelicula, 
            string genero, string titulo
    , string argumento, string foto, string actores
    , int precio, string youtube)
        {
            Pelicula pelicula = await this.FindPeliculaAsync(idpelicula);
            pelicula.Genero = genero;
            pelicula.Titulo = titulo;
            pelicula.Argumento = argumento;
            pelicula.Foto = foto;
            pelicula.Actores = actores;
            pelicula.Precio = precio;
            pelicula.YouTube = youtube;
            await this.context.SaveChangesAsync();
        }

        public async Task DeletePeliculaAsync(int idpelicula)
        {
            Pelicula pelicula = await this.FindPeliculaAsync(idpelicula);
            this.context.Peliculas.Remove(pelicula);
            await this.context.SaveChangesAsync();
        }
    }
}
