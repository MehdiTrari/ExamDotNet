# ECommerce API (Exercices 1 et 3)

API REST e-commerce avec regles metier et BDD InMemory.

## Exercice 1 - API REST
- Endpoint `GET /products` (liste des produits)
- Endpoint `POST /orders` (validation des stocks, remises et codes promo)

## Exercice 3 - BDD EF Core InMemory
- Tables : `Products` et `PromoCodes`
- Seed au demarrage pour avoir 5 produits et 2 codes promo

## Lancer l'API
```bash
dotnet run --project src/ECommerceApi/ECommerceApi.csproj
```

## Tester rapidement
```bash
curl http://localhost:5000/products
```

```bash
curl -X POST http://localhost:5000/orders \
  -H "Content-Type: application/json" \
  -d '{"products":[{"id":1,"quantity":2},{"id":2,"quantity":1}]}'
```

## Tests
```bash
dotnet test /home/mehdi/ExamDotNet/ECommerceApi/ECommerceApi.sln
```
