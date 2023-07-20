# Map_tool
A tool for giving 3d elements to 2d game maps in Unity, with multi-player access.

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
