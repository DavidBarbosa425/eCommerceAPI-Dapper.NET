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
            //return _connection.Query<Usuario>("SELECT * FROM Usuarios").ToList();
            List<Usuario> usuarios = new List<Usuario>();
            string sql = "SELECT U.*, C.*, EE.*, D.* FROM Usuarios as U LEFT JOIN Contatos C ON C.UsuarioId = U.Id LEFT JOIN EnderecosEntrega EE ON EE.UsuarioId = U.Id LEFT JOIN UsuariosDepartamentos UD ON UD.UsuarioId = U.Id LEFT JOIN Departamentos D ON UD.DepartamentoId = D.Id";

            _connection.Query<Usuario, Contato, EnderecoEntrega, Departamento, Usuario>(sql,
                (usuario, contato, enderecoEntrega, departamento) => {

                    //Verificação do usuário.
                    if (usuarios.SingleOrDefault(a => a.Id == usuario.Id) == null)
                    {
                        usuario.Departamentos = new List<Departamento>();
                        usuario.EnderecosEntrega = new List<EnderecoEntrega>();
                        usuario.Contato = contato;
                        usuarios.Add(usuario);
                    }
                    else
                    {
                        usuario = usuarios.SingleOrDefault(a => a.Id == usuario.Id);
                    }

                    //Verificação do Endereço de Entrega.
                    if (usuario.EnderecosEntrega.SingleOrDefault(a => a.Id == enderecoEntrega.Id) == null)
                    {
                        usuario.EnderecosEntrega.Add(enderecoEntrega);
                    }

                    //Verificação do Departamento.
                    if (usuario.Departamentos.SingleOrDefault(a => a.Id == departamento.Id) == null)
                    {
                        usuario.Departamentos.Add(departamento);
                    }

                    return usuario;
                });

            return usuarios;
        }
        public Usuario Get(int id)
        {
            //return _connection.Query<Usuario>("SELECT * FROM Usuarios").ToList();
            List<Usuario> usuarios = new List<Usuario>();
            string sql = "SELECT U.*, C.*, EE.*, D.* FROM Usuarios as U LEFT JOIN Contatos C ON C.UsuarioId = U.Id LEFT JOIN EnderecosEntrega EE ON EE.UsuarioId = U.Id LEFT JOIN UsuariosDepartamentos UD ON UD.UsuarioId = U.Id LEFT JOIN Departamentos D ON UD.DepartamentoId = D.Id where u.id = @Id";

            _connection.Query<Usuario, Contato, EnderecoEntrega, Departamento, Usuario>(sql,
                (usuario, contato, enderecoEntrega, departamento) => {

                    //Verificação do usuário.
                    if (usuarios.SingleOrDefault(a => a.Id == usuario.Id) == null)
                    {
                        usuario.Departamentos = new List<Departamento>();
                        usuario.EnderecosEntrega = new List<EnderecoEntrega>();
                        usuario.Contato = contato;
                        usuarios.Add(usuario);
                    }
                    else
                    {
                        usuario = usuarios.SingleOrDefault(a => a.Id == usuario.Id);
                    }

                    //Verificação do Endereço de Entrega.
                    if (usuario.EnderecosEntrega.SingleOrDefault(a => a.Id == enderecoEntrega.Id) == null)
                    {
                        usuario.EnderecosEntrega.Add(enderecoEntrega);
                    }

                    //Verificação do Departamento.
                    if (usuario.Departamentos.SingleOrDefault(a => a.Id == departamento.Id) == null)
                    {
                        usuario.Departamentos.Add(departamento);
                    }

                    return usuario;
                },new { Id = id});

            return usuarios.SingleOrDefault();
        }
    
        public void Insert(Usuario usuario)
        {
            _connection.Open();
            var transaction = _connection.BeginTransaction();

            try
            {
                string sql = "insert into usuarios (nome,email,sexo,rg,cpf,nomeMae,situacaoCadastro,datacadastro)" +
                             "values (@nome,@email,@sexo,@rg,@cpf,@nomeMae,@situacaoCadastro,@datacadastro);" +
                             " select cast(scope_identity() as int);";
                usuario.Id = _connection.Query<int>(sql, usuario, transaction).Single();

                if (usuario.Contato != null)
                {
                    usuario.Contato.UsuarioId = usuario.Id;
                    string sqlContato = "insert into contatos (usuarioId, telefone,celular) " +
                        "values(@usuarioId, @telefone,@celular); select cast(scope_identity() as int);";
                    usuario.Contato.Id = _connection.Query<int>(sqlContato, usuario.Contato, transaction).Single();
                }
                if (usuario.EnderecosEntrega != null && usuario.EnderecosEntrega.Count > 0)
                {
                    foreach (var enderecoEntrega in usuario.EnderecosEntrega)
                    {
                        enderecoEntrega.UsuarioId = usuario.Id;
                        string sqlEndereco = "insert into enderecosEntrega (usuarioId, nomeEndereco,cep,estado, cidade, bairro,endereco,numero,complemento)" +
                            "values (@usuarioId, @nomeEndereco,@cep,@estado, @cidade, @bairro,@endereco,@numero,@complemento); select cast(scope_identity() as int);";
                        enderecoEntrega.Id = _connection.Query<int>(sqlEndereco, enderecoEntrega, transaction).Single();
                    }
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex)
                {
                    //
                }
            }
            finally
            {
                
                _connection.Close();
            }

        }
        public void Update(Usuario usuario)
        {
            _connection.Open();
            var transaction = _connection.BeginTransaction();

            try
            {
                string sql = "update usuarios set nome = @nome ,email = @email,sexo = @sexo,rg = @rg,cpf = @cpf,nomeMae" +
                             " = @nomemae,situacaoCadastro = @situacaocadastro,datacadastro = @datacadastro where id = @id";
                _connection.Execute(sql, usuario, transaction);

                if (usuario.Contato != null)
                {
                    string sqlContato = "update contatos set telefone = @telefone, celular = @celular where id = @id";
                    _connection.Execute(sqlContato, usuario.Contato, transaction);
                }

                string sqlDeletarEndereco = "delete enderecosEntrega where usuarioId = @usuarioId";
                _connection.Execute(sqlDeletarEndereco, usuario.EnderecosEntrega, transaction);


                if (usuario.EnderecosEntrega != null && usuario.EnderecosEntrega.Count > 0)
                {
                    foreach (var enderecoEntrega in usuario.EnderecosEntrega)
                    {
                        enderecoEntrega.UsuarioId = usuario.Id;
                        string sqlEndereco = "insert into enderecosEntrega (usuarioId, nomeEndereco,cep,estado, cidade, bairro,endereco,numero,complemento)" +
                            "values (@usuarioId, @nomeEndereco,@cep,@estado, @cidade, @bairro,@endereco,@numero,@complemento); select cast(scope_identity() as int);";
                        enderecoEntrega.Id = _connection.Query<int>(sqlEndereco, enderecoEntrega, transaction).Single();
                    }
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception)
                {
                    //
                }
            }
            finally
            {
                _connection.Close();
            }

        }
        public void Delete(int id)
        {


            _connection.Execute("delete from usuarios where id = @Id", new {Id = id});
        }

    }
}
