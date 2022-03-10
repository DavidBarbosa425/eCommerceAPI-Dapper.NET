using eCommerce.API.Models;
using System.Data;
using System.Data.SqlClient;

namespace eCommerce.API.Repositories
{
   
    public class UsuarioRepository : IUsuarioRepository
    {
        private IDbConnection _connection;
        public UsuarioRepository()
        {
            _connection = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=eCommerce;Data Source=DESKTOP-G95Q0DC\\SQLEXPRESS");
        }


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
            var ultimoUsuario = _db.LastOrDefault();

            if (ultimoUsuario == null)
            {
                usuario.Id = 1;
            }
            else
            {
                usuario.Id = ultimoUsuario.Id;
                usuario.Id++;
            }
            _db.Add(usuario);
        }
        public void Update(Usuario usuario)
        {
            _db.Remove(_db.FirstOrDefault(x => x.Id == usuario.Id));
            _db.Add(usuario);


        }
        public void Delete(int id)
        {

            _db.Remove(_db.FirstOrDefault(x => x.Id == id));
        }

    }
}
