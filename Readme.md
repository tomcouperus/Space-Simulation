# Jupiter Asteroid Simulation
This project was made for the RUG MSc course Modelling and Simulation, 2023-2024.

## How to make an experiment
The base situation is the `Basic Setup` scene in the Unity project.
Any time you want to test some deviation, copy the scene to a new one and rename it to reflect the experiment. 

The `Simulation` objects under which all bodies are placed also holds the `SpaceSimulation` script, which is required to start the simulation. A `play` button will appear in that script once the scene is in Play mode.
This object also contains a `TimeController`, which can be used to increase the tick speed of Unity, although too high tick speeds will crash the program.

The results of a simulation will be saved in the `Data` folder in a file with the same name as the scene, the name of which is printed in the Debug Log.

**Make sure to stop play mode in the editor to properly finish the json object**

## Data
The following data is taken from Wikipedia for 624 Hektor, and the Sun and Jupiter are taken from [Nasa](https://nssdc.gsfc.nasa.gov/planetary/planetfact.html).
| Body       | Mass (e24 kg) | Mean Body Radius (km) | Semi-Major Axis (e6 km) |
| ---------- | ------------- | --------------------- | ----------------------- |
| Sun        | 1,988,500.000 | 695,700.0             | 0.0                     |
| Jupiter    | 1,898.200     | 69,911.0              | 778.479                 |
| 624 Hektor | 0.0000079     | 225.0                 | 778.479                 |