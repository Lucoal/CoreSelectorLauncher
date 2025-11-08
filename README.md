# <img src="Assets/Textures/icons8-processore-96.png" width="50" align="center"> **CoreSelectorLauncher**

A **Unity-based desktop tool** for advanced process management on Windows, allowing you to:
- choose which **CPU cores** a process should use (CPU affinity)
- **launch processes** directly from the interface
- manage and **save favorites** for quick reuse


## Features

### CPU Core Selection
- Displays all **logical CPU cores** available on the system (works with both Intel and AMD)
- Each core can be individually **enabled or disabled** via UI toggles
- Automatically generates the correct **affinity bitmask** for the selected configuration

### Process Launching
- Allows you to start any executable with a custom **CPU affinity**.
- Handles the correct affinity assignment using Windows’ `ProcessorAffinity` API
- Fully compatible with both **Intel hybrid architectures (P/E cores)** and **AMD** CPUs

### Favorites Management
#### Save frequently used paths as **favorites**

- **Add** new empty favorites
- **Edit** the value directly in the input field
- **Start** the process with the current configuration
- **Delete** a favorite

### Wallpaper Switching
- You can change the **app wallpaper** selecting the frame icon in the **Side Menu**
- Supports common image formats (`.jpg`, `.png`)


## How to Use

1. **Launch the application**
2. Select the **CPU cores** you want the process to use
3. Enter the **path to the executable**
4. Press **Start** to launch it with the selected affinity
### To save a path:
 - Click on the **Star** in the **Side Menu**
 - Click **Add Favorite** to create an empty entry
 - Edit its name or bitmask directly in the input field
#### Use the buttons:
 - **Start** → Launch the process using the current input value. 
 - **Delete** → Remove the favorite
