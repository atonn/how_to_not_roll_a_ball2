# How to not roll a ball 2

One Paragraph of project description goes here

## Controls
* A: move left
* D: move right
* Space: Jump
* Escape: Quit game & write run data to CSV (in Assets/Data folder)

## Idea & Approach

We wanted to use Unity to do actual 3D work with some fun fast-paced gameplay and incorporate a Stroop Task into it,
By combining both tutorials from the course,
we went for a 3D Endless Runner type of game, since here you can generate obstacles (stroop tasks) on the fly 
instead of designing complex gigantic levels beforehand.

## Learning Progress
* creating and destroying Prefab instances via script
* how and when to manipulate them at runtime
* shiny particle explosion
* general communication between script-generated prefabs for data collection 

## Difficulties
* Interpreting movement data from 3D objects for scientific purposes is quite difficult
* we originally wanted more complex "stroop task walls" where the visible wall could be fake/passable and an invisible wall next to it would be the actual wall, but it turned out clunky
* trying to get all tunable parameters for the experiment in one convenient unity GUI inspector is tricky
 
## What grade we think is suitable
### Pros:
* we think the gameplay and feel of it turned out quite cool
* some meaningful advances to what was taught in the course
### Cons:
* no VR used
* not a lot of optimization for smooth framerates or great graphics
* focus should be on data collection more than gameplay, which turned out difficult for our type of game
* speaking of data collection focus, we also just rehashed the csv code from the course with an "eventType" row instead of implementing a different technique
* there is also no final code (f.e. python pandas) that takes our csv and creates some meaningful evaluation results from the generated csv (not sure if that was part of the task though)

All in all we still hope the project is worth a spot somewhere between B or C? 