using eCommerce.API.Models;

namespace eCommerce.API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private static List<Usuario> _db = new List<Usuario>()
        {
            new Usuario(){Id = 1, Nome = "Axl Rose", Email = "gunsandroses@gmail.com"},
            new Usuario(){Id = 2, Nome = "Slash", Email = "gunsandroses@gmail.com"},
            new Usuario{Id = 3, Nome = "Duff Mackgan", Email = "gunsandroses@gmail.com"}
        };

        public List<Usuario> Get()
        {
            return _db;
        }
        public Usuario Get(int id)
        {
            return _db.FirstOrDefault(x => x.Id == id);
        }
        public void Insert(Usuario usuario)
        {
            _db.Add(usuario);
        }
        public void Update(Usuario usuario)
        {

            usuario.Id = usuario.Id;
            usuario.Nome = usuario.Nome;
            usuario.Email = usuario.Email;
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

    }
}
