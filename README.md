# 🍴 Online Food Order System

![C#](https://img.shields.io/badge/C%23-.NET_Framework-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![WinForms](https://img.shields.io/badge/WinForms-UI-512BD4?style=for-the-badge&logo=windows&logoColor=white)
![MySQL](https://img.shields.io/badge/MySQL-Database-4479A1?style=for-the-badge&logo=mysql&logoColor=white)
![ADO.NET](https://img.shields.io/badge/ADO.NET-Data_Access-00599C?style=for-the-badge&logo=dotnet&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

## 📌 Overview
The **Online Food Order System** is a C# Windows Forms application integrated with a MySQL database. It allows users to browse cuisines, select restaurants, add food items to a cart, and complete orders with payment tracking. This project demonstrates practical use of **data structures, database operations, and UI design** in a real-world food ordering scenario.

---

## 🚀 Features
- Cuisine & restaurant selection
- Cart management (add, remove, clear items)
- Payment system with status tracking (Pending → Completed)
- Restriction: one restaurant per cart
- Admin panel for managing cuisines, restaurants, and menu items
- Data validation (quantity > 0, availability checks)
- User authentication & session handling

---

## 🛠️ Tech Stack
- **C# (.NET Framework, WinForms)**
- **MySQL Database**
- **ADO.NET / DataTables**
- **GitHub Desktop** for version control

---

## 📂 Project Structure

```
OnlineFoodOrderSystem/
│── BL/             # Business Logic classes (RestaurantBL, PaymentBL, CartBL, etc.)
│── DL/             # Data Layer classes (DatabaseHelper, PaymentDL, CartDL, etc.)
│── UI/             # Windows Forms for user interaction
│── screenshots/    # UI screenshots
│── README.md       # Project documentation
│── LICENSE         # License file
│── .gitignore      # Ignore build artifacts
```

---

## ⚙️ Prerequisites
- Visual Studio (2019 or later)
- .NET Framework
- MySQL Server
- GitHub Desktop (for version control)

---

## 🔧 Installation

1. Clone the repository:
```bash
   git clone https://github.com/Mairaarshad19/OnlineFoodOrderSystem.git
```
2. Open the solution in Visual Studio.
3. Configure your MySQL connection string in `DatabaseHelper.cs`.
4. Run database migrations or import the provided SQL schema.
5. Build and run the project.

---

## 🎮 Usage
- Log in as a user.
- Select a cuisine → restaurant → food item.
- Add items to the cart (quantity > 0).
- Proceed to payment, choose a method, and confirm.
- Admin can manage restaurants, cuisines, and menu items.

---

## 📸 Screenshots
*(Add screenshots of your forms here, e.g., login screen, cart, payment form.)*

---

## 📜 License
This project is licensed under the **MIT License** — you are free to use, modify, and distribute with attribution.

---

## 🤝 Contributing
Contributions are welcome!

1. Fork the repo
2. Create a feature branch (`git checkout -b feature-name`)
3. Commit changes (`git commit -m "Added feature"`)
4. Push to branch (`git push origin feature-name`)
5. Open a Pull Request

---

## 🧭 Roadmap
- ✅ Cart & payment integration
- ✅ Cuisine/restaurant management
- 🔜 Online database hosting
- 🔜 User roles (Admin vs Customer)
- 🔜 Reporting & analytics

---

## 👩‍💻 Author
Developed by **Maira Arshad**
University project — Computer Science specialization in security & data structures.
