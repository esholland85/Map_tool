# Map_tool
A tool for giving 3d elements to 2d game maps in Unity, with multi-player access.

Introduction:
This project has been a journey of exploration and innovation. I sought to enhance 2D game maps by adding 3D elements. Unsatisfied with existing solutions, I embarked on this endeavor to both challenge myself and provide a missing solution. Along the way, I acquired a plethora of skills, diving deep into image and file manipulation, network communication, UI design, and more.

Learning the Inner Workings:
As I delved into the project, I gained a profound understanding of image manipulation through code. I discovered how to break down images into pixels and manipulate their RGBA properties, effectively modifying their positions in 3D space. Furthermore, I mastered the art of slicing files into bits, transmitting them over a network connection to another computer, reassembling them seamlessly, and saving them as files on the target machine.

Empowering Multiplayer Access:
One of the highlights of this project was enabling multiplayer access. I successfully connected multiple users to a host and facilitated real-time synchronization, ensuring that every participant experienced the same view as changes were made. This feat required careful management of network connections and data exchange.

Dynamic User Interface:
Throughout the development process, I ventured into the Unity UI realm, overcoming challenges related to automatic light balancing. I devised solutions to make UI buttons block clicks from the main app, granting a seamless user experience. Additionally, I dabbled in multiple cameras, seamlessly switching views, and implemented intuitive keyboard controls for navigation.

File Navigation and Asset Management:
An exciting aspect of the project was the creation of a custom file navigation system that automatically populated with images and text corresponding to folder contents. This dynamic feature enabled users to easily navigate and access their custom images. Furthermore, I utilized similar techniques to generate placeable 3D objects from the contents of an asset folder, streamlining the creative process.

Optimizing Performance:
When I allowed customizable height-node parameters, I encountered an interesting challenge that required optimizing the performance of my operations. My original solution did not scale well with a high density of nodes; I successfully reduced the Big O complexity, ensuring efficient processing and smooth performance.

Conclusion:
This project has been a journey of growth and discovery, as I immersed myself in a myriad of coding challenges and solutions. From image manipulation to networking, UI design, and performance optimization, I have honed my skills and expanded my expertise. I am eager to apply these lessons and further my career as a coding professional, contributing my unique insights and problem-solving abilities to future projects.

Onced running in Unity, here's a brief breakdown of how to get started:

Select New Map

Select Find Image

Use navigation in the upper left to get to the location of your image file.

Press the button in the bottom right to select a clicked-on image file.

Set the dimensions of your map in terms of the number height nodes in each direction (they will be equidistant, so should be in roughly the ratio of your image).

Click Generate

In the upper left, select LAN Host.

Use the right mouse button to point the camera down towards the image. WASD +QE to move the camera.

The buttons at the bottom of the screen offer various controls

  Left: Terrain selection. pick how many nodes in each direction you want to select, click on the map to select. In the upper left, 4 buttons will appear. Type a number in the numeral box (negatives accepted), then add that height or set the height to that value. Toggle Transperancy is currently broken. Escape to end selection.
  
  Middle: place entity prefabs with user-accessible cameras. Escape drops current selection without placing it.
  
  Right: placeholder, no effect.
  
Click the button again to close its menu.

Maps can be saved and loaded again. When a client joins a hosted map, the data will be transferred to their machine.
