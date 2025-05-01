**FormCraftPlus** is a dynamic and extensible web application built with **ASP.NET Core MVC**, **Entity Framework Core**, and **SQL Server**, allowing users to create, manage, and interact with custom forms and templates. It includes features like user authentication, real-time interactivity, role-based access, template sharing, likes, comments, admin moderation, and a powerful dashboard for insights.


## 🎯 Key Features

- **User Authentication** with ASP.NET Core Identity
- **Role Management** (Admin / User) seeded at startup
- **Entity Framework Core**:
  - PostgreSQL in production (Render)
  - SQL Server for local development
- **Real-Time Updates** with SignalR
- **Media Uploads** via Cloudinary

---

## ⚙️ Configuration

### 1. Environment Variables

| Key                   | Description                                 |
| --------------------- | ------------------------------------------- |
| `CONNECTION_STRING`   | PostgreSQL connection string for Render    |
| `ADMIN_EMAIL`         | Email for seeded Admin user                |
| `ADMIN_PASSWORD`      | Password for seeded Admin user             |

> **Example (Render Dashboard)**
>
> | Key                | Value                                           |
> |--------------------|-------------------------------------------------|
> | `CONNECTION_STRING`| `Host=...;Port=5432;Database=app;User=...;Pass=...;` |
> | `ADMIN_EMAIL`      | `admin@finalproject.com`                             |
> | `ADMIN_PASSWORD`   | `Admin@123`                             |


### 2. Local Development

#### SQL Server (default)
1. Copy `appsettings.json` to `appsettings.Development.json`.
2. Set your `DefaultConnection`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=FormCraftPlus;Trusted_Connection=True;"
   }
   ```
3. (Optional) Use User Secrets for admin creds:
   ```bash
   dotnet user-secrets init
   dotnet user-secrets set "Admin:Email" "admin@finalproject.com"
   dotnet user-secrets set "Admin:Password" "Admin@123"
   ```

#### PostgreSQL (alternative)
1. Start a Postgres container:
   ```bash
   docker run --name fcp-pg -e POSTGRES_PASSWORD=YourPass -e POSTGRES_DB=FormCraftPlus -p 5432:5432 -d postgres
   ```
2. Update `appsettings.Development.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=FormCraftPlus;Username=postgres;Password=YourPass;"
   }
   ```

---

## 🚀 Running the Application

```bash
# Restore dependencies
dotnet restore

# Apply migrations & seed DB
dotnet ef database update

# Run the app
dotnet run
```

Visit `https://localhost:5001` (HTTPS) or `http://localhost:5000` (HTTP).

---

## 📦 Deployment on Render

1. Commit & push to GitHub.
2. Render automatically detects `.render-build.sh`:
   ```bash
   dotnet restore
   dotnet publish -c Release -o out
   ```
3. On startup, migrations and seed data (including roles and admin user) are applied via runtime migrations.

---

## 🛡️ Admin User

On first run, the app seeds:

- **Roles**: `Admin`, `User`
- **Admin** user with credentials from `ADMIN_EMAIL` / `ADMIN_PASSWORD`

Use these to log in at `/Account/Login` and manage the application.

---

## 🤝 Contributing

Feel free to open issues or pull requests. Please follow the existing code style and include relevant tests.

---

## 📄 License

This project is licensed under the MIT License.

