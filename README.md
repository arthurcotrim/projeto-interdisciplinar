# Sistema de Gerenciamento de Endereços para Clientes

Este projeto é uma aplicação simples, porém essencial, para o gerenciamento eficiente de endereços de clientes. Criado com foco em praticidade e organização, o sistema permite cadastrar, editar, visualizar e excluir endereços, sendo uma solução útil para empresas que buscam centralizar essas informações e otimizar processos relacionados a dados de localização.

## 🛠 Tecnologias Utilizadas

- **C# .NET MVC**: Arquitetura robusta para criar uma aplicação estruturada e escalável.
- **SQL Server**: Banco de dados relacional para armazenamento seguro e eficiente das informações.
- **Dapper**: Micro-ORM utilizado para realizar consultas de alto desempenho ao banco de dados.
- **HTML, CSS e JavaScript**: Para construir a interface web, proporcionando uma experiência amigável para o usuário.

## 🌟 Funcionalidades

- **Cadastro de Endereços**: Insira informações completas de endereços dos clientes.
- **Edição**: Atualize endereços facilmente em caso de mudanças.
- **Consulta**: Pesquise e visualize endereços de forma rápida.
- **Exclusão**: Remova registros de endereços obsoletos ou incorretos.
- **Integração com API ViaCEP**: Automatize a busca de endereços com base no CEP fornecido.

## 🚀 Benefícios

- **Eficiência**: Centraliza e organiza dados de localização dos clientes.
- **Escalabilidade**: Projetado para suportar o aumento no número de clientes e endereços.
- **Fácil Integração**: Ideal para integração com sistemas maiores, como ERPs ou CRMs.
- **Exemplo Didático**: Ótimo para desenvolvedores que buscam aprender ou aprimorar habilidades em C# .NET MVC, Dapper e manipulação de APIs.

## 📂 Estrutura do Projeto

O projeto segue uma arquitetura clara e organizada:

- **Controllers**: Controladores para gerenciar as requisições e respostas da aplicação.
- **Views**: Interface do usuário desenvolvida com Razor Pages.
- **Models**: Estruturação das entidades e lógica de negócios.
- **Services**: Comunicação com a API ViaCEP e validação de dados.
- **Database**: Consultas otimizadas e mapeamento dinâmico utilizando o Dapper.

## 🎯 Como Executar

1. Clone o repositório:
   ```bash
   [git clone https://github.com/seu-usuario/nome-do-repositorio.git](https://github.com/arthurcotrim/projeto-interdisciplinar)
   Configure a conexão com o banco de dados no arquivo appsettings.json.
2. Execute as migrações do banco de dados (se aplicável).
3. Inicie o projeto a partir do Visual Studio ou outro IDE de sua preferência.
4. Acesse a aplicação em http://localhost:5000.

## 📈 Próximos Passos

1. Implementar autenticação e autorização para melhorar a segurança.
2. Adicionar suporte para múltiplos idiomas.
3. Criar relatórios detalhados sobre os endereços cadastrados.
4. Melhorar a interface do usuário com frameworks modernos como Bootstrap ou Tailwind CSS.

## 📜 Licença

Este projeto é distribuído sob a licença MIT. Consulte o arquivo LICENSE para mais detalhes.

## 🤝 Contribuições

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues e enviar pull requests para melhorias ou novas funcionalidades.
