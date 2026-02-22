# 🐰 Hare's Blooming Lace

> A 2D story-driven adventure game about courage, family, and unexpected twists. 
> *Developed as a graduation project for the "Software Development" course at ItStep Academy (Finished in September 2025).*

## 📖 About the Game
Our brave protagonist returns home only to find her younger brother kidnapped by a fearsome wolf. Realizing that no one else can help, she embarks on a journey to find the wolf's lair. 

**Genre:** 2D Linear Adventure / Walking Simulator
**Story:** Features a quest and dialogue system where player actions directly impact the environment and unlock new locations, leading to a surprising, heartwarming plot twist.

## ⚙️ Technical Highlights (C# / .NET Focus)
This project was initially planned for two programmers, but I ended up **developing the entire codebase solo**. It was a massive challenge that taught me how to architect a complete game from scratch and communicate effectively with artists.

* **Engine:** Unity 6000.1.6f1
* **Language:** C#
* **Architecture & Patterns:** * Implemented the **Singleton pattern** for core systems: `SoundManager`, `TransitionManager`, and `QuestManager` to ensure global access and persistent states across scenes.
* **Key Systems Implemented:**
  * Custom Dialogue & Quest system (quests dynamically update environment access).
  * Physics-based 2D character controller with smooth acceleration and dynamic animations tied to movement speed.

## 🎮 Controls
* **[W] [A] [S] [D]** - Move
* **[Space]** - Jump
* **[ E ]** - Interact / Advance Dialogue
