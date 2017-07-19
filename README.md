# Unity-Formation-Movement
Formation movement for Unity3D using standard Navmesh or A*Pathfinding

I've started gathering and configuring elements for Unity3D RTS style game which of course needs some pathfinding.
Early on I found out that A Star Pathfinding by Aron Granberg (https://arongranberg.com/astar/front) is a good solution for pathfinding
on Unity terrain. However it does not contain formation movement (also known as steering or flocking behavior) so I set out to create
my own. So here we are.

Youtube: https://www.youtube.com/playlist?list=PLNMba_kYUs0f9h-_BXizxOk5h2_e7A3ql

Feedback received so far:

* Support for A Star Pathfinding free version (grid graph): now included.
* Support for Unity Navmesh: now included.
* Add random changes to gridpoints to prevent "perfect movement": now included.

Installation:

Copy the Scripts folder to your Unity Project.

Usage:

<to be included>

Limitation:

* Currently the setup only supports movement of the units assigned to the grid if they have character controller or a rigidbody.
* Not all of the selectable grid types have been implemented, only: <add here> 

License:

Unity-Formation-Movement is freely available under the MIT license. If you make any improvements, then please submit a pull request with them so everyone else can share in the knowledge.

References:

* Implementing Coordinated Movement: http://www.gamasutra.com/view/feature/3314/coordinated_unit_movement.php?print=1
* How do I implement group formations in a 3D RTS? https://gamedev.stackexchange.com/questions/28146/how-do-i-implement-group-formations-in-a-3d-rts
* Movement: Path Finding, Flocking, Formation: http://web.eecs.umich.edu/~soar/Classes/494/talks/Lecture%207%20Movement%20and%20Pathing.pdf




