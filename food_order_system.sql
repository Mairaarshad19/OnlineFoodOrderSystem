CREATE DATABASE food_order_system;
USE food_order_system;

-- USERS
CREATE TABLE users (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100),
    email VARCHAR(255) UNIQUE,
    mobile_number VARCHAR(20) UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    social_provider ENUM('Google','Facebook') DEFAULT NULL,
    role ENUM('customer','restaurant','rider') NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
SELECT* FROM restaurants;
-- RESTAURANTS
CREATE TABLE restaurants (
    restaurant_id INT AUTO_INCREMENT PRIMARY KEY,
    owner_id INT NOT NULL,
    name VARCHAR(255) NOT NULL,
    cuisine VARCHAR(100),
    logo_url VARCHAR(255),
    operating_hours VARCHAR(255),
    address VARCHAR(255),
    email VARCHAR(255),
    phone VARCHAR(50),
    FOREIGN KEY (owner_id) REFERENCES users(user_id)
);

-- MENU ITEMS
CREATE TABLE menu_items (
    item_id INT AUTO_INCREMENT PRIMARY KEY,
    restaurant_id INT NOT NULL,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    price DECIMAL(10,2) NOT NULL,
    is_available BOOLEAN DEFAULT TRUE,
    cuisine_type VARCHAR(100),
    estimated_delivery_time INT DEFAULT 30,
    FOREIGN KEY (restaurant_id) REFERENCES restaurants(restaurant_id)
);

-- CARTS
CREATE TABLE carts (
    cart_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    FOREIGN KEY (user_id) REFERENCES users(user_id)
);

-- CART ITEMS
CREATE TABLE cart_items (
    cart_item_id INT AUTO_INCREMENT PRIMARY KEY,
    cart_id INT NOT NULL,
    item_id INT NOT NULL,
    quantity INT DEFAULT 1,
    FOREIGN KEY (cart_id) REFERENCES carts(cart_id),
    FOREIGN KEY (item_id) REFERENCES menu_items(item_id)
);

-- ORDERS
CREATE TABLE orders (
    order_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    restaurant_id INT NOT NULL,
    status ENUM('Confirmed','Preparing','Out for Delivery','Delivered','Cancelled') DEFAULT 'Confirmed',
    delivery_address VARCHAR(255),
    delivery_time DATETIME,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(user_id),
    FOREIGN KEY (restaurant_id) REFERENCES restaurants(restaurant_id)
);

-- ORDER ITEMS
CREATE TABLE order_items (
    order_item_id INT AUTO_INCREMENT PRIMARY KEY,
    order_id INT NOT NULL,
    item_id INT NOT NULL,
    quantity INT NOT NULL,
    unit_price DECIMAL(10,2),
    total_price DECIMAL(10,2) GENERATED ALWAYS AS (quantity * unit_price) STORED,
    FOREIGN KEY (order_id) REFERENCES orders(order_id),
    FOREIGN KEY (item_id) REFERENCES menu_items(item_id)
);

-- PAYMENTS
CREATE TABLE payments (
    payment_id INT AUTO_INCREMENT PRIMARY KEY,
    order_id INT NOT NULL,
    method ENUM('JazzCash','Easypaisa','HBLKonnect','CreditCard','DebitCard','COD') NOT NULL,
    amount DECIMAL(10,2) NOT NULL,
    status ENUM('Pending','Completed','Failed') DEFAULT 'Pending',
    FOREIGN KEY (order_id) REFERENCES orders(order_id)
);

-- RIDERS
CREATE TABLE riders (
    rider_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    vehicle_type VARCHAR(50),
    FOREIGN KEY (user_id) REFERENCES users(user_id)
);

-- DELIVERY ASSIGNMENTS
CREATE TABLE delivery_assignments (
    assignment_id INT AUTO_INCREMENT PRIMARY KEY,
    order_id INT NOT NULL,
    rider_id INT NOT NULL,
    status ENUM('Assigned','Accepted','Rejected','PickedUp','Delivered') DEFAULT 'Assigned',
    FOREIGN KEY (order_id) REFERENCES orders(order_id),
    FOREIGN KEY (rider_id) REFERENCES riders(rider_id)
);

-- FEEDBACK
CREATE TABLE feedback (
    feedback_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    restaurant_id INT,
    order_id INT,
    rating INT CHECK (rating BETWEEN 1 AND 5),
    comment TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(user_id),
    FOREIGN KEY (restaurant_id) REFERENCES restaurants(restaurant_id),
    FOREIGN KEY (order_id) REFERENCES orders(order_id)
);

INSERT INTO restaurants (owner_id, name, cuisine, logo_url, operating_hours, address, email, phone) VALUES
(3,'Bilal BBQ','BBQ','bbq.png','12PM-12AM','Model Town','bbq@bilal.com','03001111111'),
(3,'Bilal Tandoor','Pakistani','tandoor.png','8AM-10PM','Gulberg','tandoor@bilal.com','03002222222'),
(3,'Seafood Hub','Seafood','seafood.png','1PM-11PM','DHA','seafood@bilal.com','03003333333'),
(7,'Burger Hub','Fast Food','burger.png','9AM-11PM','Johar Town','burger@usman.com','03004444444'),
(7,'Pasta Place','Italian','pasta.png','11AM-11PM','Faisal Town','pasta@usman.com','03005555555'),
(7,'Coffee Cafe','Cafe','coffee.png','7AM-10PM','Shadman','coffee@usman.com','03006666666'),
(7,'Shawarma House','Middle Eastern','shawarma.png','10AM-12AM','Cantt','shawarma@usman.com','03007777777'),
(3,'Dessert Den','Desserts','dessert.png','12PM-11PM','Township','dessert@bilal.com','03008888888'),
(3,'Pizza Palace','Italian','pizza.png','11AM-12AM','Wapda Town','pizza@bilal.com','03009999999'),
(7,'Biryani House','Pakistani','biryani.png','10AM-11PM','Garden Town','biryani@usman.com','03001010101');

