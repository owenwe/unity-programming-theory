# Programming Theory in action submission
version 0.0.4

## Description
A submission to the **Programming Theory in Action** tutorial in the Unity Junior Programmer: Apply object-oriented principles mission.

### Updates
  - General code cleanup
  - Fixed face/normals issue with Gun and Rifle 3D objects
  - Replaced the placeholder objects with Prefabs with imported .obj files. The imported 3D 
objects still need work, but they offer enough detail to start developing animation and fx 
for firing
  - Added a base `ProjectileWeapon` class
  - Added a `Gun` class that inherits from the `ProjectileWeapon` class
  - Added a `Rifle` class that inherits from the `Gun` class
  - Added a `RailGun` class that inherits from the `Rifle` class
  - Added a `GatlingGun` class that inherits from the `RailGun` class
  - Created movement and opacity functionality for selecting weapons in the Title scene
  - Loads a Prefab weapon in the Main scene from the Resources folder based on the selected weapon in the Title scene

### Completed for 0.0.3
  - [x] create base class
  - [x] create inherited classes
  - [x] add properties/fields/methods/functions
  - [x] structure classes with private, protected, and public properties with getters and setters
  - [x] develop firearm testing with placeholder objects

### TODO
  - [x] implement better opacity fade functionality for the Title selection scene
  - [x] make the movement between selecting weapons a smooth transition
  - [x] basic ui control for the selected weapon on the Main scene
  - firing functionality/animation/fx for weapons on Main scene
  - add ui elements to Title screen for weapon properties such as ballistic type and firing mode(s)
  - root level scene manager
  - add some stink to the lighting and materials
