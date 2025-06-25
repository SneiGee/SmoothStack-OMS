# 🛒 Order Management System – A Discounting System 

A clean, testable .NET 8 Web API simulating an order management system using modern architectural practices.

---

## 🚀 Features Implemented

### ✅ Order Creation
- Accepts order items from a customer.
- Calculates discounts dynamically based on customer segment (New, Regular, VIP).
- Saves and returns final order details (total, discount, status).

### ✅ Discount System
- Discounts applied as:
  - `New`: 5%
  - `Regular`: 10%
  - `VIP`: 20%
- Promotion logic handled cleanly in repository using segment and order history.

### ✅ Order Status Tracking
- Order transitions follow a strict state machine:
  - `Pending` → `Processing` → `Fulfilled`
- Prevents invalid transitions.


### ✅ Order Analytics Endpoint
- Exposes statistics via an endpoint:
  - Total orders
  - Fulfilled orders
  - Average order value
  - Average fulfillment time (in hours)

---

## ⚙️ Tech Stack

| Layer             | Tech Used                    |
|------------------|-------------------------------|
| Web API          | .NET 8 Minimal APIs           |
| Architecture     | Vertical Slice + MediatR      |
| Persistence      | Entity Framework Core + Npgsq |
| Validation       | FluentValidation              |
| Testing          | xUnit, Moq, EF InMemory       |
| Documentation    | Swagger (OpenAPI)             |

---

## 📁 Project Structure (Vertical Slices)
```bash
├── Order/
│ ├── Features/
│ │ ├── CreateOrder/
│ │ ├── UpdateOrderStatus/
│ │ └── OrderAnalytics/
│ ├── Domain/ # Core entities and enums
│ ├── Data/ # DbContext & SeedData
│ ├── Shared/ # Behaviors & Exception handlers
│ └── Extensions/ # Startup extensions & migration logic
tests/
└── Order.Tests/
├── Handlers/ # Unit tests
└── Integration/ # Full integration test

````
 
---

## 🧪 Testing

Both **unit** and **integration** tests are included and automated.

```bash
dotnet test
```` 
✔️ Uses InMemory DB during tests
✔️ Tests all core features and transitions

---
## ⚡ Performance Optimizations

- ✅ AsNoTracking() used in read-only queries

- ✅ Efficient object construction in handlers

- ✅ Clean disposal via DI and scoped services



---
## 🧾 Swagger / API Documentation

- All endpoints are fully documented using Swagger.

- To view:
```bash
dotnet run --project src/Order
```` 

- Then open:
```bash
http://localhost:5000/swagger
```` 

---
## ⚙️ Run Locally

- 1️⃣ Configure your connection string:
```json
// appsettings.json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=OrderDb;Username=postgres;Password=yourpassword"
}
````

- 2️⃣ Run the API
```bash
dotnet run --project src/Order
```` 

---
## ✍️ Assumptions

- Order items are passed inline (no separate product catalog).

- Basic customer segment logic — no auth system in scope.

- No UI or frontend — API-first system.


---
## 🧠 Design Considerations
- MediatR decouples features from plumbing logic.

- Validation with FluentValidation ensures clean input.

- Logging and exception handling through clean middleware.

- Tests are isolated, fast, and use in-memory EF Core



