# TBDStore — E-commerce System (.NET)

Full-stack e-commerce application developed using C# (.NET), focused on managing products, users, and order processing workflows. The system implements RESTful APIs, relational database design, and business logic for handling purchase operations.

## 🚀 Features

* Product management (CRUD operations)
* User registration and authentication
* Shopping cart functionality
* Order processing and validation
* Data persistence with relational database

## 🛠️ Tech Stack

* **Backend:** ASP.NET Core (.NET)
* **ORM:** Entity Framework
* **Database:** MySQL
* **Frontend:** Razor

## 🧠 Architecture

The application follows a layered architecture:

* Controllers → Handle HTTP requests
* Services → Business logic and validations
* Data Access → Database interaction via Entity Framework

## ⚙️ How to Run

1. Clone the repository

   ```bash
   git clone https://github.com/PabloMU115/TBDStore.git
   ```

2. Configure database connection
   Update `appsettings.json` with your MySQL credentials.

3. Apply migrations (if applicable)

   ```bash
   dotnet ef database update
   ```

4. Run the project

   ```bash
   dotnet run
   ```

## 📌 Notes

* This project was built as a learning and practice environment for backend development with .NET.
* Focused on implementing real-world business logic and data integrity validations.

## 👨‍💻 Author

Pablo Mora Ureña
📧 [pablomu1999@gmail.com](mailto:pablomu1999@gmail.com)
🔗 LinkedIn: https://linkedin.com/in/pablo-mora-ureña-969bb5251
