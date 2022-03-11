using Dapper;
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




        public List<Usuario> Get()
        {
            return _connection.Query<Usuario>("select * from usuarios").ToList();
        }
        public Usuario Get(int id)
        {
            return _connection.QuerySingleOrDefault<Usuario>("select * from usuarios where id = @Id", new {Id = id });
        }
        public void Insert(Usuario usuario)
        {
            string sql = "insert into usuarios (nome,email,sexo,rg,cpf,nomeMae,situacaoCadastro,datacadastro)" +
                "values (@nome,@email,@sexo,@rg,@cpf,@nomeMae,@situacaoCadastro,@datacadastro); select cast(scope_identity() as int);";

            usuario.Id = _connection.Query<int>(sql, usuario).Single();
        }
        public void Update(Usuario usuario)
        {
            string sql = "update usuarios set nome = @nome ,email = @email,sexo = @sexo,rg = @rg,cpf = @cpf,nomeMae" +
                " = @nomemae,situacaoCadastro = @situacaocadastro,datacadastro = @datacadastro where id = @id";

            _connection.Execute(sql, usuario);


        }
        public void Delete(int id)
        {


            _connection.Execute("delete from usuarios where id = @Id", new {Id = id});
        }

        private static List<Usuario> _db = new List<Usuario>()
        {
            new Usuario(){Id = 1, Nome = "Axl Rose", Email = "gunsandroses@gmail.com"},
            new Usuario(){Id = 2, Nome = "Slash", Email = "gunsandroses@gmail.com"},
            new Usuario{Id = 3, Nome = "Duff Mackgan", Email = "gunsandroses@gmail.com"}
        };

    }
}
