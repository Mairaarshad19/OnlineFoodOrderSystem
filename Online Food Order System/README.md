# 🍴 Online Food Order System

## 📌 Overview
The **Online Food Order System** is a C# Windows Forms application with MySQL database integration that allows users to browse cuisines, select restaurants, add food items to a cart, and complete orders with payment tracking. It demonstrates practical use of **data structures, database operations, and UI design** in a real-world scenario.

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

## 🛠️ Technologies Used
- **C# (.NET Framework, WinForms)**
- **MySQL Database**
- **ADO.NET / DataTables**
- **GitHub Desktop** for version control

---

## 📂 Project Structure
- `BL/` → Business Logic classes (RestaurantBL, PaymentBL, CartBL, etc.)
- `DL/` → Data Layer classes (DatabaseHelper, PaymentDL, CartDL, etc.)
- `UI/` → Windows Forms for user interaction
- `screenshots/` → UI screenshots (add your images here)

---

## ⚙️ Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/<your-username>/Online-Food-Order-System.git
