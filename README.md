# Gesture Based UI Development Project Using Myo Armband

2D Snake developed in Unity controlled by Myo Armband.

> author: Mark Reilly

> Module: Gesture Based UI Development 
> Lecturer: Dr Damien Costello

The application is a recreation of the classical game [Snake](https://en.wikipedia.org/wiki/Snake_(video_game_genre)).

### Purpose of the application:
The purpose of the application is to allow the user to experience a completely new method of control using the Myo Armband. Incorporating all five gestures available the application gives the user a more in-depth feel for the game. The application can be used with keyboard and mouse or solely using the Myo Armband. Setup the Myo and Unity by following this[guide ](https://developerblog.myo.com/setting-myo-package-unity/).

#### Layout

This section will describe the rationale behind in game screens.

While running the application the user is brought to a home screen with the option to play the game. The home screen consists of: The Title, Play button, Record score and the current score. See Figure Below.

While in the game the user can use the Myo Armband that is connect via Bluetooth or the arrow keys to control the character. The objective of the game is to eat as mane purple squares without colliding with the walls or the character. The game becomes increasingly difficult when character eats more purple squares due to it growing for every square eaten. As seen in the below figure, the character has increased in size from the beginning when it is only one square in size.
### Appropriate gestures identified:

The Myo Armband had a lot of gestures that could be incorporated into this application but after testing I decided to only include four out seven available due to increased unreliability with certain gestures which I will discuss below.

The seven gestures that are made available with the Myo can be seen in the figure below:


Gestures Chosen and there functions:

Wave Right was picked due to the game having an option to control moving in the right direction, it was the most logical option. The user uses this gesture to turn the character in the right direction.

Wave Left was picked due to the game having an option to control moving in the left direction, it was the most logical option as well. The user uses this gesture to turn the character in the left direction.

The Fist gesture was given the objective to control the character in the down direction. The user uses this gesture to move the player in the downwards direction.

The Fingers Spread gesture was made to control the up movement of the character. The user uses this gesture to move the character in the upwards direction.

The Finger Tap gesture was not incorporated into the application due to it being very difficult and slow to register. After testing multiple times I had to rule it out because it would be difficult for the user to get the gesture to work all while playing a fast paced game.

The Rotate or Pan were not incorporated into the application because I did not see where these two gestures would work in this type of application.

Keyboard controls:
- Up Arrow - Moves the player up.
- Down Arrow - Moves the player down.
- Left Arrow - Moves the player to the left.
- Right Arrow - Moves the Player to the right.

### Hardware used in creating the application:

#### Myo Armband

The hardware I used in creating this application is the Myo Armband to maintain control of the application.

### Architecture:
![alt text](https://github.com/MarkReillyGMIT/GestureBasedUIProject/Images/Architecture.PNG?raw=true)

Libraries Used:
- [https://www.nuget.org/packages/MyoSDK/](Myo Windows SDK)

### Conclusions & Recommendations:
Overall I am very happy with how the application turned out, I feel the lack of documentation for the Myo Armband due to it being discontinued made it more difficult to connect with different software. I have learned so much from completing this project such as having to connect a piece of hardware I had never used before to an application that I designed was very interesting.

### Video 
Demonstration of the application:

https://photos.app.goo.gl/P1XY97dVXH9SLGX96