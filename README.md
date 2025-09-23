
# Delivery API

API RESTful desenvolvida em ASP.NET Core 8 para gerenciar um sistema de delivery completo, incluindo autenticação JWT, cadastro de usuários, estabelecimentos, produtos, pedidos, entregadores, pagamentos, cupons, avaliações e categorias.

---

## Sumário
- [Funcionalidades](#funcionalidades)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Requisitos](#requisitos)
- [Configuração](#configuração)
- [Como Executar](#como-executar)
- [Endpoints Principais](#endpoints-principais)
- [Autenticação](#autenticação)
- [Testes](#testes)
- [Observações](#observações)

---

## Funcionalidades
- Cadastro, autenticação e gerenciamento de usuários (cliente, estabelecimento, entregador, admin)
- Gerenciamento de estabelecimentos, produtos, categorias e endereços
- Criação, acompanhamento e avaliação de pedidos
- Gerenciamento de entregadores
- Processamento de pagamentos
- Aplicação de cupons de desconto
- Upload de imagens de produtos

## Estrutura do Projeto
- `Controllers/` — Endpoints da API para cada recurso (User, Auth, Product, Order, Establishment, etc)
- `Models/` — Modelos de dados das entidades
- `Dtos/` — Objetos de transferência de dados (DTOs)
- `Repositories/` — Camada de acesso a dados
- `Services/` — Lógica de negócio
- `Data/` — Contexto do Entity Framework
- `Migrations/` — Migrações do banco de dados
- `Teste/` — Testes automatizados (xUnit)
- `Uploads/` — Imagens de produtos

## Requisitos
- .NET 8.0 SDK e Runtime
- PostgreSQL (ou outro banco configurado no `appsettings.json`)

## Configuração
1. Edite as strings de conexão e configurações em `appsettings.json` e `appsettings.Development.json`:
   - Exemplo de conexão PostgreSQL:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Host=...;Port=5432;Database=...;Username=...;Password=...;SslMode=Require"
     }
     ```
   - Configure a chave JWT, issuer e audience conforme desejado.
2. (Opcional) Ajuste as configurações de logging e ambiente.

## Como Executar
1. Restaure os pacotes:
   ```powershell
   dotnet restore
   ```
2. Aplique as migrações:
   ```powershell
   dotnet ef database update
   ```
3. Execute a aplicação:
   ```powershell
   dotnet run
   ```
4. Acesse a documentação interativa via Swagger em `http://localhost:5000/swagger` (ou porta configurada).

## Endpoints Principais

### Autenticação e Usuários
- `POST /api/auth/register` — Cadastro de usuário (cliente, restaurante, entregador, admin)
- `POST /api/login` — Login e obtenção de token JWT
- `GET /api/user` — Listar usuários (autenticado)
- `GET /api/user/{id}` — Detalhes do usuário
- `DELETE /api/user/{id}` — Remover usuário

-### Estabelecimentos, Produtos e Categorias
- `GET /api/establishment` — Listar estabelecimentos
- `POST /api/establishment` — Cadastrar estabelecimento
- `GET /api/product` — Listar produtos
- `POST /api/product` — Cadastrar produto
- `POST /api/product/upload-image` — Upload de imagem de produto
- `GET /api/category` — Listar categorias

### Pedidos, Pagamentos, Cupons, Avaliações
- `GET /api/order` — Listar pedidos
- `POST /api/order` — Criar pedido
- `GET /api/payment` — Listar pagamentos
- `POST /api/payment` — Registrar pagamento
- `GET /api/coupon` — Listar cupons
- `POST /api/coupon` — Criar cupom
- `GET /api/review` — Listar avaliações
- `POST /api/review` — Avaliar pedido/estabelecimento

### Outros
- `GET /api/address` — Listar endereços
- `POST /api/address` — Cadastrar endereço
- `GET /api/deliveryperson` — Listar entregadores
- `POST /api/deliveryperson` — Cadastrar entregador

## Autenticação
Esta API utiliza autenticação JWT. Para acessar endpoints protegidos:
1. Realize login em `/api/login` e obtenha o token JWT.
2. Envie o token no header `Authorization: Bearer {token}` nas requisições.

Exemplo de login:
```json
POST /api/login
{
   "username": "email@exemplo.com",
   "password": "suaSenha"
}
```

### Exemplo de cadastro de estabelecimento
Para registrar um estabelecimento, envie para `/api/auth/register`:
```json
{
   "name": "João da Silva",
   "email": "joao@restaurante.com",
   "password": "SenhaForte123!",
   "role": "estabelecimento",
   "restaurantName": "Restaurante Sabor Caseiro",
   "restaurantAddress": "Rua das Flores, 123, Centro, Cidade"
}
```

## Testes
Para rodar os testes automatizados:
```powershell
dotnet test Teste
```

## Observações
- O projeto utiliza AutoMapper, Entity Framework Core, JWT, xUnit, Moq e Swagger.
- Uploads de imagens são salvos na pasta `Uploads/`.
- O código segue boas práticas de arquitetura em camadas.
- As configurações sensíveis (como string de conexão) são feitas via variáveis de ambiente e `.env`.
- O domínio "restaurante" foi renomeado para "estabelecimento" em todo o projeto.

---

> Projeto desenvolvido para fins de estudo e demonstração de arquitetura de APIs RESTful modernas com .NET 8.
