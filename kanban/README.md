# Backend da Aplicação Web API .NET

Este é o backend da aplicação desenvolvida como parte do projeto final da Academia DotNet, que implementa uma API RESTful para oferecer uma solução personalizável para a gestão de equipes por meio de um quadro Kanban.

## ℹ️ Sobre o Projeto

Este backend foi desenvolvido em C#/.NET com a utilização do Entity Framework Core para interação com um banco de dados SQL Server, utilizando relacionamento um para muitos. A API oferece endpoints para manipulação de usuários, listas de tarefas e tarefas relativas as listas.

### 🔑 Autenticação e Autorização

A API utiliza um sistema de autenticação baseado em JWT (JSON Web Token) para endpoints sensíveis, garantindo que apenas usuários autorizados possam realizar operações como criar, atualizar ou excluir tarefas. Além disso, foi implementado hashing com salt para armazenar com segurança as senhas dos usuários no banco de dados.

## 🚀 Como Executar

Para executar este backend localmente, siga as instruções abaixo:

1. Certifique-se de ter o SDK do .NET 6.0 instalado em seu computador ([instalação](https://dotnet.microsoft.com/download)).
2. Configure a conexão com o banco de dados SQL Server no arquivo `appsettings.json` e na model kanbanContext.
3. Execute o comando `dotnet run` no terminal dentro do diretório do projeto.
4. Verifique se a API está funcionando corretamente acessando os endpoints através de ferramenta como Seagger, acessado através da URL configurada para o ambiente de desenvolvimento (por exemplo, `https://localhost:7017/swagger`), como definido no arquivo `launchSettings.json`.

## 🛠️ Tecnologias Utilizadas

- C#/.NET Core 6
- Entity Framework Core
- SQL Server
- JWT (JSON Web Token)

## 📦 Estrutura do Projeto

A estrutura do projeto segue um padrão MVC (Model-View-Controller) para organização dos controladores e modelos.

- `Controllers/`: Contém os controladores que definem os endpoints da API. Os controladores lidam com as requisições HTTP e interagem com a lógica de negócios diretamente, realizando operações nos modelos de dados e retornando respostas HTTP.
- `Models/`: Contém as definições dos modelos de dados utilizados na aplicação. Esses modelos representam as entidades do banco de dados e são usados pelos controladores para realizar operações CRUD.

## 💡 Contribuindo

Sinta-se à vontade para sugerir melhorias, abrir problemas ou contribuir com o projeto através de pull requests.



Desenvolvido com 💚 por Carol Ferraz


