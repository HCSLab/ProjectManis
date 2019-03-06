#A Rhythm Game Prototype

####Unity Version 2018.1.6f1

##How to Play

You should press the buttons in order shown on the beat indicator exactly when the beats occur, in order to avoid the coming barrier or shoot the enemy. You can only input when the beat indicator turns green. 

All your inputs in a bar are graded into "MISS", "FINE", and "GREAT". Your attack has a bigger strength when your inputs in that bar is "GREAT". 

The bpm(beats per minute) increases as you kill enemies. 

##Known Issue

###Wrong Pitch When Builded on WebGL
Causes:
1. The Audio Mixer of Unity is not working when building on WebGL. 

###Poor Audio
Causes:
1. All the audio files are made via non-music-making-purpose softwares.
2. When bpm increases, audio clips should be acclerated. However, it is realized by controling the pitches, so a lot of loss is made. 