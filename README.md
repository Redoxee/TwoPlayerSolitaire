# Two Players Solitaire

This will be a multiplayer card game, akin to the solitaire genre, playable via browser.  
The ruleset is inspiered by [karanos-solitaire](https://rokasv.itch.io/karanos-solitaire)  
The code is forked from an other [unfinished project](https://github.com/Redoxee/Wist)  

I will try to code as much as possible in a single solution with a .Net5.0 server and a html/css/js front end.  
The goal is to keep the project light in dependecies, although I'm not entierly closed to the idea of using tools if I find them usefull enough.

In the solution you'll find several projects :  
- AMG : A library of various general purpose code  
- MultiplayerSolitaireGame : The card game code
- ConsoleCardGame : Simple wrapper to play the game in the console
- WebCardGame : Web server that play the game. It contains the web client
- WebConsoleLauncher : Wrapper to launch the web server with command line
- WebGUILauncher : Windows GUI interface to launch a server
- DockerGameClient : Wrapper destined to be in a docker container
- GameDealer : Web server that is able to run a game docker image on demand

---
About me:  
I'm Anton, I mostly make games, both in my [free time](https://antonmakesgames.itch.io/) and in my professional time.  
I often don't finish my personal projects, but I hope this one has a scope small enough that I can reach a satifying point before running out of motivation.
