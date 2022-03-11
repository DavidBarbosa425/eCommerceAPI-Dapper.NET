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
            return _connection.Query<Usuario, Contato, Usuario>("select * from Usuarios u" +
                " left join Contatos c" +
                " on u.id = c.UsuarioId" +
                " where u.Id = @Id", (usuario, contato) =>
                {
                    usuario.Contato = contato;
                    return usuario;
                },
                new {Id = id}
           ).SingleOrDefault();
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

    }
}
