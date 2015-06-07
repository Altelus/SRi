# SRi (working title)
2D Top down cooperative shooter built using Unity Engine, Photon, NGUI, C#, PHP, MySQL

###Info & Features

**v0.51 Features**
 - Player login, & score tracking via communication with php script hosted on AWS; script interfaces with MySQL database
 - Multiplayer gameplay, bullets interact with other players
 - Crates destroyed/items pickup events are sent to other clients
 - Master client spawns pickups at random locations in set intervals
 - Lobby system to create/join rooms, assign name
 - In-game chat
 - Destructible, idle monsters with health bars  
 **Future changes**
 - Networked AI for monsters
 - Expanded map
 - Skill system

###CONTROLS

Input Key    |  Function
-------- | ---
	W, A, S, D  | Movement
	R, Spacebar	| Reload
	Mouse		|Selecting menu options / shooting
	Enter		| Toggle typing in chat
	
###Built with Unity 5

Assets Used

 - Photon Unity Networking Free 1.51
 - NGUI 2.7
 - Dungeon Crawl 32x32 tiles (OpenGameArt)
 - bathyscape11 (j_peeba)
