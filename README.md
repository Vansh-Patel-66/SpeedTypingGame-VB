# 🖥️ Turbo Typist - VB.NET Typing Speed Game

A **Typing Speed Game** built in **VB.NET WinForms** with login/signup system, multiple difficulty modes, real-time scoring, leaderboard, and settings.  
The project uses **SQL Server LocalDB** for authentication and storing scores.

---

## 📂 Project Structure

```
TurboTypist/
│
├── Form1.vb # Login form
├── Form2.vb # Signup / Registration form
├── Form3.vb # Main menu (Welcome screen)
├── Form4.vb # Typing Game (Easy/Hard/Expert modes)
├── Form5.vb # Leaderboard screen
├── Form6.vb # Settings screen
│
├── Module1.vb # Global variables (fullscreen, fonts, background)
├── My Project/ # Resources & app settings
│ ├── Resources.resx # Contains background images (img1, img2, img3)
│ └── Application.myapp
│
└── Database
```


---

## 🚀 Features

- 🔑 **Login / Signup** with SQL Server LocalDB  
- 🎮 **Typing Game** with three difficulty levels: Easy, Hard, Expert  
- ⏱ **Real-time scoring** with speed (WPM) & accuracy calculation  
- 🏆 **Leaderboard** showing best scores by user  
- ⚙️ **Settings screen** (fullscreen toggle, font, background theme)  
- 🎨 Flicker-free rendering & modern UI design  
- 💾 **Persistent data storage** using SQL Server LocalDB  

---

## 🗄️ Database Structure

**Database:** `Database1`  

### `Users` Table
| Column       | Type        | Description               |
|--------------|-------------|---------------------------|
| `UserID`     | INT (PK)    | Auto-increment user ID    |
| `Username`   | NVARCHAR(50)| Unique username           |
| `Password`   | NVARCHAR(50)| Hashed password (basic)   |

### `Scores` Table
| Column       | Type        | Description                          |
|--------------|-------------|--------------------------------------|
| `ScoreID`    | INT (PK)    | Auto-increment score ID              |
| `UserID`     | INT (FK)    | Foreign key → Users.UserID           |
| `WPM`        | INT         | Words per minute                     |
| `Accuracy`   | FLOAT       | Accuracy percentage                  |
| `Difficulty` | NVARCHAR(20)| Game difficulty (Easy/Hard/Expert)   |
| `DatePlayed` | DATETIME    | Timestamp of game played             |

---

## ⚙️ Setup Instructions

### 1️⃣ Requirements
- **Visual Studio 2022** (with VB.NET WinForms support)  
- **SQL Server LocalDB** (comes with Visual Studio)  

### 2️⃣ Clone the Repository
```bash
git clone https://github.com/your-username/TurboTypist.git
cd TurboTypist
```
### 3️⃣ Configure Database

- Open SQL Server Object Explorer in Visual Studio.<br>
- Attach the Database1.mdf file located in Database/.<br>
- Ensure connection string in App.config points to Database1.mdf:<br>
```
<connectionStrings>
  <add name="TurboTypistDB"
       connectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database\Database1.mdf;Integrated Security=True"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```
### 4️⃣ Run the Project

- Open the solution in Visual Studio 2022.<br>
- Press F5 (Run).<br>
- Signup with a new user, then log in to start playing.

  ---

# Contact

If you'd like to connect or collaborate, feel free to reach out:

**Email: vansh2966.patel@gmail.com**<br>
**LinkedIn:** [Vansh Patel](https://www.linkedin.com/in/vansh-patel-0b3538321)  
**GitHub:** [Vansh-Patel-66](https://github.com/Vansh-Patel-66)
  
