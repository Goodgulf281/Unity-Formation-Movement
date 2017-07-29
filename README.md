# Unity-Formation-Movement
Formation movement for Unity3D using standard Navmesh or A*Pathfinding

I've started gathering and configuring elements for Unity3D RTS style game which of course needs some pathfinding.
Early on I found out that A Star Pathfinding by Aron Granberg (https://arongranberg.com/astar/front) is a good solution for pathfinding
on Unity terrain. However it does not contain formation movement (also known as steering or flocking behavior) so I set out to create
my own. So here we are.

##Important to know:

* This code is not perfect nor complete. Based on below instructions you should be able to make it work. Take a look at the limitations.
* You need some basic knowledge of Navmesh. You can try this video: [Unity3D and Nav Meshes in Five Minutes (Unity 5 NavMesh)](https://youtu.be/9amYqRFxW1o)
* Make sure you known enough about animation and understand this article: [Coupling Animation and Navigation](https://docs.unity3d.com/Manual/nav-CouplingAnimationAndNavigation.html)

Want to check what's possible with this code? Check out [my channel on Youtube](https://www.youtube.com/playlist?list=PLNMba_kYUs0f9h-_BXizxOk5h2_e7A3ql)

##Feedback received so far:

* Support for A Star Pathfinding free version (grid graph): now included.
* Support for Unity Navmesh: now included.
* Add random changes to gridpoints to prevent "perfect movement": now included.
(Thanks Adam Goodrich for the feedback.)

#Installation:

Copy the Scripts folder to your Unity Project.

#Usage:

A FormationGrid contains a list of FormationGridPoints which have an offset position (in the x-z plane) from the FormationGrid position. Units (for example animated characters) can be assigned to the FormationGrid and will be automatically assigned to a FormationGridPoint. 
The FormationGrid will start following the FormationAnchor which contains the actual pathfinding components (AStarPathfinding or Navmesh).

##Quick start:
* Create a scene with a terrain or a navmesh setup.
* Create a layer "terrain" and assign the terrain or navmesh "floor" elements to this layer.
* Select the menu item "Window - Formations - Add Formation Manager" or alternatively add an empty object and assign the FormationManager.cs script to it.
* Select "AStar Pathfinding" or "Unity Navmesh" using the buttons on the Formation Manager.
* Create a new formation by clicking on the "New Formation" button on the Formation Manager. It will be created in the middle of the scene. You can alternatively create two empty game objects for a FormationGrid and a FormationAchor and connect these two (anchor to formation grid using the inspector.
* Don't forget to: set the grid type (see limitations below), the grid mask to the layer created above and the movement type to rigidbody or charactercontroller depending on which of both you have added to the units which will become part of the formation.
* If the grid has a sound associated with it (although a better practice is to have individual units have sound associated) then don't forget to add the Audio Source next to the FormationGrid component.
* Assign the objects to the grid using script, see the FormationSample.cs script. 
* Set the grid to form by using ChangeState(FormationStates.Form) on the formationgrid.
* Set the grid to move by using ChangeState(FormationStates.Move) on the formationgrid.

The key scripts to look into first are: FormationSample.cs, FormationGrid.cs and FormationAnchor.cs.

##Instruction video
A video instruction will follow soon.

#Limitations:

* Currently the setup only supports movement of the units assigned to the grid if they have character controller or a rigidbody.
* Not all of the selectable grid types have been implemented, only: Box9, Wedge9 and Column10 (More to be added in FormationGrid.cs: SetupGrid(GridTypes gridtype)).

#License:

Unity-Formation-Movement is freely available under the MIT license. If you make any improvements, then please submit a pull request with them so everyone else can share in the knowledge.

#References:

* Implementing Coordinated Movement: http://www.gamasutra.com/view/feature/3314/coordinated_unit_movement.php?print=1
* How do I implement group formations in a 3D RTS? https://gamedev.stackexchange.com/questions/28146/how-do-i-implement-group-formations-in-a-3d-rts
* Movement: Path Finding, Flocking, Formation: http://web.eecs.umich.edu/~soar/Classes/494/talks/Lecture%207%20Movement%20and%20Pathing.pdf




