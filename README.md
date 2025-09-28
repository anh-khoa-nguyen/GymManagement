# FiTusion - Gym Management System

[![ASP.NET](https://img.shields.io/badge/ASP.NET-MVC%205-blueviolet.svg)](https://dotnet.microsoft.com/apps/aspnet)
[![C#](https://img.shields.io/badge/C%23-v7.3-blue.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-6-lightgrey.svg)](https://docs.microsoft.com/en-us/ef/)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## 📖 Overview

**FiTusion** is a comprehensive, web-based Gym Management System designed to streamline the operations of a modern fitness center. Built with ASP.NET MVC 5 and Entity Framework, this platform provides a robust, role-based solution for managing members, personal trainers (PTs), workout plans, equipment, billing, and member engagement.

The system is architected to serve three primary user roles: **Managers (Quản lý)**, **Personal Trainers (PT)**, and **Members (Hội viên)**, each with a tailored dashboard and feature set to enhance their experience.

## ✨ Key Features

### 👑 For Managers (Admin Role)

-   **📊 Interactive Dashboard:** A central hub for monitoring key performance indicators, including daily revenue, new check-ins, and data visualizations for visitor traffic and revenue trends over time.
-   **👤 User Management:** Full CRUD (Create, Read, Update, Delete) functionality for all user accounts (Members, PTs, other Managers). Includes features to lock/unlock accounts.
-   **📦 Package Management:** Create and manage various membership packages (`Gói Tập`) with different pricing, durations, and included PT sessions.
-   **🏋️ Workout Plan Management:** Design and manage structured workout plans (`Kế hoạch`) consisting of daily exercises from a predefined library.
-   **🤸 Exercise Library:** A comprehensive module to manage a library of exercises (`Bài Tập`), including descriptions, target muscle groups, required equipment, and step-by-step instructions.
-   **💰 Financial & Promotions Management:**
    -   Create and manage invoices (`Hóa Đơn`) for members.
    -   Design and manage promotional campaigns (`Khuyến Mãi`) and membership tiers (`Hạng Hội Viên`).
-   **🔧 Equipment & Facility Management:**
    -   Manage gym equipment (`Thiết Bị`), including status tracking (operational, needs maintenance, etc.).
    -   Maintain a detailed history log for all equipment changes.
    -   Manage different gym rooms/areas (`Phòng`).
-   **📈 Reporting & Analytics:** View detailed reports on top-selling packages, most popular workout plans, and top-performing personal trainers based on completed sessions and member ratings.

### 💪 For Personal Trainers (PT Role)

-   **📅 Schedule Management:** A personal calendar view (powered by FullCalendar.js) to manage and view all booked sessions with members.
-   **✅ Appointment Approval:** Approve or decline booking requests from members, with automated notifications sent upon action.
-   **👤 Member Oversight:** View detailed profiles of assigned members, including their health metrics, progress, and workout history.
-   **📊 Progress Tracking:** Update members' health indexes (`Chỉ số Sức khỏe`) such as weight and body fat percentage after sessions.

### 🏃 For Members (HoiVien Role)

-   **📱 Personalized Dashboard:** A central hub to view personal information, QR code for check-in, current membership tier, and referral program status.
-   **📦 Package Subscription:** Browse available gym packages and complete the purchase process via **MoMo Payment Gateway**.
-   **📅 PT Session Booking:** Schedule one-on-one sessions with available Personal Trainers through an interactive calendar.
-   **🏋️ Workout Plan Execution:** Follow structured, day-by-day workout plans. Includes a real-time exercise tracking view with rep counting capabilities.
-   **📈 Progress Tracking:** View personal progress through visualized charts of weight and other health metrics over time.
-   **🤖 AI-Powered Health Assistant:** An integrated chatbot powered by **Google's Gemini AI** that provides personalized health and workout advice based on the user's latest health data.
-   **💎 Loyalty & Referral Program:**
    -   Earn rewards and upgrade membership tiers based on spending.
    -   Receive a unique referral code to invite friends and earn rewards for successful referrals.
-   **💬 Real-time Chat:** Engage in one-on-one chats with Personal Trainers.

### 🚀 System-Wide Features

-   **🔒 Role-Based Access Control:** Secure and distinct access levels for Managers, PTs, and Members, managed by **ASP.NET Identity**.
-   **📲 QR Code Check-in:** Members and staff can check in to the facility by scanning a unique, dynamically generated QR code.
-   **📧 Automated Email Notifications:** Integrated with **SendGrid** to send welcome emails, payment confirmations, and reward notifications.
-   **☁️ Cloud Media Storage:** All user-uploaded images (avatars, exercise photos) are securely stored and served via **Cloudinary**.

## 🏗️ Technology Stack

| Category              | Technology / Service                                                              |
| --------------------- | --------------------------------------------------------------------------------- |
| **Backend Framework** | **C#**, **ASP.NET MVC 5**, **ASP.NET Web API**, **ASP.NET Identity**                |
| **Database**          | **Microsoft SQL Server** with **Entity Framework 6 (Code-First)**                 |
| **Frontend**          | **Razor Views**, **HTML5**, **CSS3**, **JavaScript**, **jQuery**, **AJAX**, **Bootstrap** |
| **Authentication**    | **ASP.NET Identity**, **OWIN Middleware**                                         |
| **Third-Party APIs**  | **Cloudinary** (Image Storage), **SendGrid** (Email), **MoMo** (Payment Gateway)  |
| **AI Integration**    | **Google Gemini API** (for Chatbot & Health Advice)                               |
| **Libraries**         | **QRCoder** (QR Code Generation), **Newtonsoft.Json**, **FullCalendar.js**        |

## ⚙️ Setup and Installation

### 1. Prerequisites
-   **Visual Studio 2019** or later (with ASP.NET and web development workload).
-   **.NET Framework 4.7.2**.
-   **Microsoft SQL Server** (Express version or higher).

### 2. Clone the Repository
```bash
git clone https://github.com/your-username/GymManagementSystem.git
cd GymManagementSystem
```

### 3. Configure the Application
1.  **Open the solution** (`GymManagementSystem.sln`) in Visual Studio.
2.  **Database Connection:** Open the `Web.config` file. Locate the `<connectionStrings>` section and update the `DefaultConnection` string with your SQL Server instance details (server name, database name, credentials).
3.  **API Keys:** In the `Web.config` file, update the `<appSettings>` section with your own API keys for:
    -   `Cloudinary:CloudName`
    -   `Cloudinary:ApiKey`
    -   `Cloudinary:ApiSecret`
    -   `SendGrid:ApiKey`
    -   `Momo:PartnerCode`, `Momo:AccessKey`, `Momo:SecretKey`, etc.
    -   `GoogleAI:ApiKey`

### 4. Database Migration
1.  Open the **Package Manager Console** in Visual Studio (`View` -> `Other Windows` -> `Package Manager Console`).
2.  Ensure the `GymManagementSystem` project is selected as the default project.
3.  Run the following command to apply the database migrations and create the schema:
    ```powershell
    Update-Database
    ```
    This command will also execute the `Seed` method in `Migrations/Configuration.cs` to populate the database with initial data (roles, admin account, sample data, etc.).

### 5. Run the Application
Press **F5** or click the "IIS Express" button in Visual Studio to build and run the project. The application will open in your default web browser.

## 👤 Author

*   **Anh Khoa Nguyen** - [anh-khoa-nguyen](https://github.com/anh-khoa-nguyen)
*   **Thien Doan Nguyen** - [ThienDoanPlus](https://github.com/ThienDoanPlus)
