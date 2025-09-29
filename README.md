# Delivery API

API RESTful desenvolvida em ASP.NET Core 8 para gerenciar um sistema de delivery completo, incluindo autenticação JWT, cadastro de usuários, estabelecimentos, produtos, pedidos, entregadores, pagamentos, cupons, avaliações, categorias e endereços.

---

## Sumário
- [Funcionalidades](#funcionalidades)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Requisitos](#requisitos)
- [Configuração](#configuração)
- [Como Executar](#como-executar)
- [Endpoints Principais](#endpoints-principais)
- [Autenticação](#autenticação)
- [Exemplos de Uso](#exemplos-de-uso)
- [Testes](#testes)
- [Observações](#observações)

---

## Funcionalidades
- **Autenticação e Usuários**: Cadastro, autenticação JWT e gerenciamento de usuários com roles (cliente, estabelecimento, entregador, admin)
- **Estabelecimentos**: Gerenciamento completo de restaurantes e estabelecimentos comerciais
- **Produtos e Categorias**: Cadastro de produtos com categorias, preços e upload de imagens
- **Sistema de Pedidos Completo**: 
  - Criação de pedidos com múltiplos itens
  - Cálculo automático de subtotal, taxa de entrega e total
  - Acompanhamento de status (Pendente, Aceito, EmPreparo, ProntoParaEntrega, ACaminho, Entregue, Cancelado)
  - Observações para estabelecimento e entregador
  - Histórico de pedidos por usuário, estabelecimento e entregador
- **Gerenciamento de Entregadores**: Atribuição e acompanhamento de entregas
- **Endereços**: Múltiplos endereços por usuário e estabelecimento
- **Processamento de Pagamentos**: Integração com métodos de pagamento
- **Cupons de Desconto**: Sistema de promoções e descontos
- **Avaliações**: Sistema de reviews para pedidos e estabelecimentos

## Estrutura do Projeto
- `Controllers/` — Endpoints da API para cada recurso (User, Auth, Product, Order, Establishment, Address, etc)
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
4. Acesse a documentação interativa via Swagger em `http://localhost:5122/swagger` (ou porta configurada).

## Endpoints Principais

### Autenticação e Usuários
- `POST /api/auth/register` — Cadastro de usuário (cliente, estabelecimento, entregador, admin)
- `POST /api/login` — Login e obtenção de token JWT
- `GET /api/user` — Listar usuários (autenticado)
- `GET /api/user/{id}` — Detalhes do usuário
- `DELETE /api/user/{id}` — Remover usuário

### Estabelecimentos, Produtos e Categorias
- `GET /api/establishment` — Listar estabelecimentos
- `POST /api/establishment` — Cadastrar estabelecimento
- `GET /api/product` — Listar produtos
- `POST /api/product` — Cadastrar produto
- `POST /api/product/upload-image` — Upload de imagem de produto
- `GET /api/category` — Listar categorias

### Sistema de Pedidos
- `POST /api/order` — Criar novo pedido (cliente)
- `GET /api/order/{id}` — Detalhes do pedido
- `GET /api/order/my-orders` — Pedidos do usuário logado (cliente)
- `GET /api/order/establishment/{id}` — Pedidos do estabelecimento (estabelecimento/admin)
- `GET /api/order/delivery-person/{id}` — Pedidos do entregador (entregador/admin)
- `PUT /api/order/{id}/status` — Atualizar status do pedido (estabelecimento/entregador/admin)
- `PUT /api/order/{id}/cancel` — Cancelar pedido (cliente/admin)
- `PUT /api/order/{id}/assign-delivery` — Atribuir entregador (estabelecimento/admin)
- `GET /api/order` — Listar todos os pedidos (admin)

### Pagamentos, Cupons, Avaliações
- `GET /api/payment` — Listar pagamentos
- `POST /api/payment` — Registrar pagamento
- `GET /api/coupon` — Listar cupons
- `POST /api/coupon` — Criar cupom
- `GET /api/review` — Listar avaliações
- `POST /api/review` — Avaliar pedido/estabelecimento

### Endereços e Entregadores
- `GET /api/address` — Listar endereços
- `POST /api/address` — Cadastrar endereço (usuário ou estabelecimento)
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

## Exemplos de Uso

### Cadastro de usuário cliente
```json
{
  "name": "Maria Souza",
  "email": "maria@exemplo.com",
  "password": "senha123",
  "role": "cliente"
}
```

### Cadastro de estabelecimento
```json
{
  "establishmentName": "Restaurante Sabor Caseiro",
  "address": {
    "description": "Matriz",
    "street": "Rua das Flores",
    "number": "123",
    "neighborhood": "Centro",
    "city": "Cidade",
    "state": "UF",
    "zipCode": "12345-678",
    "complement": "Próximo à praça"
  },
  "categoryId": 1,
  "description": "Comida caseira e delivery rápido",
  "imageUrl": "https://exemplo.com/imagem.jpg",
  "openingTime": "08:00:00",
  "closingTime": "22:00:00",
  "hasDeliveryPerson": true,
  "minimumOrderValue": 30.00,
  "deliveryFee": 5.00
}
```

### Cadastro de endereço para usuário
```json
{
  "userId": 1,
  "description": "Casa",
  "street": "Rua das Flores",
  "number": "123",
  "neighborhood": "Centro",
  "city": "Cidade Exemplo",
  "state": "EX",
  "zipCode": "12345-678",
  "complement": "Apto 101",
  "isMain": true
}
```

### Criação de pedido completo
```json
POST /api/order
Authorization: Bearer {token_jwt}
Content-Type: application/json

{
  "establishmentId": 1,
  "items": [
    {
      "productId": 1,
      "quantity": 2,
      "observations": "Sem cebola, por favor"
    },
    {
      "productId": 3,
      "quantity": 1,
      "observations": "Ponto da carne mal passado"
    }
  ],
  "deliveryAddress": "Rua das Flores, 123 - Bairro Centro - CEP: 12345-678",
  "deliveryFee": 5.00,
  "observationsForEstablishment": "Pedido com urgência, aniversário!",
  "observationsForDelivery": "Apartamento 101, interfone 23. Portão azul."
}
```

**Resposta esperada:**
```json
{
  "id": 1,
  "userId": 13,
  "establishmentId": 1,
  "status": "Pendente",
  "subTotal": 35.00,
  "deliveryFee": 5.00,
  "totalAmount": 40.00,
  "createdAt": "2025-09-29T18:30:00Z",
  "deliveryAddress": "Rua das Flores, 123 - Bairro Centro - CEP: 12345-678",
  "observationsForEstablishment": "Pedido com urgência, aniversário!",
  "observationsForDelivery": "Apartamento 101, interfone 23. Portão azul.",
  "orderItems": [
    {
      "id": 1,
      "productId": 1,
      "quantity": 2,
      "unitPrice": 15.00,
      "totalPrice": 30.00,
      "observations": "Sem cebola, por favor"
    },
    {
      "id": 2,
      "productId": 3,
      "quantity": 1,
      "unitPrice": 5.00,
      "totalPrice": 5.00,
      "observations": "Ponto da carne mal passado"
    }
  ]
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
- O código segue boas práticas de arquitetura em camadas com Repository e Service patterns.
- As configurações sensíveis (como string de conexão) são feitas via variáveis de ambiente e `.env`.
- O domínio "restaurante" foi renomeado para "estabelecimento" em todo o projeto.
- **Sistema de Pedidos**: Implementa cálculo automático de taxas, controle de status e histórico completo.
- **Resolução de Ciclos JSON**: Configurado `ReferenceHandler.IgnoreCycles` para evitar loops infinitos.
- **Autenticação por Roles**: Controle de acesso baseado em funções (cliente, estabelecimento, entregador, admin).
- **Taxa de Entrega**: Calculada automaticamente (R$ 5,00 para pedidos < R$ 30,00, grátis acima disso).

---

> Projeto desenvolvido para fins de estudo e demonstração de arquitetura de APIs RESTful modernas com .NET 8.
